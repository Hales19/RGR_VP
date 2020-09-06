using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Word = Microsoft.Office.Interop.Word;
using System.Reflection;
using System.Drawing.Printing;

namespace RGR_VP
{
    public partial class Form1 : Form
    {
        private SqlConnection sqlConnection = null;

        private SqlCommandBuilder sqlBuilder = null;

        private SqlDataAdapter sqlDataAdapter = null;

        private DataSet dataSet = null;

        private string result = "";

        int people = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void LoadData()
        {
            try
            {
                sqlDataAdapter = new SqlDataAdapter("SELECT * FROM Recepts", sqlConnection);

                dataSet = new DataSet();

                sqlDataAdapter.Fill(dataSet, "Recepts");

                dataGridView1.DataSource = dataSet.Tables["Recepts"];

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Евгений\source\repos\RGR_VP\RGR_VP\Database1.mdf;Integrated Security=True");

            sqlConnection.Open();

            LoadData();
        }

        

        

        void PrintPageHandler(object sender, PrintPageEventArgs e)
        {
            // печать строки result
            e.Graphics.DrawString(result, new Font("Calibri", 14), Brushes.Black, 0, 0);
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string s;
            s = textBox1.Text;

            try
            {
                people = Convert.ToInt32(s);
                if(people <=0) MessageBox.Show("The number of peopole can't be <=0", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch(Exception ex)
            {

            }
            
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                sqlDataAdapter = new SqlDataAdapter("SELECT * FROM Recepts WHERE (Dish LIKE'%" + textBox2.Text +
                    "%') AND (Type LIKE '" + textBox3.Text +
                    "%') AND (Ingredients LIKE '%" + textBox4.Text +
                    "%')", sqlConnection);

                dataSet = new DataSet();

                sqlDataAdapter.Fill(dataSet, "Recepts");

                dataGridView1.DataSource = dataSet.Tables["Recepts"];



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void button1_Click(object sender, EventArgs e)
        { string s1="Dish          ", s2="Type           ", s3 = "Ingredients";
            
            result += s1;
            result += s2.PadRight(14);
            result += s3.PadRight(20);
            result += "\n";

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                int l;
                s1= dataGridView1.Rows[i].Cells[0].Value + "";
                l = s1.Length;

                if (l > 7)
                    l = 4;
                else
                    l = 14 - l;
                result += s1;
                while (l > 0)
                {
                    result += " ";
                    l--;
                }

                s2 = dataGridView1.Rows[i].Cells[1].Value + "";
                l = s2.Length;
                l = 20 - l;
                result += s2;
                while (l > 0)
                {
                    result += " ";
                    l--;
                }

                s3 = dataGridView1.Rows[i].Cells[2].Value + "";
                
                result += s3;

                result += "\n";

            }
            PrintDocument printDocument = new PrintDocument();

            // обработчик события печати
            printDocument.PrintPage += PrintPageHandler;

            // диалог настройки печати
            PrintDialog printDialog = new PrintDialog();

            // установка объекта печати для его настройки
            printDialog.Document = printDocument;

            // если в диалоге было нажато ОК
            if (printDialog.ShowDialog() == DialogResult.OK)
                printDialog.Document.Print(); // печатаем
            result = "";
            
        }

        private void обАвтореToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Сарветников Алексей ИП-815", "Author", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
