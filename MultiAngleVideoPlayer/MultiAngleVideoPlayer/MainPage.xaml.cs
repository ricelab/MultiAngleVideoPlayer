using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MultiAngleVideoPlayer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        bool playing = false;
        TimeSpan position;


        public MainPage()
        {
            this.InitializeComponent();

            VersionSelectGrid.Visibility = Visibility.Visible;
            EgoViewGrid.Visibility = Visibility.Collapsed;
            ExoViewGrid.Visibility = Visibility.Collapsed;

            LoadVideos();
            PlayPauseButton.IsEnabled = false;
        }

        private void LoadVideos()
        {
            AngleChoice0.IsTapEnabled = true;
            AngleChoice1.IsTapEnabled = true;
            AngleChoice2.IsTapEnabled = true;
            AngleChoice3.IsTapEnabled = true;
            AngleChoice4.IsTapEnabled = true;

            try
            {
                AngleChoice0.Source = new Uri("ms-appx:///Videos/concatNum1.mp4");
                AngleChoice1.Source = new Uri("ms-appx:///Videos/concatNum2.mp4");
                AngleChoice2.Source = new Uri("ms-appx:///Videos/concatNum3.mp4");
                AngleChoice3.Source = new Uri("ms-appx:///Videos/concatNum4.mp4");
                AngleChoice4.Source = new Uri("ms-appx:///Videos/concatNum5.mp4");

            } catch(Exception e)
            {
                Debug.WriteLine(e);
            }
            
        }

        private void V1Button_Click(object sender, RoutedEventArgs e)
        {
            VersionSelectGrid.Visibility = Visibility.Collapsed;
            EgoViewGrid.Visibility = Visibility.Visible;
        }

        private void V2Button_Click(object sender, RoutedEventArgs e)
        {
            VersionSelectGrid.Visibility = Visibility.Collapsed;
            ExoViewGrid.Visibility = Visibility.Visible;
        }

        private void ExoBackButton_Click(object sender, RoutedEventArgs e)
        {
            ExoViewGrid.Visibility = Visibility.Collapsed;
            VersionSelectGrid.Visibility = Visibility.Visible;
        }

        private void EgoBackButton_Click(object sender, RoutedEventArgs e)
        {
            EgoViewGrid.Visibility = Visibility.Collapsed;
            VersionSelectGrid.Visibility = Visibility.Visible;
        }

        private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (!playing)
            {
                PlayPauseButton.Content = "Pause";
                playing = true;
                PlayVids();
            } else
            {
                PlayPauseButton.Content = "Play";
                playing = false;
                PauseVids();
            }      
        }

        private void PlayVids()
        {
            AngleChoice0.Play();
            AngleChoice1.Play();
            AngleChoice2.Play();
            AngleChoice3.Play();
            AngleChoice4.Play();
            CurrentVideo.Play();
        }

        private void PauseVids()
        {
            AngleChoice0.Pause();
            AngleChoice1.Pause();
            AngleChoice2.Pause();
            AngleChoice3.Pause();
            AngleChoice4.Pause();
            CurrentVideo.Pause();
        }

        private void AngleChoice_Tapped(object sender, TappedRoutedEventArgs e)
        {
            PauseVids();

            NoVidMessage.Visibility = Visibility.Collapsed;
            PlayPauseButton.IsEnabled = true;

            //adjust border colors
            foreach (Border b in Borders.Children)
            {
                b.BorderBrush = new SolidColorBrush(Colors.Black);
            }
            MediaElement selected = (MediaElement)sender;
            char[] name = selected.Name.ToCharArray();
            int vidNum = Int32.Parse(name.Last<char>().ToString());

            Border border = (Border)(Borders.FindName("Border" + vidNum));
            border.BorderBrush = new SolidColorBrush(Colors.Blue);          

            //set main video
            CurrentVideo.Source = selected.Source;
            PlayVids();
            playing = true;
            PlayPauseButton.Content = "Pause";
            position = selected.Position;
        }

        private void CurrentVideo_MediaOpened(object sender, RoutedEventArgs e)
        {
            if (position != null)
            {
                CurrentVideo.Position = position;
            }
        }
    }
}
