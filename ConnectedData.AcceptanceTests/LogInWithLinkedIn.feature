Feature: LogInWithLinkedIn
	As an end uer
	I want to be able to login with my LinkedIn credentials
	so that the system can download my skills and profile
	

@mytag
Scenario: Login Button
	Given I am currently not logged in
	When I go to the login screen
	Then I should be presented a button to login with LinkedIn

Scenario: Register a new user
	Given I am currently not logged in
	And  I am not a current registered user
	When I go to the login screen
	And I press the login with LinkedIn button
	Then I should be registered into the system

