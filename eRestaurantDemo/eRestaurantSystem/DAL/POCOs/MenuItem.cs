using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
 
#endregion 
namespace eRestaurantSystem.DAL.POCOs
{
    public class MenuItem //define data container
    {
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int? CAlories { get; set; }
        public string Comment { get; set; }
    }
}
