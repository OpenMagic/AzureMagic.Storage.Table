Feature: InsertAsync

Scenario: Insert single entity
	Given a cloud table
    And an table entity
    When cloudTable.InsertAsync(ITableEntity) is called
    Then the table entity is added to the cloud table

Scenario: Insert multiple entities
	Given a cloud table
    And multiple table entities
    When cloudTable.InsertAsync(IEnumerable<ITableEntity>) is called
    Then the table entities are added to the cloud table
