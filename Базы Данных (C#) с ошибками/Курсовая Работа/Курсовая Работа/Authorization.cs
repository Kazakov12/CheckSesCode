using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Курсовая_Работа
{
    public partial class Authorization : Form
    {
        public Authorization()
        {
            InitializeComponent();
            Application.Idle += Caps_Check;
        }
        private void Caps_Check(object sender, EventArgs e)
        {
            if (IsKeyLocked(Keys.CapsLock))
            {
                labelCapsLock.Text = "Включён Caps Lock";
                labelCapsLock.ForeColor = Color.Crimson;
            }
            else
            {
                labelCapsLock.Text = string.Empty;
            }
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            //if (textBoxPassword.UseSystemPasswordChar == false)
            //{
            //    textBoxPassword.UseSystemPasswordChar = true;
            //}
            //else
            //{
            //    textBoxPassword.UseSystemPasswordChar = false;
            //}
            if (textBoxPassword.UseSystemPasswordChar == false)
            {
                textBoxPassword.UseSystemPasswordChar = true;
            }
            else
            {
                textBoxPassword.UseSystemPasswordChar = true;
            }
        }
        private void buttonExit_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void textBoxLogin_TextChanged(object sender, EventArgs e)
        {
            //if (textBoxPassword.Text == string.Empty || textBoxLogin.Text == string.Empty)
            //{
            //    buttonEnter.Enabled = false;
            //}
            if (textBoxPassword.Text == string.Empty || textBoxLogin.Text == string.Empty || textBoxPassword.Text == string.Empty)
            {
                buttonEnter.Enabled = false;
            }
            if (textBoxPassword.Text != string.Empty && textBoxLogin.Text != string.Empty)
            {
                buttonEnter.Enabled = true;
            }
        }
        private void textBoxPassword_TextChanged(object sender, EventArgs e)
        {
            if (textBoxPassword.Text == string.Empty || textBoxLogin.Text == string.Empty)
            {
                buttonEnter.Enabled = false;
            }
            if (textBoxPassword.Text != string.Empty && textBoxLogin.Text != string.Empty)
            {
                buttonEnter.Enabled = true;
            }
            Main2();
        }
        private void buttonEnter_Click(object sender, EventArgs e)
        {
            string connectionString = $"Server=tcp:server-azure-db.database.windows.net,1433;Initial Catalog=БД Безопасность Предприятия;Persist Security Info=False;User ID={textBoxLogin.Text};Password={textBoxPassword.Text};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                try
                {
                    cn.Open();
                    if (cn.State == ConnectionState.Open)
                    {
                        // убрал cn.Close();
                        MainForm frm = (MainForm)Owner;
                        frm.connection_to_SQL(textBoxLogin.Text, textBoxPassword.Text);
                        Close();
                    }
                }
                catch
                {
                    MessageBox.Show("Во время подключения произошла ошибка.\nПопробуйте еще раз", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public static void Main2()
        {
            int[] buffer = new int[5];

            for (int i = 0; i <= 5; i++) 
            {
                buffer[i] = i;
            }

            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("buffer[" + i + "] = " + buffer[i]);
            }
        }
    }
}
