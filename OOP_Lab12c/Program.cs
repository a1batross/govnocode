using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace WindowsFormsApp12
{
    public static class Program
    {
        public static readonly string[] colorsNames = { "Красный", "Зелёный", "Синий" };
        public static readonly Color[] colors = { Color.Red, Color.Green, Color.Blue };


        public enum Render
        {
            LINE,
            BAR
        }


        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
