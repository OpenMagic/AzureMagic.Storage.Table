using System;
using System.Linq;
using Anotar.LibLog;
using FluentAssertions;
using Microsoft.WindowsAzure.Storage.Table;
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

        [Given(@"clean table (.*)")]
        public void GivenCleanTable(string tableName)
        {
            LogTo.Info($"Given clean table '{tableName}'");

            _given.CloudTable = AzureTableProvider.GetTable(AzureTableNamer.GetTableName(tableName), true);
        }

        [Given(@"(.*) table entities with partition key '(.*)'")]
        public void GivenTableEntitiesWithPartitionKey(int entityCount, string partitionKey)
        {
            LogTo.Info($"And '{entityCount:N0}' table entities with partition key '{partitionKey}'");

            _given.TableEntities = Enumerable.Range(1, entityCount).Select(n => new TableEntity(partitionKey, n.ToString("N5"))).ToArray();
            _given.CloudTable.InsertAsync(_given.TableEntities).Wait();
        }

        [Given(@"todo")]
        public void GivenTodo()
        {
            throw new NotImplementedException("todo: implement step");
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

        [Then(@"(.*) entities should be returned")]
        public void ThenEntitiesShouldBeReturned(int expectedCount)
        {
            LogTo.Info($"Then '{expectedCount:N0}' entities should be returned");

            _actual.DummyEntities.Length.Should().Be(expectedCount);
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