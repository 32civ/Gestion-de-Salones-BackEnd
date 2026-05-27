namespace Clase1
{
    partial class frmPizzeria
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbTamaño = new System.Windows.Forms.ComboBox();
            this.chkToci = new System.Windows.Forms.CheckBox();
            this.rdbHawa = new System.Windows.Forms.RadioButton();
            this.btnPagar = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbBorde = new System.Windows.Forms.ComboBox();
            this.cmbMasa = new System.Windows.Forms.ComboBox();
            this.rdbMarga = new System.Windows.Forms.RadioButton();
            this.rdbPepe = new System.Windows.Forms.RadioButton();
            this.rdbSala = new System.Windows.Forms.RadioButton();
            this.rdbAnti = new System.Windows.Forms.RadioButton();
            this.rdbMari = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkCama = new System.Windows.Forms.CheckBox();
            this.chkChampi = new System.Windows.Forms.CheckBox();
            this.chkQueso = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbFormaP = new System.Windows.Forms.ComboBox();
            this.txtPagar = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdbMari);
            this.groupBox1.Controls.Add(this.rdbAnti);
            this.groupBox1.Controls.Add(this.rdbMarga);
            this.groupBox1.Controls.Add(this.rdbPepe);
            this.groupBox1.Controls.Add(this.rdbSala);
            this.groupBox1.Controls.Add(this.rdbHawa);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(73, 306);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(290, 160);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Sabores";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Comic Sans MS", 27.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(323, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(253, 52);
            this.label1.TabIndex = 1;
            this.label1.Text = "PizzaPizzería";
            // 
            // cmbTamaño
            // 
            this.cmbTamaño.FormattingEnabled = true;
            this.cmbTamaño.Items.AddRange(new object[] {
            "Personal",
            "Mediana",
            "Grande",
            "Familiar"});
            this.cmbTamaño.Location = new System.Drawing.Point(226, 147);
            this.cmbTamaño.Name = "cmbTamaño";
            this.cmbTamaño.Size = new System.Drawing.Size(121, 21);
            this.cmbTamaño.TabIndex = 2;
            // 
            // chkToci
            // 
            this.chkToci.AutoSize = true;
            this.chkToci.Location = new System.Drawing.Point(17, 33);
            this.chkToci.Name = "chkToci";
            this.chkToci.Size = new System.Drawing.Size(87, 20);
            this.chkToci.TabIndex = 3;
            this.chkToci.Text = "Tocineta";
            this.chkToci.UseVisualStyleBackColor = true;
            // 
            // rdbHawa
            // 
            this.rdbHawa.AutoSize = true;
            this.rdbHawa.Location = new System.Drawing.Point(6, 31);
            this.rdbHawa.Name = "rdbHawa";
            this.rdbHawa.Size = new System.Drawing.Size(94, 20);
            this.rdbHawa.TabIndex = 4;
            this.rdbHawa.TabStop = true;
            this.rdbHawa.Text = "Hawaiana";
            this.rdbHawa.UseVisualStyleBackColor = true;
            // 
            // btnPagar
            // 
            this.btnPagar.Location = new System.Drawing.Point(531, 370);
            this.btnPagar.Name = "btnPagar";
            this.btnPagar.Size = new System.Drawing.Size(234, 23);
            this.btnPagar.TabIndex = 5;
            this.btnPagar.Text = "Pagar";
            this.btnPagar.UseVisualStyleBackColor = true;
            this.btnPagar.Click += new System.EventHandler(this.btnPagar_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::Clase1.Properties.Resources.PAnquequesJAke;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(157, 41);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(152, 68);
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(68, 146);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 22);
            this.label2.TabIndex = 7;
            this.label2.Text = "Tamaño:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(501, 318);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(150, 22);
            this.label3.TabIndex = 8;
            this.label3.Text = "Forma de Pago:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(68, 242);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(150, 22);
            this.label4.TabIndex = 9;
            this.label4.Text = "Tipo de Borde:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(69, 193);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(140, 22);
            this.label5.TabIndex = 10;
            this.label5.Text = "Tipo de Masa:";
            // 
            // cmbBorde
            // 
            this.cmbBorde.FormattingEnabled = true;
            this.cmbBorde.Items.AddRange(new object[] {
            "Borde Queso",
            "Borde Tocineta"});
            this.cmbBorde.Location = new System.Drawing.Point(226, 245);
            this.cmbBorde.Name = "cmbBorde";
            this.cmbBorde.Size = new System.Drawing.Size(121, 21);
            this.cmbBorde.TabIndex = 11;
            // 
            // cmbMasa
            // 
            this.cmbMasa.FormattingEnabled = true;
            this.cmbMasa.Items.AddRange(new object[] {
            "Delgada",
            "Romana"});
            this.cmbMasa.Location = new System.Drawing.Point(226, 196);
            this.cmbMasa.Name = "cmbMasa";
            this.cmbMasa.Size = new System.Drawing.Size(121, 21);
            this.cmbMasa.TabIndex = 12;
            // 
            // rdbMarga
            // 
            this.rdbMarga.AutoSize = true;
            this.rdbMarga.Location = new System.Drawing.Point(153, 34);
            this.rdbMarga.Name = "rdbMarga";
            this.rdbMarga.Size = new System.Drawing.Size(91, 20);
            this.rdbMarga.TabIndex = 13;
            this.rdbMarga.TabStop = true;
            this.rdbMarga.Text = "Margarita";
            this.rdbMarga.UseVisualStyleBackColor = true;
            // 
            // rdbPepe
            // 
            this.rdbPepe.AutoSize = true;
            this.rdbPepe.Location = new System.Drawing.Point(6, 111);
            this.rdbPepe.Name = "rdbPepe";
            this.rdbPepe.Size = new System.Drawing.Size(88, 20);
            this.rdbPepe.TabIndex = 14;
            this.rdbPepe.TabStop = true;
            this.rdbPepe.Text = "Peperoni";
            this.rdbPepe.UseVisualStyleBackColor = true;
            // 
            // rdbSala
            // 
            this.rdbSala.AutoSize = true;
            this.rdbSala.Location = new System.Drawing.Point(6, 67);
            this.rdbSala.Name = "rdbSala";
            this.rdbSala.Size = new System.Drawing.Size(73, 20);
            this.rdbSala.TabIndex = 15;
            this.rdbSala.TabStop = true;
            this.rdbSala.Text = "Salami";
            this.rdbSala.UseVisualStyleBackColor = true;
            // 
            // rdbAnti
            // 
            this.rdbAnti.AutoSize = true;
            this.rdbAnti.Location = new System.Drawing.Point(153, 111);
            this.rdbAnti.Name = "rdbAnti";
            this.rdbAnti.Size = new System.Drawing.Size(103, 20);
            this.rdbAnti.TabIndex = 16;
            this.rdbAnti.TabStop = true;
            this.rdbAnti.Text = "Antioqueña";
            this.rdbAnti.UseVisualStyleBackColor = true;
            // 
            // rdbMari
            // 
            this.rdbMari.AutoSize = true;
            this.rdbMari.Location = new System.Drawing.Point(153, 70);
            this.rdbMari.Name = "rdbMari";
            this.rdbMari.Size = new System.Drawing.Size(86, 20);
            this.rdbMari.TabIndex = 17;
            this.rdbMari.TabStop = true;
            this.rdbMari.Text = "Marinera";
            this.rdbMari.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkQueso);
            this.groupBox2.Controls.Add(this.chkChampi);
            this.groupBox2.Controls.Add(this.chkCama);
            this.groupBox2.Controls.Add(this.chkToci);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(514, 147);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(261, 119);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Adiciones";
            // 
            // chkCama
            // 
            this.chkCama.AutoSize = true;
            this.chkCama.Location = new System.Drawing.Point(17, 71);
            this.chkCama.Name = "chkCama";
            this.chkCama.Size = new System.Drawing.Size(105, 20);
            this.chkCama.TabIndex = 4;
            this.chkCama.Text = "Camarones";
            this.chkCama.UseVisualStyleBackColor = true;
            // 
            // chkChampi
            // 
            this.chkChampi.AutoSize = true;
            this.chkChampi.Location = new System.Drawing.Point(143, 33);
            this.chkChampi.Name = "chkChampi";
            this.chkChampi.Size = new System.Drawing.Size(120, 20);
            this.chkChampi.TabIndex = 5;
            this.chkChampi.Text = "Champiñones";
            this.chkChampi.UseVisualStyleBackColor = true;
            // 
            // chkQueso
            // 
            this.chkQueso.AutoSize = true;
            this.chkQueso.Location = new System.Drawing.Point(143, 70);
            this.chkQueso.Name = "chkQueso";
            this.chkQueso.Size = new System.Drawing.Size(71, 20);
            this.chkQueso.TabIndex = 6;
            this.chkQueso.Text = "Queso";
            this.chkQueso.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(498, 418);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(150, 22);
            this.label6.TabIndex = 14;
            this.label6.Text = "Total a Pagar:";
            // 
            // cmbFormaP
            // 
            this.cmbFormaP.FormattingEnabled = true;
            this.cmbFormaP.Items.AddRange(new object[] {
            "Efectivo",
            "Tarjeta",
            "QR"});
            this.cmbFormaP.Location = new System.Drawing.Point(654, 321);
            this.cmbFormaP.Name = "cmbFormaP";
            this.cmbFormaP.Size = new System.Drawing.Size(121, 21);
            this.cmbFormaP.TabIndex = 15;
            // 
            // txtPagar
            // 
            this.txtPagar.Enabled = false;
            this.txtPagar.Location = new System.Drawing.Point(654, 418);
            this.txtPagar.Name = "txtPagar";
            this.txtPagar.Size = new System.Drawing.Size(118, 20);
            this.txtPagar.TabIndex = 16;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.panel1.Location = new System.Drawing.Point(428, 106);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(10, 428);
            this.panel1.TabIndex = 17;
            // 
            // frmPizzeria
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(800, 527);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.txtPagar);
            this.Controls.Add(this.cmbFormaP);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.cmbMasa);
            this.Controls.Add(this.cmbBorde);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnPagar);
            this.Controls.Add(this.cmbTamaño);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmPizzeria";
            this.Text = "frmPizzeria";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbTamaño;
        private System.Windows.Forms.CheckBox chkToci;
        private System.Windows.Forms.RadioButton rdbHawa;
        private System.Windows.Forms.Button btnPagar;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rdbMari;
        private System.Windows.Forms.RadioButton rdbAnti;
        private System.Windows.Forms.RadioButton rdbMarga;
        private System.Windows.Forms.RadioButton rdbPepe;
        private System.Windows.Forms.RadioButton rdbSala;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbBorde;
        private System.Windows.Forms.ComboBox cmbMasa;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkQueso;
        private System.Windows.Forms.CheckBox chkChampi;
        private System.Windows.Forms.CheckBox chkCama;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbFormaP;
        private System.Windows.Forms.TextBox txtPagar;
        private System.Windows.Forms.Panel panel1;
    }
}