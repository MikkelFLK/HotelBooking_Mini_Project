Feature: CreateBooking
	In order to Book a room
	As a Customer
	I want to be able create a booking 


Scenario Outline: Create a booking
	Given the first date is <startDate>
	And the second date <endDate>
	When it checks both startDate and endDate
	Then the result should create a booking or refuse 

	Examples: 
	| startDate    | endDate      |
	| '12-1-2021'  | '12-6-2021'  |
	| '11-10-2021' | '11-19-2021' |
	| '11-19-2021' | '12-1-2021'  |
	| '11-19-2021' | '11-20-2021' |
	| '11-19-2021' | '11-30-2021' |
	| '11-20-2021' | '12-1-2021'  |
	| '11-30-2021' | '12-1-2021'  |
	| '11-20-2021' | '11-21-2021' |
	| '11-29-2021' | '11-30-2021' |
	| '11-20-2021' | '11-30-2021' |

	