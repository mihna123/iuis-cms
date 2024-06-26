using System;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;

namespace CMSystem
{
    public class EditDeviceForm : Form
    {
        private Device device;

        private FlowLayoutPanel table = new FlowLayoutPanel();
        private TextBox tbName = new TextBox();
        private TextBox tbPath = new TextBox();
        private Button filePickBtn = new Button();
        private RichTextBox rtbDescription = new RichTextBox();
        private FontStyle style = FontStyle.Regular;
        private Button saveBtn = new Button();
        private Button cancelBtn = new Button();
        private Button colorBtn = new Button();
        private ToolStripDropDownButton toolStripDropDownButton;
        private Label lbErrName = new Label();

        public EditDeviceForm(Device dev)
        {
            this.device = dev;
            this.Text = "Content Managenemt Systems";
            SetupLayout();
        }

        private void SetupLayout()
        {
            if (device != null)
            {
                //this.rtbDescription.LoadFile(device.RichTextPath);
                var rtf = loadRichText(device);
                this.rtbDescription.Rtf = rtf;
                this.tbName.Text = device.Name;
                this.tbPath.Text = device.ImagePath;
                this.rtbDescription.Font =
                  new Font(rtbDescription.SelectionFont.FontFamily,
                           12,
                           rtbDescription.SelectionFont.Style
                           );
            }

            this.Controls.Add(this.table);
            this.table.Size = new Size(450, 500);
            this.table.WrapContents = true;
            this.table.FlowDirection = FlowDirection.LeftToRight;

            // Name field
            var namePanel = getNamePanel();

            // Image field 
            var imgPanel = getImagePanel();

            // Rich Text field
            var richPanel = getRichTextPanel();

            // Save button and cancel buttons
            this.saveBtn.Text = "Save";
            this.saveBtn.Margin = new Padding(
                    240,
                    saveBtn.Margin.Top,
                    saveBtn.Margin.Right,
                    saveBtn.Margin.Bottom
                );
            this.cancelBtn.Text = "Cancel";
            this.saveBtn.Click += new EventHandler((o, e) => onSave());
            this.cancelBtn.Click += new EventHandler((o, e) => this.Close());

            this.table.Controls.Add(namePanel);
            this.table.Controls.Add(imgPanel);
            this.table.Controls.Add(richPanel);
            this.table.Controls.Add(saveBtn);
            this.table.Controls.Add(cancelBtn);
        }

        private FlowLayoutPanel getEditingButtonsPanel()
        {
            FlowLayoutPanel panel = new FlowLayoutPanel();
            panel.Size = new Size(400, 35);
            panel.WrapContents = true;
            panel.FlowDirection = FlowDirection.LeftToRight;

            var boldBtn = new Button();
            boldBtn.Text = "B";
            boldBtn.Name = "B";
            boldBtn.Size = new Size(25, 25);
            boldBtn.Click += new EventHandler(onFontStyleChange);

            var italBtn = new Button();
            italBtn.Text = "I";
            italBtn.Name = "I";
            italBtn.Size = new Size(25, 25);
            italBtn.Click += new EventHandler(onFontStyleChange);

            var undrBtn = new Button();
            undrBtn.Text = "U";
            undrBtn.Name = "U";
            undrBtn.Size = new Size(25, 25);
            undrBtn.Click += new EventHandler(onFontStyleChange);

            colorBtn.Size = new Size(25, 25);
            colorBtn.BackColor = this.rtbDescription.SelectionColor;
            colorBtn.FlatStyle = FlatStyle.Flat;
            colorBtn.FlatAppearance.BorderSize = 3;
            colorBtn.Anchor = AnchorStyles.Bottom;
            colorBtn.Click += new EventHandler(colorSelect);


            var toolStrip = new ToolStrip();
            toolStripDropDownButton = new ToolStripDropDownButton();
            foreach (var fn in getAllFonts())
            {
                toolStripDropDownButton.DropDownItems.Add(fn);
            }

            toolStripDropDownButton.Text = rtbDescription.Font.Name;
            toolStripDropDownButton.DropDownItemClicked += dropDownItemClicked;
            toolStrip.Items.Add(toolStripDropDownButton);


            panel.Controls.Add(boldBtn);
            panel.Controls.Add(italBtn);
            panel.Controls.Add(undrBtn);
            panel.Controls.Add(colorBtn);
            panel.Controls.Add(toolStrip);

            return panel;
        }

