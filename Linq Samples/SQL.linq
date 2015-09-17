<Query Kind="Expression">
  <Connection>
    <ID>79439a8d-f149-4d73-b9c2-67559a15d74d</ID>
    <Persist>true</Persist>
    <Server>.</Server>
    <Database>eRestaurant</Database>
  </Connection>
</Query>

//where clause

//list all tables that hold more than 3 people
//query syntax row is instance
from row in Tables
where row.Capacity > 3
select row

//method syntax
 
Tables.Where(row => row.Capacity >3)

//list all items with more than 500 calories

from food in Items
where food.Calories > 500
select food

//list all items with more than 500 calories and selling for more than $10.00
//double end with d, /decimal end with m /food is the instance
from food in Items
where food.Calories > 500 &
food.CurrentPrice > 10.00m
select food

//list all items with more than 500 calories and are entrees on the menu
//HINT: navigational properties of the database are known by LinqPad

from food in Items
where food.Calories > 500 & 
food.MenuCategory.Description.Equals("Entree")
select food