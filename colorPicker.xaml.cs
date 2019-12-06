using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using YeelightAPI;

namespace yeelight_controler
{
    /// <summary>
    /// Logika interakcji dla klasy colorPicker.xaml
    /// </summary>
    public partial class colorPicker : Window
    {

        Device device;

        public colorPicker(Device device)
        {
            this.device = device;
            InitializeComponent();

            LabelName.Text = device.Name;
            LabelIP.Content = device.Hostname;

        }

        private void LabelName_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter){
                device.Name = LabelName.Text;
            }
        }
    }
}
