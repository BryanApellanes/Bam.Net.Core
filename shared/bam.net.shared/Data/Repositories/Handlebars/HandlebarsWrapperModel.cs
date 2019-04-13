using System;
using System.Linq;

namespace Bam.Net.Data.Repositories.Handlebars
{
    public class HandlebarsWrapperModel : WrapperModel
    {
        public HandlebarsWrapperModel(Type pocoType, TypeSchema schema, string wrapperNamespace = "TypeWrappers", string daoNameSpace = "Daos") : base(pocoType, schema, wrapperNamespace, daoNameSpace)
        {
            ForeignKeys = ForeignKeys.Select(fk => fk.CopyAs<HandlebarsTypeFkModel>()).ToArray();
            LeftXrefs = LeftXrefs.Select(lxref => lxref.CopyAs<HandlebarsTypeXrefModel>()).ToArray();
            RightXrefs = RightXrefs.Select(rx => rx.CopyAs<HandlebarsTypeXrefModel>()).ToArray();
        }
    }
}