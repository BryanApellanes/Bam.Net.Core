using Bam.Net.Services.DataReplication;

namespace Bam.Net.Data
{
    public class DaoId : QueryValue
    {
        public DaoId(object value) : base(value)
        {
            IdentifierName = "Id";
        }
        
        /// <summary>
        /// The name of the property or column that represents the Dao's identifier, this is typically "Id".
        /// </summary>
        public string IdentifierName { get; set; }

        public override object GetValue()
        {
            return base.GetValue(false);
        }
    }
}