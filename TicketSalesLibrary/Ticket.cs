using System;
using System.Data;

namespace TicketSalesLibrary
{
    public class Ticket
    {
        public Guid Id { get; set; }
        public Guid? OrderId { get; set; }
        public Guid EventId { get; set; }
        public string Section { get; set; }
        public string Row { get; set; }
        public int Number { get; set; }
        public double TicketPrice { get; set; }
        public Plan FoodPlan { get; set; }
        public double TotalTicketCost { get; set; }
        public bool IsTicketSold { get; set; }

        public Ticket(DataRow dataRow)
        {
            Id = new Guid(dataRow["Id"].ToString());
            if (!string.IsNullOrEmpty(dataRow["OrderId"].ToString()))
                OrderId = new Guid(dataRow["OrderId"].ToString());
            EventId = new Guid(dataRow["EventId"].ToString());
            Section = dataRow["Section"].ToString();
            Row = dataRow["Row"].ToString();
            Number = Convert.ToInt32(dataRow["Number"]);
            TicketPrice = Convert.ToDouble(dataRow["TicketPrice"]);
            FoodPlan = (Plan)Convert.ToInt32(dataRow["FoodPlan"].ToString());
            TotalTicketCost = Convert.ToDouble(dataRow["TotalTicketCost"]);
            IsTicketSold = Convert.ToBoolean(dataRow["IsTicketSold"].ToString());
        }

        public double SetPriceBasedOnPlan(Ticket ticket)
        {
            switch (ticket.FoodPlan)
            {
                case Plan.Food:
                    return ticket.TicketPrice * 1.3;
                case Plan.Beverage:
                    return ticket.TicketPrice * 1.2;
                case Plan.Both:
                    return ticket.TicketPrice * 1.5;
                default:
                    return ticket.TicketPrice;
            }
        }
    }

    public static class TicketExtension
    {
        public static double SetPriceBasedOnPlan(double ticketPrice, Plan foodPlan)
        {
            switch (foodPlan)
            {
                case Plan.Food:
                    return ticketPrice * 1.3;
                case Plan.Beverage:
                    return ticketPrice * 1.2;
                case Plan.Both:
                    return ticketPrice * 1.5;
                default:
                    return ticketPrice;
            }
        }
    }

    public enum Plan
    {
        None,
        Food,
        Beverage,
        Both
    }
}
