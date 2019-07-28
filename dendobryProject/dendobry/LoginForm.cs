using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace dendobry
{
    public partial class LoginForm : Form
    {
        SqlConnection conn;
        bool flag_login;

        public LoginForm()
        {
            InitializeComponent();

            string datasource = @"laptop-u4b7kruu\SQLEXPRESS";
            string database = "AIS_DEN_DOBRII";
            string username = "sa";
            string password = "123456";

            string connString = @"Data Source=" + datasource + ";Initial Catalog="
                        + database + ";Persist Security Info=True;User ID=" + username + ";Password=" + password;

            conn = new SqlConnection(connString);

            conn.Open();
        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand("select id, pass_h from Adminka");
            command.Connection = conn;
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    if (loginTextBox.Text == reader["id"].ToString() && passTextBox.Text == reader["pass_h"].ToString())
                    {
                        MainForm mainForm = new MainForm(this, loginTextBox.Text, conn, reader);
                        mainForm.Show();
                        Hide();
                        flag_login = true;
                    }
                }
                if (!flag_login)
                {
                    MessageBox.Show("Такого аккаунта не существует", "Ошибка");
                    flag_login = false;
                }
            }
        }
    }
}
