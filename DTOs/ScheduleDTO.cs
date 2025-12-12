using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.DTOs
{
    public class ScheduleDTO
    {
        public int ScheduleId { get; set; }
        public string FromPlace { get; set; }
        public string ToPlace { get; set; }
        public string DateDelay { get; set; }
        public string DateReturn { get; set; }
        public int? FineCost { get; set; }
        public int? TotalCost { get; set; }
        public int BookingId { get; set; }
        public int CarId { get; set; }
    }
}
