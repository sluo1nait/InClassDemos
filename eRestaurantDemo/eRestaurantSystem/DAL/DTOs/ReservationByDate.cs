using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using System.Collections;
#endregion 

namespace eRestaurantSystem.DAL.DTOs
{
   public class ReservationByDate
    {
       public string Description { get; set; }
       //hold a set of rows; the next variable will hold a collection
       //of reservation rows
       public IEnumerable Reservations { get; set; } //collect of flat data
    }
}
