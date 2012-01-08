﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FacturaElectronica.Common.Contracts;
using FacturaElectronica.Common.Services;
using FacturaElectronica.Ui.Web.Code;
using FacturaElectronica.Ui.Web.Code.Security;

namespace FacturaElectronica.Ui.Web.Pages
{
    public partial class ClienteDetalle : BasePage
    {
        private const string pagListado = "ClienteListado.aspx";

        public ClienteDto clienteCurrent
        {
            get
            {
                return (ClienteDto)ViewState["Cliente"];
            }
            set
            {
                ViewState["Cliente"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.HasPermissionToSeeMe(Operaciones.ClienteDetalle);
            if(this.BaseMaster.EsCliente)
                this.lblTituloPagina.Text = "Mis Datos";
            try
            {
                IClienteService clienteService = ServiceFactory.GetClienteService();

                if (!this.IsPostBack)
                {
                    if (this.BaseMaster.EsCliente)
                    {
                        clienteCurrent = clienteService.ObtenerCliente(this.BaseMaster.ClienteId);                        
                    }
                    else if (this.Request.QueryString["Id"] == null)
                    {
                        clienteCurrent = new ClienteDto();
                    }
                    else
                    {
                        clienteCurrent = clienteService.ObtenerCliente(Convert.ToInt64(this.Request.QueryString["Id"]));
                    }

                    Bindear();
                }

            }
            catch (Exception ex)
            {
                ExceptionManager.Instance.HandleException(ex);
            }

        }

        private void Bindear()
        {
            if (clienteCurrent.Id != 0)
            {
                this.txtRazonSocial.Text = clienteCurrent.RazonSocial;
                this.txtCuit.Text = clienteCurrent.CUIT.ToString();
                this.txtNombreContacto.Text = clienteCurrent.NombreContacto;
                this.txtApellidoContacto.Text = clienteCurrent.ApellidoContacto;
                this.txtEmailContacto.Text = clienteCurrent.EmailContacto;
                this.txtNombreContactoSecundario.Text = clienteCurrent.NombreContactoSecundario;
                this.txtApellidoContactoSecundario.Text = clienteCurrent.ApellidoContactoSecundario;
                this.txtEmailContactoSecundario.Text = clienteCurrent.EmailContactoSecundario;

                // asigno usuario
                ISeguridadService seguridadSvc = ServiceFactory.GetSecurityService();
                UsuarioDto usuario = seguridadSvc.ObtenerUsuario(clienteCurrent.UsuarioId);
                if (usuario != null)
                {
                    this.txtNombreUsuario.Text = usuario.NombreUsuario;
                    this.pnlUsuario.Visible = true;
                }
                else
                {
                    this.pnlUsuario.Visible = false;
                }
            }
        }

        private void SetData()
        {
            clienteCurrent.RazonSocial = this.txtRazonSocial.Text.Trim();
            clienteCurrent.CUIT = long.Parse(this.txtCuit.Text.Trim());
            clienteCurrent.NombreContacto = this.txtNombreContacto.Text.Trim();
            clienteCurrent.ApellidoContacto = this.txtApellidoContacto.Text.Trim();
            clienteCurrent.EmailContacto = this.txtEmailContacto.Text.Trim();
            clienteCurrent.NombreContactoSecundario = this.txtNombreContactoSecundario.Text.Trim();
            clienteCurrent.ApellidoContactoSecundario = this.txtApellidoContactoSecundario.Text.Trim();
            clienteCurrent.EmailContactoSecundario = this.txtEmailContactoSecundario.Text.Trim();
        }

        private void Save()
        {
            IClienteService clienteSvc = ServiceFactory.GetClienteService();
            if (clienteCurrent.Id == 0)
            {
                clienteCurrent = clienteSvc.CrearCliente(clienteCurrent);
            }
            else
            {
                clienteCurrent = clienteSvc.EditarCliente(clienteCurrent);
            }
        }

        private bool Validar()
        {
            return true;
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    SetData();
                    if (Validar())
                    {
                        Save();
                    }

                    ShowMessage("Los datos fueron grabados con éxito", WebMessageType.Notification);
                }
                catch (Exception ex)
                {
                    ExceptionManager.Instance.HandleException(ex);
                }
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect(pagListado, true);
            }
            catch (Exception ex)
            {
                ExceptionManager.Instance.HandleException(ex);
            }
        }

        protected void cvCuit_ServerValidate(object source, ServerValidateEventArgs args)
        {
            //args.IsValid = ValidaCuit(args.Value);
            args.IsValid = true;
        }

        /// <summary>
        /// Calcula el dígito verificador dado un CUIT completo o sin él.
        /// </summary>
        /// <param name="cuit">El CUIT como String sin guiones</param>
        /// <returns>El valor del dígito verificador calculado.</returns>
        public static int CalcularDigitoCuit(string cuit)
        {
            int[] mult = new[] { 5, 4, 3, 2, 7, 6, 5, 4, 3, 2 };
            char[] nums = cuit.ToCharArray();
            int total = 0;
            for (int i = 0; i < mult.Length; i++)
            {
                total += int.Parse(nums[i].ToString()) * mult[i];
            }
            var resto = total % 11;
            return resto == 0 ? 0 : resto == 1 ? 9 : 11 - resto;
        }

        /// <summary>
        /// Valida el CUIT ingresado.
        /// </summary>
        /// <param name="cuit" />Número de CUIT como string con o sin guiones
        /// <returns>True si el CUIT es válido y False si no.</returns>
        public static bool ValidaCuit(string cuit)
        {
            if (cuit == null)
            {
                return false;
            }
            if (cuit.Length != 11)
            {
                return false;
            }
            else
            {
                int calculado = CalcularDigitoCuit(cuit);
                int digito = int.Parse(cuit.Substring(10));
                return calculado == digito;
            }
        }

    }
}