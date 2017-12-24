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

namespace OOP_Lab12
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string fileData;
        int cachedWordCount = 0;
        bool scanned = false;

        private void loadFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Filter = "*.txt|*.txt";
            dialog.FilterIndex = 1;

            if( dialog.ShowDialog() == DialogResult.OK )
            {
                try
                {
                    fileData = File.ReadAllText(dialog.FileName);

                    
                    cachedWordCount = 0;
                    scanned = false;

                    label2.Text = label1.Text = "Не посчитано";

                    wordCountToolStripMenuItem.Enabled = true;
                    symbolCountToolStripMenuItem.Enabled = true;
                }
                catch( Exception ex )
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void wordCountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if( !scanned )
            {
                cachedWordCount = GetSymbolCountFromArray(fileData, ' ', true) + 1;
            }

            label1.Text = String.Format(Properties.Resources.WordCount_msg, cachedWordCount);
        }

        private void symbolCountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 dialog = new Form2();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                int symbolCount = GetSymbolCountFromArray(fileData, dialog.Symbol);

                label2.Text = String.Format(Properties.Resources.SymbolCount_msg, dialog.Symbol, symbolCount);
            }
        }

        private static int GetSymbolCountFromArray( string str, char symbol, bool wordMode = false )
        {
            int ret = 0;
            bool flag = false;

            for( int i = 0; i < str.Length; i++ )
            {
                if( str[i] == symbol )
                {
                    if (wordMode)
                    {
                        if (!flag)
                        {
                            ret++;
                        }
                    }
                    else ret++;
                    flag = true;
                }
                else if( !char.IsControl(str[i]) )
                {
                    flag = false;
                }
            }

            if( wordMode && flag )
            {
                ret--;
            }

            return ret;
        }
    }
}
