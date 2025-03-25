using System;
using System.Collections.Generic;

namespace TicketSalesLibrary
{
    public static class Payment
    {
        public static List<string> PaymentMethod = new List<string>
        {
            "None",
            "Credit Card",
            "Electronic Check"
        };
    }

    [Serializable]
    public class Checkout
    {
        public string PaymentMethod { get; set; }
        public Guid EventId { get; set; }
        public string EventName { get; set; }
        public Guid TicketId { get; set; }
        public string SeatLocation { get; set; }
        public Plan FoodPlan { get; set; }
        public double TicketPrice { get; set; }
        public double TotalCost { get; set; }
    }
}