using System;
using System.Windows.Forms;

namespace EzDecoder
{
    static class MainClass
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new EzDecoder.App.MainForm());
        }
    }
}