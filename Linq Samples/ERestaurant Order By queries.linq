<Query Kind="Expression">
  <Connection>
    <ID>79439a8d-f149-4d73-b9c2-67559a15d74d</ID>
    <Persist>true</Persist>
    <Server>.</Server>
    <Database>eRestaurant</Database>
  </Connection>
</Query>

//orderby

//default is ascending
from food in Items
orderby food.Description
select food

//also use both ascending and available descending
from food in Items
orderby food.CurrentPrice descending, food.Calories ascending
select food

//select one column
from food in Items
orderby food.CurrentPrice descending, food.Calories ascending
select food.CurrentPrice

//you can use where and order by together
from food in Items
orderby food.CurrentPrice descending, food.Calories ascending
where food.MenuCategory.Description.Equals("Entree")
select food

//Where and orderby have no specific sequences
from food in Items
where food.MenuCategory.Description.Equals("Entree")
orderby food.CurrentPrice descending, food.Calories ascending
select food

