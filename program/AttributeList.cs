using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider
{
    public class AttributeList:Attribute//保存所有对象的属性
    {
        public ArrayList list;
        public AttributeList()
        {
            list = new ArrayList();
        }
        public void Add(Attribute x)
        {
            list.Add(x);
        }
        public void Clear()
        {
            list.Clear();
        }
        public ArrayList List
        {
            get { return list; }
        }
        public Attribute this[int index]
        {
            get
            {
                if (index < list.Count)
                    return (Attribute)list[index];
                else return null;
            }
        }
        public Attribute this[string index]
        {
            get
            {
                int i = 0;
                while (this[i] != null)
                {
                    if (this[i].Name.ToLower().Equals((index.ToLower()))|| this[i].Tag.ToLower().Equals((index.ToLower())))
                        return this[i];
                    i++;
                }
                return null;
            }
        }
    }
}
