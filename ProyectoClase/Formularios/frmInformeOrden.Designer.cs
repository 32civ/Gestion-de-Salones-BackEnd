namespace ProyectoClase.Formularios
{
    partial class frmInformeOrden
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
            this.dtgOrden = new System.Windows.Forms.DataGridView();
            this.c = new System.Windows.Forms.TextBox();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.chtOrdenes = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.dtgOrden)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chtOrdenes)).BeginInit();
            this.SuspendLayout();
            // 
            // dtgOrden
            // 
            this.dtgOrden.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgOrden.Location = new System.Drawing.Point(32, 163);
            this.dtgOrden.Name = "dtgOrden";
            this.dtgOrden.RowHeadersVisible = false;
            this.dtgOrden.Size = new System.Drawing.Size(275, 224);
            this.dtgOrden.TabIndex = 0;
            // 
            // c
            // 
            this.c.Location = new System.Drawing.Point(190, 52);
            this.c.Name = "c";
            this.c.Size = new System.Drawing.Size(100, 20);
            this.c.TabIndex = 1;
            this.c.Leave += new System.EventHandler(this.textBox1_Leave);
            // 
            // txtNombre
            // 
            this.txtNombre.Location = new System.Drawing.Point(494, 52);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(100, 20);
            this.txtNombre.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(441, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Nombre:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(137, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Cédula:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(348, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(152, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Listado de Ordenes por Cliente";
            // 
            // chtOrdenes
            // 
            chartArea1.Name = "ChartArea1";
            this.chtOrdenes.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chtOrdenes.Legends.Add(legend1);
            this.chtOrdenes.Location = new System.Drawing.Point(338, 163);
            this.chtOrdenes.Name = "chtOrdenes";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bar;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chtOrdenes.Series.Add(series1);
            this.chtOrdenes.Size = new System.Drawing.Size(419, 224);
            this.chtOrdenes.TabIndex = 6;
            this.chtOrdenes.Text = "chart1";
            // 
            // frmInformeOrden
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.chtOrdenes);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtNombre);
            this.Controls.Add(this.c);
            this.Controls.Add(this.dtgOrden);
            this.Name = "frmInformeOrden";
            this.Text = "frmInformeOrden";
            ((System.ComponentModel.ISupportInitialize)(this.dtgOrden)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chtOrdenes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dtgOrden;
        private System.Windows.Forms.TextBox c;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataVisualization.Charting.Chart chtOrdenes;
    }
}