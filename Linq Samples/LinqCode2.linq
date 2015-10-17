<Query Kind="Statements">
  <Connection>
    <ID>79439a8d-f149-4d73-b9c2-67559a15d74d</ID>
    <Persist>true</Persist>
    <Server>.</Server>
    <Database>eRestaurant</Database>
  </Connection>
</Query>

//simpliest form for dumpinhg an entity
Waiters

//simple query syntax
from person in Waiters
select person

//simple method syntax
Waiters.Select(person => person)
Waiters.Select(x => x)


 
//inside our project we will be writting C# statement
var results = from person in Waiters
				select person;
//to display the contents of a variable in LinqPad
//use the .Dump() method
results.Dump();

//implemented inside a VS project's class library BLL method
////[DataObjectMethod(DataObjectMethodType.Select,false)]
////public list<Waiters> SomeMethodName ()
////{
		//you will need to connect to your DAL object
		//this will be done using a new xxxxxx() constructor
		//assume your connection variable is called contextvariable
		
		//do your query
		////var results = from person in contextvariable.Waiters
				////select person;
				//return your results
				////return results.ToList();
		
////}
