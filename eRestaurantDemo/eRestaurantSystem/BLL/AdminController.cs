using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using eRestaurantSystem.DAL;//context
using eRestaurantSystem.DAL.Entites;
using eRestaurantSystem.DAL.DTOs;
using eRestaurantSystem.DAL.POCOs;
//using System.ComponentModel.DataAnnotations;//used for entity
using System.ComponentModel;//Object Data Source
using System.Data.Entity;
#endregion 

namespace eRestaurantSystem.BLL
{
    [DataObject] //required for ODS
    public class AdminController  //something call from outside? If so, then yes.
    {
        #region Queries
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<SpecialEvent> SpecialEvents_List()
        {
            //connect to our DbContext class in the DAL
            //create an instance of the class
            //we will use a transaction to hold our query
            using (var context = new eRestaurantContext())  //create a new instance 'using' is a transaction
            {
                //method syntax
                //return context.SpecialEvents.OrderBy(x => x.Description).ToList();//convert to list so can be sent back


                //query syntax
                var results = from item in context.SpecialEvents
                              orderby item.Description
                              select item;
                return results.ToList();
            }
        }

        //Don't forget to build it, then the webpage can find it. 
        //Clean build library before try to use on webpage;

        //get data
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<Reservation> GetReservationsByEventCode(string eventcode)
        {

            using (var context = new eRestaurantContext())  //create a new instance 'using' is a transaction
            {

                //query syntax
                var results = from item in context.Reservations
                              where item.EventCode.Equals(eventcode)
                              orderby item.CustomerName, item.ReservationDate
                              select item;
                return results.ToList();
            }

        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<ReservationByDate> GetReservationsByDate(string reservationdate)
        {
            using (var context = new eRestaurantContext())
            {
                //Linq is not very playful or cooperative with
                //DateTime

                //extract the year, month and day ourselves out
                //of the passed parameter value//
                int theYear = (DateTime.Parse(reservationdate)).Year;
                int theMonth = (DateTime.Parse(reservationdate)).Month;
                int theDay = (DateTime.Parse(reservationdate)).Day;

                var results = from eventitem in context.SpecialEvents
                              orderby eventitem.Description
                              select new ReservationByDate() //a new instance for each specialevent row on the table, build a DTO for eventCodeA, then b, then c...
                              {
                                  Description = eventitem.Description,
                                  Reservations = from row in eventitem.Reservations//nested here,
                                                 //only need entity belongs to that row
                                                 where row.ReservationDate.Year == theYear
                                                 && row.ReservationDate.Month == theMonth
                                                 && row.ReservationDate.Day == theDay
                                                 select new ReservationDetail() //a new for each reservation of a particular event, looping to get each corresponding reservation
                                                 {
                                                     CustomerName = row.CustomerName,
                                                     ReservationDate = row.ReservationDate,
                                                     NumberInParty = row.NumberInParty,
                                                     ContactPhone = row.ContactPhone,
                                                     ReservationStatus = row.ReservationStatus
                                                 }
                              };
                return results.ToList();

            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<MenuCategoryItems> MenuCategoryItems_List()
        {
            using (var context = new eRestaurantContext())
            {

                var results = from menuitem in context.MenuCategories
                              orderby menuitem.Description
                              select new MenuCategoryItems() //a new instance for each specialevent row on the table, build a DTO for eventCodeA, then b, then c...
                              {
                                  Description = menuitem.Description,
                                  MenuItems = from row in menuitem.MenuItems//nested here,

                                              select new MenuItem()
                                              {
                                                  Description = row.Description,
                                                  Price = row.CurrentPrice,
                                                  Calories = row.Calories,
                                                  Comment = row.Comment,

                                              }
                              };
                return results.ToList();
            }
        }
        #endregion
        #region Add, Update, Delete of CRUD for CQRS
        [DataObjectMethod(DataObjectMethodType.Insert, false)]
        public void SpecialEvents_Add(SpecialEvent item)
        {
            using (eRestaurantContext context = new eRestaurantContext())
            {
                //these methods are execute using an instance level item
                //set up an instance pointer and initialize to null
                SpecialEvent added = null;
                //setup the command to execute the add
                added = context.SpecialEvents.Add(item); //set up command, command is not executed until it is acutally
                //saved.
                context.SaveChanges();

            }
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public void SpecialEvents_Update(SpecialEvent item)
        {
            using (eRestaurantContext context = new eRestaurantContext())
            {
                //indicate the updating item instance
                //alter the Modified status flag for this instance

                context.Entry<SpecialEvent>(context.SpecialEvents.Attach(item)).State =
                    System.Data.Entity.EntityState.Modified; //telling it update, SpecialEvent is the entity;
                //attach is passing in the the item

                context.SaveChanges();

            }
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public void SpecialEvents_Delete(SpecialEvent item)
        {
            using (eRestaurantContext context = new eRestaurantContext())
            {
                //lookup the instance and record if found (set pointer to instance)

                SpecialEvent existing = context.SpecialEvents.Find(item.EventCode);

                //Setup the comand to execute the delete
                context.SpecialEvents.Remove(existing);
                context.SaveChanges(); 

            }

      
        #endregion
        } //eof class

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<WaiterBilling> GetWaiterBillingReport()
        {
            using (eRestaurantContext context = new eRestaurantContext())
            {
                var result = from abillrow in context.Bills //need to add the context;
                             where abillrow.BillDate.Month == 5
                             orderby abillrow.BillDate,
                             abillrow.Waiter.LastName,
                             abillrow.Waiter.FirstName
                             select new WaiterBilling()  //add class name;
                          {
                              BillDate = abillrow.BillDate.Year+"/" +
                                                abillrow.BillDate.Month+"/" +
                                                abillrow.BillDate.Day,
                              WaiterName = abillrow.Waiter.LastName + " , " +
                                    abillrow.Waiter.FirstName,
                              BillID = abillrow.BillID,
                              BillTotal = abillrow.Items.Sum(eachbillitemrow => eachbillitemrow.Quantity * eachbillitemrow.SalePrice),
                              PartySize = abillrow.NumberInParty,
                              Contact = abillrow.Reservation.CustomerName
                              //abillrow.BillItems.Sum
                          };
           return result.ToList();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CategoryMenuItems> GetReportCategoryMenuItems()
        {
            using (eRestaurantContext context = new eRestaurantContext())
            {
                var results = from cat in context.Items
                              orderby cat.Category.Description, cat.Description
                              select new CategoryMenuItems
                              {
                                  CategoryDescription = cat.Category.Description,
                                  ItemDescription = cat.Description,
                                  Price = cat.CurrentPrice,
                                  Calories = cat.Calories,
                                  Comment = cat.Comment
                              };

                return results.ToList(); // this was .Dump() in Linqpad
            }
        }

        //WaiterList
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<Waiter> Waiters_List()
        {
            using (var context = new eRestaurantContext())  //create a new instance 'using' is a transaction
            {
                //method syntax
                //return context.SpecialEvents.OrderBy(x => x.Description).ToList();//convert to list so can be sent back


                //query syntax
                var results = from item in context.Waiters
                              orderby item.LastName, item.FirstName
                              select item;
                return results.ToList(); //none, 1 or more rows
            }
        }

        //getWaiter by ID
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public Waiter GetWaiterByID(int waiterid)
        {
            using (var context = new eRestaurantContext())  //create a new instance 'using' is a transaction
            {
                //method syntax
                //return context.SpecialEvents.OrderBy(x => x.Description).ToList();//convert to list so can be sent back


                //query syntax
                var results = from item in context.Waiters
                              where item.WaiterID == waiterid
                              select item;
                return results.FirstOrDefault(); //one row at most
            }
        }



       //return an integer
        [DataObjectMethod(DataObjectMethodType.Insert, false)]
        public int Waiters_Add(Waiter item)
        {
            using (eRestaurantContext context = new eRestaurantContext())
            {
                //these methods are execute using an instance level item
                //set up an instance pointer and initialize to null
                Waiter added = null;
                //setup the command to execute the add
                added = context.Waiters.Add(item); //set up command, command is not executed until it is acutally
                //saved.
                context.SaveChanges();
                //the Waiter instance added contains the newly inserted record
                //to sql including the generated pkey value
                return added.WaiterID;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public void waiter_Update(Waiter item)
        {
            using (eRestaurantContext context = new eRestaurantContext())
            {
                //indicate the updating item instance
                //alter the Modified status flag for this instance

                context.Entry<Waiter>(context.Waiters.Attach(item)).State =
                    System.Data.Entity.EntityState.Modified; //telling it update, SpecialEvent is the entity;
                //attach is passing in the the item

                context.SaveChanges();

            }
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public void waiter_Delete(Waiter item)
        {
            using (eRestaurantContext context = new eRestaurantContext())
            {
                //lookup the instance and record if found (set pointer to instance)

                Waiter existing = context.Waiters.Find(item.WaiterID);

                //Setup the comand to execute the delete
                context.Waiters.Remove(existing);
                context.SaveChanges(); 

            }
      
        
    }//eof namespace

}
}