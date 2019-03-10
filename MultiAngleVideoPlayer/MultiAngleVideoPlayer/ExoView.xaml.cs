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

        // --------------------------------------------------- CONSTRUCTORS ---------------------------------------------------

        /// <summary>
        /// Generic constructor
        /// </summary>
        public ExoView()
        {
            this.InitializeComponent();
            VideoControlGrid.viewer = this;
        }


        // -------------------------------------------------- PUBLIC METHODS --------------------------------------------------

        /// <summary>
        /// Enables tap and sets the source URI of all video choices.
        /// </summary>
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

        /// <summary>
        /// To be called by the main page.
        /// </summary>
        /// <param name="page">A reference to the application's main page.</param>
        public void SetMainPageReference(MainPage page)
        {
            mainPage = page;
        }

        /// <summary>
        /// To be called by the main page when this control becomes visible.
        /// Disables the play/pause button to start and starts the video setup.
        /// </summary>
        public void GridVisible()
        {
            VideoControlGrid.EnableButton(false);
            SetupVideos();
        }

        /// <summary>
        /// Chooses whether to play or pause the video based on the current video status
        /// </summary>
        public void PlayPause()
        {
            if (!playing)
            {
                PlayVid();
            }
            else
            {
                PauseVid();
            }
        }

        /// <summary>
        /// Pause the video.
        /// </summary>
        public void PauseVid()
        {
            VideoControlGrid.ChangeButtonLabel("Play");
            playing = false;
            CurrentVideo.Pause();
        }

        /// <summary>
        /// Play the video.
        /// </summary>
        public void PlayVid()
        {
            VideoControlGrid.ChangeButtonLabel("Pause");
            playing = true;
            CurrentVideo.Play();
        }


        // ------------------------------------------------ UI EVENT HANDLERS -------------------------------------------------

        /// <summary>
        /// Called when the user selects a video angle.
        /// Sets the source of the current playing video to match the selected angle.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void AngleChoice_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //record the video position and then pause
            position = CurrentVideo.Position;
            CameraControl camera = (CameraControl)sender;
            CurrentVideo.Pause();

            //remove no vid message if it is still there
            NoVidMessage.Visibility = Visibility.Collapsed;
            VideoControlGrid.EnableButton(true);

            //set main video
            CurrentVideo.Source = camera.GetVideoUri();
            PlayVid();
        }

        /// <summary>
        /// Called when the back button is clicked. Navigates back to the version selection menu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExoBackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
            mainPage.ShowMenu();
        }

        // ---------------------------------------------- NON-UI EVENT HANDLERS ----------------------------------------------

        /// <summary>
        /// Called when the media source for the current video is opened.
        /// Video position is not settable until the media is opened.
        /// Sets the video position to match the position of the mini video.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CurrentVideo_MediaOpened(object sender, RoutedEventArgs e)
        {
            if (position != null)
            {
                CurrentVideo.Position = position;
            }
        }

    }
}
