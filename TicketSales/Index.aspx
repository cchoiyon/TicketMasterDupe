<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="TicketSales.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Home Page</title>
    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server" class="">
        <div class="text-center">
            <h3>Welcome to the event ticket buying application</h3>
            <h6>
                Project 1 - Ticket Sales System that allows users to buy tickets for events. 
            </h6>
            <asp:Button CssClass="btn btn-primary" ID="BtnGoToIndex" runat="server" Text="Buy Tickets" OnClick="BtnGoToIndex_Click" />
        </div>
    </form>
</body>
</html>
