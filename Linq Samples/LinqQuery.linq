<Query Kind="Statements">
  <Connection>
    <ID>7f6b7d38-7921-47c1-a7e1-a7d61b58041a</ID>
    <Persist>true</Persist>
    <Server>.</Server>
    <Database>WorkSchedule</Database>
  </Connection>
</Query>

//list of employees and their YOE, years of experience;
//sum the rows containing the YOE for an employee
var employeeYOE = from eachEmployeerow in Employees
select new 
{
    Name =eachEmployeerow.FirstName + "" +eachEmployeerow.LastName,
	YOE = eachEmployeerow.EmployeeSkills.Sum(
	eachEmployeeskillRow => eachEmployeeskillRow.YearsOfExperience)
};   //looking a set, not a row

 employeeYOE.Dump();
 
//from list of employeesYOE find the Max()
 var MaxYOE = employeeYOE.Max(eachEYOErow => eachEYOErow.YOE);
 MaxYOE.Dump();



//using employeesYOE and YOEMax create a final list of most experienced employees

var finalresult = from eachEYOErow in employeeYOE
                 where compare eachEYOErow to the max value
				 select new
{
                 Name = 
};
				 name
finalresult.Dump();