using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Prog2SlutProjekt03
{
    static class Program
    {
        public static void GameLoop()
        {
            Application.Run(new Form1());
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            GameLoop();
        }
    }
}
