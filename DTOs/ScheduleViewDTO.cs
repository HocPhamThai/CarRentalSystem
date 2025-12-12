using System;

namespace CarRentalSystem.DTOs
{
    public class ScheduleViewDTO
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int CarID { get; set; }
        public string Status { get; set; }
        public string PickupLocation { get; set; }
        public string ReturnLocation { get; set; }
        public int TotalCarCost { get; set; }
        public int BookingID { get; set; }
        public int ScheduleID { get; set; }
    }
}