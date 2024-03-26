using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace CMSystem
{
    public class EditDeviceForm : Form
    {
        private Device device;

        private TableLayoutPanel table = new TableLayoutPanel();
        private TextBox tbName = new TextBox();
        private TextBox tbPath = new TextBox();
        private Button filePickBtn = new Button();
        private RichTextBox rtbDescription = new RichTextBox();
        private FontStyle style = FontStyle.Regular;
        private Button saveBtn = new Button();
        private Button cancelBtn = new Button();

        public EditDeviceForm(Device dev)
        {
            this.device = dev;
            this.Text = "Control Managenemt Systems";
            SetupLayout();
        }

        private void SetupLayout()
        {
            if (device != null)
            {
                this.tbName.Text = device.Name;
                this.tbPath.Text = device.ImagePath;
                this.rtbDescription.LoadFile(device.RichTextPath, RichTextBoxStreamType.RichText);
            }
            this.Controls.Add(this.table);
            this.table.RowCount = 0;
            this.table.ColumnCount = 1;
            this.table.Size = new Size(400, 600);

            // Name field
            this.tbName.Dock = DockStyle.Fill;

            // Image field and browse button 
            this.tbPath.Dock = DockStyle.Fill;
            this.tbPath.Width = 300;
            this.filePickBtn.Text = "Browse";
            this.filePickBtn.Width = 70;
            this.filePickBtn.Click +=
              new EventHandler((o, e) => openFilePicker());
            var pathPanel = new FlowLayoutPanel();
            pathPanel.Margin = new Padding(0);
            pathPanel.AutoSize = true;
            pathPanel.WrapContents = true;
            pathPanel.FlowDirection = FlowDirection.LeftToRight;
            pathPanel.Controls.Add(this.tbPath);
            pathPanel.Controls.Add(this.filePickBtn);


            // Labels
            var lblName = new Label();
            var lblPath = new Label();
            var lblDesc = new Label();
            lblName.Text = "Name:";
            lblPath.Text = "Image:";
            lblDesc.Text = "Description:";

            // Rich Text field
            rtbDescription.Dock = DockStyle.Fill;
            rtbDescription.AutoWordSelection = true;
            rtbDescription.Font = new Font("Segoe UI", 10);

            // Save button and cancel buttons
            this.saveBtn.Text = "Save";
            this.cancelBtn.Text = "Cancel";
            this.saveBtn.Click += new EventHandler((o, e) => onSave());
            this.cancelBtn.Click += new EventHandler((o, e) => this.Close());
            var savCanPanel = new FlowLayoutPanel();
            savCanPanel.Margin = new Padding(0);
            savCanPanel.Size = new Size(400, 35);
            savCanPanel.WrapContents = true;
            savCanPanel.FlowDirection = FlowDirection.LeftToRight;
            savCanPanel.Controls.Add(saveBtn);
            savCanPanel.Controls.Add(cancelBtn);

            this.table.Controls.Add(lblName);
            this.table.Controls.Add(tbName);
            this.table.Controls.Add(lblPath);
            this.table.Controls.Add(pathPanel);
            this.table.Controls.Add(getEditingButtons());
            this.table.Controls.Add(rtbDescription);
            this.table.Controls.Add(savCanPanel);
        }

        private FlowLayoutPanel getEditingButtons()
        {
            FlowLayoutPanel panel = new FlowLayoutPanel();
            panel.Size = new Size(400, 35);
            panel.WrapContents = true;
            panel.FlowDirection = FlowDirection.LeftToRight;

            var boldBtn = new Button();
            boldBtn.Text = "B";
            boldBtn.Name = "B";
            boldBtn.Size = new Size(30, 30);
            boldBtn.Click += new EventHandler(onFontStyleChange);

            var italBtn = new Button();
            italBtn.Text = "I";
            italBtn.Name = "I";
            italBtn.Size = new Size(30, 30);
            italBtn.Click += new EventHandler(onFontStyleChange);

            var undrBtn = new Button();
            undrBtn.Text = "U";
            undrBtn.Name = "U";
            undrBtn.Size = new Size(30, 30);
            undrBtn.Click += new EventHandler(onFontStyleChange);

            var colorBtn = new Button();
            colorBtn.Text = "Change color";
            colorBtn.Size = new Size(100, 30);
            colorBtn.Anchor = AnchorStyles.Bottom;

            var fontBtn = new Button();
            fontBtn.Text = "Change font";
            fontBtn.Size = new Size(100, 30);
            fontBtn.Anchor = AnchorStyles.Bottom;

            panel.Controls.Add(boldBtn);
            panel.Controls.Add(italBtn);
            panel.Controls.Add(undrBtn);
            panel.Controls.Add(colorBtn);
            panel.Controls.Add(fontBtn);

            return panel;
        }
        private void onFontStyleChange(object sender, EventArgs e)
        {
            var btn = sender as Button;
            switch (btn.Name)
            {
                case "B":
                    if ((this.style & FontStyle.Bold) == FontStyle.Bold)
                        this.style &= ~FontStyle.Bold; // Unset bold
                    else
                        this.style |= FontStyle.Bold; // Set bold
                    break;
                case "I":
                    if ((this.style & FontStyle.Italic) == FontStyle.Italic)
                        this.style &= ~FontStyle.Italic; // Unset italic
                    else
                        this.style |= FontStyle.Italic; // Set italic
                    break;
                case "U":
                    if ((this.style & FontStyle.Underline) == FontStyle.Underline)
                        this.style &= ~FontStyle.Underline; // Unset underline
                    else
                        this.style |= FontStyle.Underline; // Set underline
                    break;
            }

            this.rtbDescription.SelectionFont =
              new Font(this.rtbDescription.Font, this.style);
        }

        private void openFilePicker()
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.Title = "Select an image File";
            fileDialog.Filter = "Image Files (*.jpg;*.jpeg;*.png;*.gif;*.bmp)|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            fileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                this.tbPath.Text = fileDialog.FileName;
            }

        }

        private void onSave()
        {
            if (device == null)
            {
                device = new Device();
            }
            device.Name = this.tbName.Text;
            device.ImagePath = this.tbPath.Text;
            device.RichTextPath = saveRichText(device);
            device.CreationDate = DateTime.Now;

            DeviceService.GetInstance().AddDevice(device);
            this.Close();
        }

        private string saveRichText(Device d)
        {
            var path = DeviceService.GetInstance().GetRtfPath(d);
            rtbDescription.SaveFile(path, RichTextBoxStreamType.RichText);
            return path;
        }
    }
}
