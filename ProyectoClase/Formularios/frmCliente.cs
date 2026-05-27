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


namespace ProyectoClase
{
    public partial class frmCliente : Form
    {
        cConexion cn; //varible para llamar la clase
        SqlCommand cmd; //para llamar los comandos
        SqlDataAdapter da;//se necesita para las consultas
        DataTable dt, dtBusqueda;//especifica como se van a traer los datos
        int i, contador, boton;//var. indice, contador y boton


        public frmCliente()
        {
            InitializeComponent();
            boton = 0;
            cargarClientes();
        }

        void cargarClientes()
        {
            i = 0;
            cn = new cConexion();
            cmd = new SqlCommand("select * from tblCliente", cn.AbrirConexion());
            da = new SqlDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0 )
            {
                llenar(dt, i);
            }
        }

        private void frmCliente_Load(object sender, EventArgs e)
        {

        }

        void habilitar()
        {
            txtCedula.Enabled = true;
            txtDirecc.Enabled = true;
            txtNombre.Enabled = true;
            txtTele.Enabled = true;
        }

        void deshabilitar()
        {
            txtCedula.Enabled = false;
            txtDirecc.Enabled = false;
            txtNombre.Enabled = false;
            txtTele.Enabled = false;
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
            llenar (dt, i);
        }

        private void btnIngreso_Click(object sender, EventArgs e)
        {
            boton = 1;
            limpiar();
            habilitar();
            txtCedula.Focus();
        }

        private void btnModifi_Click(object sender, EventArgs e)
        {
            boton = 2;
            limpiar();
            habilitar();
            txtCedula.Focus();
        }

        private void btnConsulta_Click(object sender, EventArgs e)
        {
            boton = 3;
            limpiar();
            txtCedula.Enabled = true;
            txtCedula.Focus();
        }

        private void btnRetiro_Click(object sender, EventArgs e)
        {
            boton = 4;
            limpiar();
            txtCedula.Enabled = true;
            txtCedula.Focus();
        }

        private void txtCedula_Leave(object sender, EventArgs e)
        {
            cmd = new SqlCommand("select * from tblCliente where cedula='" + txtCedula.Text + "'", cn.AbrirConexion());
            da = new SqlDataAdapter(cmd);
            dtBusqueda = new DataTable();
            da.Fill(dtBusqueda);

            //Ingrese
            if (boton == 1)
            {
                if (dtBusqueda.Rows.Count > 0)
                {
                    MessageBox.Show("El cliente ya existe");
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
                    btnGuardar.Visible=true;
                }
                else
                {
                    MessageBox.Show("El cliente No existe");
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
                    MessageBox.Show("El cliente No existe");
                }
            }
            //Retiro

            if(boton == 4)
            {
                if(dtBusqueda.Rows.Count > 0)
                {
                    llenar(dtBusqueda, 0);
                    var result = MessageBox.Show("Realmente desea borrarlo?", "Mensaje de alerta",MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        SqlCommand comando = new SqlCommand("Delete from tblCliente where cedula='" + txtCedula.Text + "'", cn.AbrirConexion());
                        comando.ExecuteNonQuery();
                        MessageBox.Show("Cliente retirado");
                        cargarClientes();
                    }
                }
            }

        }
   

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (boton ==1)
            {
                cmd = new SqlCommand("insert into tblCliente values('" + txtCedula.Text + "','" + txtNombre.Text + "','" + txtDirecc.Text + "','" + txtTele.Text + "')", cn.AbrirConexion());
                cmd.ExecuteNonQuery();
                MessageBox.Show("Cliente Agregado");
            }

            if (boton == 2)
            {
                cmd = new SqlCommand("update tblCliente set nombre= '" + txtNombre.Text + "', direccion='" + txtDirecc.Text + "',telefono= '" + txtTele.Text + "' where cedula='" + txtCedula.Text + "'", cn.AbrirConexion());
                cmd.ExecuteNonQuery();//Se utiliza para modificar la tabla
                MessageBox.Show("Cliente Modificado");
            }
            btnGuardar.Visible = false;

        }

        void limpiar()
        {
            txtCedula.Clear();
            txtDirecc.Clear();
            txtNombre.Clear();
            txtTele.Clear();
        }

        void llenar(DataTable dt, int i)
        {
            txtCedula.Text = dt.Rows[i][0].ToString();
            txtNombre.Text = dt.Rows[i][1].ToString();
            txtDirecc.Text = dt.Rows[i][2].ToString();
            txtTele.Text = dt.Rows[i][3].ToString();
            contador = dt.Rows.Count;
        }
    }
}
