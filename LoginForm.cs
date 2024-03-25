using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace CMSystem
{
    public class LoginForm : Form
    {
        private List<User> users = new List<User>();
        private UserService userService = new UserService();

        private Button btnLogIn;
        private TextBox tbName;
        private TextBox tbPass;
        private TableLayoutPanel table;


        public LoginForm()
        {
            this.Text = "Welcome to Control Managenemt Systems!";
            users = userService.LoadUsers();
            this.SetupLayout();
        }

        private void SetupLayout()
        {
            table = new TableLayoutPanel();
            tbName = new TextBox();
            tbPass = new TextBox();
            btnLogIn = new Button();
            var lblName = new Label();
            var lblPass = new Label();

            table.RowCount = 0;
            table.ColumnCount = 1;
            table.Height = 300;

            tbName.Dock = DockStyle.Fill;
            tbPass.Dock = DockStyle.Fill;
            tbPass.PasswordChar = '*';
            lblName.Text = "Enter your username:";
            lblPass.Text = "Enter your password:";
            lblName.Dock = DockStyle.Fill;
            lblPass.Dock = DockStyle.Fill;
            btnLogIn.Text = "Log in";
            btnLogIn.Anchor = AnchorStyles.Top;
            btnLogIn.Click += new EventHandler(OnLogin);

            table.Controls.Add(lblName, 0, 0);
            table.Controls.Add(tbName, 0, 1);
            table.Controls.Add(lblPass, 0, 2);
            table.Controls.Add(tbPass, 0, 3);
            table.Controls.Add(btnLogIn, 0, 4);

            this.Controls.Add(table);
        }

        private void OnLogin(object sender, EventArgs e)
        {
            var user = users.Find(u => u.UserName == tbName.Text);
            if (user == null)
            {
                Console.WriteLine("Thene is no such user");
                return;
            }

            if (tbPass.Text != user.Password)
            {
                Console.WriteLine("Password is incorect!");
                return;
            }

            Console.WriteLine("You are logged in!");
            var tableFrm = new TableForm();
            tableFrm.Location = this.Location;
            tableFrm.StartPosition = FormStartPosition.Manual;
            tableFrm.FormClosing += delegate { this.Show(); };
            tableFrm.Show();
            this.Hide();
        }
    }
}
