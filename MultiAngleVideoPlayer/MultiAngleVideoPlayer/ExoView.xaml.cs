using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace MultiAngleVideoPlayer
{
    public sealed partial class ExoView : UserControl, IViewable
    {
        bool playing = false;
        TimeSpan position;
        MainPage mainPage;

        // ---------------------------------------------- NON-UI EVENT HANDLERS ----------------------------------------------

        

        // -------------------------------------------------- PUBLIC METHODS --------------------------------------------------

        // ------------------------------------------------- PRIVATE METHODS --------------------------------------------------

        // --------------------------------------------------- CONSTRUCTORS ---------------------------------------------------

        public ExoView()
        {
            this.InitializeComponent();
            VideoControlGrid.viewer = this;
        }


        // ------------------------------------------------ UI EVENT HANDLERS -------------------------------------------------

        public void AngleChoice_Tapped(object sender, TappedRoutedEventArgs e)
        {
            position = CurrentVideo.Position;
            CameraControl camera = (CameraControl)sender;
            PauseVid();

            NoVidMessage.Visibility = Visibility.Collapsed;
            VideoControlGrid.EnableButton(true);

            //set main video
            CurrentVideo.Source = camera.GetVideoUri();
            PlayVid();
            playing = true;
            VideoControlGrid.ChangeButtonLabel("Pause");
        }

        public void SetMainPageReference(MainPage page)
        {
            mainPage = page;
        }

        public void CurrentVideo_MediaOpened(object sender, RoutedEventArgs e)
        {
            if (position != null)
            {
                CurrentVideo.Position = position;
            }
        }

        public void GridVisible()
        {
            VideoControlGrid.EnableButton(false);
            SetupVideos();
        }

        public void PauseVid()
        {
            CurrentVideo.Pause();
        }

        public void PlayPause()
        {
            if (!playing)
            {
                VideoControlGrid.ChangeButtonLabel("Pause");
                playing = true;
                PlayVid();
            }
            else
            {
                VideoControlGrid.ChangeButtonLabel("Play");
                playing = false;
                PauseVid();
            }
        }

        public void PlayVid()
        {
            CurrentVideo.Play();
        }

        public void SetupVideos()
        {
            Camera0.IsTapEnabled = true;
            Camera1.IsTapEnabled = true;
            Camera2.IsTapEnabled = true;
            Camera3.IsTapEnabled = true;
            Camera4.IsTapEnabled = true;

            Uri[] paths = mainPage.GetVideos();

            Camera0.SetVideoUri(paths[0]);
            Camera1.SetVideoUri(paths[1]);
            Camera2.SetVideoUri(paths[2]);
            Camera3.SetVideoUri(paths[3]);
            Camera4.SetVideoUri(paths[4]);

        }

        private void ExoBackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
            mainPage.ShowMenu();
        }
    }
}
