Feature: FindByPartitionKeyAsync

Scenario Outline: Partition key exists
	Given clean table <tableName> 
    And <countA> table entities with partition key 'a' 
    And <countB> table entities with partition key 'b' 
    When Table.FindByPartitionKeyAsync(<findPartitionKey>) is called
    Then <expectedCount> entities should be returned
    And they should have partition key <findPartitionKey>

    Examples:
    | tableName                | countA | countB | findPartitionKey | expectedCount |
    | SeveralRows              | 5      | 2      | a                | 5             |
    | PartitionKeyDoesNotExist | 5      | 2      | c                | 0             |
    # Azure's TableQuery.ExecuteSegmentedAsync(...) returns a maximum of 1,000
    # rows. Uncomment the following to test Table.FindByPartitionKeyAsync(...)
    # handles this fact.
    #
    # The following is usually commented because it is slow to run
    # | GreaterThan1000Rows | 2433   | 2      |
