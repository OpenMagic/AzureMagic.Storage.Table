Feature: Serialize

Scenario: Happy path
	Given a DummyEntityWithTypes object
	When DummyEntityWithTypesSerializer.Serialize(dummyEntityWithTypes) is called
    Then a DynamicTableEntity should be returned
	And DynamicTableEntity.PartitionKey should be DummyEntityWithTypes.StringValue
	And DynamicTableEntity.RowKey should be empty string
    And DynamicTableEntity.Properties should include all DummyEntityWithTypes properties except DummyEntityWithTypes.Name
