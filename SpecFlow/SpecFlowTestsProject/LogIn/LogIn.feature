Feature: Log In
	In order to log in to the NxVMS Nx Cloud Systems dashboard
	As a user
	I want to be able to log in using assigned credentials
	And must not be able to log in with invalid credentials

@login @Valid
Scenario: Log in with valid credentials
	Given I visit the NxVMS landing page
	And I access the Log In form
	And I input "pauledwards@gmail.com" as username and "AQpuQ!Qp5Dt6Q1wn" as password
	And I set the remember checkbox to "true"
	When I click the Log In button
	Then the Systems page should load without error
	And the user can logout and return to the landing page

@login @invalid
Scenario Outline: Attempt log in with invalid credentials
	Given I visit the NxVMS landing page
	And I access the Log In form
	And I input "<Username>" as username and "<Password>" as password
	When I click the Log In button
	Then the Systems page should NOT load
Scenarios:
	| Username              | Password         |
	| pauledwards@gmail.com | invalid          |
	| garbage               | AQpuQ!Qp5Dt6Q1wn |