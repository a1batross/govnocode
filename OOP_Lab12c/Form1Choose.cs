using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp12
{
    public partial class Form1Choose : Form
    {
        public Form1Choose()
        {
            InitializeComponent();
        }

        
        private void Task3Choose_Load(object sender, EventArgs e)
        {
            listBox1.Items.AddRange(Program.colorsNames);
        }

        public Color color { get; private set; }

        public Program.Render render { get; private set; }

        private void button1_Click(object sender, EventArgs e)
        {
            if( listBox1.SelectedIndex < 0 && listBox1.SelectedIndex >= Program.colors.Length ||
                !radioButton1.Checked && !radioButton2.Checked )
            {
                MessageBox.Show("Ашыпка!");
                this.DialogResult = DialogResult.Retry;
                return;
            }

            if (radioButton1.Checked)
                render = Program.Render.LINE;
            else
                render = Program.Render.BAR;

            color = Program.colors[listBox1.SelectedIndex];

            this.Close();
        }
    }
}
