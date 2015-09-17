<Query Kind="Expression">
  <Connection>
    <ID>79439a8d-f149-4d73-b9c2-67559a15d74d</ID>
    <Persist>true</Persist>
    <Server>.</Server>
    <Database>eRestaurant</Database>
  </Connection>
</Query>

//groupby

from food in Items
group food by food.MenuCategory.Description

//this creates a key with a value and the row collection for that key value

//more than one field, new is to create new key instance
from food in Items
group food by new {food.MenuCategory.Description, food.CurrentPrice}