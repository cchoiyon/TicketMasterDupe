using System;
using System.Web.UI;

namespace TicketSales
{
    public partial class Index : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void BtnGoToIndex_Click(object sender, EventArgs e)
        {
            Response.Redirect("Feature/TicketSales.aspx");
        }
    }
}