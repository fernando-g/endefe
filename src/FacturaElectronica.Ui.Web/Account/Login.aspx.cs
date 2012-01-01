using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using FacturaElectronica.Ui.Web.Code;
using FacturaElectronica.Common.Services;

namespace FacturaElectronica.Ui.Web.Account
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterHyperLink.NavigateUrl = "Register.aspx?ReturnUrl=" + HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
        }

        protected void LoginUser_Authenticate(object sender, AuthenticateEventArgs e)
        {
            ISeguridadService svc = ServiceFactory.GetSecurityService();
            try
            {
                if (svc.AutenticarUsuario(this.LoginUser.UserName, this.LoginUser.Password))
                {
                    FormsAuthentication.RedirectFromLoginPage(this.LoginUser.UserName, false);
                }
            }
            catch (Exception ex)
            {
                CustomValidator validator = new CustomValidator();
                validator.IsValid = false;
                validator.ValidationGroup = "LoginUserValidationGroup";
                validator.ErrorMessage = ex.Message;
                this.Validators.Add(validator);
            }
        }
    }
}
