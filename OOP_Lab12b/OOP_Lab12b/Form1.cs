using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace OOP_Lab12b
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public static readonly string[] znakNames = { "Овен", "Телец", "Близнецы", "Рак", "Лев", "Дева", "Весы", "Скорпион", "Стрелец", "Козерог", "Водолей", "Рыбы" };

        bool CheckBirthdayArray( int[] arr )
        {
            return arr.Length == 3 && arr[1] >= 1 && arr[1] <= 12 && arr[0] > 0 && arr[0] <= Program.GetMaxDayForDate(arr[1], arr[2]);
        }

        void RegenerateTable( List<Program.ZNAK> znaks )
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("N"));
            dt.Columns.Add(new DataColumn("Имя"));
            dt.Columns.Add(new DataColumn("Фамилия"));
            dt.Columns.Add(new DataColumn("Знак"));
            dt.Columns.Add(new DataColumn("Дата"));

            int id = 0;

            foreach ( var znak in znaks )
            {
                DataRow dr = dt.NewRow();

                dr["N"] = id++;

                if (znak.firstName.Length > 0)
                    dr["Имя"] = znak.firstName;
                else
                    dr["Имя"] = "Corrupted";

                if (znak.lastName.Length > 0)
                    dr["Фамилия"] = znak.lastName;
                else
                    dr["Фамилия"] = "Corrupted";

                if (znak.zodiac >= 0 && znak.zodiac < 12)
                    dr["Знак"] = znakNames[znak.zodiac];
                else
                    dr["Знак"] = "Corrupted";

                if (CheckBirthdayArray(znak.birthday))
                    dr["Дата"] = String.Format("{0}.{1}.{2}", znak.birthday[0], znak.birthday[1], znak.birthday[2]);
                else
                    dr["Дата"] = "Corrupted";

                dt.Rows.Add(dr);
            }

            dataGridView1.DataSource = dt;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Add_Click(object sender, EventArgs e)
        {
            Form2 form = new Form2();

            if( form.ShowDialog() == DialogResult.OK )
            {
                Program.znaks.Add( form.znak );
                RegenerateTable(Program.znaks);
            }
        }

        private void Remove_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex];
            int id = Convert.ToInt32(row.Cells[0].Value);

            Program.znaks.RemoveAt(id);
            // dataGridView1.Rows.Remove(row);
            RegenerateTable(Program.znaks);
        }



        private void Reset_Click(object sender, EventArgs e)
        {
            RegenerateTable(Program.znaks);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_Sorted(object sender, EventArgs e)
        {

        }

        private void Load_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            if( dialog.ShowDialog() == DialogResult.OK )
            {
                Stream stream = null;
                try
                {
                    stream = dialog.OpenFile();

                    Program.DeserializeZnaks(stream);

                    RegenerateTable(Program.znaks);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                finally
                {
                    if (stream != null) stream.Close();
                }
            }
        }

        private void Save_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Stream stream = null;
                try
                {
                    stream = dialog.OpenFile();

                    Program.SerializeZnaks(stream);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                finally
                {
                    if( stream != null ) stream.Close();
                }
            }

        }

        private void CondSearch_Click(object sender, EventArgs e)
        {
            Form3 form = new Form3();

            if( form.ShowDialog() == DialogResult.OK )
            {
                List<Program.ZNAK> znaks = new List<Program.ZNAK>();

                foreach (Program.ZNAK znak in Program.znaks)
                {
                    if( form.IsSpecial )
                    {
                        if (form.Month == znak.birthday[1])
                            znaks.Add(znak);
                    }
                    else
                    {
                        if (form.SearchName == znak.firstName)
                            znaks.Add(znak);
                    }
                }

                RegenerateTable(znaks);
            }
        }
        
    }
}
