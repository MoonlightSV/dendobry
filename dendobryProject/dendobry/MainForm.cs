using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dendobry
{
    public partial class MainForm : Form
    {
        public LoginForm loginForm;
        public SqlConnection conn;
        public SqlDataReader reader;
        public string[] count1;
        public string[] count2;
        public string[,] mass;

        public MainForm()
        {
            InitializeComponent();
        }

        public MainForm(LoginForm f, string l, SqlConnection c, SqlDataReader r) : this()
        {
            loginForm = f;
            label1.Text = l;
            conn = c;
            reader = r;

            FileStream file = new FileStream("C:\\Users\\Moonlight\\Desktop\\dendobry\\matrix.txt", FileMode.Open, FileAccess.Read);
            StreamReader file_reader = new StreamReader(file);
            string str;
            str = file_reader.ReadLine();
            string[] count = str.Split(new char[] { ' ' });
            int mat_size = count.Length;
            mass = new string[count.Length, count.Length];
            for (int i = 0; i < count.Length; i++)
            {
                mass[0, i] = count[i];
            }
            int j = 1;
            while (!file_reader.EndOfStream)
            {
                str = file_reader.ReadLine();
                count = str.Split(new char[] { ' ' });
                for (int i = 0; i < count.Length; i++)
                {
                    mass[j, i] = count[i];
                }
                j++;
            }
            file.Close();

            FileStream file1 = new FileStream("C:\\Users\\Moonlight\\Desktop\\dendobry\\service.txt", FileMode.Open, FileAccess.Read);
            StreamReader file_reader1 = new StreamReader(file1);
            string str1;
            str1 = file_reader1.ReadLine();
            count1 = str1.Split(new char[] { ' ' });
            str1 = file_reader1.ReadLine();
            count2 = str1.Split(new char[] { ' ' });

            for (int i = 0; i < count2.Length; i++)
            {
                if (count2[i] == "None")
                {
                    count2[i] = "NULL";
                }
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            loginForm.Close();
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Hide();
            loginForm.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string id_serv = textBox1.Text;
            string id_client = textBox2.Text;

            int ind = Array.IndexOf(count1, id_serv);

            int cnt = 5;
            int cnt_ = 35;
            int i = 0;
            int[] save = new int[cnt + cnt_];
            for (int j = 0; j < mass.GetLength(0) - 1; j++)
            {
                string tmp = mass[ind, j];
                if (tmp != "0.0")
                {
                    save[i] = j;
                    i++;
                    if (i >= cnt)
                        break;
                }
            }

            for (int j = 0; j < mass.GetLength(0) - 1; j++)
            {
                string tmp = mass[ind, j];
                if (tmp == "0.0")
                {
                    save[i] = j;
                    i++;
                    if (i >= cnt + cnt_)
                        break;
                }
            }

            conn.ChangeDatabase("AIS_STAT_Training");
            for (int k = 0; k < save.Length; k++)
            {
                SqlCommand command = new SqlCommand(
                string.Format("select top 1 service_title from AIS_cpgu_order where service = {0} and custom_service_id = {1}", count1[save[k]], count2[save[k]]), conn);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                    comboBox2.Items.Add(reader["service_title"]);
                reader.Close();
            }

            conn.ChangeDatabase("AIS_DEN_DOBRII");
            SqlCommand command1 = new SqlCommand(
            string.Format("select Docs.id_doc, Docs.Doc_name, requesters.id_doc from Docs, requesters where Docs.id_doc = requesters.id_doc and requesters.id_requester = {0}", id_client), conn);
            SqlDataReader reader1 = command1.ExecuteReader();
            while (reader1.Read())
                listBox1.Items.Add(reader1["Doc_name"]);
            reader1.Close();

        }
    }


}
