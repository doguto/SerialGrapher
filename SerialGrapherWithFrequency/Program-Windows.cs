// Windows Forms Program.cs for Serial Grapher with Frequency Setting
// Use this file when building the Windows Forms version

using System;
using System.Windows.Forms;

namespace SerialGrapherWithFrequency
{
    internal static class WindowsFormsProgram
    {
        /// <summary>
        /// The main entry point for the Windows Forms application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // Uncomment the line below when MainForm.cs is properly restored for Windows Forms
            // Application.Run(new MainForm());
            
            MessageBox.Show("Windows Forms version requires Windows environment.\nUse the console version for cross-platform demonstration.", 
                           "Serial Grapher with Frequency Setting", 
                           MessageBoxButtons.OK, 
                           MessageBoxIcon.Information);
        }
    }
}