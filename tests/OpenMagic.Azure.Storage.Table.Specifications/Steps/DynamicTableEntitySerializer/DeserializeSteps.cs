using FluentAssertions;
using OpenMagic.Azure.Storage.Table.Specifications.Helpers;
using OpenMagic.Azure.Storage.Table.Specifications.Helpers.Dummies;
using TechTalk.SpecFlow;

namespace OpenMagic.Azure.Storage.Table.Specifications.Steps.DynamicTableEntitySerializer
{
    [Binding]
    public class DeserializeSteps
    {
        private readonly Actual _actual;
        private readonly DummyFactory _dummy;
        private readonly Given _given;
        private readonly IDynamicTableEntitySerializer<DummyEntityWithTypes> _serializer;

        public DeserializeSteps(Given given, Actual actual, DummyFactory dummy)
        {
            _given = given;
            _actual = actual;
            _dummy = dummy;
            _serializer = new DynamicTableEntitySerializer<DummyEntityWithTypes>(
                (partitionKey, rowKey) =>
                {
                    var entity = _dummy.Object<DummyEntityWithTypes>();
                    entity.StringValue = partitionKey;

                    return entity;
                },
                entity => entity.StringValue,
                entity => "",
                new[] {nameof(DummyEntityWithTypes.StringValue)});
        }

        [Given(@"a DynamicTableEntity object for DummyEntityWithTypes")]
        public void GivenADynamicTableEntityObjectForDummyEntityWithTypes()
        {
            _given.DummyEntityWithTypes = _dummy.Object<DummyEntityWithTypes>();
            _given.DynamicTableEntity = _serializer.Serialize(_given.DummyEntityWithTypes);
        }

        [When(@"DynamicTableEntitySerializer\.Deserialize\(row\) is called")]
        public void WhenDynamicTableEntitySerializer_DeserializeRowIsCalled()
        {
            _actual.DummyEntityWithTypes = _serializer.Deserialize(_given.DynamicTableEntity);
        }

        [Then(@"a DummyEntityWithTypes object should be returned")]
        public void ThenADummyEntityWithTypesObjectShouldBeReturned()
        {
            _actual.DummyEntityWithTypes.Should().NotBeNull();
        }

        [Then(@"the DummyEntityWithTypes properties should be initialized from the DynamicTableEntity")]
        public void ThenTheDummyEntityWithTypesPropertiesShouldBeInitializedFromTheDynamicTableEntity()
        {
            _actual.DummyEntityWithTypes.ShouldBeEquivalentTo(_given.DummyEntityWithTypes);
        }
    }
}