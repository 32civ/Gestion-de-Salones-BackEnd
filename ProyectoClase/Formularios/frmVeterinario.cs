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
    public partial class frmVeterinario : Form
    {

        cConexion cn; //varible para llamar la clase
        SqlCommand cmd; //para llamar los comandos
        SqlDataAdapter da;//se necesita para las consultas
        DataTable dt, dtBusqueda;//especifica como se van a traer los datos
        int i, contador, boton;//var. indice, contador y boton

        public frmVeterinario()
        {
            InitializeComponent();
            boton = 0;
            cargarVeterinario();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (boton == 1)
            {
                cmd = new SqlCommand("insert into tblVeterinario values('" + txtCedula.Text + "','" + txtNombreVe.Text + "','" + txtTele.Text + "','" + txtEspecia.Text +  "','" + txtSalario + "')", cn.AbrirConexion());
                cmd.ExecuteNonQuery();
                MessageBox.Show("Veterinario Agregado");
            }

            if (boton == 2)
            {
                cmd = new SqlCommand("update tblVeterinario set nombre= '" + txtNombreVe.Text + "', telefono='" + txtTele.Text + "',especializacion= '" + txtEspecia.Text + "',salario= '" + txtSalario.Text + "' where cedula='" + txtCedula.Text + "'", cn.AbrirConexion());
                cmd.ExecuteNonQuery();//Se utiliza para modificar la tabla
                MessageBox.Show("Cliente Modificado");
            }
            btnGuardar.Visible = false;
        }

        private void txtCedula_Leave(object sender, EventArgs e)
        {
            cmd = new SqlCommand("select * from tblVeterinaria where cedula='" + txtCedula.Text + "'", cn.AbrirConexion());
            da = new SqlDataAdapter(cmd);
            dtBusqueda = new DataTable();
            da.Fill(dtBusqueda);

            //Ingrese
            if (boton == 1)
            {
                if (dtBusqueda.Rows.Count > 0)
                {
                    MessageBox.Show("El Veterinario ya existe");
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
                    MessageBox.Show("El Veterinario No existe");
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
                    MessageBox.Show("El Veterinario No existe");
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
                        SqlCommand comando = new SqlCommand("Delete from tblVeterinaria where cedula='" + txtCedula.Text + "'", cn.AbrirConexion());
                        comando.ExecuteNonQuery();
                        MessageBox.Show("Veterinario retirado");
                        cargarVeterinario();
                    }
                }
            }
        }

        private void btnRetiro_Click(object sender, EventArgs e)
        {
            boton = 4;
            limpiar();
            txtCedula.Enabled = true;
            txtCedula.Focus();
        }

        private void btnConsulta_Click(object sender, EventArgs e)
        {
            boton = 3;
            limpiar();
            txtCedula.Enabled = true;
            txtCedula.Focus();
        }

        private void btnModifi_Click(object sender, EventArgs e)
        {
            boton = 2;
            limpiar();
            habilitar();
            txtCedula.Focus();
        }

        private void btnIngreso_Click(object sender, EventArgs e)
        {
            boton = 1;
            limpiar();
            habilitar();
            txtCedula.Focus();
        }

        private void btnUltimo_Click(object sender, EventArgs e)
        {
            i = contador - 1;
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

        private void btnPrimero_Click(object sender, EventArgs e)
        {
            i = 0;
            llenar(dt, i);
        }

        void cargarVeterinario()
        {
            i = 0;
            cn = new cConexion();
            cmd = new SqlCommand("select * from tblVeterinaria", cn.AbrirConexion());
            da = new SqlDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                llenar(dt, i);
            }
        }

        void llenar(DataTable dt, int i)
        {
            txtCedula.Text = dt.Rows[i][0].ToString();
            txtNombreVe.Text = dt.Rows[i][1].ToString();
            txtEspecia.Text = dt.Rows[i][2].ToString();
            txtTele.Text = dt.Rows[i][3].ToString();
            txtSalario.Text = dt.Rows[i][4].ToString();
            contador = dt.Rows.Count;
        }

        void limpiar()
        {
            txtCedula.Clear();
            txtNombreVe.Clear();
            txtEspecia.Clear();
            txtTele.Clear();
            txtSalario.Clear();
        }

        void deshabilitar()
        {
            txtCedula.Enabled = false;
            txtNombreVe.Enabled = false;
            txtEspecia.Enabled = false;
            txtTele.Enabled = false;
            txtSalario.Enabled = false;
        }

        void habilitar()
        {
            txtCedula.Enabled = true;
            txtNombreVe.Enabled = true;
            txtEspecia.Enabled = true;
            txtTele.Enabled = true;
            txtSalario.Enabled = true;
        }

    }
}
