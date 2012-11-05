using System;
using System.Windows;
using System.Data;
using System.Xml;
using System.Configuration;

namespace Facebooker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static App()
        {
            Facebooker.Startup.Check();
            if (Facebooker.Startup.Rantoday() == false)
            {
                try
                {
                    Facebook.Login();
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Error logging in: " + ex.Message);
                }
                //this.Run(new MainWindow());
                //Application.Current.Run(new MainWindow());
                new MainWindow().ShowDialog();
            }
            else
            {
                Environment.Exit(0);
            }
        }
    }
}