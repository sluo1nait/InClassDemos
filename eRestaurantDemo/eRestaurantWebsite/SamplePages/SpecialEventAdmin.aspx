<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="SpecialEventAdmin.aspx.cs" Inherits="SamplePages_SpecialEventAdmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

    <table align="center" style="width: 70%">
       
         <tr>
            <td align="right" style ="width:50%">Select an Event;&nbsp:</td>
            <td>
                <asp:DropDownList ID="SpecialEventList" runat="server" AppendDataBoundItems="True" DataSourceID="ODSSpecialEvents" DataTextField="Description" DataValueField="EventCode">
                    <asp:ListItem Value ="z">Select event</asp:ListItem>
                </asp:DropDownList>
                <asp:LinkButton ID="FetchRegistrations" runat="server">Fetch Registrations</asp:LinkButton>
            </td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr>
            <td colspan="2" align="center">
            <asp:GridView ID="ReservationList" runat="server" AllowPaging="True" AutoGenerateColumns="False" DataSourceID="ODSReservations">
                <AlternatingRowStyle BackColor="#006666" Font-Size="Large" />
                <Columns>
                    <asp:BoundField DataField="ReservationDate" DataFormatString="{0:MMM dd, yyyy}" HeaderText="Date" SortExpression="ReservationDate">
                    <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="NumberInParty" HeaderText="Size" SortExpression="NumberInParty">
                    <HeaderStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ContactPhone" HeaderText="Phone" SortExpression="ContactPhone">
                    <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ReservationStatus" HeaderText="Status" SortExpression="ReservationStatus">
                    <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CustomerName" HeaderText="Name" SortExpression="CustomerName">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                </Columns>
                <EmptyDataTemplate>
                    No data to dispaly at this time
                </EmptyDataTemplate>
                <PagerSettings FirstPageText="Start" LastPageText="End" Mode="NumericFirstLast" PageButtonCount="4" Position="TopAndBottom" />
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            
        </tr>
    </table>
    <asp:ObjectDataSource ID="ODSSpecialEvents" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="SpecialEvents_List" TypeName="eRestaurantSystem.BLL.AdminController"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ODSReservations" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="GetReservationsByEventCode" TypeName="eRestaurantSystem.BLL.AdminController">
        <SelectParameters>
            <asp:ControlParameter ControlID="SpecialEventList" Name="eventcode" PropertyName="SelectedValue" Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>

