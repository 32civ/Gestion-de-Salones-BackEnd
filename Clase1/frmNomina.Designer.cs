namespace Clase1
{
    partial class frmNomina
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
            this.cmbHijos = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtSalarioBasicoH = new System.Windows.Forms.TextBox();
            this.txtNumeroH = new System.Windows.Forms.TextBox();
            this.txtEPS = new System.Windows.Forms.TextBox();
            this.txtSubsidioT = new System.Windows.Forms.TextBox();
            this.txtSalarioBruto = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.rdbSi = new System.Windows.Forms.RadioButton();
            this.rdbNo = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.btnCalcular = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.txtSalario = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbHijos
            // 
            this.cmbHijos.FormattingEnabled = true;
            this.cmbHijos.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "Diomedes"});
            this.cmbHijos.Location = new System.Drawing.Point(613, 253);
            this.cmbHijos.Name = "cmbHijos";
            this.cmbHijos.Size = new System.Drawing.Size(121, 21);
            this.cmbHijos.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(238, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(270, 28);
            this.label1.TabIndex = 2;
            this.label1.Text = "Nómina de Empleados";
            // 
            // txtNombre
            // 
            this.txtNombre.Location = new System.Drawing.Point(190, 151);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(193, 20);
            this.txtNombre.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(107, 151);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 15);
            this.label2.TabIndex = 5;
            this.label2.Text = "Nombre:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(107, 196);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(140, 15);
            this.label3.TabIndex = 6;
            this.label3.Text = "Salario Básico Hora:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(107, 253);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(105, 15);
            this.label4.TabIndex = 7;
            this.label4.Text = "Número Horas:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(107, 299);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(96, 15);
            this.label5.TabIndex = 8;
            this.label5.Text = "Salario bruto:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(107, 346);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(150, 15);
            this.label6.TabIndex = 9;
            this.label6.Text = "Subsidio de Tranporte";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(107, 393);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 15);
            this.label7.TabIndex = 10;
            this.label7.Text = "EPS(4%)";
            // 
            // txtSalarioBasicoH
            // 
            this.txtSalarioBasicoH.Location = new System.Drawing.Point(276, 196);
            this.txtSalarioBasicoH.Name = "txtSalarioBasicoH";
            this.txtSalarioBasicoH.Size = new System.Drawing.Size(107, 20);
            this.txtSalarioBasicoH.TabIndex = 11;
            // 
            // txtNumeroH
            // 
            this.txtNumeroH.Location = new System.Drawing.Point(276, 248);
            this.txtNumeroH.Name = "txtNumeroH";
            this.txtNumeroH.Size = new System.Drawing.Size(107, 20);
            this.txtNumeroH.TabIndex = 12;
            // 
            // txtEPS
            // 
            this.txtEPS.Enabled = false;
            this.txtEPS.Location = new System.Drawing.Point(276, 388);
            this.txtEPS.Name = "txtEPS";
            this.txtEPS.Size = new System.Drawing.Size(107, 20);
            this.txtEPS.TabIndex = 14;
            // 
            // txtSubsidioT
            // 
            this.txtSubsidioT.Enabled = false;
            this.txtSubsidioT.Location = new System.Drawing.Point(276, 346);
            this.txtSubsidioT.Name = "txtSubsidioT";
            this.txtSubsidioT.Size = new System.Drawing.Size(107, 20);
            this.txtSubsidioT.TabIndex = 15;
            // 
            // txtSalarioBruto
            // 
            this.txtSalarioBruto.Enabled = false;
            this.txtSalarioBruto.Location = new System.Drawing.Point(276, 299);
            this.txtSalarioBruto.Name = "txtSalarioBruto";
            this.txtSalarioBruto.Size = new System.Drawing.Size(107, 20);
            this.txtSalarioBruto.TabIndex = 16;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(612, 97);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(122, 15);
            this.label8.TabIndex = 17;
            this.label8.Text = "Fondo Empleados";
            // 
            // rdbSi
            // 
            this.rdbSi.AutoSize = true;
            this.rdbSi.Location = new System.Drawing.Point(17, 19);
            this.rdbSi.Name = "rdbSi";
            this.rdbSi.Size = new System.Drawing.Size(35, 17);
            this.rdbSi.TabIndex = 18;
            this.rdbSi.TabStop = true;
            this.rdbSi.Text = "SI";
            this.rdbSi.UseVisualStyleBackColor = true;
            // 
            // rdbNo
            // 
            this.rdbNo.AutoSize = true;
            this.rdbNo.Location = new System.Drawing.Point(87, 19);
            this.rdbNo.Name = "rdbNo";
            this.rdbNo.Size = new System.Drawing.Size(41, 17);
            this.rdbNo.TabIndex = 19;
            this.rdbNo.TabStop = true;
            this.rdbNo.Text = "NO";
            this.rdbNo.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rdbNo);
            this.panel1.Controls.Add(this.rdbSi);
            this.panel1.Location = new System.Drawing.Point(604, 133);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(143, 54);
            this.panel1.TabIndex = 20;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(610, 226);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(121, 15);
            this.label9.TabIndex = 21;
            this.label9.Text = "Números de Hijos";
            // 
            // btnCalcular
            // 
            this.btnCalcular.Location = new System.Drawing.Point(638, 299);
            this.btnCalcular.Name = "btnCalcular";
            this.btnCalcular.Size = new System.Drawing.Size(75, 23);
            this.btnCalcular.TabIndex = 22;
            this.btnCalcular.Text = "Calcular";
            this.btnCalcular.UseVisualStyleBackColor = true;
            this.btnCalcular.Click += new System.EventHandler(this.btnCalcular_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(626, 351);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(88, 15);
            this.label10.TabIndex = 23;
            this.label10.Text = "Salario Neto";
            // 
            // txtSalario
            // 
            this.txtSalario.Enabled = false;
            this.txtSalario.Location = new System.Drawing.Point(621, 378);
            this.txtSalario.Name = "txtSalario";
            this.txtSalario.Size = new System.Drawing.Size(107, 20);
            this.txtSalario.TabIndex = 24;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.AliceBlue;
            this.panel2.Location = new System.Drawing.Point(541, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(10, 450);
            this.panel2.TabIndex = 25;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::Clase1.Properties.Resources.Captura_de_pantalla_2026_02_04_130921;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(110, 34);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(122, 78);
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // frmNomina
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.txtSalario);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.btnCalcular);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtSalarioBruto);
            this.Controls.Add(this.txtSubsidioT);
            this.Controls.Add(this.txtEPS);
            this.Controls.Add(this.txtNumeroH);
            this.Controls.Add(this.txtSalarioBasicoH);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.txtNombre);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbHijos);
            this.Name = "frmNomina";
            this.Text = "frmNomina";
            this.Load += new System.EventHandler(this.frmNomina_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbHijos;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtSalarioBasicoH;
        private System.Windows.Forms.TextBox txtNumeroH;
        private System.Windows.Forms.TextBox txtEPS;
        private System.Windows.Forms.TextBox txtSubsidioT;
        private System.Windows.Forms.TextBox txtSalarioBruto;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.RadioButton rdbSi;
        private System.Windows.Forms.RadioButton rdbNo;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnCalcular;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtSalario;
        private System.Windows.Forms.Panel panel2;
    }
}