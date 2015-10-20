<Query Kind="Program">
  <Connection>
    <ID>79439a8d-f149-4d73-b9c2-67559a15d74d</ID>
    <Persist>true</Persist>
    <Server>.</Server>
    <Database>eRestaurant</Database>
  </Connection>
</Query>

void Main()
{
	//anonymous data type queries cost, profit is a new field/if do not assign to another member, use the origial field as your entities
	//from food in Items
	//where food.MenuCategory.Description.Equals("Entree")
	//		&& food.Active 
	//orderby food.CurrentPrice descending
	//select new
	//{
	//	Description = food.Description,
	//	Price =food.CurrentPrice,
	//	cost = food.CurrentCost,
	//	Profit = food.CurrentPrice - food.CurrentCost
	//}
	//
	////default
	//from food in Items
	//where food.MenuCategory.Description.Equals("Entree")
	//		&& food.Active 
	//orderby food.CurrentPrice descending
	//select new
	//{
	//	 food.Description,
	//	 food.CurrentPrice,
	//	 food.CurrentCost,
	//	//Profit = food.CurrentPrice - food.CurrentCost
	//}
	//
	//from food in Items
	//where food.MenuCategory.Description.Equals("Entree")
	//		&& food.Active 
	//orderby food.CurrentPrice descending
	//select new //POCOObjectName
	//{
	//	Description = food.Description,
	//	Price =food.CurrentPrice,
	//	cost = food.CurrentCost,
	//	//Profit = food.CurrentPrice - food.CurrentCost
	//}//eop

	
	var results = from food in Items
	    where food.MenuCategory.Description.Equals("Entree")
			&& food.Active 
	     orderby food.CurrentPrice descending
	    select new FoodMargins()  //That's how to use this class definition
	   {
		Description = food.Description,
		Price =food.CurrentPrice,
		cost = food.CurrentCost,
		Profit = food.CurrentPrice - food.CurrentCost
	   };
	   results.Dump();
	   

}
	 //Define other methods and classes here
	 //only those bills which were paid.
	 
	 
	 var results2 =from order in bills
	 				where orders.PaidStatus &&
					(orders.BillDate.Month == 9 && orders.BillDate.Year == 2014)
					orderby Waiter.LastName, Waiter.FirstName
					select new 
		{
	 						BillID=orders.BillID,
							WaiterName = orders.Waiter.LastName + ", " + orders.Waiter.FirstName,
							Orders =orders.BillItems
		};
	   //get all the bills and bill items for waiters in Sep of 2014
	   //eop
	
	//this is a POCO class  
			public class FoodMargins
			{
				public string Description {get; set;}
				public decimal Price {get; set;}
				public decimal cost {get; set;}
				public decimal Profit {get; set;}
			}   	 
				}
	
	
	
//this is a DTO class

		
  


// Define other methods and classes here