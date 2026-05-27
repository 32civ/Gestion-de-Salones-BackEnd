namespace ProyectoClase.Formularios
{
    partial class frmInformeVtaPcto
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.dtgVentas = new System.Windows.Forms.DataGridView();
            this.chtVentas = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dtgVentas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chtVentas)).BeginInit();
            this.SuspendLayout();
            // 
            // dtgVentas
            // 
            this.dtgVentas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgVentas.Location = new System.Drawing.Point(58, 139);
            this.dtgVentas.Name = "dtgVentas";
            this.dtgVentas.Size = new System.Drawing.Size(274, 175);
            this.dtgVentas.TabIndex = 0;
            // 
            // chtVentas
            // 
            chartArea1.Name = "ChartArea1";
            this.chtVentas.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chtVentas.Legends.Add(legend1);
            this.chtVentas.Location = new System.Drawing.Point(436, 139);
            this.chtVentas.Name = "chtVentas";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chtVentas.Series.Add(series1);
            this.chtVentas.Size = new System.Drawing.Size(324, 175);
            this.chtVentas.TabIndex = 1;
            this.chtVentas.Text = "chart1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(320, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(156, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Listadp de Ventas por Producto";
            // 
            // frmInformeVtaPcto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chtVentas);
            this.Controls.Add(this.dtgVentas);
            this.Name = "frmInformeVtaPcto";
            this.Text = "frmInformeVtaPcto";
            this.Load += new System.EventHandler(this.frmInformeVtaPcto_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dtgVentas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chtVentas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dtgVentas;
        private System.Windows.Forms.DataVisualization.Charting.Chart chtVentas;
        private System.Windows.Forms.Label label1;
    }
}