using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FacturaElectronica.Ui.Web.Code;
using System.Configuration;

namespace FacturaElectronica.Ui.Web
{
    public partial class _Default : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void lnkEndesa_Click(object sender, EventArgs e)
        {
            this.SignOut();
            this.Response.Redirect(ConfigurationManager.AppSettings["siteCompany"]);            
        }
    }
}
