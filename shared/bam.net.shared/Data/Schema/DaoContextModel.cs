using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bam.Net.Data.Schema
{
    public class DaoContextModel
    {
        public SchemaDefinition Model { get; set; }
        public string Namespace { get; set; }

        public DaoTableSchemaModel[] Tables
        {
            get
            {
                return Model.Tables.Select(t => new DaoTableSchemaModel { Model = t, Namespace = Namespace, Schema = Model }).ToArray();
            }
        }
    }
}
