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
        double rate = 1;
        MainPage mainPage;
        DispatcherTimer timer;

        // --------------------------------------------------- CONSTRUCTORS ---------------------------------------------------

        /// <summary>
        /// Generic constructor
        /// </summary>
        public EgoView()
        {
            this.InitializeComponent();
            VideoControlGrid.viewer = this;
            TimerSetup();
        }

        // -------------------------------------------------- PRIVATE METHODS -------------------------------------------------

        private void TimerSetup()
        {
            timer = new DispatcherTimer();
            timer.Tick += TimerTick;
            timer.Interval = new TimeSpan(0, 0, 0, 1);
        }

        private void TimerTick(object sender, object e)
        {
            if (playing)
            {
                TimeSpan currentPosition = CurrentVideo.Position;
                double minutes = (currentPosition.TotalHours / 60) + currentPosition.TotalMinutes;
                VideoControlGrid.UpdateProgressBar((minutes / 60) + currentPosition.TotalSeconds);
            }
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

            AngleChoice0.Opacity = 0.3;
            AngleChoice1.Opacity = 0.3;
            AngleChoice2.Opacity = 0.3;
            AngleChoice3.Opacity = 0.3;
            AngleChoice4.Opacity = 0.3;
        }

        /// <summary>
        /// Starts all videos playing.
        /// </summary>
        public void PlayVid()
        {
            VideoControlGrid.ChangeButtonLabel("Pause");
            playing = true;

            AngleChoice0.Play();
            AngleChoice1.Play();
            AngleChoice2.Play();
            AngleChoice3.Play();
            AngleChoice4.Play();
            CurrentVideo.Play();

            CurrentVideo.PlaybackRate = rate;
            AngleChoice0.PlaybackRate = rate;
            AngleChoice1.PlaybackRate = rate;
            AngleChoice2.PlaybackRate = rate;
            AngleChoice3.PlaybackRate = rate;
            AngleChoice4.PlaybackRate = rate;

            timer.Start();
            CurrentVideo.PlaybackRate = rate;
        }

        /// <summary>
        /// Pause all videos.
        /// </summary>
        public void PauseVid()
        {
            VideoControlGrid.ChangeButtonLabel("Play");
            playing = false;
            rate = CurrentVideo.PlaybackRate;

            AngleChoice0.Pause();
            AngleChoice1.Pause();
            AngleChoice2.Pause();
            AngleChoice3.Pause();
            AngleChoice4.Pause();
            CurrentVideo.Pause();
            timer.Stop();
        }

        /// <summary>
        /// To be called by the main page when this control becomes visible.
        /// Disables the play/pause button to start and starts the video setup.
        /// </summary>
        public void GridVisible()
        {
            VideoControlGrid.EnableButtons(false);
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

        public void UpdateVideoSpeed(double speed)
        {
            CurrentVideo.PlaybackRate = speed;
            AngleChoice0.PlaybackRate = speed;
            AngleChoice1.PlaybackRate = speed;
            AngleChoice2.PlaybackRate = speed;
            AngleChoice3.PlaybackRate = speed;
            AngleChoice4.PlaybackRate = speed;
        }

        public void UpdatePosition(int change)
        {
            CurrentVideo.Position += new TimeSpan(0,0,change);
            AngleChoice0.Position += new TimeSpan(0, 0, change);
            AngleChoice1.Position += new TimeSpan(0, 0, change);
            AngleChoice2.Position += new TimeSpan(0, 0, change);
            AngleChoice3.Position += new TimeSpan(0, 0, change);
            AngleChoice4.Position += new TimeSpan(0, 0, change);
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

        /// <summary>
        /// Called when the user selects a video angle.
        /// De-highlights any previously highlighted video angles, then highlights the currently selected angle.
        /// Sets the source of the current playing video to match the selected angle.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void AngleChoice_Tapped(object sender, TappedRoutedEventArgs e)
        {
            CurrentVideo.Pause();

            NoVidMessage.Visibility = Visibility.Collapsed;
            VideoControlGrid.EnableButtons(true);

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
            rate = selected.PlaybackRate;
            PlayVid();
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
                CurrentVideo.PlaybackRate = rate;
            }

            TimeSpan duration = CurrentVideo.NaturalDuration.TimeSpan;
            double minutes = (duration.TotalHours / 60) + duration.TotalMinutes;
            VideoControlGrid.SetDuration((minutes / 60) + duration.TotalSeconds);
        }

        private void CurrentVideo_MediaEnded(object sender, RoutedEventArgs e)
        {
            timer.Stop();
        }
    }
}
