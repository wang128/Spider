using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider
{
    public class Attribute : ICloneable//对象的属性
    {
        private string tag;//标签
        private string name;//属性名
        private string value;//属性值
        public string Tag { get => tag; set => tag = value; }
        public string Name { get => name; set => name = value; }
        public string Value { get => value; set => this.value = value; }
        public Attribute() : this("", "", "") { }
        public Attribute(string tag, string name, string value)
        {
            Tag = tag;
            Name = name;
            Value = value;
        }
        public virtual object Clone()
        {
            return new Attribute(tag, name, value);
        }
    }
}
