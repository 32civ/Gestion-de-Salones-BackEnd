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
    public partial class frmCalculadora : Form
    {
        public frmCalculadora()
        {
            InitializeComponent();
        }

        private void btnSu_Click(object sender, EventArgs e)
        {
            txtResul.Text = (Convert.ToDouble(txtNum1.Text) + Convert.ToDouble(txtNum2.Text)).ToString();
        }

        private void btnRe_Click(object sender, EventArgs e)
        {
            txtResul.Text = (Convert.ToDouble(txtNum1.Text) - Convert.ToDouble(txtNum2.Text)).ToString();
        }

        private void btnMul_Click(object sender, EventArgs e)
        {
            txtResul.Text = (Convert.ToDouble(txtNum1.Text) * Convert.ToDouble(txtNum2.Text)).ToString();
        }

        private void btnDi_Click(object sender, EventArgs e)
        {
            txtResul.Text = (Convert.ToDouble(txtNum1.Text) / Convert.ToDouble(txtNum2.Text)).ToString();
        }
    }
}
