<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ServingTables.aspx.cs" Inherits="UXPages_ServingTables" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <h1>Serving Tables</h1>
    <details class="well">
        <summary>About this Page...</summary>
        <p>This page demonstrates the following features/characteristics.</p>
        <ul>
            <li>Waiter First Name = '<%= this.Request.QueryString["waiter"] %>'<br />
            <code>Request.QueryString["waiter"]</code> = The first name of the waiter</li>
            <li>Bill ID = '<%= this.Request.QueryString["bill"] %>'<br />
            <code>Request.QueryString["bill"]</code> = The ID of the bill</li>
            <li>Mock-Date (ticks) = '<%= this.Request.QueryString["md"] %>'<br />
            <code>Request.QueryString["md"]</code> = The date (in ticks) that was being used on the Front Desk form</li>
            <li>Mock-Time (ticks) = '<%= this.Request.QueryString["mt"] %>'<br />
            <code>Request.QueryString["mt"]</code> = The time (in ticks) that was being used on the Front Desk form</li>
        </ul>
    </details>
</asp:Content>

