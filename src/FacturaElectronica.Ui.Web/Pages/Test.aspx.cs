using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FacturaElectronica.Ui.Web.Pages
{
    public partial class Test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void HideButton_Click(object sender, EventArgs e)
        {
            OptionDropDownList.Visible = !OptionDropDownList.Visible;
        }
    }
}