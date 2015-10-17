using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

#region Additional Namespaces 
using eRestaurantSystem.BLL; //controller
using eRestaurantSystem.DAL.Entites;//entity
using EatIn.UI; //delegate ProcessRequest

#endregion
public partial class CommandPages_WaiterAdmin : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

   protected void CheckForException(object sender, ObjectDataSourceStatusEventArgs e)
    {
        MessageUserControl.HandleDataBoundException(e);
    }
   protected void FetchWaiter_Click(object sender, EventArgs e)
   {
       //to properly interface with our MessageUserControl
       //we will delegate the execution of this Click event
       //under the MessageUserControl 
       if (WaiterList.SelectedIndex == 0) //you didn't pick a waiter
       {
           //issue our own error message
           MessageUserControl.ShowInfo("Please select a waiter to process.");

       }
       else
       { 
           //execute the necessary standard lookup code under 
           //the control of the MessageUserControl
           MessageUserControl.TryRun((ProcessRequest) GetWaiterInfo);
       }
   }

   public void GetWaiterInfo ()
   { 
       //a standard lookup process
       AdminController sysmgr = new AdminController();
       var Waiter =sysmgr.GetWaiterByID(int.Parse(WaiterList.SelectedValue));
       WaiterID.Text  =Waiter.WaiterID.ToString ();
       FirstName.Text = Waiter.FirstName;
       LastName.Text = Waiter.LastName;
       Address.Text = Waiter.Address;
       Phone.Text =Waiter.Phone;
       HireDate.Text = Waiter.HireDate.ToShortDateString();
       //null field check

       if (Waiter.ReleaseDate.HasValue)
   {
       ReleaseDate.Text = Waiter.ReleaseDate.ToString();
   }
   
       else
   {
       ReleaseDate.Text ="";

   }
   }

}
