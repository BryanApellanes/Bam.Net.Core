using Bam.Net;
using Bam.Net.Testing;
using Bam.Net.Testing.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Ion.Tests.UnitTests
{
    [Serializable]
    public class IonCollectionTests : CommandLineTool
    {
        [UnitTest]
        public async Task IonCollectionShouldContainValue()
        {
            string testValue = "test Value";
            When.A<IonCollection>("has a value added to it", ionCollection =>
            {
                ionCollection.Add(testValue);
            })
            .TheTest
            .ShouldPass((because, assertionProvider) =>
            {
                because.ItsTrue("the collection contains the expected value", assertionProvider.Value.Contains(testValue), "the collection did NOT contain the expected value");
            })
            .SoBeHappy()
            .UnlessItFailed();
        }

        [UnitTest]
        public async Task IonCollectionShouldContainGenericValue()
        {
            IonMember testValue = new IonMember { Name = "ion name", Value = "the value" };
            When.A<IonCollection<IonMember>>("has a value added to it", ionCollection =>
            {
                ionCollection.Add(testValue);
            })
            .TheTest
            .ShouldPass((because, assertionProvider) =>
            {
                because.ItsTrue("the collection contains the expected value", assertionProvider.Value.Contains(testValue), "the collection did NOT contain the expected value");
            })
            .SoBeHappy()
            .UnlessItFailed();
        }
    }
}
