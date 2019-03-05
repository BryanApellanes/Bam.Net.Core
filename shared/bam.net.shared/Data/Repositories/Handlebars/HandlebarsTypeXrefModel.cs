using System;
using System.Collections.Generic;
using System.Text;

namespace Bam.Net.Data.Repositories.Handlebars
{
    public class HandlebarsTypeXrefModel: TypeXrefModel
    {
        public string XrefTableName
        {
            get
            {
                return string.Format("{0}.{1}{2}", DaoNamespace, Left.Name, Right.Name);
            }
        }

        public string CamelCasedRightNamePluralized
        {
            get
            {
                return RightNamePluralized.CamelCase();
            }
        }

        public string RightNamePluralized
        {
            get
            {
                return Right.Name.Pluralize();
            }
        }

        public string RightDaoNamePluralized
        {
            get
            {
                return RightDaoName.Pluralize();
            }
        }

        public string CamelCasedLeftNamePluralized
        {
            get
            {
                return LeftNamePluralized.CamelCase();
            }
        }

        public string LeftNamePluralized
        {
            get
            {
                return Left.Name.Pluralize();
            }
        }

        public string LeftDaoNamePluralized
        {
            get
            {
                return LeftDaoName.Pluralize();
            }
        }
    }
}
