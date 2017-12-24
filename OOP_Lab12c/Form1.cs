using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace WindowsFormsApp12
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public struct Coord
        {
            public float x;
            public float y;
        }

        private List<Coord> coords = new List<Coord>();

        private Color barColor;
        private Color lineColor;

        private void inputDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            if( dialog.ShowDialog() == DialogResult.OK )
            {
                Stream stream = null;

                try
                {
                    stream = dialog.OpenFile();

                    JsonSerializer jsonSerializer = new JsonSerializer();
                    
                    

                    coords = (List<Coord>)jsonSerializer.Deserialize(new StreamReader( stream ), typeof(List<Coord>));

                    RenderAxes();
                }
                catch( Exception ex )
                {
                    MessageBox.Show("ашыпка " + ex.Message);
                }
                finally
                {
                    stream.Close();
                }
            }
        }

        private void chooseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1Choose choose = new Form1Choose();
            
            if( choose.ShowDialog() == DialogResult.OK )
            {
                switch( choose.render )
                {
                case Program.Render.BAR:
                        barToolStripMenuItem.Enabled = true;
                        barColor = choose.color;
                        break;
                case Program.Render.LINE:

                        lineToolStripMenuItem.Enabled = true;
                        lineColor = choose.color;

                        break;
                }
            }
        }

        private void DescribeRenderParms()
        {
            String text = "";

            if( lineToolStripMenuItem.Enabled )
            {
                text += lineColor.ToString() + " Line Render";
            }

            if( barToolStripMenuItem.Enabled )
            {
                text += barColor.ToString() + " Bar Render";
            }
        }

        private void RenderLine()
        {
            DescribeRenderParms();
            Pen pen = new Pen(lineColor, 1);

            for (int i = 1; i < coords.Count; i++)
            {
                float w = pictureBox1.Width;
                float h = pictureBox1.Height;
                Graphics g = pictureBox1.CreateGraphics();
                g.DrawLine(pen, w/2 + coords[i-1].x, h/2 - coords[i - 1].y, w/2 + coords[i].x, h/2 - coords[i].y);
            }
        }

        private void RenderBar()
        {
            DescribeRenderParms();
            Pen pen = new Pen(barColor, 1);

            for (int i = 0; i < coords.Count; i++)
            {
                float w = pictureBox1.Width;
                float h = pictureBox1.Height;
                Graphics g = pictureBox1.CreateGraphics();
                g.DrawLine(pen, w / 2 + coords[i].x, h / 2 - coords[i].y, w / 2 + coords[i].x, h / 2);
            }
        }

        private void RenderAxes()
        {
            float w = pictureBox1.Width;
            float h = pictureBox1.Height;
            Graphics g = pictureBox1.CreateGraphics();
            g.DrawLine(Pens.Black, w / 2, 0, w / 2, h);
            g.DrawLine(Pens.Black, 0, h / 2, w, h / 2);

            Font f = new Font("Trebuchet MS", 9);
            SolidBrush b = new SolidBrush(Color.Black);
            StringFormat strf = new StringFormat();
            StringFormat strf2 = new StringFormat();
            strf.FormatFlags = StringFormatFlags.DirectionVertical;
            strf2.Alignment = StringAlignment.Far;
            for (int i = 0; i < 500; i+=20)
            {
                PointF p1 = new PointF(w / 2 + i, h / 2    );
                PointF p2 = new PointF(w / 2,     h / 2 - i);
                PointF p3 = new PointF(w / 2 - i, h / 2    );
                PointF p4 = new PointF(w / 2,     h / 2 + i);
                g.DrawString(Convert.ToString(i), f, b, p1, strf);
                g.DrawString(Convert.ToString(i), f, b, p2, strf2);
                g.DrawString(Convert.ToString(i), f, b, p3, strf);
                g.DrawString(Convert.ToString(i), f, b, p4, strf2);
            }
        }

        private void lineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RenderLine();
        }

        private void barToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RenderBar();
        }

        private void Task3_Load(object sender, EventArgs e)
        {
            
        }

        private void Task3_Resize(object sender, EventArgs e)
        {
            pictureBox1.Update();
            RenderAxes();

            if (lineToolStripMenuItem.Enabled)
                RenderLine();

            if (barToolStripMenuItem.Enabled)
                RenderBar();
        }
    }
}
