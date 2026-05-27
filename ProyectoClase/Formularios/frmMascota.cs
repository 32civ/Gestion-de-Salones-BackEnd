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
    public partial class frmMascota : Form
    {

        cConexion cn; //varible para llamar la clase
        SqlCommand cmd; //para llamar los comandos
        SqlDataAdapter da;//se necesita para las consultas
        DataTable dt, dtBusqueda;//especifica como se van a traer los datos
        int i, sw, boton;//var. indice, contador y boton

        public frmMascota()
        {
            InitializeComponent();
            cn = new cConexion();
            sw = 0;
        }

        void buscarDatos(string cedulab, string nombreMascotab)
        {
            cmd = new SqlCommand("select especie, raza, fechaNacimiento, sexo from tblMascota where cedula='" + cedulab + "' and nombreMascota='" + nombreMascotab + "'", cn.AbrirConexion());
            da = new SqlDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0 )//pregunta para saber si lo encuentro
            {
                cmbEspecie.Text= dt.Rows[0][0].ToString();
                txtRaza.Text= dt.Rows[0][1].ToString();
                mskFechaN.Text= dt.Rows[0][2].ToString();
                txtSexo.Text= dt.Rows[0][3].ToString();
            }
        }

        bool cargarCliente(string cedulab)
        {
            cmd = new SqlCommand("select nombre from tblCliente where cedula='" + cedulab +"'",cn.AbrirConexion());
            da = new SqlDataAdapter(cmd);
            dtBusqueda = new DataTable();
            da.Fill(dtBusqueda);
            if(dtBusqueda.Rows.Count > 0)
            {
                txtNPropietario.Text = dtBusqueda.Rows[0][0].ToString();
                return true;
            }
            else
            {
                return false;
            }
        }

        private void txtCedula_Leave(object sender, EventArgs e)
        {
            if (!cargarCliente(txtCedula.Text))
            {
                MessageBox.Show("El cliente no existe. Agregalo");
                frmCliente frm = new frmCliente();
                frm.Show();
                return;
            }
            cargarMascota(txtCedula.Text);
        }

        private void cmbMascota_SelectedIndexChanged(object sender, EventArgs e)
        {
            buscarDatos(txtCedula.Text, cmbMascota.Text);
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            //Verifica si la mascota ya existe 
            cmd = new SqlCommand("select count(*) from tblMascota where cedula = @cedula and nombreMascota = @nombre", cn.AbrirConexion());
            cmd.Parameters.AddWithValue("@cedula", txtCedula.Text);
            cmd.Parameters.AddWithValue("@nombre", cmbMascota.Text);
            int cantidad = Convert.ToInt32(cmd.ExecuteScalar());
            if (cantidad > 0)
            {
                MessageBox.Show("La mascota ya esta registrada");
                return;
            }

            //Insertar la mascota
            cmd = new SqlCommand("insert into tblMascota values (@cedula, @nombre, @especie, @raza, @fecha, @sexo)", cn.AbrirConexion());
            cmd.Parameters.AddWithValue("@cedula", txtCedula.Text);
            cmd.Parameters.AddWithValue("@nombre", cmbMascota.Text);
            cmd.Parameters.AddWithValue("@especie", cmbEspecie.Text);
            cmd.Parameters.AddWithValue("@raza", txtRaza.Text);
            cmd.Parameters.AddWithValue("@fecha", mskFechaN.Text);
            cmd.Parameters.AddWithValue("@sexo", txtSexo.Text);

            cmd.ExecuteNonQuery();
            MessageBox.Show("Mascota guardada correctamente");
            cargarMascota(txtCedula.Text);
        }

        private void txtCedula_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnModifi_Click(object sender, EventArgs e)
        {

        }

        List<string> obtenerMascotas(string cedula)
        {
            List<string> mascotas = new List<string>();
            cmd = new SqlCommand("SELECT nombreMascota, especie from tblMascota where cedula='" + txtCedula.Text + "'", cn.AbrirConexion());
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                mascotas.Add(reader["nombreMascota"].ToString());
            }
            reader.Close();
            return mascotas;
        }

        void cargarMascota(string cedula)
        {
            cmbMascota.Items.Clear();
            cmbEspecie.Items.Clear();
            List<string> mascotas = obtenerMascotas(cedula);
            if (mascotas.Count > 0)
            {
                foreach (var mascota in mascotas)
                { 
                    cmbMascota.Items.Add(mascota);
                }
                cmbMascota.SelectedIndex = 0;
            }
        }

        private void frmMascota_Load(object sender, EventArgs e)
        {

        }
    }
}
