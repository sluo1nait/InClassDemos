using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#region Additional Namespaces
using eRestaurantSystem.DAL.Entites;
//using System.ComponentModel.DataAnnotations;//used for entity
using System.Data.Entity;
#endregion 

namespace eRestaurantSystem.DAL
{
    //this class should only be accessiable from classes inside 
    //this component library (security concerns we dont want everyone has access)
    //therefore the access level for this class will be internal

    //this class will inherit from DbContext (EntityFramework)
    internal class eRestaurantContext : DbContext//???????internal can only be called inside
    {
        //create a constructor which will pass the connection string 
        //name to the DbContext
        public eRestaurantContext() : base("name =EatIn")
            //connect to the DB Pass this name to a pair of class
            //so entity frame work can use it
        {


        }
        //set of mapping DbSet<T> property
        //map an entity to a database table
        public DbSet<SpecialEvent> SpecialEvents { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Table> Tables { get; set; }

        public DbSet<Bill> Bills { get; set; }
        public DbSet<BillItem> BillItems { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<MenuCategory> MenuCategories { get; set; }
        public DbSet<Waiter> Waiters { get; set; }

        //when overriding the OnModelCreating(), it is important
        //to remember to call the base method's implementation
        //before you exit the method


        //the manyToManyNavigationPropertyConfiguration.Map method
        //lets you configure the tables and columns used for this
        //many to many relationship.

        //it takes a manyToManyNavigationPropertyConfiguration instance
        //in which you specify the column names by calling the MapLeftKey,
        //MapRightKey, and ToTable methods

        //the "left" key is the on specified the HasMany method
        //the "right" key is the one specified in the WithMany method


        //this navigation replaces the SQL associated table that breaks 
        //up a many to many relationship
        //this technique should ONLY be used if the associated table in 
        //SQL has ONLY compound primary key and NO non-key attributes

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Reservation>().HasMany(r => r.Tables)
                .WithMany(t => t.Reservations)
                .Map(mapping =>
                {
                    mapping.ToTable("ReservationTables");
                    mapping.MapLeftKey("ReservationID");
                    mapping.MapRightKey("TableID");
                });
            base.OnModelCreating(modelBuilder);//Do not remove
        }

    }
}
