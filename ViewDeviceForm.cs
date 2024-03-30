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
            this.layout.Size = new Size(450, 300);
            this.layout.WrapContents = true;
            this.layout.FlowDirection = FlowDirection.LeftToRight;

            var lblName = new Label();
            var lblDate = new Label();

            lblName.Text = string.Format("Name: {0}", this.device.Name);
            lblName.Text = string.Format("Created Date: {0}",
                                          this.device.CreationDate);
            this.layout.Controls.Add(lblName);
            this.layout.Controls.Add(lblDate);
        }
    }
}
