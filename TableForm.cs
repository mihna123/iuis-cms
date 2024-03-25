using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
namespace CMSystem
{
    public class TableForm : Form
    {
        private DataGridView gridView = new DataGridView();
        private IOService ioService = new IOService();
        private List<Device> devices = new List<Device>();

        public TableForm()
        {
            this.Text = "Control Managenemt Systems!";
            devices = ioService.LoadDevices();
            SetupTable();
        }

        private void SetupTable()
        {
            this.Controls.Add(gridView);

            gridView.ColumnCount = 5;
            gridView.Name = "Devices View";
            gridView.Location = new Point(8, 8);
            gridView.Size = new Size(600, 800);
            gridView.GridColor = Color.Black;
            var checkboxColumn = new DataGridViewCheckBoxColumn();

            checkboxColumn.HeaderText = "Select";
            checkboxColumn.Name = "Select";
            checkboxColumn.AutoSizeMode =
                DataGridViewAutoSizeColumnMode.DisplayedCells;
            checkboxColumn.FlatStyle = FlatStyle.Standard;
            checkboxColumn.ThreeState = true;
            checkboxColumn.CellTemplate = new DataGridViewCheckBoxCell();
            checkboxColumn.CellTemplate.Style.BackColor = Color.Beige;

            gridView.Columns.Insert(0, checkboxColumn);
            gridView.Columns[0].Name = "Select";
            gridView.Columns[1].Name = "ID";
            gridView.Columns[2].Name = "Name";
            gridView.Columns[3].Name = "Path";
            gridView.Columns[4].Name = "Creation Date";

            foreach (var dev in devices)
            {
                var row = new List<string>();
                row.Add(dev.ID.ToString());
                row.Add(dev.Name);
                row.Add(dev.RichTextPath);
                row.Add(dev.CreationDate.ToString());

                gridView.Rows.Add(row.ToArray());
            }
        }

        private void SetupLayout()
        {
        }
    }
}

// This is to make it shaped cool
//this.FormBorderStyle = FormBorderStyle.None;
//this.BackgroundImage = Image.FromFile("shape.bmp");
//this.ClientSize = this.BackgroundImage.Size;
//this.TransparencyKey = Color.White;
