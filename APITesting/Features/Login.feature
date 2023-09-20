Feature: LoginUser
		Feature: User Login
		In order to access the system
		As a user
		I want to login with valid/invalid credentials

@SmokeTest @HappyFlow
Scenario: Successful User Login
Given I have the following login details:
| email | password |
| eve.holt@reqres.in | cityslicka |
When I make a POST request to "/api/login" with the login details
Then the response status code should be 200
Then the response body should contain the token

@SmokeTest @NegativeFlow
Scenario: Unsuccessful user login
    Given I have the following invalid login details:
      | email          | password |
      | peter@klaven   |          |
    When I make a POST request to "/api/login" with the invalid login details
    Then the response status code should be 400
    And the response body should contain the error message: "Missing password"

