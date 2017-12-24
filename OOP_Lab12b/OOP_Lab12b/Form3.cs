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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        public bool IsSpecial { get; private set; }
        public string SearchName { get; private set; }
        public int Month { get; private set; }

        private void button2_Click(object sender, EventArgs e)
        {
            if( !radioButton1.Checked && textBox1.Text.Length == 0 )
            {
                MessageBox.Show("Введите критерий поиска!");
                this.DialogResult = DialogResult.None;
                return;
            }

            IsSpecial = radioButton1.Checked;
            SearchName = textBox1.Text;
            Month = dateTimePicker1.Value.Month;
            Close();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Visible = true;
            dateTimePicker1.Visible = false;

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Visible = false;
            dateTimePicker1.Visible = true;

        }
    }
}
