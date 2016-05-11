using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Newtonsoft.Json;
using OpenMagic.Azure.Storage.Table.Specifications.Helpers;
using OpenMagic.Azure.Storage.Table.Specifications.Helpers.Dummies;
using TechTalk.SpecFlow;

namespace OpenMagic.Azure.Storage.Table.Specifications.Steps.DynamicTableEntitySerializer
{
    [Binding]
    public class SerializeSteps
    {
        private readonly Actual _actual;
        private readonly DummyFactory _dummy;
        private readonly Given _given;
        private readonly DynamicTableEntitySerializer<DummyEntityWithTypes> _serializer;

        public SerializeSteps(Given given, Actual actual, DummyFactory dummy)
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

        [Given(@"a DummyEntityWithTypes object")]
        public void GivenADummyEntityWithTypesObject()
        {
            _given.DummyEntityWithTypes = _dummy.Object<DummyEntityWithTypes>();
        }

        [When(@"DummyEntityWithTypesSerializer\.Serialize\(dummyEntityWithTypes\) is called")]
        public void WhenDummyEntityWithTypesSerializer_SerializeDummyEntityWithTypesIsCalled()
        {
            _actual.DynamicTableEntity = _serializer.Serialize(_given.DummyEntityWithTypes);
        }

        [Then(@"a DynamicTableEntity should be returned")]
        public void ThenADynamicTableEntityShouldBeReturned()
        {
            _actual.DynamicTableEntity.Should().NotBeNull();
        }

        [Then(@"DynamicTableEntity\.PartitionKey should be DummyEntityWithTypes\.StringValue")]
        public void ThenDynamicTableEntity_PartitionKeyShouldBeDummyEntityWithTypes_StringValue()
        {
            _actual.DynamicTableEntity.PartitionKey
                .Should().Be(_given.DummyEntityWithTypes.StringValue);
        }

        [Then(@"DynamicTableEntity\.RowKey should be empty string")]
        public void ThenDynamicTableEntity_RowKeyShouldBeEmptyString()
        {
            _actual.DynamicTableEntity.RowKey
                .Should().BeEmpty();
        }

        [Then(@"DynamicTableEntity\.Properties should include all DummyEntityWithTypes properties except DummyEntityWithTypes\.Name")]
        public void ThenDynamicTableEntity_PropertiesShouldIncludeAllDummyEntityWithTypesPropertiesExceptDummyEntityWithTypes_Name()
        {
            var ignoreProperties = new[] {"StringValue", "EnumValue", "ListValue", "ClassValue"};
            var properties = _given.DummyEntityWithTypes.GetType().GetProperties().Where(p => !ignoreProperties.Contains(p.Name));
            var expectedValues = properties.Select(p => new KeyValuePair<string, object>(p.Name, p.GetValue(_given.DummyEntityWithTypes)));
            var dynamicTableEntity = _actual.DynamicTableEntity;
            var actualValues = dynamicTableEntity.Properties.Where(p => !ignoreProperties.Contains(p.Key)).Select(p => new KeyValuePair<string, object>(p.Key, p.Value.PropertyAsObject));

            actualValues.ShouldAllBeEquivalentTo(expectedValues);

            dynamicTableEntity.Properties["EnumValue"].StringValue.Should().Be(_given.DummyEntityWithTypes.EnumValue.ToString());
            JsonConvert.DeserializeObject<List<DummyEntity>>(dynamicTableEntity.Properties["ListValue"].StringValue).ShouldAllBeEquivalentTo(_given.DummyEntityWithTypes.ListValue);
            JsonConvert.DeserializeObject<DummyEntity>(dynamicTableEntity.Properties["ClassValue"].StringValue).ShouldBeEquivalentTo(_given.DummyEntityWithTypes.ClassValue);
        }
    }
}