using CarRentalSystem.DTOs;
using CarRentalSystem.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Services
{
    public class UserService
    {
        private readonly UserRepository _repo;

        public UserService() 
        { 
            _repo = new UserRepository();
        }

        public List<UserDTO> GetAllUsers()
        {
            return _repo.GetAll();
        }

        public List<UserDTO> SearchUsers(string column, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return _repo.GetAll();

            return _repo.Search(column, text);
        }

        public void AddUser(UserDTO user)
        {
            Validate(user);

            int affected = _repo.Add(user);
            if (affected <= 0)
                throw new Exception("Failed to add user.");
        }

        public void UpdateUser(UserDTO user)
        {
            if (user.UserId <= 0)
                throw new Exception("User ID is invalid.");

            Validate(user);

            int affected = _repo.Update(user);
            if (affected <= 0)
                throw new Exception("Failed to update user.");
        }

        public void DeleteUser(int userId)
        {
            if (userId <= 0)
                throw new Exception("Invalid User ID.");

            int affected = _repo.Delete(userId);
            if (affected <= 0)
                throw new Exception("Failed to delete user.");
        }

        private void Validate(UserDTO user)
        {
            if (string.IsNullOrWhiteSpace(user.Username))
                throw new Exception("Username is required.");

            if (string.IsNullOrWhiteSpace(user.UserPassword))
                throw new Exception("Password is required.");

            if (user.UserPassword.Length < 6)
                throw new Exception("Password must be at least 6 characters.");

            if (user.Role < 0)
                throw new Exception("Role is required.");
        }
    }
}
