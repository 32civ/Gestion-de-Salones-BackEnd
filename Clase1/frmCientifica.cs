using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Clase1
{
    public partial class fmrCientifica : Form
    {
        double numero1 = 0;
        double numero2 = 0;
        double Resultado = 0;
        int suma, res, mul, div = 0;

        public fmrCientifica()
        {
            InitializeComponent();
        }

        private void fmrCientifica_Load(object sender, EventArgs e)
        {

        }

        private void btnMas_Click(object sender, EventArgs e)
        {
            //double.TryParse(txtResultado.Text, out double numero1);
            numero1 = Convert.ToDouble(txtResultado.Text);
            txtResultado.Clear();
            suma = 1;
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            txtResultado.Text = txtResultado.Text + 1.ToString();
            
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            txtResultado.Text = txtResultado.Text + 2.ToString();
            
        }

        private void btnIgual_Click(object sender, EventArgs e)
        {
            double.TryParse(txtResultado.Text, out double numero2);
            
            if (suma == 1 ){
                Resultado = numero1 + numero2;
                txtResultado.Text = Resultado.ToString();
                suma = 0;
            }
            if (res == 1)
            {
                Resultado = numero1 - numero2;
                txtResultado.Text = Resultado.ToString();
                res = 0;
            }
            if (mul == 1)
            {
                Resultado = numero1 * numero2;
                txtResultado.Text = Resultado.ToString();
                mul = 0;
            }
            if (div == 1)
            {
                Resultado = numero1 / numero2;
                txtResultado.Text = Resultado.ToString();
                div = 0;
            }
        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            if (txtResultado.Text.Length > 0)
            {
                txtResultado.Text = txtResultado.Text.Remove(txtResultado.Text.Length - 1, 1);
            }
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            txtResultado.Text = txtResultado.Text + 3.ToString();
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            txtResultado.Text = txtResultado.Text + 4.ToString();
        }

        private void btn5_Click(object sender, EventArgs e)
        {
            txtResultado.Text = txtResultado.Text + 5.ToString();
        }

        private void btn6_Click(object sender, EventArgs e)
        {
            txtResultado.Text = txtResultado.Text + 6.ToString();
        }

        private void btn7_Click(object sender, EventArgs e)
        {
            txtResultado.Text = txtResultado.Text + 7.ToString();
        }

        private void btnMenos_Click(object sender, EventArgs e)
        {
            numero1 = Convert.ToDouble(txtResultado.Text);
            txtResultado.Clear();
            res = 1;
        }

        private void btnMulti_Click(object sender, EventArgs e)
        {
            numero1 = Convert.ToDouble(txtResultado.Text);
            txtResultado.Clear();
            mul = 1;
        }

        private void btnDivi_Click(object sender, EventArgs e)
        {
            numero1 = Convert.ToDouble(txtResultado.Text);
            txtResultado.Clear();
            div = 1;
        }

        private void btnPunto_Click(object sender, EventArgs e)
        {
            txtResultado.Text = txtResultado.Text + ",".ToString();
        }

        private void btn8_Click(object sender, EventArgs e)
        {
            txtResultado.Text = txtResultado.Text + 8.ToString();
        }

        private void btn9_Click(object sender, EventArgs e)
        {
            txtResultado.Text = txtResultado.Text + 9.ToString();
        }

        private void btn0_Click(object sender, EventArgs e)
        {
            txtResultado.Text = txtResultado.Text + 0.ToString();
        }
    }
}
