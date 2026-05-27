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
    public partial class frmPizzeria : Form
    {
        public frmPizzeria()
        {
            InitializeComponent();
        }

        private void btnPagar_Click(object sender, EventArgs e)
        {
            double tamaño = 0, masa = 0, sabores = 0, adiciones = 0, pago = 0;


            switch (cmbTamaño.SelectedIndex)
            {
                case 0:
                    tamaño = 5000;
                    break;
                case 1:
                    tamaño = 10000;
                    break;
                case 2:
                    tamaño = 15000;
                    break;
                case 3:
                    tamaño = 30000;
                    break;
            }

            switch (cmbMasa.SelectedIndex)
            {
                case 0:
                    masa = 0;
                    break;
                case 1:
                    masa = tamaño*0.02;
                    break;
            }

            if (rdbAnti.Checked)//
            {
                sabores = tamaño * 0.1;
            }
            if (rdbHawa.Checked)//
            {
                sabores = 0;// creo que esto sobra
            }
            if (rdbMarga.Checked)//
            {
                sabores = tamaño * 0.01;
            }
            if (rdbMari.Checked)//
            {
                sabores = tamaño * 0.15;
            }
            if (rdbPepe.Checked)//
            {
                sabores = tamaño * 0.04;
            }
            if (rdbSala.Checked)//
            {
                sabores = tamaño * 0.03;
            }

            if (chkToci.Checked)
            {
                adiciones = adiciones + 8000;
            }
            if (chkChampi.Checked)
            {
                adiciones = adiciones + 4000;
            }
            if (chkCama.Checked)
            {
                adiciones = adiciones + 10000;
            }
            if (chkQueso.Checked)
            {
                adiciones = adiciones + 5000;
            }

            pago = (tamaño + masa + sabores + adiciones + pago);

            if (cmbFormaP.SelectedIndex == 0)
            {
                pago = pago - (pago * 0.1);
            }

            txtPagar.Text = pago.ToString();


            
        }
    }
}
