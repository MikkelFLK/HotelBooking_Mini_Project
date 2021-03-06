using System;
using System.Collections.Generic;
using HotelBooking.Core;
using HotelBooking.Infrastructure.Repositories;
using HotelBooking.UnitTests.Fakes;
using Moq;
using Xunit;

namespace HotelBooking.UnitTests
{
    public class BookingManagerTests
    {
        private BookingManager bookingManager;
        private Mock<IRepository<Booking>> fakeBookingRepo;
        private Mock<IRepository<Room>> fakeRoomRepo;


        public BookingManagerTests(){
            DateTime start = DateTime.Today.AddDays(10);
            DateTime end = DateTime.Today.AddDays(20);
            
            fakeBookingRepo = new Mock<IRepository<Booking>>();
            fakeRoomRepo = new Mock<IRepository<Room>>();
            bookingManager = new BookingManager(fakeBookingRepo.Object, fakeRoomRepo.Object);
        }

        [Fact]
        public void FindAvailableRoom_StartDateNotInTheFuture_ThrowsArgumentException()
        {
            
            //Arrange
            DateTime date = DateTime.Today.AddDays(-1);

            // Assert
            Assert.Throws<ArgumentException>(() => bookingManager.FindAvailableRoom(date, date));

        }

        [Fact]
        public void FindAvailableRoom_StartDateOnToday_ThrowsArgumentException()
        {
            //Arrange
            DateTime date = DateTime.Today;

            // Assert
            Assert.Throws<ArgumentException>(() => bookingManager.FindAvailableRoom(date, date));

        }

        [Fact]
        public void FindAvailableRoom_StartDateInTheFuture()
        {
            //Arrange
            DateTime startDate = DateTime.Today.AddDays(10);
            DateTime endDate = DateTime.Today.AddDays(20);

            var result = bookingManager.FindAvailableRoom(startDate, endDate);
            // Assert
            Assert.True(true, result.ToString());

        }

        [Fact]
        public void FindAvailableRoom_RoomOccupiedDate()
        {
            //Arrange
            DateTime startDate = DateTime.Today.AddDays(10);
            DateTime endDate = DateTime.Today.AddDays(20);
            var room = new Room
            {
                Id=1, 
                Description="A"
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
            //Act
            fakeRoomRepo.Setup(x => x.Get(1)).Returns(room);
            fakeBookingRepo.Setup(x => x.Get(32)).Returns(booking);
            bookingManager.GetFullyOccupiedDates(startDate, endDate);

            var result = bookingManager.FindAvailableRoom(startDate, endDate);

            //Assert
            Assert.False(false, result.ToString());
        }

        [Fact]
        public void FindAvailableRoom_RooomAvailable_RoomAvaibableAfterOccupiedTime()
        {
            //Arrange
            DateTime occupiedStartDate = DateTime.Today.AddDays(10);
            DateTime occupiedEndDate = DateTime.Today.AddDays(20);
            DateTime startDate = DateTime.Today.AddDays(21);
            DateTime endDate = DateTime.Today.AddDays(30);
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
                IsActive = false,
                Room = room,
                RoomId = 1
            };
            //Act
            fakeRoomRepo.Setup(x => x.Get(1)).Returns(room);
            fakeBookingRepo.Setup(x => x.Get(32)).Returns(booking);
            bookingManager.GetFullyOccupiedDates(occupiedStartDate, occupiedEndDate);

            var result = bookingManager.FindAvailableRoom(startDate, endDate);

            Assert.True(true, result.ToString());
        }

        [Fact]
        public void FindAvailableRoom_RoomAvailable_RoomIdNotMinusOne()
        {
            // Arrange
            var rooms = new List<Room>
            {
                new Room { Id=1, Description="A" },
                new Room { Id=2, Description="B" },
            };
            fakeRoomRepo.Setup(x => x.GetAll()).Returns(rooms);
            DateTime date = DateTime.Today.AddDays(1);
            // Act
            int roomId = bookingManager.FindAvailableRoom(date, date);
            // Assert
            Assert.NotEqual(-1, roomId);
        }


        [Fact]
        public void CreateBooking_CreatesABooking()
        {

            //Arrange
            DateTime startDate = DateTime.Today.AddDays(21);
            DateTime endDate = DateTime.Today.AddDays(30);
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
                IsActive = false,
                Room = room,
                RoomId = 1
            };

            //Act
            var result = bookingManager.CreateBooking(booking);

            //Assert
            Assert.True(true, result.ToString());
        }

        [Fact]
        public void CreateBooking_InThePast()
        {
            //Arrange
            DateTime date = DateTime.Today.AddDays(-1);
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
                EndDate = date,
                StartDate = date,
                IsActive = false,
                Room = room,
                RoomId = 1
            };

            //Assert
            Assert.Throws<ArgumentException>(() => bookingManager.CreateBooking(booking));
        }

        [Fact]
        public void CreateBooking_InTheFuture()
        {
            //Arrange
            DateTime startDate = DateTime.Today.AddDays(7);
            DateTime endDate = DateTime.Today.AddDays(10);
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
                IsActive = false,
                Room = room,
                RoomId = 1
            };

            //Act
            var result = bookingManager.CreateBooking(booking);

            //Arrange
            Assert.True(true, result.ToString());
        }

        [Fact]
        public void CreateBooking_OccupiedBookingDates()
        {
            //Arrange
            DateTime startDate = DateTime.Today.AddDays(10);
            DateTime endDate = DateTime.Today.AddDays(20);
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
            //Act
            fakeRoomRepo.Setup(x => x.Get(1)).Returns(room);
            fakeBookingRepo.Setup(x => x.Get(32)).Returns(booking);
            bookingManager.GetFullyOccupiedDates(startDate, endDate);

            var result = bookingManager.CreateBooking(booking);

            //Assert
            Assert.False(false, result.ToString());
        }

        [Fact]
        public void GetOccupiedDates_GetOccupiedDateList()
        {
            DateTime startDate = DateTime.Today.AddDays(2);
            DateTime endDate = DateTime.Today.AddDays(5);
            //Arrange
            var rooms = new List<Room>
            {
                new Room
                {
                    Id = 1,
                    Description = "A"
                },
                new Room
                {
                    Id = 2,
                    Description = "B"
                }
            };

            var customers = new List<Customer>
            {
                new Customer
                {
                    Id = 13,
                    Email = "casper@mail.com",
                    Name = "Casper"
                },
                new Customer
                {
                    Id = 12,
                    Email = "Pierre@mail.com",
                    Name = "Pierre"
                }
            };

            var bookings = new List<Booking>
            {
                new Booking
                {
                    CustomerId = 13,
                    Id = 32,
                    EndDate = endDate,
                    StartDate = startDate,
                    IsActive = true,
                    RoomId = 1
                },
                new Booking
                {
                    CustomerId = 12,
                    Id = 33,
                    EndDate = endDate,
                    StartDate = startDate,
                    IsActive = true,
                    RoomId = 2
                }
            };

            var dates = new List<DateTime>
            {
                DateTime.Today.AddDays(2),
                DateTime.Today.AddDays(3),
                DateTime.Today.AddDays(4),
                DateTime.Today.AddDays(5)
            };
            //Act
            fakeRoomRepo.Setup(x => x.GetAll()).Returns(rooms);
            fakeBookingRepo.Setup(x => x.GetAll()).Returns(bookings);

            var result = bookingManager.GetFullyOccupiedDates(startDate, endDate);

            //Assert
            Assert.Equal(dates, result);
        }
    }
}
