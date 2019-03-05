using System;
using System.Collections.Generic;
using System.Text;

namespace Bam.Net.Data.Repositories
{
    public interface IRepositorySourceGenerator: ISourceGenerator
    {
        void GenerateRepositorySource();
    }
}
