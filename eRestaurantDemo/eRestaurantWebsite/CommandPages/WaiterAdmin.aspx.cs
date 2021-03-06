﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

#region Additional Namespaces 
using eRestaurantSystem.BLL; //controller
using eRestaurantSystem.DAL.Entites;//entity
using EatIn.UI; //delegate ProcessRequest
using Microsoft.AspNet.Identity;//extension methods of AspNet.Identity--Get 
//GetUserName
#endregion
public partial class CommandPages_WaiterAdmin : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            ////have you logged in yet?
            //if (!Request.IsAuthenticated)
            //{
            //    //no
            //    Response.Redirect("~/Account/Login.aspx");
            //}
            //else
            //{
            //get the current login user nam

            //currentLogin.Test = User.Identity.GetUserName();
            //}
            HireDate.Text = DateTime.Today.ToShortDateString();
            RefreshWaiterList("0"); //set drop down list to the prompt
        }
    }

    protected void RefreshWaiterList(string selectedvalue)
    { 
      //force the re-execution of the query for the drop down list
        WaiterList.DataBind();
        //inser the prompt line into the drop down list data
        WaiterList.Items.Insert(0, "Select a waiter");
        //position the WaiterList to the desired row representing the waiter
        WaiterList.SelectedValue = selectedvalue;


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

   protected void WaiterInsert_Click(object sender, EventArgs e)
   {
       //inline version of using MessageUserControl
       MessageUserControl.TryRun(() =>
           //remainder of the code is what would have gone in the external 
           //method of (ProcessRequest(MethodName))
           {
               Waiter item = new Waiter();
               item.FirstName = FirstName.Text;
               item.LastName = LastName.Text;
               item.Address = Address.Text;
               item.Phone = Phone.Text;
               item.HireDate = DateTime.Parse(HireDate.Text);
               //what about nullable fields
               if (string.IsNullOrEmpty(ReleaseDate.Text))
               {
                   item.ReleaseDate = null;
               }
               else
               {
                   item.ReleaseDate = DateTime.Parse(ReleaseDate.Text);
               }
               AdminController sysmgr = new AdminController();
               WaiterID.Text = sysmgr.Waiters_Add(item).ToString();
               MessageUserControl.ShowInfo("Waiter added ");
               RefreshWaiterList(WaiterID.Text);
               //WaiterList.DataBind();//force drop down list to be refreshed
           }

           );
   }
   protected void WaiterUpdate_Click(object sender, EventArgs e)
   {
       if (string.IsNullOrEmpty(WaiterID.Text))
       {
           MessageUserControl.ShowInfo("Please select a waiter first before updating.");
       }
       else 
       { 
           //standard update process
           MessageUserControl.TryRun(() =>
           //remainder of the code is what would have gone in the external 
           //method of (ProcessRequest(MethodName))
           {
               Waiter item = new Waiter();
               //for an update you must supply the pkey value
               item.WaiterID = int.Parse(WaiterID.Text);
               item.FirstName = FirstName.Text;
               item.LastName = LastName.Text;
               item.Address = Address.Text;
               item.Phone = Phone.Text;
               item.HireDate = DateTime.Parse(HireDate.Text);
               //what about nullable fields
               if (string.IsNullOrEmpty(ReleaseDate.Text))
               {
                   item.ReleaseDate = null;
               }
               else
               {
                   item.ReleaseDate = DateTime.Parse(ReleaseDate.Text);
               }
               AdminController sysmgr = new AdminController();
               sysmgr.waiter_Update(item);
               MessageUserControl.ShowInfo("Waiter updated");//force drop down list to be refreshed
               RefreshWaiterList(WaiterID.Text);
           }

           );
       }
   }

  
}
