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
using System.Data.Entity; //help with DateTime and TimeSpan linq concerns
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
      
       
        
    }//eofclass

         #region FrontDesk
       [DataObjectMethod(DataObjectMethodType.Select, false)]
        public DateTime GetLastBillDateTime()
        {
            using (var context = new eRestaurantContext())
            {
                var result = context.Bills.Max(eachBillrow => eachBillrow.BillDate);
                return result;
            }
        }


       
        [DataObjectMethod(DataObjectMethodType.Select)]
           public List<ReservationCollection> ReservationsByTime(DateTime date)
           {
               using (var context = new eRestaurantContext())
               {
                   var result = (from eachReservationRow in context.Reservations //take each row from the Reservation
                                 where eachReservationRow.ReservationDate.Year == date.Year
                                 && eachReservationRow.ReservationDate.Month == date.Month
                                 && eachReservationRow.ReservationDate.Day == date.Day
                                     // && data.ReservationDate.Hour == timeSlot.Hours
                                 && eachReservationRow.ReservationStatus == "B" //Reservation.Booked
                                 select new ReservationSummary()
                                 {
                                     ID = eachReservationRow.ReservationID,
                                     Name = eachReservationRow.CustomerName,
                                     Date = eachReservationRow.ReservationDate,
                                     NumberInParty = eachReservationRow.NumberInParty,
                                     Status = eachReservationRow.ReservationStatus,
                                     Event = eachReservationRow.Event.Description,
                                     Contact = eachReservationRow.ContactPhone
                                 }).ToList(); //causes execution of the query so 
                                              //that the retrieved data is in memory
                                                //so as to be used in the following query

                   var finalResult = from item in result
                                     orderby item.NumberInParty
                                     group item by item.Date.Hour into itemGroup //Datetime is not liked for C#;
                                                                            //temporary data collection, put result into new 'itemGroup' collection
                                     select new ReservationCollection()//DTO
                                     {
                                         Hour = itemGroup.Key,
                                         Reservations = itemGroup.ToList()
                                     };
                   return finalResult.OrderBy(x => x.Hour).ToList(); //method syntax
               }
           }
        //time saving to send information to webpage;
        //do it in BLL will spead up the sponse time;
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public bool ReservationsForToday(DateTime date)
        {
            return ReservationsByTime(date).Count > 0 ? true : false; //if not >0, then send back false

        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<SeatingSummary> SeatingByDateTime(DateTime date, TimeSpan newtime)

        {
            using (eRestaurantContext context = new eRestaurantContext())
            {
                var step1 = from data in context.Tables
                            select new
                            {
                                Table = data.TableNumber,
                                Seating = data.Capacity,
                                // This sub-query gets the bills for walk-in customers
                                WalkIns = from walkIn in data.Bills
                                          where
                                                 walkIn.BillDate.Year == date.Year
                                              && walkIn.BillDate.Month == date.Month
                                              && walkIn.BillDate.Day == date.Day
                                              //Linq to Entity does not play nicely with DateTime/Timespan
                                              //of which TimeOfDay belongs
                                              //&& walkIn.BillDate.TimeOfDay <= newtime
                                             && DbFunctions.CreateTime(walkIn.BillDate.Hour,walkIn.BillDate.Minute,walkIn.BillDate.Second) <=newtime
                                              //inside System.Data.Entity is a class of functions
                                              //that will help with DateTime/TimeSpan concerns
                                              && (!walkIn.OrderPaid.HasValue || walkIn.OrderPaid.Value >= newtime)
                                          //                          && (!walkIn.PaidStatus || walkIn.OrderPaid >= time)
                                          select walkIn,
                                // This sub-query gets the bills for reservations
                                //Linq connects to SQL, we don't create the reservationtables entity
                                //OnModelCreate such table
                                Reservations = from booking in data.Reservations
                                               from reservationParty in booking.Bills //dont need Reservation table, 
                                               //b/c we use the OnModelCreate
                                               where
                                                      reservationParty.BillDate.Year == date.Year
                                                   && reservationParty.BillDate.Month == date.Month
                                                   && reservationParty.BillDate.Day == date.Day
                                                   //&& reservationParty.BillDate.TimeOfDay <= newtime
                                                   && DbFunctions.CreateTime(reservationParty.BillDate.Hour, reservationParty.BillDate.Minute, reservationParty.BillDate.Second) <= newtime
                                                   && (!reservationParty.OrderPaid.HasValue || reservationParty.OrderPaid.Value >= newtime)
                                               //                          && (!reservationParty.PaidStatus || reservationParty.OrderPaid >= time)
                                               select reservationParty
                            };
              


                var step2 = from data in step1.ToList() // .ToList() forces the first result set to be in memory
                            select new
                            {
                                Table = data.Table,
                                Seating = data.Seating,
                                CommonBilling = from info in data.WalkIns.Union(data.Reservations)
                                                select new // info
                                                {
                                                    BillID = info.BillID,
                                                    BillTotal = info.Items.Sum(bi => bi.Quantity * bi.SalePrice),
                                                    Waiter = info.Waiter.FirstName,
                                                    Reservation = info.Reservation
                                                }
                            };
                //we call it Items, so we change from BillItems to Items above
               


                // Step 3 - Get just the first CommonBilling item
                //         (presumes no overlaps can occur - i.e., two groups at the same table at the same time)
                var step3 = from data in step2.ToList()
                            select new
                            {
                                Table = data.Table,
                                Seating = data.Seating,
                                Taken = data.CommonBilling.Count() > 0,
                                // .FirstOrDefault() is effectively "flattening" my collection of 1 item into a 
                                // single object whose properties I can get in step 4 using the dot (.) operator
                                CommonBilling = data.CommonBilling.FirstOrDefault()
                            };
                

                // Step 4 - Build our intended seating summary info
                var step4 = from data in step3
                            select new  SeatingSummary() // the DTO class to use in my BLL
                            {
                                Table = data.Table,
                                Seating = data.Seating,
                                Taken = data.Taken,
                                // use a ternary expression to conditionally get the bill id (if it exists)
                                BillID = data.Taken ?               // if(data.Taken)
                                         data.CommonBilling.BillID  // value to use if true
                                       : (int?)null,               // value to use if false
                                BillTotal = data.Taken ?
                                            data.CommonBilling.BillTotal : (decimal?)null,
                                Waiter = data.Taken ? data.CommonBilling.Waiter : (string)null,
                                ReservationName = data.Taken ?
                                                  (data.CommonBilling.Reservation != null ?
                                                   data.CommonBilling.Reservation.CustomerName : (string)null)
                                                : (string)null
                            };

                return step4.ToList();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<WaiterOnDuty> ListWaiter()
        {
            using (eRestaurantContext context = new eRestaurantContext())
            {
                var result = from person in context.Waiters
                             where person.ReleaseDate == null
                             select new WaiterOnDuty()
                             {
                                 WaiterID = person.WaiterID,
                                 FullName = person.FirstName + " " + person.LastName
                             };
                return result.ToList();
            }
        }


        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<SeatingSummary> AvailableSeatingByDateTime(DateTime date, TimeSpan time)
        {
            var results = from seats in SeatingByDateTime(date, time)
                          where !seats.Taken
                          select seats;
            return results.ToList();
        }


   
        public void SeatCustomer(DateTime when, byte tablenumber,
                                 int numberinparty, int waiterid)
        {
            //business logic checking should be done
            //before attempting to place data on the database
            //rule 1: is the seat available
            //rule 2: is the selected table capacity sufficient

            //get the available seats
            var availableseats = AvailableSeatingByDateTime(when.Date, when.TimeOfDay);

            //start my transaction
            using (eRestaurantContext context = new eRestaurantContext())
            { 
                //my transaction
                //create a holding list for possible business logic errors
                //this is needed for the MessageUserControl
                List<string> errors = new List<string>();

                if (!availableseats.Exists(foreachseat => foreachseat.Table == tablenumber))
                { 
                    //the table number is not available
                    errors.Add("Table is currently not available");
                }
                else if (!availableseats.Exists(foreachseat => foreachseat.Table == tablenumber
                                                         &&foreachseat.Seating >= numberinparty))          
                    
                {
                    //the table is available but not large enough
                    errors.Add("insufficient seating capacity fro number of customers");
                }

                //check if any errors to business rules exist
                if (errors.Count > 0)
                {
                    //throw an exception which will terminate the transaction
                    //BusinessRule Exception is part of the MessageUserControl setup
                    throw new BusinessRuleException("Unable to seat customer", errors);
                 }
                    //assume your data is valid
                    //create an instance of the Bill entity and fill with data
                    Bill seatedcustomer = new Bill();
                    seatedcustomer.BillDate = when;
                    seatedcustomer.NumberInParty = numberinparty;
                    seatedcustomer.WaiterID = waiterid;
                    seatedcustomer.TableID = tablenumber;
                    //issue the command to add a record to the Bill entity 
                context.Bills.Add(seatedcustomer);
                //save and commit the changes to the entity
                context.SaveChanges();
               
            }//end of the transaction
        }

        public void SeatCustomer(DateTime when, int reservationId, List<byte> tables, int waiterId)
        {
            var availableSeats = AvailableSeatingByDateTime(when.Date, when.TimeOfDay);
            using (var context = new eRestaurantContext())
            {
                List<string> errors = new List<string>();
                // Rule checking:
                // - Reservation must be in Booked status
                // - Table must be available - typically a direct check on the table, but proxied based on the mocked time here
                // - Table must be big enough for the # of customers
                var reservation = context.Reservations.Find(reservationId);
                if (reservation == null)
                    errors.Add("The specified reservation does not exist");
                if (reservation != null && reservation.ReservationStatus != Reservation.Booked)
                    //not null and not booked
                    errors.Add("The reservation's status is not valid for seating. Only booked reservations can be seated.");
                var capacity = 0;
                foreach (var tableNumber in tables)
                {
                    if (!availableSeats.Exists(x => x.Table == tableNumber))
                        errors.Add("Table " + tableNumber + " is currently not available");
                    else
                        capacity += availableSeats.Single(x => x.Table == tableNumber).Seating;
                }
                if (capacity < reservation.NumberInParty)
                    errors.Add("Insufficient seating capacity for number of customers. Alternate tables must be used.");
                if (errors.Count > 0)
                    throw new BusinessRuleException("Unable to seat customer", errors);
                // 1) Create a blank bill with assigned waiter
                Bill seatedCustomer = new Bill()
                {
                    BillDate = when,
                    NumberInParty = reservation.NumberInParty,
                    WaiterID = waiterId,
                    ReservationID = reservation.ReservationID
                };
                context.Bills.Add(seatedCustomer);
                // 2) Add the tables for the reservation and change the reservation's status to arrived
                foreach (var tableNumber in tables)
                    reservation.Tables.Add(context.Tables.Single(x => x.TableNumber == tableNumber));
                reservation.ReservationStatus = Reservation.Arrived;
                var updatable = context.Entry(context.Reservations.Attach(reservation));
                updatable.Property(x => x.ReservationStatus).IsModified = true;
                //updatable.Reference(x=>x.Tables).
                // 3) Save changes one and one only/comitted the change/more than two will roll back to the old data;
                context.SaveChanges(); 
            }
            //string message = String.Format("Not yet implemented. Need to seat reservation {0} for waiter {1} at tables {2}", reservationId, waiterId, string.Join(", ", tables));
            //throw new NotImplementedException(message);
        }


  #endregion
}//eof class
}//eof namespace

