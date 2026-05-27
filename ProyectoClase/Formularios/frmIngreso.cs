using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoClase.Formularios
{
    public partial class frmIngreso : Form
    {
        private Form activeForm = null;
        public frmIngreso()
        {
            InitializeComponent();
            disenoPersonal();
        }

        void disenoPersonal()
        {
            pnlCliente.Visible = false;
            pnlProduto.Visible = false;
            pnlVete.Visible = false;
            pnlVete.Visible = false;
            pnlMascota.Visible = false;
            pnlOrden1.Visible = false;
        }

        void ocultarSubmenu()
        {
            if (pnlCliente.Visible)
            {
                pnlCliente.Visible = false;
            }
            if(pnlProduto.Visible)
            {
                pnlProduto.Visible = false;
            }
            if (pnlVete.Visible)
            {
                pnlVete.Visible = false;
            }
            if (pnlOrden1.Visible)
            {
                pnlOrden1.Visible = false;
            }
            if (pnlMascota.Visible)
            {
                pnlMascota.Visible = false;
            }
        }

        void mostrarSubmenu(Panel submenu)
        {
            if(submenu.Visible == false)
            {
                ocultarSubmenu();
                submenu.Visible = true;
            }
        }

        private void abrirPanel(Form frmHijo)
        {
            if(activeForm != null)
            {
                activeForm.Close();
            }
            activeForm = frmHijo;
            frmHijo.TopLevel = false;
            frmHijo.FormBorderStyle = FormBorderStyle.None;
            frmHijo.Dock = DockStyle.Fill;
            pnlCentrar.Controls.Add(frmHijo);
            pnlCentrar.Tag = frmHijo;
            frmHijo.BringToFront();
            frmHijo.Show();
        }

        private void btnCliente_Click(object sender, EventArgs e)
        {
            mostrarSubmenu(pnlCliente);
        }

        private void btnIngreso_Click(object sender, EventArgs e)
        {
            abrirPanel(new frmCliente());
            ocultarSubmenu();
        }

        private void btnProducto_Click(object sender, EventArgs e)
        {
            mostrarSubmenu(pnlProduto);
        }

        private void btnIngresoP_Click(object sender, EventArgs e)
        {
            abrirPanel(new frmProducto());
            ocultarSubmenu();
        }


        private void btnVeterinario_Click(object sender, EventArgs e)
        {
            mostrarSubmenu(pnlVete);
        }

        private void btnIngresoVete_Click(object sender, EventArgs e)
        {
            abrirPanel(new frmVeterinario());
            ocultarSubmenu();
        }

        private void btnMascota_Click(object sender, EventArgs e)
        {
            mostrarSubmenu(pnlMascota);
        }

        private void btnIngresoMasco_Click(object sender, EventArgs e)
        {
            abrirPanel(new frmMascota());
            ocultarSubmenu();
        }

        private void btnIngresoOrden_Click(object sender, EventArgs e)
        {
            abrirPanel(new frmOrdenCompra());
            ocultarSubmenu();
        }

        private void btnInformeOrden_Click(object sender, EventArgs e)
        {
            abrirPanel(new frmInformeOrden());
            ocultarSubmenu();
        }

        private void btnOrden_Click(object sender, EventArgs e)
        {
            mostrarSubmenu(pnlOrden1);
        }

        private void btnInforme_Click(object sender, EventArgs e)
        {
            abrirPanel(new frmInformeCliente());
            ocultarSubmenu();
        }

        private void btnInformeP_Click(object sender, EventArgs e)
        {
            abrirPanel(new frmInformeProducto());
            ocultarSubmenu();
        }

        private void btnInformeVete_Click(object sender, EventArgs e)
        {
            abrirPanel(new frmInformeVeterinario());
            ocultarSubmenu();
        }

        private void btnInformeMasco_Click(object sender, EventArgs e)
        {
            abrirPanel(new frmInformeMascota());
            ocultarSubmenu();
        }

        private void btnCitas_Click(object sender, EventArgs e)
        {
            
        }

        private void btnIngresoCitas_Click(object sender, EventArgs e)
        {
            abrirPanel(new frmCitas());
            ocultarSubmenu();
        }

        private void btnInformeCitas_Click(object sender, EventArgs e)
        {
            abrirPanel(new frmInformeCitas());
            ocultarSubmenu();
        }
    }
}
