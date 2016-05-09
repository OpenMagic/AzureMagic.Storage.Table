using System;
using System.Collections.Generic;
using System.Linq;
using OpenMagic.Azure.Storage.Table.Specifications.Helpers;
using FluentAssertions;
using FluentAssertions.Equivalency;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using TechTalk.SpecFlow;

namespace OpenMagic.Azure.Storage.Table.Specifications.Steps.CloudTableExtensions
{
    [Binding]
    public class InsertAsyncSteps
    {
        private Given _given;

        public InsertAsyncSteps(Given given)
        {
            _given = given;
        }

        [Given(@"a clould table")]
        public void GivenAClouldTable()
        {
            _given.CloudTable = AzureTableProvider.GetTable(create: true);
        }

        [Given(@"an table entity")]
        public void GivenAnTableEntity()
        {
            _given.TableEntity = new TableEntity(Guid.NewGuid().ToString(), "");
        }

        [Given(@"multiple table entities")]
        public void GivenMultipleTableEntities()
        {
            _given.TableEntities = Enumerable
                .Range(1, 10)
                .Select(n => new TableEntity(Guid.NewGuid().ToString(), n.ToString()))
                .ToArray();
        }

        [When(@"cloudTable\.InsertAsync\(ITableEntity\) is called")]
        public void WhenCloudTable_InsertAsyncITableEntityIsCalled()
        {
            _given.CloudTable.InsertAsync(_given.TableEntity).Wait();
        }

        [When(@"cloudTable\.InsertAsync\(IEnumerable\<ITableEntity\>\) is called")]
        public void WhenCloudTable_InsertAsyncIEnumerableITableEntityIsCalled()
        {
            _given.CloudTable.InsertAsync(_given.TableEntities).Wait();
        }

        [Then(@"the table entity is added to the cloud table")]
        public void ThenTheTableEntityIsAddedToTheCloudTable()
        {
            var expected = _given.TableEntity;
            var actual = _given.CloudTable
                .CreateQuery<TableEntity>()
                .Where(t => t.PartitionKey == _given.TableEntity.PartitionKey)
                .Single();

            actual.ShouldBeEquivalentTo(expected, ConfigureEquivalencyAssertionOptions);
        }

        [Then(@"the table entities are added to the cloud table")]
        public void ThenTheTableEntitiesAreAddedToTheCloudTable()
        {
            var expected = _given.TableEntities.OrderBy(e => e.PartitionKey).ToArray();
            var actual = ReadTableEntitiesFromCloudTable().OrderBy(e => e.PartitionKey).ToArray();

            for (int i = 0; i < expected.Length; i++)
            {
                actual[i].ShouldBeEquivalentTo(
                    expected[i], 
                    ConfigureEquivalencyAssertionOptions);
            }
        }

        private IEnumerable<TableEntity> ReadTableEntitiesFromCloudTable()
        {
            var filters = _given.TableEntities
                .Select(t => TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, t.PartitionKey));

            var query = new TableQuery<TableEntity>()
                .Where(string.Join($" {TableOperators.Or} ", filters.Select(f => $"({f})")));

            return _given.CloudTable
                .ExecuteQuery(query);
        }

        private EquivalencyAssertionOptions<TableEntity> ConfigureEquivalencyAssertionOptions<TableEntity>(EquivalencyAssertionOptions<TableEntity> options)
        {
            return options
                .Excluding(ctx => ctx.SelectedMemberPath.StartsWith("Compiled"));
        }
    }
}
