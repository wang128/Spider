using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spider
{
    public partial class MainForm : Form
    {
        private Spider spider;
        public MainForm()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;//允许从不是创建控件的线程访问控件
        }
        private void begin1_Click(object sender, EventArgs e)
        {
            //启动一个线程
            ThreadStart starter = new ThreadStart(this.SpiderThread1);
            Thread spider = new Thread(starter);
            spider.Start();
        }
        public void SpiderThread1()
        {
            if (begin1.Text.Equals("停止下载网址"))
            {
                spider.Quit = true;
                begin1.Enabled = false;
            }
            else
            {
                begin1.Text = "停止下载网址";
                begin2.Enabled = false;
                targetURL.Enabled = false;
                threadCount.Enabled = false;
                output.Enabled = false;
                spider = new Spider();
                spider.MainForm = this;
                spider.Output = output.Text;
                int threads = Convert.ToInt32(threadCount.Text);
                if (threads <= 1) threads = 2;
                threadCount.Text = threads.ToString();
                spider.Flag = 0;
                spider.Start(new Uri(this.targetURL.Text), threads);
                begin1.Text = "开始下载网址";
                begin1.Enabled = true;
                begin2.Enabled = true;
                targetURL.Enabled = true;
                threadCount.Enabled = true;
                output.Enabled = true;
            }
        }
        private void begin2_Click(object sender, EventArgs e)
        {
            //启动一个线程
            ThreadStart starter = new ThreadStart(this.SpiderThread2);
            Thread spider = new Thread(starter);
            spider.Start();
        }
        public void SpiderThread2()
        {
            if (begin2.Text.Equals("停止下载图片"))
            {
                spider.Quit = true;
                begin2.Enabled = false;
            }
            else
            {
                begin2.Text = "停止下载图片";
                begin1.Enabled = false;
                targetURL.Enabled = false;
                threadCount.Enabled = false;
                output.Enabled = false;
                spider = new Spider();
                spider.MainForm = this;
                spider.Output = output.Text;
                int threads = Convert.ToInt32(threadCount.Text);
                if (threads <= 1) threads = 2;
                threadCount.Text = threads.ToString();
                spider.Flag = 1;
                spider.Start(new Uri(this.targetURL.Text), threads);
                begin2.Text = "开始下载图片";
                begin2.Enabled = true;
                begin1.Enabled = true;
                targetURL.Enabled = true;
                threadCount.Enabled = true;
                output.Enabled = true;
            }
        }
        public void setlastURL(string str)
        {
            currentURL.Text = str;
        }
        public void settime(string str)
        {
            time.Text = str;
        }
        public void setprocessedCount(string str)
        {
            processedURLs.Text = str;
        }
    }
}