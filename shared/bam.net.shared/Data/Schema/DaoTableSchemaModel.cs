using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Bam.Net.Data.Schema
{
    /// <summary>
    /// A model that represents the data necessary to render a Table into a Dao.
    /// </summary>
    public class DaoTableSchemaModel
    {
        public Table Model { get; set; }
        public SchemaDefinition Schema { get; set; }
        public string Namespace { get; set; }

        public string CamelCasedPluralizedClassName
        {
            get
            {
                return PluralizedClassName.CamelCase();
            }
        }

        public string PluralizedClassName
        {
            get
            {
                return Model.ClassName.Pluralize();
            }
        }

        public ForeignKeyColumnModel[] SuffixedForeignKeys
        {
            get
            {
                int i = 0;
                List<ForeignKeyColumnModel> results = new List<ForeignKeyColumnModel>();
                foreach(ForeignKeyColumn fk in Model.ForeignKeys)
                {
                    ForeignKeyColumn copy = fk.CopyAs<ForeignKeyColumn>();
                    copy.ReferenceNameSuffix = (++i).ToString();
                    results.Add(new ForeignKeyColumnModel(copy, Namespace));
                }
                return results.ToArray();
            }
        }

        public NonForeignKeyColumnModel[] NonForeignKeyColumns
        {
            get
            {
                return Model.Columns.Where(c => !(c is ForeignKeyColumn)).Select(c => new NonForeignKeyColumnModel(c)).ToArray();
            }
        }

        public ReferencingForeignKeyModel[] ReferencingForeignKeys
        {
            get
            {
                return Model.ReferencingForeignKeys.Select(rfk => new ReferencingForeignKeyModel(rfk)).ToArray();
            }
        }

        public XrefInfoModel[] LeftXrefs
        {
            get
            {
                return Schema.LeftXrefsFor(Model.Name).Select(x => new XrefInfoModel(x)).ToArray();
            }
        }

        public XrefInfoModel[] RightXrefs
        {
            get
            {
                return Schema.RightXrefsFor(Model.Name).Select(x => new XrefInfoModel(x)).ToArray();
            }
        }
    }
}
