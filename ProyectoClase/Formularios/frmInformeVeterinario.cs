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
    public partial class frmInformeVeterinario : Form
    {
        cConexion cn;
        SqlCommand cmd;
        SqlDataAdapter da;
        DataTable dt;

        public frmInformeVeterinario()
        {
            InitializeComponent();
            cn = new cConexion();
        }

        private void frmInformeVeterinario_Load(object sender, EventArgs e)
        {
            SqlDataAdapter da = new SqlDataAdapter("Select * From tblVeterinaria", cn.AbrirConexion());
            DataTable dt = new DataTable();
            da.Fill(dt);
            dtgVeterinario.DataSource = dt;
        }
    }
}
