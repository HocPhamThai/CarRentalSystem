using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.DTOs
{
    public class LoginResultDto
    {
        public bool Success { get; set; }
        public int UserId { get; set; }
        public int Role { get; set; }
        public string ErrorMessage { get; set; }
    }
}
