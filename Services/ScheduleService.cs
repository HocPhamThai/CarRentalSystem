using CarRentalSystem.DTOs;
using CarRentalSystem.Repositories;
using System;
using System.Collections.Generic;

namespace CarRentalSystem.Services
{
    public class ScheduleService
    {
        private readonly ScheduleRepository _scheduleRepo;
        private readonly BookingRepository _bookingRepo;
        private readonly CustomerRepository _customerRepo;

        public ScheduleService()
        {
            _scheduleRepo = new ScheduleRepository();
            _bookingRepo = new BookingRepository();
            _customerRepo = new CustomerRepository();
        }

        public List<ScheduleViewDTO> GetSchedulesByCustomerId(int customerId)
        {
            if (customerId <= 0)
                throw new Exception("Invalid Customer ID.");

            return _scheduleRepo.GetSchedulesByCustomerId(customerId);
        }

        public ScheduleDTO GetScheduleById(int scheduleId)
        {
            if (scheduleId <= 0)
                throw new Exception("Invalid Schedule ID.");

            return _scheduleRepo.GetById(scheduleId);
        }

        public void UpdateSchedule(ScheduleDTO schedule)
        {
            if (schedule.ScheduleId <= 0)
                throw new Exception("Schedule ID is invalid.");

            ValidateSchedule(schedule);

            int affected = _scheduleRepo.Update(schedule);

            if (affected <= 0)
                throw new Exception("Failed to update schedule.");
        }

        public void CompleteScheduleWithPayment(int scheduleId, int bookingId, ScheduleDTO schedule, int totalCost)
        {
            if (scheduleId <= 0)
                throw new Exception("Invalid Schedule ID.");

            if (bookingId <= 0)
                throw new Exception("Invalid Booking ID.");

            if (totalCost < 0)
                throw new Exception("Total cost cannot be negative.");

            ValidateSchedule(schedule);

            int affected = _scheduleRepo.CompleteScheduleWithPayment(scheduleId, bookingId, schedule, totalCost);

            if (affected <= 0)
                throw new Exception("Failed to complete schedule payment.");
        }

        public void DeleteSchedule(int scheduleId)
        {
            if (scheduleId <= 0)
                throw new Exception("Invalid Schedule ID.");

            int affected = _scheduleRepo.Delete(scheduleId);

            if (affected <= 0)
                throw new Exception("Failed to delete schedule.");
        }

        public string GetCustomerName(int customerId)
        {
            if (customerId <= 0)
                throw new Exception("Invalid Customer ID.");

            return _customerRepo.GetCustomerName(customerId);
        }

        public System.Data.DataTable GetAllCustomerIds()
        {
            return _customerRepo.GetAllCustomerIds();
        }

        public (int delayDays, int fineCost, int totalCost) CalculatePayment(
            DateTime endDate, 
            DateTime returnDate, 
            int totalCarCost)
        {
            if (returnDate < DateTime.Now.Date)
                throw new Exception("Return date cannot be in the past.");

            TimeSpan delay = returnDate.Date - endDate.Date;
            int delayDays = (int)delay.TotalDays;

            int fineCost = 0;
            if (delayDays > 0)
            {
                fineCost = delayDays * 250; // $250 per day late fee
            }

            int totalCost = totalCarCost + fineCost;

            return (delayDays, fineCost, totalCost);
        }

        private void ValidateSchedule(ScheduleDTO schedule)
        {
            if (schedule.CarId <= 0)
                throw new Exception("Car ID is required.");

            if (schedule.BookingId <= 0)
                throw new Exception("Booking ID is required.");

            // Additional validation can be added here
        }
    }
}