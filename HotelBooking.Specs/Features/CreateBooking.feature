Feature: CreateBooking
	In order to Book a room
	As a Customer
	I want to be able create a booking 

@DataSource:
Scenario Outline: Create a booking
	Given the first date is <startDate>
	And the second date is <endDate>
	When it checks both startDate and endDate
	Then the result should create a booking or refuse 

	Examples: 
	| startDate		| endDate		|
	| 1/1/2012		| 31/1/2012		|
	| 1/1/2012		| 31/1/2012		|
	| 1/1/2012		| 31/1/2012		|
	| 1/1/2012		| 31/1/2012		| 
	| 1/1/2012		| 31/1/2012		|
	| 1/1/2012		| 31/1/2012		| 
	| 1/1/2012		| 31/1/2012		|
	| 1/1/2012		| 31/1/2012		| 
	| 1/1/2012		| 31/1/2012		|
	| 1/1/2012		| 31/1/2012		|