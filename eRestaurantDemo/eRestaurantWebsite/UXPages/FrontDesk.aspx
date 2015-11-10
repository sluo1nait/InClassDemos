<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="FrontDesk.aspx.cs" Inherits="UXPages_FrontDesk" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

           <div class="well">
        <div class="pull-right col-md-5">
            <h4>
                <small>Last Billed Date/Time:</small>
                <asp:Repeater ID="AdHocBillDateRepeater" runat="server" DataSourceID="AdHocBillDateDataSource" ItemType="System.DateTime">
                    <itemtemplate><b class="label label-primary"><%# Item.ToShortDateString() %></b> &ndash; <b class="label label-info"><%# Item.ToShortTimeString() %></b></itemtemplate>
                </asp:Repeater>
            </h4>
            <asp:ObjectDataSource runat="server" ID="AdHocBillDateDataSource" OldValuesParameterFormatString="original_{0}" SelectMethod="GetLastBillDateTime" TypeName="eRestaurantSystem.BLL.AdminController"></asp:ObjectDataSource>
        </div>
        <h4>Mock Date/Time</h4>
        <asp:LinkButton ID="MockDateTime" runat="server" CssClass="btn btn-primary">Post-back new date/time:</asp:LinkButton>
        <asp:LinkButton ID="MockLastBillingDateTime" runat="server" CssClass="btn btn-default" OnClick="MockLastBillingDateTime_Click">Set to Last-Billed date/time:</asp:LinkButton>
        &nbsp;
        <asp:TextBox id="SearchDate" runat="server" TextMode="Date" Text="2014-10-16"></asp:TextBox>
        <asp:TextBox id="SearchTime" runat="server" TextMode="Time" Text="13:00" CssClass="clockpicker"></asp:TextBox>
        
        <script src="../Scripts/clockpicker.js"></script>
        <script type="text/javascript">
            $('.clockpicker').clockpicker({ donetext: 'Accept' });
        </script>
        <link itemprop="url" href="../Content/standalone.css" rel="stylesheet">
        <link itemprop="url" href="../Content/clockpicker.css" rel="stylesheet">
        &nbsp;&nbsp;
        <details style="display:inline-block; vertical-align: top;">
            <summary class="badge">About ClockPicker &hellip;</summary>
            <h4>Fancy Bootstrap <a href="http://weareoutman.github.io/clockpicker/">ClockPicker</a></h4>
            <p>The time uses the ClockPicker Bootstrap extension</p>
        </details>
    </div>


        <!--this source is to display the seating summary-->
    <div class="col-md-7">
        <details open>
            <summary>Tables</summary>
