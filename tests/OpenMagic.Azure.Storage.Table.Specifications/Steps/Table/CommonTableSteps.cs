using System.Linq;
using Anotar.LibLog;
using FluentAssertions;
using Microsoft.WindowsAzure.Storage.Table;
using OpenMagic.Azure.Storage.Table.Specifications.Helpers;
using TechTalk.SpecFlow;

namespace OpenMagic.Azure.Storage.Table.Specifications.Steps.Table
{
    [Binding]
    public class CommonTableSteps
    {
        private readonly Actual _actual;
        private readonly Given _given;

        public CommonTableSteps(Given given, Actual actual)
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

        [Then(@"(.*) entities should be returned")]
        public void ThenEntitiesShouldBeReturned(int expectedCount)
        {
            LogTo.Info($"Then '{expectedCount:N0}' entities should be returned");

            _actual.DummyEntities.Length.Should().Be(expectedCount);
        }
    }
}