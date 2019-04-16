using Bam.Net;

namespace Bam.Templates.Shell.Models
{
    public class DelegatorMethodModel
    {
        string _camelCasedMethodName;

        public string CamelCasedMethodName
        {
            get
            {
                if (string.IsNullOrEmpty(_camelCasedMethodName))
                {
                    _camelCasedMethodName = MethodName.CamelCase();
                }

                return _camelCasedMethodName;
            }
            set { _camelCasedMethodName = value; }
        }
        public string MethodName { get; set; }
        public string BaseTypeName { get; set; }
    }
}