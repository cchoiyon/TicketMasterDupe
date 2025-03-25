using System;
using System.Text;
using System.Data;
using System.Linq;
using System.Web.UI;
using TicketSalesLibrary;
using System.Data.SqlClient;
using Utilities;
using System.Web.UI.WebControls;
using System.Collections.Generic;

namespace TicketSales
{
    public partial class TicketSales : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DdlEventType.DataSource = Enum.GetValues(typeof(TypeOfEvent));
                DdlEventType.DataBind();
                DdlPaymentMethod.DataSource = Payment.PaymentMethod;
                DdlPaymentMethod.DataBind();
            }
        }

        private Checkout Checkout
        {
            get { return ViewState["Checkout"] as Checkout; }
            set { ViewState["Checkout"] = value; }
        }

        private List<Checkout> CheckoutList
        {
            get { return ViewState["CheckoutList"] as List<Checkout>; }
            set { ViewState["CheckoutList"] = value; }
        }

        private Customer CustomerData
        {
            get { return ViewState["CustomerData"] as Customer; }
            set { ViewState["CustomerData"] = value; }
        }

        //Queries
        protected List<Event> GetAllEvent(TypeOfEvent type)
        {
            try
            {
                var events = new List<Event>();
                DBConnect dbConnect = new DBConnect();
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "GetEvents";
                SqlParameter typeOfEventParam = new SqlParameter("@TypeOfEvent", SqlDbType.Int);
                typeOfEventParam.Value = (int)type;
                command.Parameters.Add(typeOfEventParam);
                DataSet dataSet = dbConnect.GetDataSetUsingCmdObj(command);
                if (dataSet != null && dataSet.Tables.Count > 0)
                {
                    DataTable table = dataSet.Tables[0];
                    foreach (DataRow row in table.Rows)
                    {
                        events.Add(new Event(row));
                    }
                }
                return events;
            }
            catch (Exception ex)
            {
                return new List<Event>()
                {
                    new Event
                    {
                        EventName = ex.StackTrace,
                        PerformerOrPerformance = ex.Message
                    }
                };
            }
        }

        protected List<Ticket> GetEventTicket(Guid eventId)
        {
            try
            {
                var tickets = new List<Ticket>();
                DBConnect dbConnect = new DBConnect();
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "GetAvailableEventTickets";
                SqlParameter eventIdParam = new SqlParameter("@EventId", SqlDbType.UniqueIdentifier);
                eventIdParam.Value = eventId;
                command.Parameters.Add(eventIdParam);
                DataSet dataSet = dbConnect.GetDataSetUsingCmdObj(command);
                if (dataSet != null && dataSet.Tables.Count > 0)
                {
                    DataTable table = dataSet.Tables[0];
                    foreach (DataRow row in table.Rows)
                    {
                        tickets.Add(new Ticket(row));
                    }
                }
                return tickets;
            }
            catch
            {
                return new List<Ticket>();
            }
        }

        protected string GetEventSeatingChart(Guid eventId)
        {
            try
            {
                var seatingChartUrl = string.Empty;
                DBConnect dbConnect = new DBConnect();
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "GetEventSeatingChart";
                SqlParameter eventIdParam = new SqlParameter("@EventId", SqlDbType.UniqueIdentifier);
                eventIdParam.Value = eventId;
                command.Parameters.Add(eventIdParam);
                DataSet dataSet = dbConnect.GetDataSetUsingCmdObj(command);
                if (dataSet != null && dataSet.Tables.Count > 0)
                {
                    DataTable table = dataSet.Tables[0];
                    foreach (DataRow row in table.Rows)
                    {
                        seatingChartUrl = row["SeatingChartURL"].ToString();
                        if (!string.IsNullOrWhiteSpace(seatingChartUrl))
                            break;
                    }
                }
                return seatingChartUrl;
            }
            catch
            {
                return string.Empty;
            }
        }

        protected int InsertOrderData(Guid orderId, Guid customerId)
        {

            try
            {
                var query = $@"
INSERT INTO Orders(Id, CustomerId, TotalPayment)
VALUES ('{orderId}', '{customerId}', '{CheckoutList.Sum(Q => Q.TotalCost)}');";
                DBConnect dbConnect = new DBConnect();
                var value = dbConnect.DoUpdate(query);
                return value;
            }
            catch
            {
                return -1;
            }
        }

        protected int InsertCustomerData(Guid customerId)
        {
           
            try
            {
                var query = $@"
INSERT INTO Customers(Id, Name, Phone, Email, Address)
VALUES ('{customerId}', '{CustomerData.Name}', '{CustomerData.Phone}', '{CustomerData.Email}', '{CustomerData.Address}');";
                DBConnect dbConnect = new DBConnect();
                var value = dbConnect.DoUpdate(query);
                return value;
            }
            catch
            {
                return -1;
            }
        }

        protected int UpdateTicketData(Guid orderId)
        {
            

            try
            {
                StringBuilder query = new StringBuilder();
                foreach (var item in CheckoutList)
                {
                    query.AppendLine($@"UPDATE Tickets SET
OrderId = '{orderId}', 
IsTicketSold = 1, 
TotalTicketCost = {item.TotalCost}, 
FoodPlan = {(int)item.FoodPlan} 
WHERE Id = '{item.TicketId}';");
                }
                DBConnect dbConnect = new DBConnect();
                var value = dbConnect.DoUpdate(query.ToString());
                return value;
            }
            catch
            {
                return -1;
            }
        }

        protected int UpdateEventData()
        {

            try
            {
                var query = $@"
UPDATE Events
SET TotalRevenue = TotalRevenue + {CheckoutList.Sum(Q => Q.TotalCost)},
	TotalTicketSold = TotalTicketSold + {CheckoutList.Count}
WHERE Id = '{Checkout.EventId}';";
                DBConnect dbConnect = new DBConnect();
                var value = dbConnect.DoUpdate(query);
                return value;
            }
            catch
            {
                return -1;
            }
        }

        //Actions
        protected void OnSubmitDdlEventType(object sender, EventArgs e)
        {
            var selectedValue = (TypeOfEvent)Enum.Parse(typeof(TypeOfEvent), DdlEventType.SelectedValue);
            var data = GetAllEvent(selectedValue);
            EventGrid.DataSource = data;
            EventGrid.DataBind();
        }

        protected bool OnSubmitContainer2(object sender, EventArgs e)
        {
            Guid? eventId = null;
            string eventName = null;
            foreach (GridViewRow row in EventGrid.Rows)
            {
                CheckBox chk = (CheckBox)row.FindControl("EventGridSelect");
                if (chk != null && chk.Checked)
                {
                    eventId = new Guid(EventGrid.DataKeys[row.RowIndex].Value.ToString());
                    eventName = row.Cells[2].Text;
                    break;
                }
            }
            if (eventId == null)
            {
                Container2_EventContainer_ErrorMessage.Text = "Select at least 1 event to proceed";
                Container2_EventContainer_ErrorMessage.Visible = true;
                return false;
            }
            var data = GetEventTicket(eventId.Value);
            var seatingChart = GetEventSeatingChart(eventId.Value);
            Checkout = new Checkout()
            {
                EventId = eventId.Value,
                EventName = eventName
            };
            TicketGrid.DataSource = data;
            TicketGrid.DataBind();
            SeatingChartUrl.Text = seatingChart;
            return true;
        }

        protected bool OnSubmitContainer3(object sender, EventArgs e)
        {
            var checkoutList = new List<Checkout>();
            foreach (GridViewRow row in TicketGrid.Rows)
            {
                CheckBox chk = (CheckBox)row.FindControl("TicketGridSelect");
                if (chk != null && chk.Checked)
                {
                    DropDownList ddl = (DropDownList)row.FindControl("ddlFoodPlan");
                    if (ddl != null)
                    {
                        var plan = (Plan)Enum.Parse(typeof(Plan), ddl.SelectedValue);
                        checkoutList.Add(new Checkout
                        {
                            EventId = Checkout.EventId,
                            EventName = Checkout.EventName,
                            FoodPlan = plan,
                            TicketId = new Guid(TicketGrid.DataKeys[row.RowIndex].Value.ToString()),
                            SeatLocation = $"{row.Cells[2].Text} - {row.Cells[3].Text} - {row.Cells[4].Text}",
                            TicketPrice = Convert.ToDouble(row.Cells[1].Text),
                            TotalCost = TicketExtension.SetPriceBasedOnPlan(Convert.ToDouble(row.Cells[1].Text), plan)
                        });
                    }
                    else
                    {
                        checkoutList.Add(new Checkout
                        {
                            EventId = Checkout.EventId,
                            EventName = Checkout.EventName,
                            FoodPlan = Plan.None,
                            TicketId = new Guid(row.Cells[row.Cells.Count - 1].Text),
                            SeatLocation = $"{row.Cells[2].Text} - {row.Cells[3].Text} - {row.Cells[4].Text}",
                            TicketPrice = Convert.ToDouble(row.Cells[1].Text),
                            TotalCost = TicketExtension.SetPriceBasedOnPlan(Convert.ToDouble(row.Cells[1].Text), Plan.None)
                        });
                    }
                }
            }
            CheckoutList = checkoutList;
            if (checkoutList.Count == 0)
            {
                Container3_TicketContainer_ErrorMessage.Text = "Select at least 1 ticket to purchase";
                Container3_TicketContainer_ErrorMessage.Visible = true;
                return false;
            }
            return true;
        }

        protected void TicketGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlFoodPlan = (DropDownList)e.Row.FindControl("ddlFoodPlan");
                ddlFoodPlan.DataSource = Enum.GetValues(typeof(Plan));
                ddlFoodPlan.DataBind();

                Ticket ticket = (Ticket)e.Row.DataItem;
                ddlFoodPlan.SelectedValue = ticket.FoodPlan.ToString();
            }
        }

        protected bool OnSubmitContainer4(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Customer_Name?.Text) || string.IsNullOrEmpty(Customer_Address?.Text)
                || string.IsNullOrEmpty(Customer_Phone?.Text) || string.IsNullOrEmpty(Customer_Email?.Text))
            {
                Container4_CustomerContainer_ErrorMessage.Text = "Must fill all required field";
                Container4_CustomerContainer_ErrorMessage.Visible = true;
                return false;
            }
            if (DdlPaymentMethod.Text == "None")
            {
                Container4_CustomerContainer_ErrorMessage.Text = "Choose a payment method";
                Container4_CustomerContainer_ErrorMessage.Visible = true;
                return false;
            }
            Label_Customer_Name.Text = "Name : " + Customer_Name.Text;
            Label_Customer_Phone.Text = "Phone : " + Customer_Phone.Text;
            Label_Customer_Email.Text = "Email : " + Customer_Email.Text;
            Label_Customer_Address.Text = "Address : " + Customer_Address.Text;
            CustomerData = new Customer(Customer_Name.Text, Customer_Address.Text, Customer_Phone.Text, Customer_Email.Text);
            Checkout = new Checkout() { EventId = Checkout.EventId, EventName = Checkout.EventName, PaymentMethod = DdlPaymentMethod.Text };
            PaymentGrid.DataSource = CheckoutList;
            PaymentGrid.Columns[0].FooterText = "Total";
            PaymentGrid.Columns[PaymentGrid.Columns.Count - 2].FooterText = CheckoutList.Sum(Q => Q.TicketPrice).ToString("C2");
            PaymentGrid.Columns[PaymentGrid.Columns.Count - 1].FooterText = CheckoutList.Sum(Q => Q.TotalCost).ToString("C2");
            PaymentGrid.DataBind();
            return true;
        }

        protected bool OnSubmitContainer5(object sender, EventArgs e)
        {
            if (Checkout.PaymentMethod == "Credit Card" &&
                (string.IsNullOrWhiteSpace(Customer_CreditCardNumber.Text) || string.IsNullOrWhiteSpace(Customer_CreditCardCcv.Text) ||
                string.IsNullOrWhiteSpace(Customer_CreditCardExpirationMonth.Text) || string.IsNullOrWhiteSpace(Customer_CreditCardExpirationYear.Text)))
            {
                Container5_TicketContainer_ErrorMessage.Text = "Please fill all the required Credit Card details";
                Container5_TicketContainer_ErrorMessage.Visible = true;
                return false;
            }
            else if (Checkout.PaymentMethod == "Electronic Check" &&
                (string.IsNullOrWhiteSpace(Customer_ElectronicRoutingNumber.Text) || string.IsNullOrWhiteSpace(Customer_ElectronicCheckingAccountNumber.Text)))
            {
                Container5_TicketContainer_ErrorMessage.Text = "Please fil all the Electronic Check details";
                Container5_TicketContainer_ErrorMessage.Visible = true;
                return false;
            }
            var customerId = Guid.NewGuid();
            var orderId = Guid.NewGuid();
            InsertCustomerData(customerId);
            InsertOrderData(orderId, customerId);
            UpdateTicketData(orderId);
            UpdateEventData();
            PaymentConfirmedGrid.DataSource = CheckoutList;
            PaymentConfirmedGrid.Columns[0].FooterText = "Totals";
            PaymentConfirmedGrid.Columns[PaymentGrid.Columns.Count - 2].FooterText = CheckoutList.Sum(Q => Q.TicketPrice).ToString("C2");
            PaymentConfirmedGrid.Columns[PaymentGrid.Columns.Count - 1].FooterText = CheckoutList.Sum(Q => Q.TotalCost).ToString("C2");
            PaymentConfirmedGrid.DataBind();
            return true;
        }

        //Helpers
        protected void BtnNext_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton != null)
            {
                bool proceed;
                switch (clickedButton.ID)
                {
                    case "Container_1_Submit":
                        Container1_EventTypeContainer.Visible = false;
                        Container2_EventContainer.Visible = true;
                        OnSubmitDdlEventType(sender, e);
                        break;
                    case "Container_2_Submit":
                        proceed = OnSubmitContainer2(sender, e);
                        if (proceed)
                        {
                            Container2_EventContainer.Visible = false;
                            Container3_TicketContainer.Visible = true;
                        }
                        break;
                    case "Container_3_Submit":
                        proceed = OnSubmitContainer3(sender, e);
                        if (proceed)
                        {
                            Container3_TicketContainer.Visible = false;
                            Container4_CustomerContainer.Visible = true;
                        }
                        break;
                    case "Container_4_Submit":
                        proceed = OnSubmitContainer4(sender, e);
                        if (proceed)
                        {
                            Container4_CustomerContainer.Visible = false;
                            Container5_PaymentContainer.Visible = true;
                            if (Checkout.PaymentMethod == "Credit Card")
                            {
                                Payment_CreditCard.Visible = true;
                                Payment_ElectronicCheck.Visible = false;
                            }
                            else if (Checkout.PaymentMethod == "Electronic Check")
                            {
                                Payment_CreditCard.Visible = false;
                                Payment_ElectronicCheck.Visible = true;
                            }
                        }
                        break;
                    case "Container_5_Submit":
                        proceed = OnSubmitContainer5(sender, e);
                        if (proceed)
                        {
                            Container5_PaymentContainer.Visible = false;
                            Container6_PaymentConfirmedContainer.Visible = true;
                        }
                        break;
                    case "Container_6_Submit":
                        Container6_PaymentConfirmedContainer.Visible = false;
                        Container1_EventTypeContainer.Visible = true;
                        DdlPaymentMethod.SelectedIndex = 0;
                        DdlEventType.SelectedIndex = 0;

                        Customer_Name.Text = string.Empty;
                        Customer_Email.Text = string.Empty;
                        Customer_Phone.Text = string.Empty;
                        Customer_Address.Text = string.Empty;
                        Customer_CreditCardCcv.Text = string.Empty;
                        Customer_CreditCardExpirationMonth.Text = string.Empty;
                        Customer_CreditCardExpirationYear.Text = string.Empty;
                        Customer_CreditCardNumber.Text = string.Empty;
                        Customer_ElectronicCheckingAccountNumber.Text = string.Empty;
                        Customer_ElectronicRoutingNumber.Text = string.Empty;

                        foreach (GridViewRow row in EventGrid.Rows)
                        {
                            CheckBox chk = (CheckBox)row.FindControl("EventGridSelect");
                            if (chk != null)
                            {
                                chk.Checked = false;
                            }
                        }
                        foreach (GridViewRow row in TicketGrid.Rows)
                        {
                            CheckBox chk = (CheckBox)row.FindControl("TicketGridSelect");
                            if (chk != null)
                            {
                                chk.Checked = false;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        protected void BtnBack_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton != null)
            {
                switch (clickedButton.ID)
                {
                    case "Container_2_Back":
                        Container1_EventTypeContainer.Visible = true;
                        Container2_EventContainer.Visible = false;
                        break;
                    case "Container_3_Back":
                        Container2_EventContainer.Visible = true;
                        Container3_TicketContainer.Visible = false;
                        break;
                    case "Container_4_Back":
                        Container3_TicketContainer.Visible = true;
                        Container4_CustomerContainer.Visible = false;
                        break;
                    case "Container_5_Back":
                        Container4_CustomerContainer.Visible = true;
                        Container5_PaymentContainer.Visible = false;
                        break;
                    default:
                        break;
                }
                Container2_EventContainer_ErrorMessage.Text = string.Empty;
                Container2_EventContainer_ErrorMessage.Visible = false;
                Container3_TicketContainer_ErrorMessage.Text = string.Empty;
                Container3_TicketContainer_ErrorMessage.Visible = false;
                Container4_CustomerContainer_ErrorMessage.Text = string.Empty;
                Container4_CustomerContainer_ErrorMessage.Visible = false;
                Container5_TicketContainer_ErrorMessage.Text = string.Empty;
                Container5_TicketContainer_ErrorMessage.Visible = false;
            }
        }
    }
}