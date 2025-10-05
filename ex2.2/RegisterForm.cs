using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ex2._2
{
    public partial class RegisterForm : Form
    {
        public RegisterForm()
        {
            InitializeComponent();
            btnRegister.Click += btnRegister_Click;
            btnBack.Click += btnBack_Click;
            lblRegisterError.Visible = false;
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            lblRegisterError.Visible = false;

            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;
            string confirm = txtConfirmPassword.Text;
            string email = txtEmail.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(confirm) || string.IsNullOrEmpty(email))
            {
                lblRegisterError.Text = "Vui lòng điền đầy đủ thông tin.";
                lblRegisterError.Visible = true;
                return;
            }

            if (password != confirm)
            {
                lblRegisterError.Text = "Mật khẩu và xác nhận mật khẩu không khớp.";
                lblRegisterError.Visible = true;
                return;
            }

            if (!IsValidEmail(email))
            {
                lblRegisterError.Text = "Email không đúng định dạng.";
                lblRegisterError.Visible = true;
                return;
            }

            try
            {
                if (DbHelper.UsernameExists(username))
                {
                    lblRegisterError.Text = "Tên đăng nhập đã tồn tại. Vui lòng chọn tên khác.";
                    lblRegisterError.Visible = true;
                    return;
                }

                string hashed = DbHelper.HashPassword(password);
                DbHelper.AddUser(username, hashed, email);

                MessageBox.Show("Đăng ký thành công! Vui lòng đăng nhập.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi đăng ký: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private bool IsValidEmail(string email)
        {
            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase);
            }
            catch
            {
                return false;
            }
        }
    }
}
