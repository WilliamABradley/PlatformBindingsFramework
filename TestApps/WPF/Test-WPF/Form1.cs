using PlatformBindings;
using PlatformBindings.Models.FileSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test_WPF
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var props = new FolderPickerProperties();
            props.FileTypes.Add("*");
            props.FileTypes.Add(".mkv");
            props.FileTypes.Add(".wmv");
            props.StartingLocation = PlatformBindings.Enums.PathRoot.Application;

            var result = await AppServices.Current.IO.Pickers.PickFolder(props);
            var p = "as";
        }
    }
}