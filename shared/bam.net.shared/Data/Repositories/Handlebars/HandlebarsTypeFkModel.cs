using System;
using System.Collections.Generic;
using System.Text;

namespace Bam.Net.Data.Repositories.Handlebars
{
    public class HandlebarsTypeFkModel: TypeFk
    {
        public string PrimaryKeyTypeCamelCaseName
        {
            get { return PrimaryKeyType.Name.CamelCase(); }
        }

        public string CollectionPropertyCamelCaseName
        {
            get
            {
                return CollectionProperty.Name.CamelCase();
            }
        }

        public string CollectionPropertyTypeString
        {
            get
            {
                return CollectionProperty.PropertyType.ToTypeString(true);
            }
        }

        public string CollectionPropertyTypeName
        {
            get
            {
                return CollectionProperty.PropertyType.IsArray ? "Array" : "List";
            }
        }
    }
}
