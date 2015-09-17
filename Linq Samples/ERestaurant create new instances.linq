<Query Kind="Expression">
  <Connection>
    <ID>79439a8d-f149-4d73-b9c2-67559a15d74d</ID>
    <Persist>true</Persist>
    <Server>.</Server>
    <Database>eRestaurant</Database>
  </Connection>
</Query>

//anonymous data type queries cost, profit is a new field/if do not assign to another member, use the origial field as your entities
from food in Items
where food.MenuCategory.Description.Equals("Entree")
		&& food.Active 
orderby food.CurrentPrice descending
select new
{
	Description = food.Description,
	Price =food.CurrentPrice,
	cost = food.CurrentCost,
	Profit = food.CurrentPrice - food.CurrentCost
}

//default
from food in Items
where food.MenuCategory.Description.Equals("Entree")
		&& food.Active 
orderby food.CurrentPrice descending
select new
{
	 food.Description,
	 food.CurrentPrice,
	 food.CurrentCost,
	//Profit = food.CurrentPrice - food.CurrentCost
}

from food in Items
where food.MenuCategory.Description.Equals("Entree")
		&& food.Active 
orderby food.CurrentPrice descending
select new //POCOObjectName
{
	Description = food.Description,
	Price =food.CurrentPrice,
	cost = food.CurrentCost,
	Profit = food.CurrentPrice - food.CurrentCost
}


