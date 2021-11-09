using System;
using System.Collections.Generic;
using Gherkin;
using HotelBooking.Core;
using Moq;
using TechTalk.SpecFlow;
using FluentAssertions;

namespace HotelBooking.Specs.Steps
{
    [Binding]
    public sealed class CreateBookingSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly BookingManager _bookingManager;
        private Mock<IRepository<Booking>> fakeBookingRepo;
        private Mock<IRepository<Room>> fakeRoomRepo;
        private DateTime startDate;
        private DateTime endDate;
        public CreateBookingSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            DateTime start = DateTime.Today.AddDays(10);
            DateTime end = DateTime.Today.AddDays(20);

            var room = new Room
            {
                Id = 1,
                Description = "A"
            };

            var customer = new Customer
            {
                Id = 13,
                Email = "casper@mail.com",
                Name = "Casper"
            };

            var booking = new Booking
            {
                Customer = customer,
                CustomerId = 13,
                Id = 32,
                EndDate = endDate,
                StartDate = startDate,
                IsActive = true,
                Room = room,
                RoomId = 1
            };

            fakeBookingRepo = new Mock<IRepository<Booking>>();
            fakeRoomRepo = new Mock<IRepository<Room>>();
            fakeRoomRepo.Setup(x => x.Get(1)).Returns(room);
            fakeBookingRepo.Setup(x => x.Get(32)).Returns(booking);
            
            _bookingManager = new BookingManager(fakeBookingRepo.Object, fakeRoomRepo.Object);
            _bookingManager.GetFullyOccupiedDates(start, end);

        }

        [Given(@"the first date is '(.*)'")]
        public void GivenTheFirstDateIs(string startDate)
        {
            this.startDate = DateTime.Parse(startDate);
        }

        [Given(@"the second date '(.*)'")]
        public void GivenTheSecondDate(string endDate)
        {
            this.endDate = DateTime.Parse(endDate);
        }

        [When(@"it checks both startDate and endDate")]
        public void WhenItChecksBothStartDateAndEndDate()
        {
            _bookingManager.FindAvailableRoom(this.startDate, this.endDate);
            _bookingManager.GetFullyOccupiedDates(this.startDate, this.endDate);
            System.Diagnostics.Debug.WriteLine(_bookingManager.GetFullyOccupiedDates(this.startDate, this.startDate));
        }
        
        [Then(@"the result should create a booking or refuse")]
        public void ThenTheResultShouldCreateABookingOrRefuse()
        {
            var room = new Room
            {
                Id = 1,
                Description = "A"
            };

            var customer = new Customer
            {
                Id = 13,
                Email = "casper@mail.com",
                Name = "Casper"
            };

            var newbooking = new Booking
            {
                Customer = customer,
                CustomerId = 13,
                EndDate = endDate,
                StartDate = startDate,
                IsActive = false,
                Room = room,
                RoomId = 1
            };
            var result = _bookingManager.CreateBooking(newbooking);

            if (result == true)
            {
                _bookingManager.CreateBooking(newbooking);
           
            }
        }
    }
}
