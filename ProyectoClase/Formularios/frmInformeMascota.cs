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
    public partial class frmInformeMascota : Form
    {

        cConexion cn;
        SqlCommand cmd;
        SqlDataAdapter da;
        DataTable dt;

        public frmInformeMascota()
        {
            InitializeComponent();
            cn = new cConexion();
        }

        private void frmInformeMascota_Load(object sender, EventArgs e)
        {
            SqlDataAdapter da = new SqlDataAdapter("Select * From tblMascota", cn.AbrirConexion());
            DataTable dt = new DataTable();
            da.Fill(dt);
            dtgMascota.DataSource = dt;
        }
    }
}
