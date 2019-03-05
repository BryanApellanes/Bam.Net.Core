using System.Reflection;

namespace Bam.Net.Data.Repositories
{
    public interface IWrapperGenerator
    {
        string DaoNamespace { get; set; }
        string InfoFileName { get; }
        TypeSchema TypeSchema { get; set; }
        string WrapperNamespace { get; set; }
        string WriteSourceTo { get; set; }

        void Generate(TypeSchema schema, string writeTo);
        GeneratedAssemblyInfo GenerateAssembly();
        Assembly GetGeneratedAssembly();
        void WriteSource(string writeSourceDir);
    }
}