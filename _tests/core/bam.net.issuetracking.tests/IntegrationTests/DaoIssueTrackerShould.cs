using Bam.Net.CommandLine;
using Bam.Net.CoreServices;
using Bam.Net.Data;
using Bam.Net.Data.SQLite;
using Bam.Net.IssueTracking.Data;
using Bam.Net.Logging;
using Bam.Net.Testing.Integration;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Net.IssueTracking.Tests
{
    [Serializable]
    public class DaoIssueTrackerShould : CommandLineTool
    {
        [ConsoleAction]
        [IntegrationTest]
        public async Task CreateIssue()
        {
            DaoIssueTracker daoIssueTracker = GetServiceRegistry($"{nameof(CreateIssue)}_Test").Get<DaoIssueTracker>();
            ITrackedIssue issueData = await daoIssueTracker.CreateIssueAsync("test Issue Id", "Test Issue Title", "Test Issue Body");
            Expect.IsNotNull(issueData, "issue was null");
        }

        private ServiceRegistry GetServiceRegistry(string testName, IServiceLevelAgreementProvider serviceLevelAgreementProvider = null)
        {
            serviceLevelAgreementProvider = serviceLevelAgreementProvider ?? GetMockServiceLevelAgreementProvider();
            return new ServiceRegistry()
                .For<Database>().Use(new SQLiteDatabase(new FileInfo($"./{testName}_Test.sqlite")))
                .For<ILogger>().Use<ConsoleLogger>()
                .For<IServiceLevelAgreementProvider>().Use(serviceLevelAgreementProvider);
        }

        private IServiceLevelAgreementProvider GetMockServiceLevelAgreementProvider(bool slaWasMet = true)
        {
            IServiceLevelAgreementProvider serviceLevelAgreementProvider = Substitute.For<IServiceLevelAgreementProvider>();
            serviceLevelAgreementProvider.SlaWasMet(Arg.Any<ITrackedIssue>()).Returns(slaWasMet);
            return serviceLevelAgreementProvider;
        }
    }
}
