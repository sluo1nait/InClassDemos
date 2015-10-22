<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="WaiterAdmin.aspx.cs" Inherits="CommandPages_WaiterAdmin" %>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <br /> <br /> <br />
    <h1>Waiter Admin CURD</h1>
    <uc1:MessageUserControl runat="server" ID="MessageUserControl" />
    <br />
    <asp:Label ID="Label1" runat="server" Text="Waiter Names"></asp:Label>

    <asp:DropDownList ID="WaiterList" runat="server" DataSourceID="ODSwaiterList" DataTextField="FullName" DataValueField="WaiterID" Height="16px" Width="298px">

        
    </asp:DropDownList>
    <asp:LinkButton ID="FetchWaiter" runat="server" OnClick="FetchWaiter_Click">Fetch Waiter</asp:LinkButton>
    <asp:ObjectDataSource ID="ODSwaiterList" runat="server" DataObjectTypeName="eRestaurantSystem.DAL.Entites.Waiter" DeleteMethod="waiter_Delete" InsertMethod="Waiter_Add" OldValuesParameterFormatString="original_{0}" SelectMethod="Waiters_List" TypeName="eRestaurantSystem.BLL.AdminController" UpdateMethod="waiter_Update">
    </asp:ObjectDataSource>
    <table class="nav-justified">
        <tr>
            <td style="width: 291px">ID</td>
            <td>
                <asp:Label ID="WaiterID" runat="server"  ></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="width: 291px">First Name</td>
            <td>
                <asp:TextBox ID="FirstName" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="width: 291px">Last Name</td>
            <td>
                <asp:TextBox ID="LastName" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="width: 291px">Address</td>
             <td>
                <asp:TextBox ID="Address" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="width: 291px">Phone</td>
            <td>
                <asp:TextBox ID="Phone" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="width: 291px">Hire Date</td>
            <td>
                <asp:TextBox ID="HireDate" runat="server"></asp:TextBox>
            </td>
        </tr>
            <tr>
            <td style="width: 291px">Release Date</td>
            <td>
                <asp:TextBox ID="ReleaseDate" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="width: 291px">&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td style="width: 291px">
                <a href="javascript:__doPostBack('WaiterInsert','')">Insert</a></td>
            <td>
                <a href="javascript:__doPostBack('LinkButton1','')">Update</a></td>
        </tr>
    </table>
    <br />
</asp:Content>

