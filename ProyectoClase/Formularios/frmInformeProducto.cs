using ProyectoClase.Clases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoClase.Formularios
{
    public partial class frmInformeProducto : Form
    {
        cConexion cn;
        SqlCommand cmd;
        SqlDataAdapter da;
        DataTable dt;

        public frmInformeProducto()
        {
            InitializeComponent();
            cn = new cConexion();
        }

        private void frmInformeProducto_Load(object sender, EventArgs e)
        {
            SqlDataAdapter da = new SqlDataAdapter("Select * From tblProducto", cn.AbrirConexion());
            DataTable dt = new DataTable();
            da.Fill(dt);
            dtgProducto.DataSource = dt;
        }
    }
}
