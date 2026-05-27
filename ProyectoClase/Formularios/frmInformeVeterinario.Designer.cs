namespace ProyectoClase.Formularios
{
    partial class frmInformeVeterinario
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
            this.dtgVeterinario = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dtgVeterinario)).BeginInit();
            this.SuspendLayout();
            // 
            // dtgVeterinario
            // 
            this.dtgVeterinario.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgVeterinario.Location = new System.Drawing.Point(196, 122);
            this.dtgVeterinario.Name = "dtgVeterinario";
            this.dtgVeterinario.Size = new System.Drawing.Size(414, 266);
            this.dtgVeterinario.TabIndex = 0;
            // 
            // frmInformeVeterinario
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dtgVeterinario);
            this.Name = "frmInformeVeterinario";
            this.Text = "frmInformeVeterinario";
            this.Load += new System.EventHandler(this.frmInformeVeterinario_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dtgVeterinario)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dtgVeterinario;
    }
}