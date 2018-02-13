using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//分析字符串，解析字符串文档
namespace Spider
{
    public class Parse:AttributeList
    {
        private string source;//被解析的字符串
        private int pos;//被解析的字符的位置
        private string parsename;//被分析的属性名
        private string parsevalue;//被分析的属性值
        private char delim;//被分析的属性分隔符
        public string parsetag;//被分析的tag
        public string Source { get => source; set => source = value; }
        public int Pos { get => pos; set => pos = value; }
        public string Parsename { get => parsename; set => parsename = value; }
        public string Parsevalue { get => parsevalue; set => parsevalue = value; }
        public char Parsedelim { get => delim; set => delim = value; }
        public static bool isspace(char ch)//判断是否为空格
        {
            return ("\t\n\r ".IndexOf(ch) != -1);
        }
        public bool eof()//判断是否到了被分析的字符串尾
        {
            return (pos >= Source.Length);
        }
        public char getchar()
        {
            return getchar(0);
        }
        public char getchar(int k)
        {
            if ((pos + k) < Source.Length) return Source[pos + k];
            else return (char)0;
        }
        public void eatspace()
        {
            while (!eof())
            {
                if (!isspace(getchar())) return;
                pos++;
            }
        }
        public void ParseAttributeName()//分析属性名
        {
            eatspace();
            while (!eof())//获得属性名
            {
                if (isspace(getchar()) || (getchar() == '=') || (getchar() == '>')) break;
                parsename += getchar();
                pos++;
            }
            eatspace();
        }
        public void ParseAttributeValue()//分析属性值
        {
            if (delim != 0) return;
            if (getchar() == '=')
            {
                pos++;
                eatspace();
                if ((getchar() == '\'') || (getchar() == '\"'))
                {
                    delim = getchar();
                    pos++;
                    while (getchar() != delim)
                    {
                        parsevalue += getchar();
                        pos++;
                    }
                    pos++;
                }
                else
                {
                    while (!eof() && !isspace(getchar()) && (getchar() != '>'))
                    {
                        parsevalue += getchar();
                        pos++;
                    }
                }
                eatspace();
            }
        }
        public char advancechar()
        {
            return source[pos++];
        }
        public void advance()
        {
            pos++;
        }
        public void addAttribute()
        {
            Attribute x = new Attribute(parsetag, parsename, parsevalue);
            Add(x);
        }
    }
}
