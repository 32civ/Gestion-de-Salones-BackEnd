namespace Clase1
{
    partial class frmCalculadora
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
            this.txtNum1 = new System.Windows.Forms.TextBox();
            this.btnSu = new System.Windows.Forms.Button();
            this.txtNum2 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnRe = new System.Windows.Forms.Button();
            this.btnDi = new System.Windows.Forms.Button();
            this.btnMul = new System.Windows.Forms.Button();
            this.txtResul = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(53, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Numero 1:";
            // 
            // txtNum1
            // 
            this.txtNum1.Location = new System.Drawing.Point(129, 40);
            this.txtNum1.Name = "txtNum1";
            this.txtNum1.Size = new System.Drawing.Size(100, 20);
            this.txtNum1.TabIndex = 1;
            // 
            // btnSu
            // 
            this.btnSu.Location = new System.Drawing.Point(298, 40);
            this.btnSu.Name = "btnSu";
            this.btnSu.Size = new System.Drawing.Size(75, 23);
            this.btnSu.TabIndex = 2;
            this.btnSu.Text = "Suma";
            this.btnSu.UseVisualStyleBackColor = true;
            this.btnSu.Click += new System.EventHandler(this.btnSu_Click);
            // 
            // txtNum2
            // 
            this.txtNum2.Location = new System.Drawing.Point(129, 102);
            this.txtNum2.Name = "txtNum2";
            this.txtNum2.Size = new System.Drawing.Size(100, 20);
            this.txtNum2.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(53, 107);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Numero 2:";
            // 
            // btnRe
            // 
            this.btnRe.Location = new System.Drawing.Point(298, 81);
            this.btnRe.Name = "btnRe";
            this.btnRe.Size = new System.Drawing.Size(75, 23);
            this.btnRe.TabIndex = 6;
            this.btnRe.Text = "Resta";
            this.btnRe.UseVisualStyleBackColor = true;
            this.btnRe.Click += new System.EventHandler(this.btnRe_Click);
            // 
            // btnDi
            // 
            this.btnDi.Location = new System.Drawing.Point(298, 162);
            this.btnDi.Name = "btnDi";
            this.btnDi.Size = new System.Drawing.Size(75, 23);
            this.btnDi.TabIndex = 8;
            this.btnDi.Text = "Division";
            this.btnDi.UseVisualStyleBackColor = true;
            this.btnDi.Click += new System.EventHandler(this.btnDi_Click);
            // 
            // btnMul
            // 
            this.btnMul.Location = new System.Drawing.Point(298, 121);
            this.btnMul.Name = "btnMul";
            this.btnMul.Size = new System.Drawing.Size(75, 23);
            this.btnMul.TabIndex = 7;
            this.btnMul.Text = "Multiplicacion";
            this.btnMul.UseVisualStyleBackColor = true;
            this.btnMul.Click += new System.EventHandler(this.btnMul_Click);
            // 
            // txtResul
            // 
            this.txtResul.Enabled = false;
            this.txtResul.Location = new System.Drawing.Point(129, 165);
            this.txtResul.Name = "txtResul";
            this.txtResul.Size = new System.Drawing.Size(100, 20);
            this.txtResul.TabIndex = 10;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(53, 170);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Resultado:";
            // 
            // frmCalculadora
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 290);
            this.Controls.Add(this.txtResul);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnDi);
            this.Controls.Add(this.btnMul);
            this.Controls.Add(this.btnRe);
            this.Controls.Add(this.txtNum2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnSu);
            this.Controls.Add(this.txtNum1);
            this.Controls.Add(this.label1);
            this.Name = "frmCalculadora";
            this.Text = "frmCalculadora";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtNum1;
        private System.Windows.Forms.Button btnSu;
        private System.Windows.Forms.TextBox txtNum2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnRe;
        private System.Windows.Forms.Button btnDi;
        private System.Windows.Forms.Button btnMul;
        private System.Windows.Forms.TextBox txtResul;
        private System.Windows.Forms.Label label2;
    }
}