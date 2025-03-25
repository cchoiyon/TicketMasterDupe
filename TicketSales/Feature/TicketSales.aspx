<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TicketSales.aspx.cs" Inherits="TicketSales.TicketSales" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Ticket Sales</title>
    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet">
</head>
<body>
    <form id="Form" runat="server">
        <div id="Container1_EventTypeContainer" class="container mt-3" runat="server">
            <div class="row mt-3">
                <div class="col-12 text-center">
                    <h3>Select an event type you would like to buy tickets for</h3>
                </div>
            </div>
            <div class="row">
                <div class="col-12 text-center">
                    <asp:DropDownList ID="DdlEventType" runat="server" AutoPostBack="true" CssClass="form-control"></asp:DropDownList>
                </div>
            </div>
            <div class="row mt-3">
                <div class="col-12 text-right">
                    <asp:Button ID="Container_1_Submit" runat="server" Text="Next" CssClass="btn btn-primary" OnClick="BtnNext_Click" />
                </div>
            </div>
        </div>

        <div id="Container2_EventContainer" class="container mt-3" runat="server" visible="false">
            <div class="row mt-3">
                <div class="col-12 text-center">
                    <h3>Select an event you would like to buy tickets for</h3>
                </div>
            </div>
            <div class="row">
                <div class="col-12 text-center">
                    <asp:GridView ID="EventGrid" runat="server" AutoGenerateColumns="false"
                        CssClass="table table-bordered table-hover" DataKeyNames="Id">
                        <HeaderStyle BackColor="#C0C0C0" Font-Bold="true" HorizontalAlign="Center" />
                        <Columns>
                            <asp:TemplateField HeaderText="Select">
                                <ItemTemplate>
                                    <asp:CheckBox ID="EventGridSelect" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="EventName" HeaderText="Name" SortExpression="EventName" />
                            <asp:BoundField DataField="PerformerOrPerformance" HeaderText="Performer / Performance" SortExpression="EventName" />
                            <asp:BoundField DataField="TypeOfEvent" HeaderText="Type Of Event" SortExpression="TypeOfEvent" />
                            <asp:BoundField DataField="Location" HeaderText="Location" SortExpression="Location  " />
                            <asp:BoundField DataField="TotalTicketSold" HeaderText="Total Ticket Sold" SortExpression="TotalTicketSold" />
                            <asp:BoundField DataField="TotalRevenue" HeaderText="Total Revenue" SortExpression="TotalRevenue" />
                            <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <div class="row">
                <div class="col-12 text-center">
                    <asp:Label ID="Container2_EventContainer_ErrorMessage" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                </div>
            </div>
            <div class="row">
                <div class="col-6 text-left">
                    <asp:Button ID="Container_2_Back" runat="server" Text="Back" CssClass="btn btn-danger" OnClick="BtnBack_Click" />
                </div>
                <div class="col-6 text-right">
                    <asp:Button ID="Container_2_Submit" runat="server" Text="Next" CssClass="btn btn-primary" OnClick="BtnNext_Click" />
                </div>
            </div>
        </div>

        <div id="Container3_TicketContainer" class="container mt-3" runat="server" visible="false">
            <div class="row mt-3">
                <div class="col-12 text-center">
                    <h3>Select an event you would like to buy tickets for</h3>
                </div>
            </div>
            <div class="row mt-3">
                <div class="col-12 text-center">
                    <asp:Label ID="SeatingChartUrl" runat="server"></asp:Label>
                </div>
            </div>
            <div class="row mt-3">
                <div class="col-12 text-center">
                    <asp:GridView ID="TicketGrid" runat="server" AutoGenerateColumns="false"
                        OnRowDataBound="TicketGrid_RowDataBound"
                        CssClass="table table-bordered table-hover" DataKeyNames="Id">
                        <HeaderStyle BackColor="#C0C0C0" Font-Bold="true" HorizontalAlign="Center" />
                        <Columns>
                            <asp:TemplateField HeaderText="Select">
                                <ItemTemplate>
                                    <asp:CheckBox ID="TicketGridSelect" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="TicketPrice" HeaderText="Ticket Price" SortExpression="TicketPrice" />
                            <asp:BoundField DataField="Section" HeaderText="Section" SortExpression="Section" />
                            <asp:BoundField DataField="Row" HeaderText="Row" SortExpression="Row" />
                            <asp:BoundField DataField="Number" HeaderText="Number" SortExpression="Number" />
                            <asp:TemplateField HeaderText="Food Plan">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlFoodPlan" runat="server"></asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <div class="row">
                <div class="col-12 text-center">
                    <asp:Label ID="Container3_TicketContainer_ErrorMessage" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                </div>
            </div>
            <div class="row my-3">
                <div class="col-6 text-left">
                    <asp:Button ID="Container_3_Back" runat="server" Text="Back" CssClass="btn btn-danger" OnClick="BtnBack_Click" />
                </div>
                <div class="col-6 text-right">
                    <asp:Button ID="Container_3_Submit" runat="server" Text="Purchase Tickets" CssClass="btn btn-primary" OnClick="BtnNext_Click" />
                </div>
            </div>
        </div>

        <div id="Container4_CustomerContainer" class="container mt-3" runat="server" visible="false">
            <div class="row mt-3">
                <div class="col-12 text-center">
                    <h3>Enter Your Information</h3>
                </div>
            </div>
            <div class="row">
                <div class="col-6">
                    <label for="Customer.Name">Full Name</label>
                    <asp:TextBox ID="Customer_Name" runat="server" CssClass="form-control" Placeholder="Enter your full name"></asp:TextBox>
                </div>
                <div class="col-6">
                    <label for="Customer.Email">Email</label>
                    <asp:TextBox ID="Customer_Email" runat="server" CssClass="form-control" Placeholder="Enter your email" TextMode="Email"></asp:TextBox>
                </div>
            </div>
            <div class="row mt-3">
                <div class="col-6">
                    <label for="Customer.Phone">Phone Number</label>
                    <asp:TextBox ID="Customer_Phone" onkeypress="return isNumberKey(event)" runat="server" CssClass="form-control" Placeholder="Enter your phone number" TextMode="Phone"></asp:TextBox>
                </div>
                <div class="col-6">
                    <label for="Customer.Address">Address</label>
                    <asp:TextBox ID="Customer_Address" runat="server" CssClass="form-control" Placeholder="Enter your address"></asp:TextBox>
                </div>
            </div>
            <div class="row mt-3">
                <div class="col-12">
                    <label for="Customer.PaymentMethod">Payment Method</label>
                    <asp:DropDownList ID="DdlPaymentMethod" runat="server" AutoPostBack="true" CssClass="form-control"></asp:DropDownList>
                </div>
            </div>
            <div class="row mt-3">
                <div class="col-12 text-center">
                    <asp:Label ID="Container4_CustomerContainer_ErrorMessage" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                </div>
            </div>
            <div class="row mt-3">
                <div class="col-6 text-left">
                    <asp:Button ID="Container_4_Back" runat="server" Text="Back" CssClass="btn btn-danger" OnClick="BtnBack_Click" />
                </div>
                <div class="col-6 text-right">
                    <asp:Button ID="Container_4_Submit" runat="server" Text="Next" CssClass="btn btn-primary" OnClick="BtnNext_Click" />
                </div>
            </div>
        </div>

        <div id="Container5_PaymentContainer" class="container mt-3" runat="server" visible="false">
            <div class="row mt-3">
                <div class="col-12 text-center">
                    <h3>Check out</h3>
                </div>
            </div>
            <div class="row">
                <div class="col-12">
                    <asp:Label ID="Label_Customer_Name" runat="server"></asp:Label>
                </div>
            </div>
            <div class="row">
                <div class="col-12">
                    <asp:Label ID="Label_Customer_Email" runat="server"></asp:Label>
                </div>
            </div>
            <div class="row">
                <div class="col-12">
                    <asp:Label ID="Label_Customer_Phone" runat="server"></asp:Label>
                </div>
            </div>
            <div class="row">
                <div class="col-12">
                    <asp:Label ID="Label_Customer_Address" runat="server"></asp:Label>
                </div>
            </div>

            <div class="row mt-3">
                <div class="col-12">
                    <asp:GridView ID="PaymentGrid" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-hover">
                        <HeaderStyle BackColor="#C0C0C0" Font-Bold="true" HorizontalAlign="Center" />
                        <Columns>
                            <asp:BoundField DataField="EventName" HeaderText="Event Name" SortExpression="EventName" />
                            <asp:BoundField DataField="SeatLocation" HeaderText="Seat Location" SortExpression="SeatLocation" />
                            <asp:BoundField DataField="FoodPlan" HeaderText="Row" SortExpression="FoodPlan" />
                            <asp:BoundField DataField="TicketPrice" HeaderText="Ticket Price" SortExpression="TicketPrice" />
                            <asp:BoundField DataField="TotalCost" HeaderText="Total Cost" SortExpression="TotalCost" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>

            <div id="Payment_CreditCard" runat="server" visible="false">
                <div class="row">
                    <div class="col-6">
                        <label for="Customer.CreditCardNumber">Credit Card Number</label>
                        <asp:TextBox ID="Customer_CreditCardNumber" MaxLength="16" onkeypress="return isNumberKey(event)" runat="server" CssClass="form-control" Placeholder="Enter credit card number"></asp:TextBox>
                    </div>
                    <div class="col-2">
                        <label for="Customer.CreditCardExpiration">Month</label>
                        <asp:TextBox ID="Customer_CreditCardExpirationMonth" MaxLength="2" onkeypress="return isNumberKey(event)" runat="server" CssClass="form-control" Placeholder="MM"></asp:TextBox>
                    </div>
                    <div class="col-2">
                        <label for="Customer.CreditCardExpiration">Year</label>
                        <asp:TextBox ID="Customer_CreditCardExpirationYear" MaxLength="2" onkeypress="return isNumberKey(event)" runat="server" CssClass="form-control" Placeholder="YY"></asp:TextBox>
                    </div>
                    <div class="col-2">
                        <label for="Customer.CreditCardCcv">CCV</label>
                        <asp:TextBox ID="Customer_CreditCardCcv" MaxLength="3" onkeypress="return isNumberKey(event)" runat="server" CssClass="form-control" Placeholder="CCV"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div id="Payment_ElectronicCheck" runat="server" visible="false">
                <div class="row">
                    <div class="col-6">
                        <label for="Customer.ElectronicRoutingNumber">Routing Number</label>
                        <asp:TextBox ID="Customer_ElectronicRoutingNumber" runat="server" CssClass="form-control" Placeholder="Routing Number"></asp:TextBox>
                    </div>
                    <div class="col-6">
                        <label for="Customer.ElectronicCheckingAccountNumber">Checking Account Number</label>
                        <asp:TextBox ID="Customer_ElectronicCheckingAccountNumber" runat="server" CssClass="form-control" Placeholder="Checking Account Number"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="row mt-3">
                <div class="col-12 text-center">
                    <asp:Label ID="Container5_TicketContainer_ErrorMessage" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                </div>
            </div>
            <div class="row mt-3">
                <div class="col-6 text-left">
                    <asp:Button ID="Container_5_Back" runat="server" Text="Back" CssClass="btn btn-danger" OnClick="BtnBack_Click" />
                </div>
                <div class="col-6 text-right">
                    <asp:Button ID="Container_5_Submit" runat="server" Text="Pay" CssClass="btn btn-primary" OnClick="BtnNext_Click" />
                </div>
            </div>
        </div>

        <div id="Container6_PaymentConfirmedContainer" class="container mt-3" runat="server" visible="false">
            <div class="row mt-3">
                <div class="col-12 text-center">
                    <h3>Payment Confirmed</h3>
                </div>
            </div>
            <div class="row mt-3">
                <div class="col-12">
                    <asp:GridView ID="PaymentConfirmedGrid" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-hover">
                        <HeaderStyle BackColor="#C0C0C0" Font-Bold="true" HorizontalAlign="Center" />
                        <Columns>
                            <asp:BoundField DataField="EventName" HeaderText="Event Name" SortExpression="EventName" />
                            <asp:BoundField DataField="SeatLocation" HeaderText="Seat Location" SortExpression="SeatLocation" />
                            <asp:BoundField DataField="FoodPlan" HeaderText="Row" SortExpression="FoodPlan" />
                            <asp:BoundField DataField="TicketPrice" HeaderText="Ticket Price" SortExpression="TicketPrice" />
                            <asp:BoundField DataField="TotalCost" HeaderText="Total Cost" SortExpression="TotalCost" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>

            <div class="row mt-3">
                <div class="col-12 text-center">
                    <asp:Button ID="Container_6_Submit" runat="server" Text="Go back to main screen" CssClass="btn btn-primary" OnClick="BtnNext_Click" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>
