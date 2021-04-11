using Bam.Net;
using Bam.Net.Testing.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Ion.Tests.UnitTests
{
    [Serializable]
    public class IonValueTests : CommandLineTool
    {
        [UnitTest]
        public async Task IonValueShouldSerializeAsExpected()
        {
            string json =
@"{
  ""firstName"": ""Bob"",
  ""lastName"": ""Smith"",
  ""birthDate"": ""1980-01-23""
}";

            IonCollection value = IonValue.Read(json);
            Expect.IsTrue(value.Value is List<IonMember>);
            string output = value.ToJson(true);
            Expect.AreEqual(json, output);
        }
    }
}
