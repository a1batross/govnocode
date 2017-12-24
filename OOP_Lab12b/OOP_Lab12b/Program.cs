using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;

namespace OOP_Lab12b
{
    static public class Program
    {
        [Serializable]
        public struct ZNAK
        {
            public string firstName;
            public string lastName;
            public int zodiac;
            public int[] birthday;
        }

        public static List<ZNAK> znaks = new List<ZNAK>();

        public class ZnakComparer : IComparer<ZNAK>
        {
            public int Compare(ZNAK a, ZNAK b)
            {
                if (a.zodiac < b.zodiac)
                    return -1;
                if (a.zodiac > b.zodiac)
                    return 1;
                return 0;
            }
        }

        public static void DeserializeZnaks( Stream stream )
        {
            XmlSerializer ser = new XmlSerializer(typeof(List<ZNAK>));
            znaks = (List<ZNAK>)ser.Deserialize(stream);
        }

        public static void SerializeZnaks( Stream stream )
        {
            XmlSerializer ser = new XmlSerializer(typeof(List<ZNAK>));
            ser.Serialize(stream, znaks);
        }

        public static int GetMaxDayForDate(int month, int year)
        {
            if (month == 2)
            {
                if (year % 4 != 0)
                    return 28;
                else
                    return 29;
            }
            else if (month % 2 == 0)
                return 30;
            else
                return 31;
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
