namespace ProyectoClase.Formularios
{
    partial class frmInformeCitas
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
            this.dtgMuestra = new System.Windows.Forms.DataGridView();
            this.btnConsulta = new System.Windows.Forms.Button();
            this.cmbVeterinarios = new System.Windows.Forms.ComboBox();
            this.dtpDesde = new System.Windows.Forms.DateTimePicker();
            this.dtpHasta = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dtgMuestra)).BeginInit();
            this.SuspendLayout();
            // 
            // dtgMuestra
            // 
            this.dtgMuestra.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgMuestra.Location = new System.Drawing.Point(126, 125);
            this.dtgMuestra.Name = "dtgMuestra";
            this.dtgMuestra.Size = new System.Drawing.Size(430, 252);
            this.dtgMuestra.TabIndex = 0;
            // 
            // btnConsulta
            // 
            this.btnConsulta.Location = new System.Drawing.Point(642, 325);
            this.btnConsulta.Name = "btnConsulta";
            this.btnConsulta.Size = new System.Drawing.Size(75, 23);
            this.btnConsulta.TabIndex = 1;
            this.btnConsulta.Text = "Consulta";
            this.btnConsulta.UseVisualStyleBackColor = true;
            this.btnConsulta.Click += new System.EventHandler(this.btnConsulta_Click);
            // 
            // cmbVeterinarios
            // 
            this.cmbVeterinarios.FormattingEnabled = true;
            this.cmbVeterinarios.Location = new System.Drawing.Point(161, 58);
            this.cmbVeterinarios.Name = "cmbVeterinarios";
            this.cmbVeterinarios.Size = new System.Drawing.Size(358, 21);
            this.cmbVeterinarios.TabIndex = 2;
            // 
            // dtpDesde
            // 
            this.dtpDesde.Location = new System.Drawing.Point(588, 137);
            this.dtpDesde.Name = "dtpDesde";
            this.dtpDesde.Size = new System.Drawing.Size(200, 20);
            this.dtpDesde.TabIndex = 3;
            // 
            // dtpHasta
            // 
            this.dtpHasta.Location = new System.Drawing.Point(588, 230);
            this.dtpHasta.Name = "dtpHasta";
            this.dtpHasta.Size = new System.Drawing.Size(200, 20);
            this.dtpHasta.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(639, 96);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Desde";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(631, 184);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Hasta";
            // 
            // frmInformeCitas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dtpHasta);
            this.Controls.Add(this.dtpDesde);
            this.Controls.Add(this.cmbVeterinarios);
            this.Controls.Add(this.btnConsulta);
            this.Controls.Add(this.dtgMuestra);
            this.Name = "frmInformeCitas";
            this.Text = "frmInformeCitas";
            this.Load += new System.EventHandler(this.frmInformeCitas_Load_1);
            ((System.ComponentModel.ISupportInitialize)(this.dtgMuestra)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dtgMuestra;
        private System.Windows.Forms.Button btnConsulta;
        private System.Windows.Forms.ComboBox cmbVeterinarios;
        private System.Windows.Forms.DateTimePicker dtpDesde;
        private System.Windows.Forms.DateTimePicker dtpHasta;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}