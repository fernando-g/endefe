using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FacturaElectronica.Common.Services;
using FacturaElectronica.Ui.Win.Administrador.Code;
using FacturaElectronica.Common.Contracts;
using FacturaElectronica.Core.Helpers;

namespace FacturaElectronica.Ui.Win.Administrador
{
    public partial class FormCorridas : Form
    {
        public FormCorridas()
        {
            InitializeComponent();
            this.lblCantidadReg.Text = "0";
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            ICorridaService svc = ServiceFactory.GetCorridaService();
            
            long? corridaId = null;
            bool identificadorValido = true;
            if(this.txtIdentificador.Text.Trim() != string.Empty)
            {
                try
                {
                    corridaId = long.Parse(this.txtIdentificador.Text.Trim());
                }
                catch
                {
                    MessageBox.Show("El Identificador es invalido", "Parametro invalido", MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    this.txtIdentificador.Text = string.Empty;
                    identificadorValido = false;
                }
            }
            if (identificadorValido)
            {
                this.bsCorridas.DataSource = svc.ObtenerCorridas(corridaId, this.dtpFechaDesde.Value.Date, this.dtpFechaHasta.Value.Date.AddDays(1).AddMilliseconds(-1));
                this.gridCorridas.DataSource = this.bsCorridas;
                this.lblCantidadReg.Text = this.bsCorridas.Count.ToString();
            }
        }

        private void gridCorridas_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.gridCorridas.SelectedRows.Count > 0)
            {
                CorridaAutorizacionDto corridaDto = this.gridCorridas.SelectedRows[0].DataBoundItem as CorridaAutorizacionDto;
                FormAutorizadorResultados frm = new FormAutorizadorResultados(corridaDto);
                frm.ShowDialog(this);
            }
        }
    }
}
