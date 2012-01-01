namespace FacturaElectronica.Ui.Win.Administrador
{
    partial class FormPrincipal
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.facturacionElectronicaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.solicitarCAEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.webservicesAFIPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.solicitudesDeAutorizaciónToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.facturacionElectronicaToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(952, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // facturacionElectronicaToolStripMenuItem
            // 
            this.facturacionElectronicaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.solicitarCAEToolStripMenuItem,
            this.webservicesAFIPToolStripMenuItem,
            this.solicitudesDeAutorizaciónToolStripMenuItem});
            this.facturacionElectronicaToolStripMenuItem.Name = "facturacionElectronicaToolStripMenuItem";
            this.facturacionElectronicaToolStripMenuItem.Size = new System.Drawing.Size(42, 20);
            this.facturacionElectronicaToolStripMenuItem.Text = "AFIP";
            // 
            // solicitarCAEToolStripMenuItem
            // 
            this.solicitarCAEToolStripMenuItem.Name = "solicitarCAEToolStripMenuItem";
            this.solicitarCAEToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            this.solicitarCAEToolStripMenuItem.Text = "Solicitud CAE";
            this.solicitarCAEToolStripMenuItem.Click += new System.EventHandler(this.solicitarCAEToolStripMenuItem_Click);
            // 
            // webservicesAFIPToolStripMenuItem
            // 
            this.webservicesAFIPToolStripMenuItem.Name = "webservicesAFIPToolStripMenuItem";
            this.webservicesAFIPToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            this.webservicesAFIPToolStripMenuItem.Text = "Webservices AFIP";
            this.webservicesAFIPToolStripMenuItem.Click += new System.EventHandler(this.webservicesAFIPToolStripMenuItem_Click);
            // 
            // solicitudesDeAutorizaciónToolStripMenuItem
            // 
            this.solicitudesDeAutorizaciónToolStripMenuItem.Name = "solicitudesDeAutorizaciónToolStripMenuItem";
            this.solicitudesDeAutorizaciónToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            this.solicitudesDeAutorizaciónToolStripMenuItem.Text = "Solicitudes de Autorización";
            this.solicitudesDeAutorizaciónToolStripMenuItem.Click += new System.EventHandler(this.solicitudesDeAutorizaciónToolStripMenuItem_Click);
            // 
            // FormPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(952, 704);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Endesa CEMSA - Administrador de Factura Electrónica";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem facturacionElectronicaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem solicitarCAEToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem webservicesAFIPToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem solicitudesDeAutorizaciónToolStripMenuItem;
    }
}

