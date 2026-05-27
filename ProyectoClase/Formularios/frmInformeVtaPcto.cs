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
    public partial class frmInformeVtaPcto : Form
    {

        cConexion cn;
        SqlCommand cmd;
        SqlDataAdapter da;
        DataTable dt;

        public frmInformeVtaPcto()
        {
            InitializeComponent();
            cn=new cConexion();
        }

        private void frmInformeVtaPcto_Load(object sender, EventArgs e)
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT tblProducto.idProducto as Código, descripcion as Descripción, sum(cantidad*tblDetalleOrden.vlrUnitario) as Total FROM tblProducto inner join tblDetalleOrden on tblProducto.idProducto = tblDetalleOrden.idProducto group by tblProducto.idProducto, descripcion", cn.AbrirConexion());
            DataTable dt = new DataTable();
            da.Fill(dt);
            dtgVentas.DataSource = dt;
            chtVentas.Series.Clear();

            chtVentas.DataSource = dt;
            chtVentas.Series.Add("Ventas");
            chtVentas.Series["Ventas"].XValueMember = "Descripción";
            chtVentas.Series["Ventas"].YValueMembers = "Total";
            //Formato del grid
            dtgVentas.Columns["Descripción"].Width = 150;
            dtgVentas.Columns["Total"].DefaultCellStyle.Format = "N2"; // 2 decimales
            dtgVentas.Columns["Total"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            // Tipo de gráfico
            chtVentas.Series["Ventas"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;

            // Porcentaje dentro de la torta
            chtVentas.Series["Ventas"].IsValueShownAsLabel = true;
            chtVentas.Series["Ventas"].Label = "#PERCENT{P0}";

            // Texto de la leyenda 
            chtVentas.Series["Ventas"].LegendText = "#VALX";

            // Leyenda abajo
            chtVentas.Legends[0].Enabled = true;
            chtVentas.Legends[0].Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            chtVentas.Legends[0].Alignment = StringAlignment.Center;

            // Activar 3D
            chtVentas.ChartAreas[0].Area3DStyle.Enable3D = true;
            chtVentas.ChartAreas[0].Area3DStyle.Inclination = 45;
            chtVentas.ChartAreas[0].Area3DStyle.Rotation = 45;
            chtVentas.ChartAreas[0].Area3DStyle.PointDepth = 100;

            // Título
            chtVentas.Titles.Clear();
            chtVentas.Titles.Add("Ventas por Producto");
        }
    }
}
