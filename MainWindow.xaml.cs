using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using YeelightAPI;



namespace yeelight_controler
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DeviceGroup devices;
        bool status = false;

        public MainWindow() 
        {
            lookFor(0);
            InitializeComponent();
        }

        async void ButtonTurnMode(object sender, RoutedEventArgs e)
        {

            try
            {
                await devices.Connect();
                await devices.Toggle();
                lookFor(50);
                devices.Disconnect();
            }
            catch(Exception ex)
            {
                Debug.Write("Connection ex");
            } 
           
        }

        async void ButtonFlash(object sender, RoutedEventArgs e)
        {
            DeviceGroup selected = new DeviceGroup();
            foreach (Device device in DG.SelectedItems)
            {
                selected.Add(new Device(device.Hostname));
            }

            await selected.Connect();
            int n = 0;
            while (n < 10)
            {
                foreach (Device dev in selected)
                {
                    Random random = new Random();
                    
                    int r = random.Next(1, 254);
                    int g = random.Next(1, 254);
                    int b = random.Next(1, 254);

                    await dev.SetRGBColor(r, g, b);
                    connLabel.Content = "RED: " + r + " GREEN: " + g + " BLUE: " + b;
                   
                }
                n++;
            }
            selected.Disconnect();

        }

        async void lookFor(int time)
        {
            Thread.Sleep(time);

            List<Device> lista = await DeviceLocator.Discover();
            DG.ItemsSource = lista;
            devices = new DeviceGroup();

            foreach(Device d in lista)
            {
                devices.Add(d);
                d.Name = "Sample";
            }


        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = sender as Button;
                Device device = button.DataContext as Device;


                await device.Connect();
                await device.Toggle();

                lookFor(50);


                device.Disconnect();
            }catch(Exception ex)
            {
                Debug.Write("Connection ex");
            }
        }

        private void Button_Initialized(object sender, EventArgs e)
        {
            Button button = sender as Button;
            Device device = button.DataContext as Device;
            
            if (device.Properties["power"].Equals("off"))
            {
                button.Content = "TURN ON";
            }
            else
            {
                button.Content = "TURN OFF";
            }
        }

        private void Image_Initialized(object sender, EventArgs e)
        {
            Image image = sender as Image;

            Device device = image.DataContext as Device;

            if (device.Properties["power"].Equals("off"))
            {
                image.Source = new BitmapImage(new Uri(@"/Images/icon_yeelight_bulb2.png", UriKind.Relative));
            }
            else
            {
                image.Source = new BitmapImage(new Uri(@"/Images/icon_yeelight_bulb2ON.png", UriKind.Relative));
            }
        }

        private void ButtonGroupTurn_Initialized(object sender, EventArgs e)
        {
            /*foreach(Device device in devices)
            {
                if (device.Properties["power"].Equals("on"))
                {
                    status = true;
                    GroupTurnButton.Content = "TURN OFF";
                }
            }
            */


        }

        private async void ButtonColor_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Device device = button.DataContext as Device;


            colorPicker cp = new colorPicker(device);
            cp.Show();


        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListViewItem item = sender as ListViewItem;
            Device device = item.DataContext as Device;


            colorPicker cp = new colorPicker(device);
            cp.Show();

        }
    }
}
