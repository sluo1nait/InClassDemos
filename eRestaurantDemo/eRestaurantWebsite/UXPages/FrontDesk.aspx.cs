using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

#region Additonal Namespaces
using eRestaurantSystem.BLL;
using eRestaurantSystem.DAL.Entites;
using eRestaurantSystem.DAL.DTOs;
using eRestaurantSystem.DAL.POCOs;

#endregion

public partial class UXPages_FrontDesk : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
  
    protected void SeatingGridView_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        //extract the table number, number in party and the waiter ID
        //from the grid view
        //we will also create the time from the MockDateTime controls at the top of this form
        //Typically you would use DateTime.Today for current datetime

        //once the date is collected then it will be sent to the BLL for processing

        //the command will be done under the control of the MessageUserControl
        //so if there is an error, the MUC can handle it.
        //we will use the in-line MUC TryRun technique

        MessageUserControl.TryRun(() =>
            {
                //obtain the selected gridview row
                GridViewRow agvrow = SeatingGridView.Rows[e.NewSelectedIndex];
                //accessing a web control on the gridview row uses .FindControl("xxx) as 
                //datatype xxx be the name of control
                //points to the control
                string tablenumber = (agvrow.FindControl("TableNumber") as Label).Text;
                string numberinparty = (agvrow.FindControl("NumberInParty") as TextBox).Text;
                string waiterid = (agvrow.FindControl("WaiterList") as DropDownList).SelectedValue;
                DateTime when = Mocker.MockDate.Add(Mocker.MockTime); //Parse(SearchDate.Text).Add(TimeSpan.Parse(SearchTime.Text));

                //standard call to insert a record into the DB
                AdminController sysmgr = new AdminController();
                sysmgr.SeatCustomer(when, byte.Parse(tablenumber), int.Parse(numberinparty),
                                      int.Parse(waiterid));
                //refresh the gridview
                SeatingGridView.DataBind();
            }, "Customer Seated", "New walk-in customer has been saved"); //message
           
    }
}