using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
namespace CMSystem
{
    public class TableForm : Form
    {
        private DataGridView gridView = new DataGridView();
        private FlowLayoutPanel panel = new FlowLayoutPanel();

        private Button removeBtn = new Button();
        private Button addBtn = new Button();
        private Button goBackBtn = new Button();

        public TableForm()
        {
            this.Text = "Control Managenemt Systems";
            this.VisibleChanged += new EventHandler((o, e) => FillTable());
            this.Size = new Size(500, 300);
            SetupLayout();
            SetupTable();
        }

        private void SetupTable()
        {
            this.gridView.ColumnCount = 2;
            this.gridView.Name = "Devices View";
            this.gridView.Location = new Point(8, 8);
            this.gridView.AutoSize = true;
            this.gridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            this.gridView.GridColor = Color.Black;
            this.gridView.AllowUserToAddRows = false;
            this.gridView.AllowUserToResizeRows = false;
            this.gridView.AllowUserToResizeColumns = false;

            // Checkbox column
            var checkboxColumn = new DataGridViewCheckBoxColumn();
            checkboxColumn.HeaderText = "Select";
            checkboxColumn.Name = "Select";
            checkboxColumn.AutoSizeMode =
                DataGridViewAutoSizeColumnMode.DisplayedCells;
            checkboxColumn.FlatStyle = FlatStyle.Standard;
            checkboxColumn.ThreeState = true;
            checkboxColumn.CellTemplate = new DataGridViewCheckBoxCell();
            checkboxColumn.CellTemplate.Style.BackColor = Color.Beige;

            // Link column
            var linkColumn = new DataGridViewLinkColumn();
            linkColumn.HeaderText = "Name";
            linkColumn.Name = "Name";

            // Image column
            var imageColumn = new DataGridViewImageColumn();

            this.gridView.Columns.Insert(0, checkboxColumn);
            this.gridView.Columns.Insert(1, linkColumn);
            this.gridView.Columns.Insert(3, imageColumn);

            this.gridView.Columns[0].Name = "Select";
            this.gridView.Columns[1].Name = "Name";
            this.gridView.Columns[1].ReadOnly = true;
            this.gridView.Columns[2].Name = "Description Path";
            this.gridView.Columns[2].ReadOnly = true;
            this.gridView.Columns[3].Name = "Image";
            this.gridView.Columns[3].ReadOnly = true;
            this.gridView.Columns[4].Name = "Creation Date";
            this.gridView.Columns[4].ReadOnly = true;
            this.gridView.CellContentClick += new DataGridViewCellEventHandler(CellClick);

            FillTable();
        }

        private void FillTable()
        {
            if (this.Visible)
            {
                this.gridView.Rows.Clear();
                foreach (var dev in DeviceService.GetInstance().GetDevices())
                {
                    Image img = null;

                    try
                    {
                        img = Image.FromFile(dev.ImagePath);
                    }
                    catch (Exception)
                    {
                        Console.Error.WriteLine("Couldn't open the image \"{0}\"", dev.ImagePath);
                    }
                    finally
                    {
                        this.gridView.Rows.Add(
                            false,
                            new LinkCellValue { DisplayValue = dev.Name, ID = dev.ID },
                            dev.RichTextPath,
                            img,
                            dev.CreationDate.ToString()
                        );
                    }
                }
            }
        }

        private void SetupLayout()
        {
            this.Controls.Add(this.panel);
            this.panel.Size = this.Size;
            this.panel.WrapContents = true;
            this.panel.FlowDirection = FlowDirection.RightToLeft;
            this.panel.Controls.Add(gridView);

            this.addBtn.Text = "Add";
            this.removeBtn.Text = "Remove";
            this.goBackBtn.Text = "Go back";

            this.panel.Controls.Add(goBackBtn);
            this.panel.Controls.Add(removeBtn);
            this.panel.Controls.Add(addBtn);

            this.addBtn.Click += new EventHandler(OnAddClick);
            this.removeBtn.Click += new EventHandler(OnRemoveClick);
            this.goBackBtn.Click += new EventHandler(OnGoBackClick);
        }

        private void OnAddClick(object sender, EventArgs e)
        {
            OpenEditForm(null);
        }

        private void OnRemoveClick(object sender, EventArgs e)
        {
            var ids = new List<int>();
            foreach (DataGridViewRow row in gridView.Rows)
            {
                if ((bool)(row.Cells[0].Value) == false)
                {
                    continue;
                }
                var linkVal = row.Cells[1].Value as LinkCellValue;
                ids.Add(linkVal.ID);
            }

            DeviceService.GetInstance().RemoveDevicesWithIds(ids);
            FillTable();
        }


        private void OnGoBackClick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
            {
                return;
            }

            var dataGridView = (DataGridView)sender;
            var clickedCell = dataGridView
                              .Rows[e.RowIndex]
                              .Cells[e.ColumnIndex];

            if (clickedCell is DataGridViewLinkCell)
            {
                var linkVal = clickedCell.Value as LinkCellValue;
                var dev = DeviceService.GetInstance()
                          .GetDevices()
                          .Find(d => d.ID == linkVal.ID);
                OpenEditForm(dev);
            }
        }

        private void OpenEditForm(Device dev)
        {
            var editFrm = new EditDeviceForm(dev);
            editFrm.Location = this.Location;
            editFrm.StartPosition = FormStartPosition.Manual;
            editFrm.FormClosing += delegate { this.Show(); };
            editFrm.Show();
            this.Hide();
        }
    }

    class LinkCellValue
    {
        public string DisplayValue { get; set; }
        public int ID { get; set; }

        public override string ToString()
        {
            return DisplayValue;
        }
    }
}

// This is to make it shaped cool
//this.FormBorderStyle = FormBorderStyle.None;
//this.BackgroundImage = Image.FromFile("shape.bmp");
//this.ClientSize = this.BackgroundImage.Size;
//this.TransparencyKey = Color.White;
