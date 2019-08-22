using Bam.Net.CoreServices.ApplicationRegistration.Data;
using Bam.Net.CoreServices.ApplicationRegistration.Data.Dao.Repository;
using Bam.Net.Data.Repositories;
using Bam.Net.Server;
using Bam.Net.Services.Catalog.Data;
using Bam.Net.Testing;
using Bam.Net.Testing.Unit;
using NSubstitute;

namespace Bam.Net.Services.Tests
{
    public class CatalogTests : CommandLineTestInterface
    {
        [UnitTest]
        [TestGroup("Catalog")]
        public void CanSaveCatalog()
        {
            DaoRepository repo = new DaoRepository();
            repo.AddType<CatalogDefinition>();
            Organization org = Organization.Public;
            string catalogName = "MyTestCatalog";
            CatalogDefinition catalogDefinition = new CatalogDefinition(){OrganizationKey = org.Key, Name = catalogName};
            CatalogDefinition saved = catalogDefinition.Save<CatalogDefinition>(repo);
            
            Expect.AreEqual(saved.Key, catalogDefinition.Key);
            Expect.AreEqual(saved, catalogDefinition);
        }

        [UnitTest]
        [TestGroup("AdHoc")]
        public void CanCreateCatalog()
        {
            IRepository catalogRepo = new DaoRepository();
            DaoRepository daoRepository = new DaoRepository();
            ApplicationRegistrationRepository applicationRegistrationRepository = new ApplicationRegistrationRepository();
            AsyncCallbackService asyncCallbackService = Substitute.For<AsyncCallbackService>();
            CatalogService svc = new CatalogService(catalogRepo, asyncCallbackService, daoRepository, new AppConf())
            {
                ApplicationRegistrationRepository = applicationRegistrationRepository
            };

            string testCatalogName = "TestCatalog";
            CatalogDefinition catalogDefinition = svc.CreateCatalog(testCatalogName);
            Expect.IsNotNull(catalogDefinition);
            Expect.IsGreaterThan(catalogDefinition.Id, 0);
            Expect.IsGreaterThan(catalogDefinition.Key, 0);
        }
    }
}