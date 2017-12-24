using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OOP_Lab12b
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public int Zodiac { get; private set; }
        public int BirthDay { get; private set; }
        public int BirthMonth { get; private set; }
        public int BirthYear { get; private set; }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        public Program.ZNAK znak { get; set;}

        private void button1_Click(object sender, EventArgs e)
        {
            if( FirstNameText.Text.Length == 0 ||
                LastNameText.Text.Length == 0 )
            {
                MessageBox.Show("Ошибка", "Введены неправильные данные");
                this.DialogResult = DialogResult.None;
                return;
            }

            Program.ZNAK newznak = new Program.ZNAK
            {
                firstName = FirstNameText.Text,
                lastName = LastNameText.Text,

                zodiac = ZnakComboBox.SelectedIndex,

                birthday = new int[3]
            };
            newznak.birthday[0] = dateTimePicker1.Value.Day;
            newznak.birthday[1] = dateTimePicker1.Value.Month;
            newznak.birthday[2] = dateTimePicker1.Value.Year;

            znak = newznak;

            Close();
        }
    }
}
