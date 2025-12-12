using CarRentalSystem.DTOs;
using CarRentalSystem.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Services
{
    public class BookingService
    {
        private readonly BookingRepository _bookingRepo;
        private readonly FeatureRepository _featureRepo;
        private readonly FuelRepository _fuelRepo;
        private readonly CustomerRepository _customerRepo;
        private readonly CarRepository _carRepo;

        public BookingService() { 
            _bookingRepo = new BookingRepository();
            _featureRepo = new FeatureRepository();
            _fuelRepo = new FuelRepository();
            _customerRepo = new CustomerRepository();
            _carRepo = new CarRepository();
        }

        public List<BookingDTO> GetAllBookings()
        {
            return _bookingRepo.GetAll();
        }

        public BookingDTO GetBookingById(int bookingId)
        {
            if (bookingId <= 0)
                throw new Exception("Invalid Booking ID.");

            return _bookingRepo.GetById(bookingId);
        }

        public List<BookingDTO> SearchBookings(string column, string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return _bookingRepo.GetAll();

            return _bookingRepo.Search(column, keyword);
        }

        public int AddBooking(BookingDTO booking, ScheduleDTO schedule)
        {
            ValidateBooking(booking);
            ValidateSchedule(schedule);

            // Check car availability
            DateTime fromDate = DateTime.Parse(booking.FromDate);
            DateTime toDate = DateTime.Parse(booking.ToDate);

            if (!Utils.Utils.IsCarAvailableForBooking(Helper.AppConfigHelper.ConnectionString,
                fromDate, toDate, booking.CarId.ToString()))
            {
                throw new Exception("Car is not available for the selected dates.");
            }

            int bookingId = _bookingRepo.Add(booking, schedule);

            if (bookingId <= 0)
                throw new Exception("Failed to create booking.");

            return bookingId;
        }

        public void UpdateBooking(BookingDTO booking)
        {
            if (booking.BookingId <= 0)
                throw new Exception("Booking ID is invalid.");

            ValidateBooking(booking);

            int affected = _bookingRepo.Update(booking);

            if (affected <= 0)
                throw new Exception("Failed to update booking.");
        }

        public void DeleteBooking(int bookingId)
        {
            if (bookingId <= 0)
                throw new Exception("Invalid Booking ID.");

            int affected = _bookingRepo.Delete(bookingId);

            if (affected <= 0)
                throw new Exception("Failed to delete booking.");
        }
        public int GetFeaturePrice(string featureName)
        {
            if (string.IsNullOrWhiteSpace(featureName))
                return 0;

            return _featureRepo.GetFeaturePrice(featureName);
        }

        public int GetFuelPrice(string fuelName)
        {
            if (string.IsNullOrWhiteSpace(fuelName))
                return 0;

            return _fuelRepo.GetFuelPrice(fuelName);
        }

        public string GetCustomerName(int customerId)
        {
            if (customerId <= 0)
                throw new Exception("Invalid Customer ID.");

            return _customerRepo.GetCustomerName(customerId);
        }

        public DataTable GetAllCustomerIds()
        {
            return _customerRepo.GetAllCustomerIds();
        }

        public CarDTO GetCarById(int carId)
        {
            if (carId <= 0)
                throw new Exception("Invalid Car ID.");

            var cars = _carRepo.Search("carId", carId.ToString());
            return cars.Count > 0 ? cars[0] : null;
        }

        private void ValidateBooking(BookingDTO booking)
        {
            if (booking.CarId <= 0)
                throw new Exception("Car ID is required.");

            if (booking.CusId <= 0)
                throw new Exception("Customer ID is required.");

            if (string.IsNullOrWhiteSpace(booking.FromDate))
                throw new Exception("From Date is required.");

            if (string.IsNullOrWhiteSpace(booking.ToDate))
                throw new Exception("To Date is required.");

            DateTime fromDate = DateTime.Parse(booking.FromDate);
            DateTime toDate = DateTime.Parse(booking.ToDate);

            if (fromDate < DateTime.Now.Date)
                throw new Exception("From Date cannot be in the past.");

            if (toDate < fromDate)
                throw new Exception("To Date must be after From Date.");

            if (booking.TotalCost <= 0)
                throw new Exception("Total Cost must be greater than zero.");

            if (string.IsNullOrWhiteSpace(booking.Status))
                throw new Exception("Status is required.");
        }

        private void ValidateSchedule(ScheduleDTO schedule)
        {
            if (string.IsNullOrWhiteSpace(schedule.FromPlace))
                throw new Exception("From Place is required.");

            if (string.IsNullOrWhiteSpace(schedule.ToPlace))
                throw new Exception("To Place is required.");

            if (schedule.CarId <= 0)
                throw new Exception("Car ID is required for schedule.");
        }
    }
}
