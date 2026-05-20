using Services;
using BLL;
using BE.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Printing;


namespace Servicios
{
    public partial class AuditoriaBitacora : Form
    {
        BitacoraEventosService bitService = new BitacoraEventosService();
        PrintDocument printDoc = new PrintDocument();
        public AuditoriaBitacora()
        {
            InitializeComponent();
            printDoc.PrintPage += printDoc_PrintPage;

            this.Load += new System.EventHandler(this.AuditoriaBitacora_Load_1);

            this.dgvBitacora.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvBitacora_CellFormatting);
        }

        

        private void CargarGrillaInicial()
        {
            dgvBitacora.DataSource = bitService.obtenerUltimos3Dias();

            if (dgvBitacora.Columns["Id"] != null)
            {
                dgvBitacora.Columns["Id"].Visible = false;
            }
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            DateTime desde = dtpDesde.Value.Date;
            DateTime hasta = dtpHasta.Value.Date.AddDays(1).AddSeconds(-1);

            dgvBitacora.DataSource = bitService.obtenerBitacora(desde, hasta);
        }





        private void printDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            int x = 20;
            int y = 50;
            int rowHeight = 25;

            Font font = new Font("Arial", 8);
            Font fontHeader = new Font("Arial", 8, FontStyle.Bold);

            
            e.Graphics.DrawString("Bitácora de Eventos", new Font("Arial", 14, FontStyle.Bold), Brushes.Black, x, y);
            y += 40;

           
            Dictionary<string, int> columnWidths = new Dictionary<string, int>()
            {
            { "DNI", 70 },
            { "Nombre", 80 },
            { "Apellido", 80 },
            { "Evento", 250 }, 
            { "Criticidad", 70 },
            { "Modulo", 80 },
            { "FechaHora", 130 }
            };
            int defaultWidth = 100; 
            StringFormat trimFormat = new StringFormat();
            trimFormat.Trimming = StringTrimming.EllipsisCharacter;
            trimFormat.LineAlignment = StringAlignment.Center; 

            int currentX = x; 
            foreach (DataGridViewColumn col in dgvBitacora.Columns)
            {
                if (col.Visible)
                {
                    int width = columnWidths.ContainsKey(col.Name) ? columnWidths[col.Name] : defaultWidth;

                    Rectangle cellRect = new Rectangle(currentX, y, width, rowHeight);

                    e.Graphics.DrawString(col.HeaderText, fontHeader, Brushes.Black, cellRect, trimFormat);

                    currentX += width;
                }
            }

            y += rowHeight;
            currentX = x; 

            
            foreach (DataGridViewRow row in dgvBitacora.Rows)
            {
                if (!row.IsNewRow)
                {
                    currentX = x; 

                    for (int i = 0; i < dgvBitacora.Columns.Count; i++)
                    {
                        DataGridViewColumn col = dgvBitacora.Columns[i];

                        if (col.Visible)
                        {
                            string cellText = row.Cells[i].FormattedValue?.ToString() ?? "";

                            int width = columnWidths.ContainsKey(col.Name) ? columnWidths[col.Name] : defaultWidth;

                            Rectangle cellRect = new Rectangle(currentX, y, width, rowHeight);

                            
                            e.Graphics.DrawString(cellText, font, Brushes.Black, cellRect, trimFormat);

                            currentX += width;
                        }
                    }

                    y += rowHeight;
                }
            }
        }

        private void btnLimpiat_Click_1(object sender, EventArgs e)
        {
            dtpDesde.Value = DateTime.Now.AddDays(-3).Date; 
            dtpHasta.Value = DateTime.Now;

            txtNombre.Clear();
            txtApellido.Clear();

            CargarGrillaInicial();
        }

        private void btnPDF_Click(object sender, EventArgs e)
        {
            PrintDialog pd = new PrintDialog();
            pd.Document = printDoc;

            if (pd.ShowDialog() == DialogResult.OK)
            {
                printDoc.Print();
            }
        }

        

        private void dgvBitacora_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvBitacora.CurrentRow != null)
            {
                txtNombre.Text = dgvBitacora.CurrentRow.Cells["Nombre"].Value?.ToString() ?? "";
                txtApellido.Text = dgvBitacora.CurrentRow.Cells["Apellido"].Value?.ToString() ?? "";
            }
        }

        private void AuditoriaBitacora_Load_1(object sender, EventArgs e)
        {
            dtpDesde.Value = DateTime.Now.AddDays(-3);
            dtpHasta.Value = DateTime.Now;

            CargarGrillaInicial();
        }

        private void dgvBitacora_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value == null || e.RowIndex < 0) return;

            if (dgvBitacora.Columns[e.ColumnIndex].Name == "Criticidad")
            {
                if (Int32.TryParse(e.Value.ToString(), out int criticidad))
                {
                    e.Value = ((Criticidad55CA)criticidad).ToString();
                    e.FormattingApplied = true;
                }
            }

            
            if (dgvBitacora.Columns[e.ColumnIndex].Name == "Modulo")
            {
                if (Int32.TryParse(e.Value.ToString(), out int modulo))
                {
                    e.Value = ((Modulos55CA)modulo).ToString();
                    e.FormattingApplied = true;
                }
            }
        }
    }
}
