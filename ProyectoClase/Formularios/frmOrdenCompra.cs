using Microsoft.Office.Interop.Word;
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

using Word = Microsoft.Office.Interop.Word;

namespace ProyectoClase.Formularios
{
    public partial class frmOrdenCompra : Form
    {
        private cConexion cn;
        int nroOrden;

        SqlCommand cmd; //para llamar los comandos
        SqlDataAdapter da;//se necesita para las consultas
        DataTable dt, dtBusqueda;//especifica como se van a traer los datos
        int i, sw, boton;//var. indice, contador y boton

        public frmOrdenCompra()
        {
            InitializeComponent();
            cn = new cConexion();
        }

        private void frmOrdenCompra_Load(object sender, EventArgs e)
        {
            dgvProductos.Columns[3].DefaultCellStyle.Format = "C";
            dgvSeleccionados.Columns[2].DefaultCellStyle.Format = "C";
            dgvSeleccionados.Columns[4].DefaultCellStyle.Format = "C";
            numeral();
            llenarProducto();
        }

        void numeral()
        {
            SqlCommand cmd = new SqlCommand("select max(IdOrden) from tblOrden", cn.AbrirConexion());
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable  dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count != 0)
            {
                nroOrden = int.Parse(dt.Rows[0][0].ToString()) + 1;
                lblNroOrden.Text = nroOrden.ToString();
            }
        }