        private void dropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            rtbDescription.Font = new Font(e.ClickedItem.Text, 12, FontStyle.Regular);
            toolStripDropDownButton.Text = e.ClickedItem.Text;
        }

        private List<string> getAllFonts()
        {
            var installedFontCollection = new InstalledFontCollection();

            // Get the array of FontFamily objects
            FontFamily[] fontFamilies = installedFontCollection.Families;
            var ret = new List<string>();
            foreach (FontFamily font in fontFamilies)
            {
                ret.Add(font.Name);
            }
            return ret;
        }

        private void colorSelect(object sender, EventArgs e)
        {
            var colorSelect = new ColorDialog();

            if (colorSelect.ShowDialog() == DialogResult.OK)
            {
                this.rtbDescription.SelectionColor = colorSelect.Color;
                colorBtn.BackColor = colorSelect.Color;
            }
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

        private FlowLayoutPanel getNamePanel()
        {
            var lblName = new Label();
            this.tbName.Width = 150;
            lblName.Text = "Name*";
            lblName.Margin = new Padding(3, 0, 40, 0);
            lbErrName.ForeColor = Color.Red;
            lbErrName.Width = 150;
            var namePanel = new FlowLayoutPanel();
            namePanel.Size = new Size(160, 80);
            namePanel.WrapContents = true;
            namePanel.FlowDirection = FlowDirection.LeftToRight;
            namePanel.Controls.Add(lblName);
            namePanel.Controls.Add(this.tbName);
            namePanel.Controls.Add(lbErrName);
            namePanel.Margin = new Padding(10);

            return namePanel;
        }

        private FlowLayoutPanel getImagePanel()
        {
            this.tbPath.Width = 150;
            this.filePickBtn.Text = "Browse";
            var lblImg = new Label();
            lblImg.Text = "Image";
            lblImg.Margin = new Padding(3, 0, 100, 0);
            this.filePickBtn.Width = 60;
            this.filePickBtn.Click +=
              new EventHandler((o, e) => openFilePicker());
            var imgPanel = new FlowLayoutPanel();
            imgPanel.Size = new Size(250, 50);
            imgPanel.WrapContents = true;
            imgPanel.FlowDirection = FlowDirection.LeftToRight;
            imgPanel.Controls.Add(lblImg);
            imgPanel.Controls.Add(this.tbPath);
            imgPanel.Controls.Add(this.filePickBtn);
            imgPanel.Margin = new Padding(10);

            return imgPanel;
        }

        private FlowLayoutPanel getRichTextPanel()
        {
            rtbDescription.AutoWordSelection = true;
            rtbDescription.Width = 390;
            var lblDesc = new Label();
            lblDesc.Text = "Description";
            var richPanel = new FlowLayoutPanel();
            richPanel.Size = new Size(400, 175);
            richPanel.WrapContents = true;
            richPanel.FlowDirection = FlowDirection.LeftToRight;
            richPanel.Controls.Add(lblDesc);
            richPanel.Controls.Add(getEditingButtonsPanel());
            richPanel.Controls.Add(rtbDescription);
            richPanel.Margin = new Padding(10);

            return richPanel;
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
            if (tbName.Text == "")
            {
                lbErrName.Text = "You must enter the name";
                return;
            }
            var msg = (this.device == null
              ? "The device was successfuly added."
              : "The device was successfuly edited.");
            if (device == null)
            {
                device = new Device();
            }
            device.Name = this.tbName.Text;
            device.ImagePath = this.tbPath.Text;
            device.RichTextPath = saveRichText(device);
            device.CreationDate = DateTime.Now;

            DeviceService.GetInstance().AddDevice(device);
            MessageBox.Show(
                msg,
                "Success",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Information
                );
            this.Close();
        }

        private string saveRichText(Device d)
        {
            var path = DeviceService.GetInstance().GetRtfPath(d);
            rtbDescription.SaveFile(path, RichTextBoxStreamType.RichText);
            return path;
        }

        private string loadRichText(Device d)
        {
            var path = DeviceService.GetInstance().GetRtfPath(d);
            return File.ReadAllText(path);
        }
    }
}
