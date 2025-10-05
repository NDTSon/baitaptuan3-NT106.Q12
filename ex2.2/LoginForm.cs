using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ex2._2
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            btnLogin.Click += btnLogin_Click;
            btnRegister.Click += btnRegister_Click;
            lblLoginError.Visible = false;
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            var rf = new RegisterForm();
            rf.FormClosed += (s, args) => this.Show();
            rf.Show();
            this.Hide();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            lblLoginError.Visible = false;
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                lblLoginError.Text = "Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu.";
                lblLoginError.Visible = true;
                return;
            }

            try
            {
                string hashed = DbHelper.HashPassword(password);
                bool ok = DbHelper.ValidateUser(username, hashed);
                if (ok)
                {
                    var mf = new MainForm(username);
                    mf.FormClosed += (s, args) => this.Close(); // đóng app khi main đóng
                    mf.Show();
                    this.Hide();
                }
                else
                {
                    lblLoginError.Text = "Sai tên đăng nhập hoặc mật khẩu.";
                    lblLoginError.Visible = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi đăng nhập: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }
    }
    }
