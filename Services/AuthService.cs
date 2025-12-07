using CarRentalSystem.DTOs;
using CarRentalSystem.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Services
{
    public class AuthService
    {
        private readonly UserRepository _repo;

        public AuthService()
        {
            _repo = new UserRepository();
        }

        public LoginResultDto Login(LoginRequestDto request)
        {
            // Bạn có thể thêm validate nghiệp vụ ở đây
            return _repo.GetUser(request.Username, request.Password);
        }
    }
}
