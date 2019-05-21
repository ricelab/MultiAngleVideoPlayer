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
        bool chaptersSet = false;
        bool playing = false;
        bool paused = false;
        TimeSpan position;
        double rate = 1;
        MainPage mainPage;
        DispatcherTimer timer;
        double[] chapterTimes = null;
        string[] chapterNames = null;
        ChapterMarker[] markers = null;

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

        private void SetupChapters()
        {
            ChapAttributes[] chapters = mainPage.GetVideoChapters().Attributes;
            if (chapters != null)
            {
                List<double> tempChapterTimes = new List<double>();
                List<string> tempChapterNames = new List<string>();
                foreach (ChapAttributes c in chapters)
                {
                    //put the chapters in
                    tempChapterTimes.Add(c.StartTime);
                    tempChapterNames.Add(c.Name);
                }
                chapterTimes = tempChapterTimes.ToArray();
                chapterNames = tempChapterNames.ToArray();
            }
            else
            {
                Debug.WriteLine("chapters was null.");
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
            AngleChoice5.IsTapEnabled = true;

            Uri[] paths = mainPage.GetVideos();

            SetupChapters();

            AngleChoice0.Source = paths[0];
            AngleChoice1.Source = paths[1];
            AngleChoice2.Source = paths[2];
            AngleChoice3.Source = paths[3];
            AngleChoice4.Source = paths[4];
            AngleChoice5.Source = paths[5];

            AngleChoice0.Opacity = 0.3;
            AngleChoice1.Opacity = 0.3;
            AngleChoice2.Opacity = 0.3;
            AngleChoice3.Opacity = 0.3;
            AngleChoice4.Opacity = 0.3;
            AngleChoice5.Opacity = 0.3;
        }

        /// <summary>
        /// Starts all videos playing.
        /// </summary>
        public void PlayVid()
        {
            VideoControlGrid.ChangeButtonLabel("Pause");
            playing = true;

            CurrentVideo.Play();            
            if (!paused)
            {
                AngleChoice0.Play();
                AngleChoice1.Play();
                AngleChoice2.Play();
                AngleChoice3.Play();
                AngleChoice4.Play();
                AngleChoice5.Play();
            }          
            
            CurrentVideo.PlaybackRate = rate;
            AngleChoice0.PlaybackRate = rate;
            AngleChoice1.PlaybackRate = rate;
            AngleChoice2.PlaybackRate = rate;
            AngleChoice3.PlaybackRate = rate;
            AngleChoice4.PlaybackRate = rate;
            AngleChoice5.PlaybackRate = rate;

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
            AngleChoice5.Pause();
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
            paused = !paused;

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
            AngleChoice5.PlaybackRate = speed;
        }

        public void UpdatePosition(int change)
        {
            CurrentVideo.Position += new TimeSpan(0,0,change);
            AngleChoice0.Position += new TimeSpan(0, 0, change);
            AngleChoice1.Position += new TimeSpan(0, 0, change);
            AngleChoice2.Position += new TimeSpan(0, 0, change);
            AngleChoice3.Position += new TimeSpan(0, 0, change);
            AngleChoice4.Position += new TimeSpan(0, 0, change);
            AngleChoice5.Position += new TimeSpan(0, 0, change);
        }

        public void NewVideoPosition(int pos)
        {
            CurrentVideo.Position = new TimeSpan(0, 0, pos);
            AngleChoice0.Position = new TimeSpan(0, 0, pos);
            AngleChoice1.Position = new TimeSpan(0, 0, pos);
            AngleChoice2.Position = new TimeSpan(0, 0, pos);
            AngleChoice3.Position = new TimeSpan(0, 0, pos);
            AngleChoice4.Position = new TimeSpan(0, 0, pos);
            AngleChoice5.Position = new TimeSpan(0, 0, pos);
        }

        public void ShowScrubbingPreview(int pos)
        {
            ScrubbingGrid.Visibility = Visibility.Visible;
            ScrubbingGrid.Margin = new Thickness((double)pos, 0, 0, 88);
            ScrubbingPreview.Play();
            ScrubbingPreview.Position = new TimeSpan(0, 0, pos);
            ScrubbingPreview.Pause();
        }

        public void HideScrubbingPreview()
        {
            ScrubbingGrid.Visibility = Visibility.Collapsed;
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

            MediaElement selected = null;
            char[] name = { };
            if (sender.GetType().Equals(AngleChoice0.GetType()))
            {
                selected = (MediaElement)sender;
                name = selected.Name.ToCharArray();
            }
            else if (sender.GetType().Equals(AngleIcon0.GetType()))
            {
                name = ((Image)sender).Name.ToCharArray();
                selected = (MediaElement)AngleSelectGrid.FindName("AngleChoice" + name.Last<char>().ToString());
            }

            //set main video
            CurrentVideo.Source = selected.Source;
            ScrubbingPreview.Source = selected.Source;
            rate = selected.PlaybackRate;
            PlayVid();
            position = selected.Position;

            //adjust border colors
            foreach (Border b in Borders.Children)
            {
                b.BorderBrush = new SolidColorBrush(Colors.Black);
            }     

            //char[] name = selected.Name.ToCharArray();
            int vidNum = Int32.Parse(name.Last<char>().ToString());

            Border border = (Border)(Borders.FindName("Border" + vidNum));
            border.BorderBrush = new SolidColorBrush(Colors.Blue);
     
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
                CurrentVideo.Position = AngleChoice0.Position;
                CurrentVideo.PlaybackRate = rate;
            }

            TimeSpan duration = CurrentVideo.NaturalDuration.TimeSpan;
            double minutes = (duration.TotalHours / 60) + duration.TotalMinutes;
            VideoControlGrid.SetDuration((minutes / 60) + duration.TotalSeconds);


            AngleChoice0.Play();
            AngleChoice1.Play();
            AngleChoice2.Play();
            AngleChoice3.Play();
            AngleChoice4.Play();
            AngleChoice5.Play();

            if (!chaptersSet)
            {
                chaptersSet = true;

                if (chapterTimes != null && chapterNames != null && chapterTimes.Length == chapterNames.Length)
                {
                    markers = new ChapterMarker[chapterTimes.Length];
                    for (int i = 0; i < chapterTimes.Length; i++)
                    {
                        markers[i] = new ChapterMarker();
                        markers[i].SetChapterAttributes(chapterTimes[i], chapterNames[i]);
                        markers[i].Height = 100;
                        markers[i].Width = 120;
                        //Debug.WriteLine(markers[i].GetStartTime());
                    }
                    VideoControlGrid.SetChapterPositions(markers);
                }
            }

        }



        private void CurrentVideo_MediaEnded(object sender, RoutedEventArgs e)
        {
            timer.Stop();
        }

        private void ScrubbingPreview_MediaOpened(object sender, RoutedEventArgs e)
        {
            ScrubbingPreview.Pause();
        }

        private void AngleChoice0_MediaOpened(object sender, RoutedEventArgs e)
        {
            //TimeSpan duration = AngleChoice0.NaturalDuration.TimeSpan;
            //double minutes = (duration.TotalHours / 60) + duration.TotalMinutes;
            //VideoControlGrid.SetDuration((minutes / 60) + duration.TotalSeconds);
        }
    }
}
