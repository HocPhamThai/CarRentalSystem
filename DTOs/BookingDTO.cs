using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.DTOs
{
    public class BookingDTO
    {
        public int BookingId { get; set; }
        public int CarId { get; set; }
        public int CusId { get; set; }
        public string CusName { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public int TotalCost { get; set; }
    }
}
