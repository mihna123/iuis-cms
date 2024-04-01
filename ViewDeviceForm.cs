using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;

namespace CMSystem
{
    class ViewDeviceForm : Form
    {
        private Device device;
        private FlowLayoutPanel layout = new FlowLayoutPanel();
        private Button goBackBtn = new Button();

        public ViewDeviceForm(Device d)
        {
            this.device = d;
            this.Text = "Content Management System";
            this.SetupLayout();
        }

        private void SetupLayout()
        {
            this.Controls.Add(layout);
            this.layout.Size = new Size(500, 600);
            this.layout.WrapContents = true;
            this.layout.FlowDirection = FlowDirection.LeftToRight;
            this.layout.Padding = new Padding(10);

            var dataLayout = new FlowLayoutPanel();
            dataLayout.Size = new Size(310, 200);
            dataLayout.WrapContents = true;
            dataLayout.FlowDirection = FlowDirection.LeftToRight;

            var img = new PictureBox();
            var lblName = new Label();
            var lblDate = new Label();
            var lblDesc = new Label();
            var rtbDesc = new RichTextBox();

            img.SizeMode = PictureBoxSizeMode.StretchImage;
            img.Image = Image.FromFile(device.ImagePath);
            img.ClientSize = new Size(150, 150);

            lblDate.Text = string.Format("Created Date: {0}",
                                          this.device.CreationDate);
            lblName.Text = string.Format("Name: {0}", this.device.Name);
            lblDesc.Text = "Description: ";

            lblName.Width = 300;
            lblDate.Width = 300;
            lblDesc.Width = 300;

            rtbDesc.LoadFile(device.RichTextPath);
            rtbDesc.ReadOnly = true;
            rtbDesc.Size = new Size(300, 50);

            goBackBtn.Text = "Go back";
            goBackBtn.Click += new EventHandler((o, e) => this.Close());

            dataLayout.Controls.Add(lblName);
            dataLayout.Controls.Add(lblDate);
            dataLayout.Controls.Add(lblDesc);
            dataLayout.Controls.Add(rtbDesc);

            this.layout.Controls.Add(img);
            this.layout.Controls.Add(dataLayout);
            this.layout.Controls.Add(goBackBtn);
        }
    }
}
