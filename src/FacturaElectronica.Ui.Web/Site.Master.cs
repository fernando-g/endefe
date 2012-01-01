using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FacturaElectronica.Ui.Web.Code;

namespace FacturaElectronica.Ui.Web
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.pnlMessages.CssClass = "pnlMessagesClass displayNone";
        }
        public void ShowMessage(string message, WebMessageType type)
        {
            ShowMessage("Mensaje", message, type);
        }

        public void ShowMessage(string title, string message, WebMessageType type)
        {
            this.pnlMessages.CssClass = "pnlMessagesClass displayBlock";
            this.txtErrorTitle.Text = title;
            this.txtMessages.Text = message;
        }

    }
}
