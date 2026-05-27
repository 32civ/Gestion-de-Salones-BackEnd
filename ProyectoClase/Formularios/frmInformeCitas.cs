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
    public partial class frmInformeCitas : Form
    {
        cConexion cn; // variable para llamar la clase
        SqlCommand cmd; // para llamar los comandos

        public frmInformeCitas()
        {
            InitializeComponent();
            cn = new cConexion();
        }
        

        // Carga los veterinarios en el ComboBox
        void cargarComboVeterinarios()
        {
            cmd = new SqlCommand("select cedulaV, nombre from tblVeterinaria", cn.AbrirConexion());
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            cmbVeterinarios.DataSource = dt;
            cmbVeterinarios.DisplayMember = "nombre";  // texto visible
            cmbVeterinarios.ValueMember = "cedulaV";    // valor que se usa en la consulta
            cmbVeterinarios.SelectedIndex = -1;         //Ni me acuerdo
        }

        // CConsultar al veterinario en las fechas seleccionadas
        void cargarCitas()
        {
            if (cmbVeterinarios.SelectedValue == null || cmbVeterinarios.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor selecciona un veterinario.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (dtpDesde.Value.Date > dtpHasta.Value.Date)
            {
                MessageBox.Show("La fecha 'Desde' no puede ser mayor que 'Hasta'.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //El diablo vuelto codigo sql
            string query = @"SELECT 
                        tblCliente.nombre           AS Cliente,
                        tblMascota.nombreMascota     AS Mascota,
                        CONVERT(varchar, tblCita.fechaCita, 103) AS Fecha,              
                        tblCita.horaCita             AS Hora,
                        CASE tblCita.estado 
                            WHEN 1 THEN 'Activa' 
                            ELSE 'Cancelada' 
                        END                          AS Estado
                     FROM tblCita
                     JOIN tblCliente  ON tblCliente.cedula    = tblCita.cedula
                     JOIN tblMascota  ON tblMascota.IdMascota = tblCita.IdMascota
                     WHERE tblCita.cedulaV = @vet
                       AND CAST(tblCita.fechaCita AS DATE) BETWEEN @desde AND @hasta
                     ORDER BY tblCita.fechaCita, tblCita.horaCita";

            cmd = new SqlCommand(query, cn.AbrirConexion());
            cmd.Parameters.AddWithValue("@vet", cmbVeterinarios.SelectedValue.ToString());
            cmd.Parameters.AddWithValue("@desde", dtpDesde.Value.Date);
            cmd.Parameters.AddWithValue("@hasta", dtpHasta.Value.Date);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cn.CerrarConexion();

            dtgMuestra.DataSource = dt;

            if (dt.Rows.Count == 0)
                MessageBox.Show("No se encontraron citas para el veterinario en ese rango de fechas.",
                    "Sin resultados", MessageBoxButtons.OK, MessageBoxIcon.Information); 
            return;
        }

        private void frmInformeCitas_Load_1(object sender, EventArgs e)
        {
            cargarComboVeterinarios();
        }

        private void btnConsulta_Click(object sender, EventArgs e)
        {
            cargarCitas();
        }
    }
}