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
    public class IonMemberTests : CommandLineTool
    {
        [UnitTest]
        public void IonMemberShouldSerializeAsExpected()
        {
            IonMember member = "hello";
            string expected = @"{
  ""value"": ""hello""
}";
            string memberJson = member.ToJson(true);
            Expect.AreEqual(expected, member.ToJson(true));
        }
    }
}
