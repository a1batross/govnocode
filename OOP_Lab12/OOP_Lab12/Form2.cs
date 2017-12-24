using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OOP_Lab12
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        public char Symbol { get; private set; }

        private void button1_Click(object sender, EventArgs e)
        {
            Symbol = textBox1.Text[0];

            Close();
        }
    }
}