        void llenarProducto()
        {
            int n = 0;
            SqlCommand cmd = new SqlCommand("select * from tblProducto", cn.AbrirConexion());

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count != 0)
            {
                n = dt.Rows.Count;
                dgvProductos.Rows.Add(n - 1);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dgvProductos.Rows[i].Cells[1].Value = dt.Rows[i][0].ToString();
                    dgvProductos.Rows[i].Cells[2].Value = dt.Rows[i][1].ToString();
                    dgvProductos.Rows[i].Cells[3].Value = dt.Rows[i][2].ToString();

                }
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            for (int i = dgvProductos.Rows.Count - 1; i >= 0; i--)
            {
                DataGridViewRow row = dgvProductos.Rows[i];

                bool seleccionado = Convert.ToBoolean(row.Cells["clm"].Value);

                if (seleccionado)
                {
                    int n = dgvSeleccionados.Rows.Add();

                    dgvSeleccionados.Rows[n].Cells[0].Value = row.Cells[1].Value.ToString();
                    dgvSeleccionados.Rows[n].Cells[1].Value = row.Cells[2].Value.ToString();
                    dgvSeleccionados.Rows[n].Cells[2].Value = row.Cells[3].Value.ToString();

                    //eliminar Del grid
                    dgvProductos.Rows.RemoveAt(i);
                }
            }
        }

        private void dgvSeleccionados_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            decimal cantidad = 0;
            decimal precio_unit = 0;
            decimal precio_total = 0;
            decimal total = 0;

            if (dgvSeleccionados.Columns[e.ColumnIndex].Name == "Cantidad")
            {
                if (dgvSeleccionados.Rows[e.RowIndex].Cells[3].Value != null)
                {
                    decimal.TryParse(dgvSeleccionados.Rows[e.RowIndex].Cells[3].Value.ToString(), out cantidad);

                    decimal.TryParse(dgvSeleccionados.Rows[e.RowIndex].Cells[2].Value.ToString(), System.Globalization.NumberStyles.Currency, System.Globalization.CultureInfo.CurrentCulture, out precio_unit);

                    precio_total = cantidad * precio_unit;
                    dgvSeleccionados.Rows[e.RowIndex].Cells[4].Value = precio_total;
                }
                foreach (DataGridViewRow row in dgvSeleccionados.Rows)
                {
                    if (!row.IsNewRow && row.Cells["Total"].Value != null)
                    {
                        decimal valorFila;

                        decimal.TryParse(
                            row.Cells["Total"].Value.ToString(),
                            System.Globalization.NumberStyles.Currency,
                            System.Globalization.CultureInfo.CurrentCulture,
                            out valorFila);

                        total += valorFila;
                    }

                }
                txtTotal.Text = total.ToString();
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if(txtCedula.Text.Equals(""))
            {
                MessageBox.Show("Ingrese cedula");
            }
            else
            {
                SqlCommand cmd = new SqlCommand("insert into tblOrden values('" + txtCedula.Text + "','" + dtpFecha.Text + "','" + Convert.ToDouble(txtTotal.Text) + "')", cn.AbrirConexion());
                cmd.ExecuteNonQuery();
                for (int i = 0; i < dgvSeleccionados.Rows.Count - 1; i++)
                {
                    SqlCommand cmd1 = new SqlCommand("insert into tblDetalleOrden values('" + nroOrden + "','" + dgvSeleccionados.Rows[i].Cells[0].Value.ToString() + "','" + Convert.ToDouble(dgvSeleccionados.Rows[i].Cells[3].Value.ToString()) + "','" + Convert.ToDouble(dgvSeleccionados.Rows[i].Cells[2].Value.ToString()) + "')", cn.AbrirConexion());
                    cmd1.ExecuteNonQuery();
                }
                MessageBox.Show("Orden Ingresada");
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            numeral();
            txtCedula.Clear();
            txtTotal.Clear();
            txtCedula.Clear();
            dgvSeleccionados.Rows.Clear();//sm = segun el marrano
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            String rutaWord = @"C:\Users\User\source\repos\ProyectoClase\OrdenCompra.docx";
            String rutaPDF = @"C:\Users\User\source\repos\ProyectoClase\OrdenCompra.pdf";

            Word.Application wordApp = new Word.Application();
            Word.Document doc = wordApp.Documents.Add();

            //acceder al encabezado
            Word.Section section = doc.Sections[1];
            Word.HeaderFooter header = section.Headers[Word.WdHeaderFooterIndex.wdHeaderFooterPrimary];

            //Insertar imagen
            Word.InlineShape logo = header.Range.InlineShapes.AddPicture(@"C:\Users\User\source\repos\ProyectoClase\Imagenes\images (1).jpg");

            logo.ScaleHeight = 50;
            logo.ScaleWidth = 50;

            //Alineando el logo a la derecha
            header.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;

            //Título
            wordApp.Selection.Font.Size = 18;
            wordApp.Selection.Font.Bold = 1;
            wordApp.Selection.TypeText("ORDEN DE COMPRA N° " + lblNroOrden.Text);

            wordApp.Selection.TypeParagraph();
            wordApp.Selection.TypeParagraph();

            wordApp.Selection.Font.Size = 12;
            wordApp.Selection.Font.Bold = 0;

            wordApp.Selection.TypeText("Cedula: " + txtCedula.Text);
            wordApp.Selection.TypeParagraph();

            wordApp.Selection.TypeText("Nombre Cliente: " + txtNCliente.Text);
            wordApp.Selection.TypeParagraph();
            wordApp.Selection.TypeParagraph();

            //Crear tabla
            int filas = dgvSeleccionados.Rows.Count;
            int columnas = 5;

            Word.Table tabla = doc.Tables.Add(wordApp.Selection.Range, filas, columnas);
            tabla.Cell(1, 1).Range.Text = "Código";
            tabla.Cell(1, 2).Range.Text = "Descripción";
            tabla.Cell(1, 3).Range.Text = "Valor";
            tabla.Cell(1, 4).Range.Text = "Cantidad";
            tabla.Cell(1, 5).Range.Text = "Total";

            for (int i = 0; i < dgvSeleccionados.Rows.Count - 1; i++)
            {
                tabla.Cell(i + 2, 1).Range.Text = dgvSeleccionados.Rows[i].Cells[0].Value.ToString();
                tabla.Cell(i + 2, 2).Range.Text = dgvSeleccionados.Rows[i].Cells[1].Value.ToString();
                tabla.Cell(i + 2, 3).Range.Text = dgvSeleccionados.Rows[i].Cells[2].Value.ToString();
                tabla.Cell(i + 2, 4).Range.Text = dgvSeleccionados.Rows[i].Cells[3].Value.ToString();
                tabla.Cell(i + 2, 5).Range.Text = dgvSeleccionados.Rows[i].Cells[4].Value.ToString();
            }

            // Guardar Word
            doc.SaveAs(rutaWord);

            //Crrear PDF automáticamente
            doc.ExportAsFixedFormat(rutaPDF, Word.WdExportFormat.wdExportFormatPDF);

            wordApp.Visible = true;

            MessageBox.Show("Documentos Word y PDF  generados correctamente");

        }

        private void txtCedula_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                MessageBox.Show("Solo se permiten numeros", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }

        private void txtCedula_Leave(object sender, EventArgs e)
        {
            if (!cargarCliente(txtCedula.Text))
            {
                MessageBox.Show("El cliente no existe. Debe registrarlo por favor.");
                frmCliente frm = new frmCliente();//llama a tbl cliente
                frm.Show();
                return;
            }
        }


        bool cargarCliente(string cedulab)
        {
            cmd = new SqlCommand("select nombre from tblCliente where cedula= '" + cedulab + "'", cn.AbrirConexion());

            da = new SqlDataAdapter(cmd);
            dtBusqueda = new DataTable();
            da.Fill(dtBusqueda);
            if (dtBusqueda.Rows.Count > 0)
            {
                txtNCliente.Text = dtBusqueda.Rows[0][0].ToString();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
