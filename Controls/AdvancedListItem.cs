using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Facebooker.Controls
{
    class AdvancedListItem : ListBoxItem
    {
        Label textLabel = new Label();
        Image image = new Image();

        public AdvancedListItem()
        {
            //<Image Name="image" Width="16" Height="16"/>
            //<Label Name="textLabel" Content="[Text]"/>
            image.Height = 16;
            image.Width = 16;
            image.Stretch = Stretch.Fill;
            textLabel.Content = "[TEXT]";
            StackPanel sp = new StackPanel();
            sp.Orientation = Orientation.Horizontal;
            sp.Children.Add(image);
            sp.Children.Add(textLabel);
            this.Content = sp;
        }

        public string Text
        {
            get
            {
                return textLabel.Content as string;
            }
            set
            {
                textLabel.Content = value;
            }
        }

        public string ImagePath
        {
            set
            {
                if (value.ToLower().EndsWith(".gif"))
                {
                    // todo: handle gifs
                }
                else
                {
                    if (System.IO.Path.IsPathRooted(value))
                        image.Source = new BitmapImage(new Uri(value));
                    else
                        image.Source = new BitmapImage(new Uri("file:///" + Environment.CurrentDirectory + "/" + value));
                }
            }
        }
    }
}
