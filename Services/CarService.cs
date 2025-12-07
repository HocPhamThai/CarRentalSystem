using CarRentalSystem.DTOs;
using CarRentalSystem.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Services
{
    public class CarService
    {
        private readonly CarRepository _repo;

        public CarService()
        {
            _repo = new CarRepository();
        }

        public List<CarDTO> GetAllCars()
        {
            return _repo.GetAll();
        }

        public List<CarDTO> SearchCars(string column, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return _repo.GetAll();

            return _repo.Search(column, text);
        }

        public string AddCar(CarDTO car)
        {
            // --- Validation business ---
            if (string.IsNullOrWhiteSpace(car.Brand) ||
                string.IsNullOrWhiteSpace(car.Model) ||
                string.IsNullOrWhiteSpace(car.Category) ||
                string.IsNullOrWhiteSpace(car.Available))
            {
                return "Missing required information.";
            }

            if (car.Price <= 0)
                return "Price must be greater than zero.";

            int affected = _repo.Add(car);
            return affected > 0 ? "Car added successfully" : "Failed to add car";
        }

        public string UpdateCar(CarDTO car)
        {
            if (car.CarId <= 0)
                return "Car ID is invalid.";

            if (car.Price <= 0)
                return "Price must be greater than zero.";

            int affected = _repo.Update(car);
            return affected > 0 ? "Car updated successfully" : "Update failed";
        }

        public string DeleteCar(int carId)
        {
            if (carId <= 0)
                return "Invalid Car ID";

            // - Check if car is currently booked
            // - Block delete if needed
            if (Utils.Utils.IsCarAvailableForBooking(carId))
            {
                return "Cannot delete car that is currently booked.";
            }

            int affected = _repo.Delete(carId);
            return affected > 0 ? "Car deleted successfully" : "Delete failed";
        }
    }
}
