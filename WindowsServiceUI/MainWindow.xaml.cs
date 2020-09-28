using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
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

namespace WindowsServiceUI
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string CurrentDirectory = System.Environment.CurrentDirectory;
            System.Environment.CurrentDirectory = CurrentDirectory + "\\Service";
            Process process = new Process();
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.FileName = "Install.bat";
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            System.Environment.CurrentDirectory = CurrentDirectory;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string CurrentDirectory = System.Environment.CurrentDirectory;
            System.Environment.CurrentDirectory = CurrentDirectory + "\\Service";
            Process process = new Process();
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.FileName = "Uninstall.bat";
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            System.Environment.CurrentDirectory = CurrentDirectory;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ServiceController serviceController = new ServiceController("MyService");
            serviceController.Start();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            ServiceController serviceController = new ServiceController("MyService");
            if (serviceController.CanStop)
                serviceController.Stop();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            ServiceController serviceController = new ServiceController("MyService");
            if (serviceController.CanPauseAndContinue)
            {
                if (serviceController.Status == ServiceControllerStatus.Running)
                    serviceController.Pause();
                else if (serviceController.Status == ServiceControllerStatus.Paused)
                    serviceController.Continue();
            }
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            ServiceController serviceController = new ServiceController("MyService");
            string Status = serviceController.Status.ToString();
        }
    }
}