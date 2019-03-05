using System;
using System.Collections.Generic;
using System.Text;

namespace Bam.Net.Application
{
    public class AppDataPropertyModel
    {
        public bool Key { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if(obj is AppDataPropertyModel data)
            {
                return data.Type.Equals(Type) && data.Name.Equals(Name);
            }
            return base.Equals(obj);
        }
    }
}
