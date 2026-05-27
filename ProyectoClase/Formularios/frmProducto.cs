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
    public partial class frmProducto : Form
    {

        cConexion cn; //varible para llamar la clase
        SqlCommand cmd; //para llamar los comandos
        SqlDataAdapter da;//se necesita para las consultas
        DataTable dt, dtBusqueda;//especifica como se van a traer los datos
        int i, contador, boton;//var. indice, contador y boton

        public frmProducto()
        {
            InitializeComponent();
            boton = 0;
            cargarProducto();
        }

        void cargarProducto()
        {
            i = 0;
            cn = new cConexion();
            cmd = new SqlCommand("select * from tblProducto", cn.AbrirConexion());
            da = new SqlDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                llenar(dt, i);
            }
        }

        private void btnIngreso_Click(object sender, EventArgs e)
        {
            boton = 1;
            limpiar();
            habilitar();
            txtProducto.Focus();
        }

        private void frmProducto_Load(object sender, EventArgs e)
        {

        }

        void habilitar()
        {
            txtDescrip.Enabled = true;
            txtExistencia.Enabled = true;
            txtProducto.Enabled = true;
            txtStockMaximo.Enabled = true;
            txtStockMini.Enabled = true;
            txtUnitario.Enabled = true;
        }

        private void btnModifi_Click(object sender, EventArgs e)
        {
            boton = 2;
            limpiar();
            habilitar();
            txtProducto.Focus();
        }

        private void btnConsulta_Click(object sender, EventArgs e)
        {
            boton = 3;
            limpiar();
            txtProducto.Enabled = true;
            txtProducto.Focus();
        }

        private void btnRetiro_Click(object sender, EventArgs e)
        {
            boton = 4;
            limpiar();
            txtProducto.Enabled = true;
            txtProducto.Focus();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
           //Agregar Producto
            if (boton == 1)
            {
                cmd = new SqlCommand("insert into tblProducto values('" + txtProducto.Text + "','" + txtDescrip.Text + "','" + txtUnitario.Text + "','" + txtExistencia.Text + "','" + txtStockMaximo.Text + "','" + txtStockMini.Text + "')", cn.AbrirConexion());
                cmd.ExecuteNonQuery();
                MessageBox.Show("Producto Agregado");
            }

            //Modificar Producto
            if (boton == 2)
            {
                cmd = new SqlCommand("update tblProducto set descripcion= '" + txtDescrip.Text + "', vlrUnitario='" + Convert.ToDouble(txtUnitario.Text) + "', existencias= '" + Convert.ToDouble(txtExistencia.Text) + "', stockMax='" + Convert.ToDouble(txtStockMaximo.Text) + "', stockMin='" + Convert.ToDouble(txtStockMini.Text) + "' where IdProducto='" + txtProducto.Text + "'", cn.AbrirConexion());
                cmd.ExecuteNonQuery();//Se utiliza para modificar la tabla
                MessageBox.Show("Producto Modificado");
            }
            btnGuardar.Visible = false;
        }

        private void txtProducto_Leave(object sender, EventArgs e)
        {
            cmd = new SqlCommand("select * from tblProducto where IdProducto='" + txtProducto.Text + "'", cn.AbrirConexion());
            da = new SqlDataAdapter(cmd);
            dtBusqueda = new DataTable();
            da.Fill(dtBusqueda);

            //Ingrese
            if (boton == 1)
            {
                if (dtBusqueda.Rows.Count > 0)
                {
                    MessageBox.Show("El Producto ya existe");
                    llenar(dtBusqueda, 0);
                }
                else
                {
                    btnGuardar.Visible = true;
                }
            }

            //Modificar
            if (boton == 2)
            {
                if (dtBusqueda.Rows.Count > 0)
                {
                    llenar(dtBusqueda, 0);
                    btnGuardar.Visible = true;
                }
                else
                {
                    MessageBox.Show("El Producto No existe");
                }
            }

            //Consulta
            if (boton == 3)
            {
                if (dtBusqueda.Rows.Count > 0)
                {
                    llenar(dtBusqueda, 0);
                }
                else
                {
                    MessageBox.Show("El Producto No existe");
                }
            }
            //Retiro

            if (boton == 4)
            {
                if (dtBusqueda.Rows.Count > 0)
                {
                    llenar(dtBusqueda, 0);
                    var result = MessageBox.Show("Realmente desea borrarlo?", "Mensaje de alerta", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        SqlCommand comando = new SqlCommand("Delete from tblProducto where IdProducto='" + txtProducto.Text + "'", cn.AbrirConexion());
                        comando.ExecuteNonQuery();
                        MessageBox.Show("Cliente retirado");
                        cargarProducto();
                    }
                }
            }
        }

        private void btnPrimero_Click(object sender, EventArgs e)
        {
            i = 0;
            llenar(dt, i);
        }

        private void btnAnterior_Click(object sender, EventArgs e)
        {
            i--;
            if (i == -1)
            {
                MessageBox.Show("Estas en el primer cliente");
                i++;
            }
            llenar(dt, i);
        }

        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            i++;
            if (i == contador)
            {
                MessageBox.Show("Estas en el ultimo cliente");
                i--;
            }
            llenar(dt, i);
        }

        private void btnUltimo_Click(object sender, EventArgs e)
        {
            i = contador - 1;
            llenar(dt, i);
        }

        void deshabilitar()
        {
            txtDescrip.Enabled = false;
            txtExistencia.Enabled = false;
            txtProducto.Enabled = false;
            txtStockMaximo.Enabled = false;
            txtStockMini.Enabled = false;
            txtUnitario.Enabled = false;
        }

        void limpiar()
        {
            txtDescrip.Clear();
            txtExistencia.Clear();
            txtProducto.Clear();
            txtStockMaximo.Clear();
            txtStockMini.Clear();
            txtUnitario.Clear();
        }

        void llenar(DataTable dt, int i)
        {
            txtProducto.Text = dt.Rows[i][0].ToString();
            txtDescrip.Text = dt.Rows[i][1].ToString();
            txtUnitario.Text = dt.Rows[i][2].ToString();
            txtExistencia.Text = dt.Rows[i][3].ToString();
            txtStockMaximo.Text = dt.Rows[i][4].ToString();
            txtStockMini.Text = dt.Rows[i][5].ToString();
            contador = dt.Rows.Count;
        }

    }
}
