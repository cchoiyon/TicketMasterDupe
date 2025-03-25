using System;
using System.Data;

namespace TicketSalesLibrary
{
    public class Event
    {
        public Guid Id { get; set; }
        public string EventName { get; set; }
        public DateTime EventDate { get; set; }
        public string PerformerOrPerformance { get; set; }
        public TypeOfEvent TypeOfEvent { get; set; }
        public string Location { get; set; }
        public int TotalTicketSold { get; set; }
        public double TotalRevenue { get; set; }
        public string SeatingChartURL { get; set; }

        public Event() {}

        public Event(DataRow dataRow)
        {
            Id = new Guid(dataRow["Id"].ToString());
            EventName = dataRow["EventName"].ToString();
            EventDate = Convert.ToDateTime(dataRow["EventDate"]);
            PerformerOrPerformance = dataRow["PerformerOrPerformance"].ToString();
            TypeOfEvent = (TypeOfEvent)Convert.ToInt32(dataRow["TypeOfEvent"].ToString());
            Location = dataRow["Location"].ToString();
            TotalTicketSold = Convert.ToInt32(dataRow["TotalTicketSold"]);
            TotalRevenue = Convert.ToDouble(dataRow["TotalRevenue"].ToString());
            SeatingChartURL = dataRow["SeatingChartURL"].ToString();
        }
    }

    public enum TypeOfEvent
    {
        Sport,
        Concert,
        Theater
    }
}
