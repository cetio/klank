using Klank;
using Klank.Generic;
using Microsoft.Win32;
using System.Diagnostics;
using System.Text;

namespace KlankENV
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            dotNetMemoryScan find_aob = new dotNetMemoryScan();

            // with simple array
            Debug.WriteLine(BitConverter.ToString(Encoding.ASCII.GetBytes(Hardware.DiskSerial)).Replace('-', ' '));
            var find_ptr = find_aob.scan_all("nvcontainer.exe", BitConverter.ToString(Encoding.ASCII.GetBytes(Hardware.DiskSerial)).Replace('-', ' '));
            Debug.WriteLine(find_ptr);
        }
    }
}