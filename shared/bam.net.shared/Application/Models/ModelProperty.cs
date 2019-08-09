using Bam.Net.Data;

namespace Bam.Net.Application.Models
{
    public class ModelProperty
    {
        public string Name { get; set; }
        public DataTypes Type { get; set; }
        public bool Required { get; set; }
    }
}