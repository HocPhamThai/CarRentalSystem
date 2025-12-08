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

        public void AddCar(CarDTO car)
        {
            Validate(car);

            if (car.Price <= 0)
                throw new Exception("Price must be greater than zero.");

            int affected = _repo.Add(car);
            if (affected <= 0)
                throw new Exception("Failed to add car.");
        }

        public void UpdateCar(CarDTO car)
        {
            if (car.CarId <= 0)
                throw new Exception("Car ID is invalid.");

            Validate(car);

            int affected = _repo.Update(car);
            if (affected <= 0)
                throw new Exception("Failed to update car.");
        }

        public void DeleteCar(int carId)
        {
            if (carId <= 0)
                throw new Exception("Invalid Car ID.");

            if (Utils.Utils.IsCarAvailableForBooking(carId))
                throw new Exception("Cannot delete a car that is currently booked.");

            int affected = _repo.Delete(carId);
            if (affected <= 0)
                throw new Exception("Failed to delete car.");
        }

        private void Validate(CarDTO car)
        {
            if (string.IsNullOrWhiteSpace(car.Brand))
                throw new Exception("Brand is required.");

            if (string.IsNullOrWhiteSpace(car.Model))
                throw new Exception("Model is required.");

            if (string.IsNullOrWhiteSpace(car.Category))
                throw new Exception("Category is required.");

            if (string.IsNullOrWhiteSpace(car.Available))
                throw new Exception("Availability is required.");
        }

    }
}
