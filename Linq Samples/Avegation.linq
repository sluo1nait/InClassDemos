<Query Kind="Expression">
  <Connection>
    <ID>79439a8d-f149-4d73-b9c2-67559a15d74d</ID>
    <Persist>true</Persist>
    <Server>.</Server>
    <Database>eRestaurant</Database>
  </Connection>
</Query>

(from theBill in BillItems
where theBill.BillID == 104
select theBill.SalePrice * theBill.Quantity).Sum()

BillItems
    .Where (theBill => theBill.BillID == 104)
    .Select(theBill => theBill.SalePrice * theBill.Quantity)
    .Sum()
	
	(from customer in Bills
where customer.PaidStatus == true
select customer.BillItems.Sum(theBill => theBill.SalePrice * theBill.Quantity)).Max()

//the average paid bill


(from customer in Bills
where customer.PaidStatus == true
select customer.BillItems.Sum(theBill => theBill.SalePrice * theBill.Quantity)).Average()

//the average number of items per paid bill
(from customer in Bills
where customer.PaidStatus == true //or customer.PaidStatus
//group by customer.BillItem.BillID
select customer.BillItems.Count()).Average()

from me in Waiters
//where me.Waiters.First()=="D"
select me


