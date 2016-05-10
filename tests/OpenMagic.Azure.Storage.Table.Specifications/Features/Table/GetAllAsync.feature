Feature: GetAllAsync

Scenario Outline: Table exists
	Given clean table <tableName> 
    And <countA> table entities with partition key 'a' 
    And <countB> table entities with partition key 'b' 
    When Table.GetAllAsync() is called
    Then <expectedCount> entities should be returned

    Examples:
        | tableName             | countA | countB | expectedCount |
        | gaSeveralRows         | 5      | 2      | 7             |
        # Azure's TableQuery.ExecuteSegmentedAsync(...) returns a maximum of 1,000
        # rows. Uncomment the following to test Table.FindByPartitionKeyAsync(...)
        # handles this fact.
        #
        # The following is usually commented because it is slow to run
        # | gaGreaterThan1000Rows | 2433   | 2      | 2435          |