<p class="well">This GridView uses a &lt;asp:TemplateField …&gt; for the table 
    number and the controls to handle walk-in seating. Additionally, 
    the walk-in seating form is in a panel that only shows if the seat is <em>not</em> taken. Handling of the action to <b>seat customers</b> is done in the code-behind, on the GridView's <code>OnSelectedIndexChanging</code> event.</p>
            <style type="text/css">
                .inline-div {
                    display: inline;
                }
            </style>
            <asp:GridView ID="SeatingGridView" runat="server" AutoGenerateColumns="False"
                    CssClass="table table-hover table-striped table-condensed"
                    DataSourceID="SeatingObjectDataSource" ItemType="eRestaurantSystem.DAL.POCOs.SeatingSummary">
                <Columns>
                    <asp:CheckBoxField DataField="Taken" HeaderText="Taken" SortExpression="Taken" ItemStyle-HorizontalAlign="Center"></asp:CheckBoxField>
                    <asp:TemplateField HeaderText="Table" SortExpression="Table" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="TableNumber" runat="server" Text='<%# Item.Table %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Seating" HeaderText="Seating" SortExpression="Seating" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                    <asp:TemplateField HeaderText="Reservation Notes / Seat Walk-In">
                        <ItemTemplate>
                            <asp:Panel ID="WalkInSeatingPanel" runat="server" CssClass="input-group input-group-sm"
                                    Visible='<%# !Item.Taken %>'>
                                <asp:TextBox ID="NumberInParty" runat="server" CssClass="form-control col-md-1"
                                        TextMode="Number" placeholder="# people"></asp:TextBox>
                                <span class="input-group-addon">
                                    <asp:DropDownList ID="WaiterList" runat="server"
                                            CssClass="selectpicker"
                                            AppendDataBoundItems="true" DataSourceID="WaitersDataSource"
                                            DataTextField="FullName" DataValueField="WaiterId">
                                        <asp:ListItem Value="0">[select a waiter]</asp:ListItem>
                                    </asp:DropDownList>
                                </span>
                                <span class="input-group-addon" style="width:5px;padding:0;border:0;background-color:white;"></span>
                                <asp:LinkButton ID="LinkButton1" runat="server" Text="Seat Customers"
                                    CssClass="input-group-btn" CommandName="Select" CausesValidation="False" />
                            </asp:Panel>
                            <asp:Panel ID="OccupiedTablePanel" runat="server"
                                    Visible='<%# Item.Taken  %>'>
                                <%# Item.Waiter %>
                                <asp:Label ID="ReservationNameLabel" runat="server" 
                                        Text='<%# "&mdash;" + Item.ReservationName %>'
                                        Visible='<%# !string.IsNullOrEmpty(Item.ReservationName) %>' />
                                <asp:Panel ID="BillInfo" runat="server" CssClass="inline-div"
                                        Visible='<%# Item.BillTotal.HasValue && Item.BillTotal.Value > 0 %>'>
                                    <asp:Label ID="Label1" runat="server" Text='<%# string.Format(" &ndash; {0:C}", Item.BillTotal) %>' />
                                </asp:Panel>
                            </asp:Panel>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

             </details>
    </div>
     <asp:ObjectDataSource runat="server" ID="SeatingObjectDataSource" OldValuesParameterFormatString="original_{0}" 
         SelectMethod="SeatingByDateTime" TypeName="eRestaurantSystem.BLL.AdminController">
        <SelectParameters>
            <asp:ControlParameter ControlID="SearchDate" PropertyName="Text" Name="date" Type="DateTime"></asp:ControlParameter>
            <asp:ControlParameter ControlID="SearchTime" PropertyName="Text" DbType="Time" Name="newtime"></asp:ControlParameter>
        </SelectParameters>
    </asp:ObjectDataSource>
      <asp:ObjectDataSource ID="WaitersDataSource" runat="server" OldValuesParameterFormatString="original_{0}" 
          SelectMethod="ListWaiter" TypeName="eRestaurantSystem.BLL.AdminController"></asp:ObjectDataSource>
        <!--this source is to display the Reservations for the selected day in a collaispable display controlled by
            bootstrap -->
    <div class="pull-right col-md-5">
        <details open> 
            <summary>Reservations by Date/Time</summary> <!--font style-->
            <h4>Today's Reservations</h4>
            <asp:Repeater ID="ReservationsRepeater" runat="server"
                ItemType="eRestaurantSystem.DAL.DTOs.ReservationCollection" 
                DataSourceID="ReservationsDataSource">
              
                <ItemTemplate>
                    <div>
                        <h4><%# Item.SeatingTime %></h4>
                        <!-- Insert a fancy clock-picker for a little "bling" -->
                          <!--hour/reservation especially good for repeater-->
                        <!--collaispable area controlled by bootstrap -->
                        <!--Referring to the DTOs ICollection-->
                        <asp:ListView ID="ReservationSummaryListView" runat="server"
                                ItemType="eRestaurantSystem.DAL.POCOs.ReservationSummary"
                                DataSource="<%# Item.Reservations %>"> 
                            <LayoutTemplate>
                                <div class="seating">
                                    <span runat="server" id="itemPlaceholder" />
                                </div>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <div>
                                    <%# Item.Name %> —
                                    <%# Item.NumberInParty %> —
                                    <%# Item.Status %> —
                                    PH:
                                    <%# Item.Contact %> <!--grab content and concontante into one column-->
                                </div>
                            </ItemTemplate>
                        </asp:ListView>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
            <asp:ObjectDataSource runat="server" ID="ReservationsDataSource" 
                OldValuesParameterFormatString="original_{0}" SelectMethod="ReservationsByTime" 
                TypeName="eRestaurantSystem.BLL.AdminController">
                <SelectParameters>
                    <asp:ControlParameter ControlID="SearchDate" PropertyName="Text" Name="date" Type="DateTime"></asp:ControlParameter>
                </SelectParameters>
            </asp:ObjectDataSource>
        </details>
    </div>

</asp:Content>

