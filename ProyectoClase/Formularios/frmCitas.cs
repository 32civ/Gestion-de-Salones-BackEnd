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
    public partial class frmCitas : Form
    {
        cConexion cn; //varible para llamar la clase
        SqlCommand cmd; //para llamar los comandos
        SqlDataAdapter da;//se necesita para las consultas
        DataTable dt, dtBusqueda;//especifica como se van a traer los datos
        int i, sw, boton;//var. indice, contador y boton

        public frmCitas()
        {
            InitializeComponent();
            cn = new cConexion();
            boton = 0;
            mntFecha.MinDate = DateTime.Today;

        }

        private void txtCedula_Leave(object sender, EventArgs e)
        {
            if (txtCedula.Text == "")
            {
                MessageBox.Show("Ingrese una cédula");
                txtCedula.Focus();
                return;
            }
                

            if (!cargarCliente(txtCedula.Text))
            {
                MessageBox.Show("El cliente no existe");
                txtNombre.Clear();
                cmbMascota.Items.Clear();
                return;
            }

            cargarMascota(txtCedula.Text);

            if (boton == 4)
            {
                cmd = new SqlCommand(
                    "select IdCita, fechaCita, horaCita from tblCita where cedula='" + txtCedula.Text + "' and estado=1",
                cn.AbrirConexion());

                da = new SqlDataAdapter(cmd);
                dtBusqueda = new DataTable();
                da.Fill(dtBusqueda);

                if (dtBusqueda.Rows.Count == 0)
                {
                    MessageBox.Show("Este cliente no tiene citas activas.");
                    limpiar();
                    return;
                }

                // Construir lista de citas para mostrar al usuario
                string listaCitas = "Citas activas:\n";
                for (int j = 0; j < dtBusqueda.Rows.Count; j++)
                {
                    listaCitas += $"{j + 1}. Fecha: {dtBusqueda.Rows[j]["fechaCita"]} " +
                                  $"- Hora: {dtBusqueda.Rows[j]["horaCita"]}\n";
                }

                // Si solo tiene una cita, cancelarla directamente
                if (dtBusqueda.Rows.Count == 1)
                {
                    string idCita = dtBusqueda.Rows[0]["IdCita"].ToString();
                    var r = MessageBox.Show(
                        $"¿Desea cancelar la cita del {dtBusqueda.Rows[0]["fechaCita"]} " +
                        $"a las {dtBusqueda.Rows[0]["horaCita"]}?",
                        "Confirmar cancelación", MessageBoxButtons.YesNo);

                    if (r == DialogResult.Yes)
                    {
                        cmd = new SqlCommand(
                            "update tblCita set estado=0 where IdCita='" + idCita + "'",
                            cn.AbrirConexion());
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Cita cancelada exitosamente.");
                        limpiar();
                    }
                }
                else
                {
                    // Si tiene varias, mostrarlas y pedir que elija
                    MessageBox.Show(listaCitas + "\nSeleccione la fecha y hora en el calendario y la lista.");
                }
            }
        }

        

        bool cargarCliente(string cedula)
        {
            cmd = new SqlCommand(
                "select nombre from tblCliente where cedula='" + cedula + "'",
                cn.AbrirConexion());
            da = new SqlDataAdapter(cmd);
            dtBusqueda = new DataTable();
            da.Fill(dtBusqueda);

            if (dtBusqueda.Rows.Count > 0)
            {
                txtNombre.Text = dtBusqueda.Rows[0][0].ToString();
                return true;
            }
            else
            {
                txtNombre.Clear();
                return false;
            }
        }

        List<string> obtenerMascotas(string cedula)
        {
            List<string> mascotas = new List<string>();

            cmd = new SqlCommand(
                "select nombreMascota from tblMascota where cedula='" + cedula + "'",
                cn.AbrirConexion());
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

            List<string> mascotas = obtenerMascotas(cedula);

            if (mascotas.Count > 0)
            {
                foreach (var m in mascotas)
                {
                    cmbMascota.Items.Add(m);
                }
                cmbMascota.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("Este cliente no tiene mascotas registradas.");
            }
        }

        string obtenerIdMascota(string cedula, string nombreMascota)
        {
            cmd = new SqlCommand(
                "select IdMascota from tblMascota where cedula='" + cedula +
                "' and nombreMascota='" + nombreMascota + "'",
                cn.AbrirConexion());

            object result = cmd.ExecuteScalar();
            return result != null ? result.ToString() : "";
        }

        private void mntFecha_DateChanged(object sender, DateRangeEventArgs e)
        {
            cargarDisponibles();
        }

        List<string> obtenerVeterinarios(string fecha, string hora)
        {
            List<string> vets = new List<string>();

            cmd = new SqlCommand(
                "select nombre from tblVeterinaria where cedulaV NOT IN " +
                "(select cedulaV from tblCita where fechaCita='" + fecha +
                "' and horaCita='" + hora + "' and estado=1)",
                cn.AbrirConexion());

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                vets.Add(reader["nombre"].ToString());
            }

            reader.Close();
            return vets;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {

            if (cmbMascota.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione una mascota.");
                return;
            }

            if (lstHorario.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione una hora.");
                return;
            }

            if (cmbDisponible.Items.Count == 0 || cmbDisponible.SelectedIndex == -1)
            {
                MessageBox.Show("No hay veterinarios disponibles para esa fecha y hora.");
                return;
            }

            string fecha = mntFecha.SelectionStart.ToString("yyyy-MM-dd");
            string hora = lstHorario.Text;
            string idMascota = obtenerIdMascota(txtCedula.Text, cmbMascota.Text);
            string cedulaVet = obtenerCedulaVet(cmbDisponible.Text);

            
            cmd = new SqlCommand(
                "select count(*) from tblCita where cedula='" + txtCedula.Text +
                "' and fechaCita='" + fecha +
                "' and horaCita='" + hora + "' and estado=1",
                cn.AbrirConexion());

            int citasExistentes = (int)cmd.ExecuteScalar();

            if (citasExistentes > 0)
            {
                MessageBox.Show("Este cliente ya tiene una cita activa para esa fecha y hora.");
                return;
            }

            // Insertar la cita
            cmd = new SqlCommand(
                "insert into tblCita (cedula, IdMascota, fechaCita, horaCita, cedulaV, estado) values('" +
                txtCedula.Text + "','" +
                idMascota + "','" +
                fecha + "','" +
                hora + "','" +
                cedulaVet + "',1)",
                cn.AbrirConexion());

            cmd.ExecuteNonQuery();
            MessageBox.Show("Cita guardada exitosamente.");
            limpiar();
        }

        

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            boton = 4;
            limpiar();
            txtCedula.Focus();
        }

        void cargarDisponibles()
        {
            cmbDisponible.Items.Clear();

            string fecha = mntFecha.SelectionStart.ToString("yyyy-MM-dd");
            string hora = lstHorario.Text;

            List<string> vets = obtenerVeterinarios(fecha, hora);

            if (vets.Count > 0)
            {
                foreach (var v in vets)
                {
                    cmbDisponible.Items.Add(v);
                }
                cmbDisponible.SelectedIndex = 0;
            }
        }

        string obtenerCedulaVet(string nombre)
        {
            cmd = new SqlCommand(
                "select cedulaV from tblVeterinaria where nombre='" + nombre + "'",
                cn.AbrirConexion());

            object result = cmd.ExecuteScalar();
            return result != null ? result.ToString() : "";
        }

        void limpiar()
        {
            txtCedula.Clear();
            txtNombre.Clear();

            cmbMascota.Items.Clear();
            cmbMascota.Text = "";
            cmbMascota.SelectedIndex = -1;

            cmbDisponible.Items.Clear();
            cmbDisponible.Text = "";
            cmbDisponible.SelectedIndex = -1;

            boton = 0; 
        }
    }
}
