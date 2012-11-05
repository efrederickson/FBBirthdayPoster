using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Facebooker.FacebookObjects;

namespace Facebooker
{
    public delegate void WpfControlInvoker();

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            Loaded += new RoutedEventHandler(MainWindow_Loaded);
            Closing += delegate { Environment.Exit(0); };
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            System.Threading.Tasks.Task t =
                new System.Threading.Tasks.Task(
                    new Action(
                        delegate
                        {
                            try
                            {
                                //Title = Facebook.FirstName + " " + Facebook.LastName;
                                Dispatcher.Invoke(
                                    new WpfControlInvoker(
                                        delegate
                                        {
                                            Title = "FB Birthday Poster - " + Facebook.Name;
                                        }));
                                
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.ToString());
                            }
                        }));
            t.Start();
            start();
        }
        
        void a()
        {
            string report = "";
            DateTime now = DateTime.Now;
            var x = Facebook.Friends;
            Dispatcher.Invoke(new Action(delegate { 
                                             progress.Value = 0;
                                             progress.Maximum = x.Length;
                                         }));
            int i = 1;
            int max = x.Length;
            foreach (User u in x)
            {
                Dispatcher.Invoke(new Action(delegate { 
                                                 progress.Value++;
                                                 userLabel.Content = "Checking " + u.Name + "... Friend " + (i++) + " of " + max;
                                             }));
                DateTime bd = u.Birthday;
                if (bd != DateTime.MinValue)
                {
                    if (bd.Day == now.Day)
                    {
                        if (bd.Month == now.Month)
                    {
                            report += u.Name + " has a birthday today!\r\n";
                            //;
                            Facebook.PostToFriendsWall(u, "Happy Birthday!");
                            //MessageBox.Show(u.Name + " has a birthday today!");
                    }
                    }
                }
            }
            Dispatcher.Invoke(new Action(delegate {
                                             progress.Value = 0;
                                             userLabel.Content = "done!";
                                             if (report != "")
                                                 MessageBox.Show(report);
                                             this.Close();
                                         }));
        }
        
        void start()
        {
            Thread t = new Thread(new ThreadStart(a));
            t.Start();
        }
        
        void checkButton_Click(object sender, RoutedEventArgs e)
        {
            start();
        }
    }
}