using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eRestaurantSystem.DAL.POCOs
{
    public class SeatingSummary
    {
        public byte Table {get;set;} //0-255 in SQL datatype tiny
        public int Seating { get; set; }
        public bool Taken { get; set; }
        public int? BillID { get; set; }
        public decimal? BillTotal { get; set; }
        public string Waiter { get; set; }
        public string ReservationName { get; set; }
    }
}
