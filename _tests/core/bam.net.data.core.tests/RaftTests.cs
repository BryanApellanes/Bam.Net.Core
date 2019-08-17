using System;
using Bam.Net.Data.Repositories;
using Bam.Net.Services.DataReplication.Data;
using Bam.Net.Testing;
using Bam.Net.Testing.Unit;

namespace Bam.Net.Data.Tests
{
    public class RaftTestData : KeyedAuditRepoData
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Dob { get; set; }
        public int Age { get; set; }
    }
    
    public class RaftTests : CommandLineTestInterface
    {
        [UnitTest]
        [TestGroup("Raft")]
        public void CanCreateSaveOperation()
        {
            RaftTestData testObject = new RaftTestData()
            {
                FirstName = "TestFirst",
                LastName = "Test Last",
                Dob = new DateTime(1976, 1, 1)
            };
            SaveOperation operation = SaveOperation.For(testObject);
            Expect.AreEqual(4, operation.Properties.Count);
            foreach (DataProperty prop in operation.Properties)
            {
                OutLine(prop.ToJson(true), ConsoleColor.Yellow);
            }
        }
        
/*        [UnitTest]
        [TestGroup("Raft")]
        public void Can*/
    }
}