using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

//处理HTML页面：下载HTML页面，读取Web文件的内容，并写入到本地文件
namespace Spider
{
    public class DocumentWorker
    {
        private Uri uri;//要扫描的基础URI
        private Spider spider;
        private Thread thread;
        public const string IndexFile = "index.html";//默认文档的文字
        public const string IndexImg = ".jpg";//默认图片的文字
        public DocumentWorker(Spider x)
        {
            spider = x;
        }

        //输入参数是一个URI名称，如images/blank.gif，把它转换成本地文件名称。若尚未创建相应的目录结构，创建之
        private string convertFilename(Uri uri, int flag)
        {
            string result = spider.Output;
            int index1, index2;
            if (result[result.Length - 1] != '\\') result += "\\";
            String path = uri.PathAndQuery;
            string filename = "";
            if (spider.Flag == 0)
            {
                int queryIndex = path.IndexOf("?");
                if (queryIndex != -1)
                    path = path.Substring(0, queryIndex);

                int lastSlash = path.LastIndexOf('/');
                int lastDot = path.LastIndexOf('.');
                if (path[path.Length - 1] != '/')
                {
                    if (lastSlash > lastDot)
                    {
                        path += "/" + IndexFile;
                    }
                }

                lastSlash = path.LastIndexOf('/');
                if (lastSlash != -1)
                {
                    filename = path.Substring(1 + lastSlash);
                    path = path.Substring(0, 1 + lastSlash);
                    if (filename.Equals("")) filename = IndexFile;
                }

                //必要时创建目录构造
                index1 = 1;
                do
                {
                    index2 = path.IndexOf('/', index1);
                    if (index2 != -1)
                    {
                        String dirpart = path.Substring(index1, index2 - index1);
                        result += dirpart;
                        result += "\\";
                        Directory.CreateDirectory(result);
                        index1 = index2 + 1;
                    }
                } while (index2 != -1);
            }
            else
            {
                Directory.CreateDirectory(result);
                filename = (++spider.Cnt) + IndexImg;
            }

            result += filename;
            return result;
        }

        //将二进制文件保存到磁盘
        private void SaveBinaryFile(WebResponse response)
        {
            byte[] buffer = new byte[1024];//准备一个缓冲区临时地保存二进制文件的内容
            if (spider.Output == null) return;

            //要确定文件保存到本地的路径和名称。如果要把一个myhost.com网站的内容下载到本地的c:\test文件夹，
            //二进制文件的网上路径和名称是http://myhost.com/images/logo.gif，则本地路径和名称应当是c:\test\images\logo.gif.
            //同时要确保c:\test目录下已经创建了images子目录
            string filename = convertFilename(response.ResponseUri, 1);

            //确定了输出文件的名字和路径之后就可以打开读取Web页面的输入流、写入本地文件的输出流。
            Stream outStream = File.Create(filename);
            Stream inStream = response.GetResponseStream();

            //读取Web文件的内容并写入到本地文件
            int i;
            do
            {
                i = inStream.Read(buffer, 0, buffer.Length);
                if (i > 0) outStream.Write(buffer, 0, i);
            } while (i > 0);
            outStream.Close();
            inStream.Close();
        }

        //保存文本文件
        private void SaveTextFile(string buffer)
        {
            if (spider.Output == null) return;
            string filename = convertFilename(uri, 0);
            StreamWriter outStream = new StreamWriter(filename);
            outStream.Write(buffer);
            outStream.Close();
        }

        //下载一个页面
        private string getpage(Uri uri)
        {
            WebResponse response = null;
            Stream stream = null;
            StreamReader reader = null;
            try
            {
                // 创建http链接，HttpWebRequest用于获取和操作HTTP请求
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);//创建WebRequest对象
                response = request.GetResponse();//获取答复
                stream = response.GetResponseStream();//获取应答流
                //在执行其他处理之前，我们要先确定该文件是二进制文件还是文本文件，不同的文件类型处理方式也不同
                if (!response.ContentType.ToLower().StartsWith("text/"))//确定该文件是否为二进制文件
                {
                    //二进制文件的内容类型声明不以"text/"开头，蜘蛛程序直接把二进制文件保存到磁盘，不必进行额外的处理
                    //因为二进制文件不包含HTML，不会再有需要蜘蛛程序处理的HTML链接。
                    SaveBinaryFile(response);
                    return null;
                }
                string buffer = "", line;
                reader = new StreamReader(stream);
                while ((line = reader.ReadLine()) != null)
                    buffer += line + "\r\n";
                if (spider.Flag == 0) SaveTextFile(buffer);
                return buffer;
            }
            catch (WebException e)
            {
                System.Console.WriteLine("下载失败，错误：" + e);
                return null;
            }
            catch (IOException e)
            {
                System.Console.WriteLine("下载失败，错误：" + e);
                return null;
            }
            finally
            {
                if (reader != null) reader.Close();
                if (stream != null) stream.Close();
                if (response != null) response.Close();
            }
        }
        private void ProcessLink(string link)
        {
            Uri url;
            url = new Uri(uri, link);
            if (!url.Scheme.ToLower().Equals("http") && !url.Scheme.ToLower().Equals("https")) return;
            if (!url.Host.ToLower().Equals(uri.Host.ToLower())) return;
            spider.addURI(url);
        }
        private void ProcessPage(string page)
        {
            ParseHTML parse = new ParseHTML();
            parse.Source = page;//page为要解析的HTML文档
            while (!parse.eof())//利用循环来检查HTML文档包含的所有文本和标记
            {
                char ch = parse.Parse();
                //Parse方法将返回HTML文档包含的字符--它返回的内容只包含那些非HTML标记的字符，如果遇到了HTML标记，Parse方法将返回0值，表示现在遇到了一个HTML标记。
                //遇到一个标记之后，用GetTag()方法来处理它。
                if (ch == 0)
                {
                    Attribute a = parse.get()["HREF"];
                    if (a != null) ProcessLink(a.Value);//提取出HREF属性的值
                    a = parse.get()["SRC"];
                    if (a != null) ProcessLink(a.Value);//提取出SRC属性的值
                    if (spider.Flag == 1)
                    {
                        a = parse.get()["IMG"];
                        if (a != null && (a.Name.ToLower() == "src" || a.Name.ToLower() == "href"))
                        {
                            Uri url = new Uri(uri, a.Value);
                            if(spider.addIMG(url)) getpage(url);
                        }
                    }
                }
            }
        }
        public void Process()
        {
            while (!spider.Quit)
            {
                uri = spider.work();
                spider.Done.workerbegin();
                string page = getpage(uri);
                if (page != null) ProcessPage(page);
                spider.Done.workerend();
            }
        }
        public void start()
        {
            //启动一个线程
            ThreadStart tmp = new ThreadStart(this.Process);
            thread = new Thread(tmp);
            thread.Start();
        }
    }
}
