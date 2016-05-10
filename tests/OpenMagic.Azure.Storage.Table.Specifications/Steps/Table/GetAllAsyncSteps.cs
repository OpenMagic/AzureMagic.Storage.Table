using System.Linq;
using Anotar.LibLog;
using OpenMagic.Azure.Storage.Table.Specifications.Helpers;
using OpenMagic.Azure.Storage.Table.Specifications.Helpers.Dummies;
using TechTalk.SpecFlow;

namespace OpenMagic.Azure.Storage.Table.Specifications.Steps.Table
{
    [Binding]
    public class GetAllAsyncSteps
    {
        private readonly Actual _actual;
        private readonly Given _given;

        public GetAllAsyncSteps(Given given, Actual actual)
        {
            _given = given;
            _actual = actual;
        }

        [When(@"Table\.GetAllAsync\(\) is called")]
        public void WhenTable_GetAllAsyncIsCalled()
        {
            LogTo.Info("When Table.GetAllAsync() is called");

            var table = new Table<DummyEntity>(
                AzureTableProvider.ConnectionString,
                _given.CloudTable.Name,
                new DummyEntitySerializer());

            _actual.DummyEntities = table.GetAllAsync().Result.ToArray();
        }
    }
}