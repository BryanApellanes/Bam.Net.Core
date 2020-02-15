using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bam.Net.Application
{
    public class AppDataModel
    {
        public AppDataModel()
        {
            _properties = new HashSet<AppDataPropertyModel>();
        }

        public string BaseNamespace { get; set; }
        public string Name { get; set; }

        HashSet<AppDataPropertyModel> _properties;
        public AppDataPropertyModel[] Properties
        {
            get => _properties.ToArray();
            set
            {
                _properties = new HashSet<AppDataPropertyModel>();
                value.Each(p => _properties.Add(p));
            }
        }
    }
}
