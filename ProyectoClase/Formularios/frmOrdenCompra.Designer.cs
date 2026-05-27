namespace ProyectoClase.Formularios
{
    partial class frmOrdenCompra
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCedula = new System.Windows.Forms.TextBox();
            this.dgvProductos = new System.Windows.Forms.DataGridView();
            this.clm = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.clmCodigo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmDescrip = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmVlrUnitario = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dtpFecha = new System.Windows.Forms.DateTimePicker();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtNCliente = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgvSeleccionados = new System.Windows.Forms.DataGridView();
            this.clmCodigo1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmDescripcion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmValorUnitario = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cantidad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Total = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnImprimir = new System.Windows.Forms.Button();
            this.btnNuevo = new System.Windows.Forms.Button();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.txtTotal = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.lblNroOrden = new System.Windows.Forms.Label();
            this.btnAceptar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSeleccionados)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "No.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(280, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Orden Compra";
            // 
            // txtCedula
            // 
            this.txtCedula.Location = new System.Drawing.Point(142, 77);
            this.txtCedula.Name = "txtCedula";
            this.txtCedula.Size = new System.Drawing.Size(100, 20);
            this.txtCedula.TabIndex = 2;
            this.txtCedula.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCedula_KeyPress);
            this.txtCedula.Leave += new System.EventHandler(this.txtCedula_Leave);
            // 
            // dgvProductos
            // 
            this.dgvProductos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProductos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clm,
            this.clmCodigo,
            this.clmDescrip,
            this.clmVlrUnitario});
            this.dgvProductos.Location = new System.Drawing.Point(142, 135);
            this.dgvProductos.Name = "dgvProductos";
            this.dgvProductos.Size = new System.Drawing.Size(437, 115);
            this.dgvProductos.TabIndex = 3;
            // 
            // clm
            // 
            this.clm.HeaderText = "Elegir";
            this.clm.Name = "clm";
            this.clm.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.clm.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // clmCodigo
            // 
            this.clmCodigo.HeaderText = "Código";
            this.clmCodigo.Name = "clmCodigo";
            // 
            // clmDescrip
            // 
            this.clmDescrip.HeaderText = "Descripción";
            this.clmDescrip.Name = "clmDescrip";
            // 
            // clmVlrUnitario
            // 
            this.clmVlrUnitario.HeaderText = "Valor Unitario";
            this.clmVlrUnitario.Name = "clmVlrUnitario";
            // 
            // dtpFecha
            // 
            this.dtpFecha.Location = new System.Drawing.Point(479, 19);
            this.dtpFecha.Name = "dtpFecha";
            this.dtpFecha.Size = new System.Drawing.Size(200, 20);
            this.dtpFecha.TabIndex = 5;
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(-6, 41);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(805, 10);
            this.panel1.TabIndex = 6;
            // 
            // txtNCliente
            // 
            this.txtNCliente.Location = new System.Drawing.Point(443, 80);
            this.txtNCliente.Name = "txtNCliente";
            this.txtNCliente.Size = new System.Drawing.Size(168, 20);
            this.txtNCliente.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(75, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Cedula";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(327, 83);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Nombre Cliente";
            // 
            // panel2
            // 
            this.panel2.Location = new System.Drawing.Point(-51, 106);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(759, 10);
            this.panel2.TabIndex = 10;
            // 
            // dgvSeleccionados
            // 
            this.dgvSeleccionados.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSeleccionados.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmCodigo1,
            this.clmDescripcion,
            this.clmValorUnitario,
            this.Cantidad,
            this.Total});
            this.dgvSeleccionados.Location = new System.Drawing.Point(97, 323);
            this.dgvSeleccionados.Name = "dgvSeleccionados";
            this.dgvSeleccionados.Size = new System.Drawing.Size(528, 115);
            this.dgvSeleccionados.TabIndex = 11;
            this.dgvSeleccionados.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSeleccionados_CellEndEdit);
            // 
            // clmCodigo1
            // 
            this.clmCodigo1.HeaderText = "Código";
            this.clmCodigo1.Name = "clmCodigo1";
            // 
            // clmDescripcion
            // 
            this.clmDescripcion.HeaderText = "Descripcion";
            this.clmDescripcion.Name = "clmDescripcion";
            // 
            // clmValorUnitario
            // 
            this.clmValorUnitario.HeaderText = "Valor Unitario";
            this.clmValorUnitario.Name = "clmValorUnitario";
            // 
            // Cantidad
            // 
            this.Cantidad.HeaderText = "Cantidad";
            this.Cantidad.Name = "Cantidad";
            // 
            // Total
            // 
            this.Total.HeaderText = "Total";
            this.Total.Name = "Total";
            // 
            // btnImprimir
            // 
            this.btnImprimir.Location = new System.Drawing.Point(247, 472);
            this.btnImprimir.Name = "btnImprimir";
            this.btnImprimir.Size = new System.Drawing.Size(75, 23);
            this.btnImprimir.TabIndex = 13;
            this.btnImprimir.Text = "Imprimir";
            this.btnImprimir.UseVisualStyleBackColor = true;
            this.btnImprimir.Click += new System.EventHandler(this.btnImprimir_Click);
            // 
            // btnNuevo
            // 
            this.btnNuevo.Location = new System.Drawing.Point(142, 472);
            this.btnNuevo.Name = "btnNuevo";
            this.btnNuevo.Size = new System.Drawing.Size(75, 23);
            this.btnNuevo.TabIndex = 14;
            this.btnNuevo.Text = "Nuevo";
            this.btnNuevo.UseVisualStyleBackColor = true;
            this.btnNuevo.Click += new System.EventHandler(this.btnNuevo_Click);
            // 
            // btnGuardar
            // 
            this.btnGuardar.Location = new System.Drawing.Point(40, 472);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(75, 23);
            this.btnGuardar.TabIndex = 15;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.UseVisualStyleBackColor = true;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // txtTotal
            // 
            this.txtTotal.Location = new System.Drawing.Point(445, 475);
            this.txtTotal.Name = "txtTotal";
            this.txtTotal.Size = new System.Drawing.Size(180, 20);
            this.txtTotal.TabIndex = 16;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(399, 477);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(31, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "Total";
            // 
            // lblNroOrden
            // 
            this.lblNroOrden.AutoSize = true;
            this.lblNroOrden.Location = new System.Drawing.Point(56, 25);
            this.lblNroOrden.Name = "lblNroOrden";
            this.lblNroOrden.Size = new System.Drawing.Size(0, 13);
            this.lblNroOrden.TabIndex = 18;
            // 
            // btnAceptar
            // 
            this.btnAceptar.BackgroundImage = global::ProyectoClase.Properties.Resources._6401621;
            this.btnAceptar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAceptar.Location = new System.Drawing.Point(283, 256);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(147, 61);
            this.btnAceptar.TabIndex = 4;
            this.btnAceptar.UseVisualStyleBackColor = true;
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            // 
            // frmOrdenCompra
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(703, 519);
            this.Controls.Add(this.lblNroOrden);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtTotal);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.btnNuevo);
            this.Controls.Add(this.btnImprimir);
            this.Controls.Add(this.dgvSeleccionados);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtNCliente);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.dtpFecha);
            this.Controls.Add(this.btnAceptar);
            this.Controls.Add(this.dgvProductos);
            this.Controls.Add(this.txtCedula);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "frmOrdenCompra";
            this.Text = "frmOrdenCompra";
            this.Load += new System.EventHandler(this.frmOrdenCompra_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSeleccionados)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtCedula;
        private System.Windows.Forms.DataGridView dgvProductos;
        private System.Windows.Forms.Button btnAceptar;
        private System.Windows.Forms.DateTimePicker dtpFecha;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtNCliente;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dgvSeleccionados;
        private System.Windows.Forms.Button btnImprimir;
        private System.Windows.Forms.Button btnNuevo;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.TextBox txtTotal;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridViewCheckBoxColumn clm;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmCodigo;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmDescrip;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmVlrUnitario;
        private System.Windows.Forms.Label lblNroOrden;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmCodigo1;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmDescripcion;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmValorUnitario;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cantidad;
        private System.Windows.Forms.DataGridViewTextBoxColumn Total;
    }
}