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
        private Label lblErrName;
        private Label lblErrPass;
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
            lblErrName = new Label();
            lblErrPass = new Label();

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
            lblErrName.ForeColor = Color.Red;
            lblErrPass.ForeColor = Color.Red;
            lblErrName.Dock = DockStyle.Fill;
            lblErrPass.Dock = DockStyle.Fill;
            btnLogIn.Text = "Log in";
            btnLogIn.Anchor = AnchorStyles.Top;
            btnLogIn.Click += new EventHandler(OnLogin);

            table.Controls.Add(lblName, 0, 0);
            table.Controls.Add(tbName, 0, 1);
            table.Controls.Add(lblErrName, 0, 2);
            table.Controls.Add(lblPass, 0, 3);
            table.Controls.Add(tbPass, 0, 4);
            table.Controls.Add(lblErrPass, 0, 5);
            table.Controls.Add(btnLogIn, 0, 6);

            this.Controls.Add(table);
        }

        private void OnLogin(object sender, EventArgs e)
        {
            clearAllLabels();
            bool auth = true;
            if (tbName.Text == "")
            {
                lblErrName.Text = "Please enter your name";
                auth = false;
            }
            if (tbPass.Text == "")
            {
                lblErrPass.Text = "Please enter your password";
                auth = false;
            }

            if (!auth)
            {
                return;
            }

            var user = users.Find(u => u.UserName == tbName.Text);
            if (user == null)
            {
                Console.WriteLine("Thene is no such user");
                lblErrName.Text = "The username is incorrect!";
                return;
            }

            if (tbPass.Text != user.Password)
            {
                Console.WriteLine("Password is incorect!");
                lblErrPass.Text = "The password is incorrect!";
                return;
            }

            Console.WriteLine("You are logged in!");
            var tableFrm = new TableForm(user);
            tableFrm.Location = this.Location;
            tableFrm.StartPosition = FormStartPosition.Manual;
            tableFrm.FormClosing += delegate { this.Show(); };
            tableFrm.Show();
            this.Hide();
        }

        private void clearAllLabels()
        {
            lblErrPass.Text = "";
            lblErrName.Text = "";
        }
    }
}
