<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="CategoryMenuItems.aspx.cs" Inherits="SamplePages_CategoryMenuItems" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <h1>Category Menu Items (Repeater)</h1>
    <div class="row col-md-12">
        <asp:Repeater ID="MenuCategories" runat="server">
        <ItemTemplate>
            <h3>
                <%# Eval("Description") %>>

            </h3>
            <div class ="well">
                <asp:Repeater ID="MenuItems" runat="server"
                    DateSource =' <%# Eval("MenuItems") %>'>
                <ItemTemplate>
                   <h5>
                    <%# Eval("Description") %>
                    <%# ((decimal)Eval("Price")).ToString("C") %>
                    <span class="badge"> <%# Eval("Calories") %> Calories</span>
                    <%# Eval("Comment") %>
                </h5>
               </ItemTemplate>
             </asp:Repeater>
            </div>
        </ItemTemplate>
            </asp:Repeater>
    </div>
    <asp:ObjectDataSource ID="ODSCategoryMenuItems" runat="server"></asp:ObjectDataSource>
</asp:Content>

