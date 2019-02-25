using System;
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
using Windows.UI;
using System.Diagnostics;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace MultiAngleVideoPlayer
{
    public sealed partial class EgoView : UserControl, IViewable
    {
        bool playing = false;
        TimeSpan position;
        MainPage mainPage;

        // --------------------------------------------------- CONSTRUCTORS ---------------------------------------------------

        /// <summary>
        /// Generic constructor
        /// </summary>
        public EgoView()
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
            AngleChoice0.IsTapEnabled = true;
            AngleChoice1.IsTapEnabled = true;
            AngleChoice2.IsTapEnabled = true;
            AngleChoice3.IsTapEnabled = true;
            AngleChoice4.IsTapEnabled = true;

            Uri[] paths = mainPage.GetVideos();

            AngleChoice0.Source = paths[0];
            AngleChoice1.Source = paths[1];
            AngleChoice2.Source = paths[2];
            AngleChoice3.Source = paths[3];
            AngleChoice4.Source = paths[4];
        }

        /// <summary>
        /// Starts all videos playing.
        /// </summary>
        public void PlayVid()
        {
            AngleChoice0.Play();
            AngleChoice1.Play();
            AngleChoice2.Play();
            AngleChoice3.Play();
            AngleChoice4.Play();
            CurrentVideo.Play();
        }

        /// <summary>
        /// Pause all videos.
        /// </summary>
        public void PauseVid()
        {
            AngleChoice0.Pause();
            AngleChoice1.Pause();
            AngleChoice2.Pause();
            AngleChoice3.Pause();
            AngleChoice4.Pause();
            CurrentVideo.Pause();
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
        /// To be called by the main page.
        /// </summary>
        /// <param name="page">A reference to the application's main page.</param>
        public void SetMainPageReference(MainPage page)
        {
            mainPage = page;
        }

        // ------------------------------------------------ UI EVENT HANDLERS -------------------------------------------------

        /// <summary>
        /// Called when the back button is clicked. Navigates back to the version selection menu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EgoBackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
            mainPage.ShowMenu();
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

        /// <summary>
        /// Called when the user selects a video angle.
        /// De-highlights any previously highlighted video angles, then highlights the currently selected angle.
        /// Sets the source of the current playing video to match the selected angle.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void AngleChoice_Tapped(object sender, TappedRoutedEventArgs e)
        {
            PauseVid();

            NoVidMessage.Visibility = Visibility.Collapsed;
            VideoControlGrid.EnableButton(true);

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
            PlayVid();
            playing = true;
            VideoControlGrid.ChangeButtonLabel("Pause");
            position = selected.Position;
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
