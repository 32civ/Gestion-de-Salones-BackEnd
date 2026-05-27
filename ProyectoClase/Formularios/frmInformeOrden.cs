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
    public partial class frmInformeOrden : Form
    {
        cConexion cn;
        SqlCommand cmd;
        SqlDataAdapter da, dax;
        DataTable dt, dtx;
        public frmInformeOrden()
        {
            InitializeComponent();
            cn = new cConexion();
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("select nombre from tblCliente where cedula='" + c.Text + "'", cn.AbrirConexion());
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            DataTable dtx = new DataTable();

            if (dt.Rows.Count != 0)
            {
                txtNombre.Text = dt.Rows[0][0].ToString();
                SqlDataAdapter dax = new SqlDataAdapter(@"SELECT RIGHT(fechaOrden, 4) AS Año,SUBSTRING(fechaOrden, CHARINDEX('de ', fechaOrden) + 3, CHARINDEX(' de', fechaOrden, CHARINDEX('de ', fechaOrden) + 3) - (CHARINDEX('de ', fechaOrden) + 3)) AS Mes,SUM(total) AS TotalMes FROM tblOrden WHERE cedula = '" + c.Text + @"' GROUP BY RIGHT(fechaOrden, 4),    SUBSTRING(fechaOrden, CHARINDEX('de ', fechaOrden) + 3, CHARINDEX(' de', fechaOrden, CHARINDEX('de ', fechaOrden) + 3) - (CHARINDEX('de ', fechaOrden) + 3)) ORDER BY Año", cn.AbrirConexion());
                dax.Fill(dtx);

                dtgOrden.DataSource = dtx;

                chtOrdenes.Series.Clear();
                chtOrdenes.ChartAreas.Clear();

                chtOrdenes.ChartAreas.Add("Area1");

                chtOrdenes.Series.Add("Ordenes");
                chtOrdenes.Series["Ordenes"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bar;

                chtOrdenes.Series["Ordenes"].XValueMember = "Mes";
                chtOrdenes.Series["Ordenes"].YValueMembers = "TotalMes";

                chtOrdenes.DataSource = dtx;
                chtOrdenes.DataBind();
                chtOrdenes.Legends.Clear();
            }
        }

    }
    
}
