using System.Linq;
using Anotar.LibLog;
using FluentAssertions;
using OpenMagic.Azure.Storage.Table.Specifications.Helpers;
using OpenMagic.Azure.Storage.Table.Specifications.Helpers.Dummies;
using TechTalk.SpecFlow;

namespace OpenMagic.Azure.Storage.Table.Specifications.Steps.Table
{
    [Binding]
    public class FindByPartitionKeyAsyncSteps
    {
        private readonly Actual _actual;
        private readonly Given _given;

        public FindByPartitionKeyAsyncSteps(Given given, Actual actual)
        {
            _given = given;
            _actual = actual;
        }

        [When(@"Table\.FindByPartitionKeyAsync\((.*)\) is called")]
        public void WhenTable_FindByPartitionKeyAsyncIsCalled(string partitionKey)
        {
            LogTo.Info($"When Table.FindByPartitionKeyAsync('{partitionKey}') is called");

            var table = new Table<DummyEntity>(
                AzureTableProvider.ConnectionString,
                _given.CloudTable.Name,
                new DummyEntitySerializer());

            _actual.DummyEntities = table.FindByPartitionKeyAsync(partitionKey).Result.ToArray();
        }

        [Then(@"they should have partition key (.*)")]
        public void ThenTheyShouldHavePartitionKey(string expectedPartitionKey)
        {
            LogTo.Info($"And they should have partition key '{expectedPartitionKey}'");

            _actual.DummyEntities
                .Any(e => e.PartitionKey != expectedPartitionKey)
                .Should().BeFalse();
        }
    }
}