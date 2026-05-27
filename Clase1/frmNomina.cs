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
    public partial class frmNomina : Form
    {
        public frmNomina()
        {
            InitializeComponent();
        }

        private void frmNomina_Load(object sender, EventArgs e)
        {

        }

        private void btnCalcular_Click(object sender, EventArgs e)
        {
            double salbruto, fondo, hijos, eps = 0;
            salbruto = Convert.ToDouble(txtSalarioBasicoH.Text) * Convert.ToDouble(txtNumeroH.Text);

            if (salbruto < 3000000)
            {
                txtSubsidioT.Text = "180000";
            }
            else
            {
                txtSubsidioT.Text = "0";
            }

            eps = salbruto * 0.04;

            if (rdbSi.Checked)
            {
                fondo = salbruto * 0.01;
            }
            else
            {
                fondo = 0;
            }

            hijos = Convert.ToDouble(cmbHijos.Text) * 100000;

            txtSalarioBruto.Text = salbruto.ToString();
            txtSalario.Text = (salbruto-fondo+hijos-eps+Convert.ToDouble(txtSubsidioT.Text)).ToString();

            txtEPS.Text = eps.ToString();
        }
    }
}
