Feature: CreateUser
	In order to create new users
	As a customer care specialist
	I want to create a new user profile

@SmokeTest
Scenario Outline: Create new user
	Given I populate the API call with "<FirstName>", "<LastName>", and "<Job>"
	When I make the API call to create a new user
	Then the call is successful
	Then the user profile is created

	Examples:
    | FirstName | LastName | Job |
    | John      | Tom      | QA  |
    | Jane      | Smith    | Dev | 
	#purpose for set of data Jane Smith Dev is to make the test fail just to assert that
