using System;
using System.Collections.Generic;
using System.Text;

namespace Bam.Net.Data.Schema
{
    public class ReferencingForeignKeyModel
    {
        public ReferencingForeignKeyModel(ForeignKeyColumn foreignKey)
        {
            Model = foreignKey;
        }

        public ForeignKeyColumn Model { get; set; }
        
        public string PropertyName
        {
            get
            {
                return $"{Model.ReferencingClass.Pluralize()}By{Model.Name}";
            }
        }
    }
}
