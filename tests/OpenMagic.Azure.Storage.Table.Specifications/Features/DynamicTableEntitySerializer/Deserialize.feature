Feature: Deserialize

Scenario: Happy path
	Given a DynamicTableEntity object for DummyEntityWithTypes
	When DynamicTableEntitySerializer.Deserialize(row) is called
    Then a DummyEntityWithTypes object should be returned
    And the DummyEntityWithTypes properties should be initialized from the DynamicTableEntity

@ignore @todo
Scenario: Missing properties
	Given todo

@ignore @todo
Scenario: Additional properties
	Given todo
