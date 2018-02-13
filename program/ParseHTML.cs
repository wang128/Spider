using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//HTML解析，解析HTML文档
namespace Spider
{
    public class ParseHTML:Parse
    {
        public AttributeList get()
        {
            AttributeList tmp = new AttributeList();
            foreach (Attribute x in List)
            {
                tmp.Add((Attribute)x.Clone());
            }
            return tmp;
        }
        protected void ParseTag()
        {
            parsetag = "";
            Clear();
            if ((getchar() == '!') && (getchar(1) == '-') && (getchar(2) == '-'))
            {
                while (!eof())
                {
                    if ((getchar() == '-') && (getchar(1) == '-') && (getchar(2) == '>')) break;
                    if (getchar() != '\r') parsetag += getchar();
                    advance();
                }
                parsetag += "--";
                advance();
                advance();
                advance();
                Parsedelim = (char)0;
                return;
            }

            //找tag名字
            while (!eof())
            {
                if (isspace(getchar()) || (getchar() == '>')) break;
                parsetag += getchar();
                advance();
            }
            eatspace();

            //获得属性
            while (getchar() != '>')
            {
                Parsename = "";
                Parsevalue = "";
                Parsedelim = (char)0;
                //获得属性名
                ParseAttributeName();
                if (getchar() == '>')
                {
                    addAttribute();
                    break;
                }
                //获得属性值
                ParseAttributeValue();
                addAttribute();
            }
            advance();
        }
        public char Parse()
        {
            if (getchar() == '<')
            {
                advance();
                char ch = char.ToUpper(getchar());
                if ((ch >= 'A') && (ch <= 'Z') || (ch <= '!') || (ch == '/'))
                {
                    ParseTag();
                    return (char)0;
                }
                else return (advancechar());
            }
            else return (advancechar());
        }
    }
}
