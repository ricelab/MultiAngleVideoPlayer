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
using System.Threading.Tasks;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace MultiAngleVideoPlayer
{
    public sealed partial class EgoView : UserControl, IViewable
    {
        //need video duration to set up chapters, but duration not available until video is loaded
        //use flag to set chapters just once when video is first loaded
        bool chaptersSet = false;

        //need to know when video is playing or not
        //but if not playing, also need to know if video is paused or has not yet started
        bool playing = false;
        bool paused = false;

        //other self-explanatory variables
        TimeSpan position;
        double rate = 1;
        MainPage mainPage;
        DispatcherTimer timer;
        double[] chapterTimes = null;
        string[] chapterNames = null;
        ChapterMarker[] markers = null;
        int currentChapterIndex = 0;
        Tuple<double,double> loopBounds = null;

        MediaElement currentVid = new MediaElement();

        // --------------------------------------------------- CONSTRUCTORS ---------------------------------------------------

        /// <summary>
        /// Generic constructor. General setup.
        /// </summary>
        public EgoView()
        {
            this.InitializeComponent();
            VideoControlGrid.viewer = this;
            TimerSetup();
        }

        // -------------------------------------------------- PRIVATE METHODS -------------------------------------------------

        /// <summary>
        /// Creates a new DispatcherTimer whose tick interval is 1 second.
        /// </summary>
        private void TimerSetup()
        {
            timer = new DispatcherTimer();
            timer.Tick += TimerTick;
            timer.Interval = new TimeSpan(0, 0, 0, 1);
        }

        /// <summary>
        /// Gets chapter data from main page and obtains all chapter names and start times.
        /// </summary>
        private void SetupChapters()
        {
            ChapAttributes[] chapters = mainPage.GetVideoChapters().Attributes;
            if (chapters != null)
            {
                List<double> tempChapterTimes = new List<double>();
                List<string> tempChapterNames = new List<string>();
                foreach (ChapAttributes c in chapters)
                {
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

        /// <summary>
        /// Creates a ChapterMarker object for each chapter and passes them to the VideoControlGrid for placement.
        /// </summary>
        private void CreateChapterMarkers()
        {
            if (chapterTimes != null && chapterNames != null && chapterTimes.Length == chapterNames.Length)
            {
                markers = new ChapterMarker[chapterTimes.Length];
                for (int i = 0; i < chapterTimes.Length; i++)
                {
                    markers[i] = new ChapterMarker();
                    markers[i].SetChapterAttributes(chapterTimes[i], chapterNames[i]);
                    markers[i].Height = 70;
                    markers[i].Width = 100;
                    //Debug.WriteLine(markers[i].GetStartTime());
                }
                VideoControlGrid.SetChapterPositions(markers);
            }
        }

        /// <summary>
        /// Clears previous Border highlights and turns the selected Border blue.
        /// </summary>
        /// <param name="selectionName">The name of the Border element to turn blue.</param>
        private void UpdateAngleBorders(char[] selectionName)
        {
            foreach (Border b in Borders.Children)
            {
                b.BorderBrush = new SolidColorBrush(Colors.Black);
            }

            if (selectionName != null)
            {
                int vidNum = Int32.Parse(selectionName.Last<char>().ToString());
                Border border = (Border)(Borders.FindName("Border" + vidNum));
                border.BorderBrush = new SolidColorBrush(Colors.Blue);
            }
        }

        /// <summary>
        /// Updates the current chapter if it needs updating.
        /// </summary>
        private void CheckUpdateChapter(double seconds)
        {
            bool updateChapter = true;
            if (currentChapterIndex + 1 < markers.Length)
            {
                if (markers[currentChapterIndex].GetStartTime() < seconds && seconds < markers[currentChapterIndex + 1].GetStartTime())
                {
                    updateChapter = false;
                }
            } else
            {
                if (markers[currentChapterIndex].GetStartTime() < seconds)
                {
                    updateChapter = false;
                }
            }
            if (updateChapter)
            {
                int i = 0;
                bool found = false;
                while (!found && i < markers.Length - 1)
                {
                    if (markers[i].GetStartTime() < seconds && markers[i+1].GetStartTime() > seconds)
                    {
                        currentChapterIndex = i;
                        found = true;
                    }
                    i++;
                }
                if (!found)
                {
                    currentChapterIndex = markers.Length - 1;
                }
            }
        }

        private void CheckRestartLoop(double seconds)
        {
            if (seconds > loopBounds.Item2)
            {
                NewVideoPosition((int)loopBounds.Item1);
                Logger.Log("Restarting chapter.");
            }
        }

        // -------------------------------------------------- PUBLIC METHODS --------------------------------------------------

        /// <summary>
        /// Enables and initiates features for video playback.
        /// </summary>
        public void SetupVideos()
        {
            //enable tap
            AngleChoice0.IsTapEnabled = true;
            AngleChoice1.IsTapEnabled = true;
            AngleChoice2.IsTapEnabled = true;
            AngleChoice3.IsTapEnabled = true;
            AngleChoice4.IsTapEnabled = true;
            AngleChoice5.IsTapEnabled = true;

            //get video paths
            Uri[] paths = mainPage.GetVideos();

            //set source URIs
            AngleChoice0.Source = paths[0];
            AngleChoice1.Source = paths[1];
            AngleChoice2.Source = paths[2];
            AngleChoice3.Source = paths[3];
            AngleChoice4.Source = paths[4];
            AngleChoice5.Source = paths[5];
            CurrentVideo0.Source = paths[0];
            CurrentVideo1.Source = paths[1];
            CurrentVideo2.Source = paths[2];
            CurrentVideo3.Source = paths[3];
            CurrentVideo4.Source = paths[4];
            CurrentVideo5.Source = paths[5];

            //dim the angle choices
            AngleChoice0.Opacity = 0.3;
            AngleChoice1.Opacity = 0.3;
            AngleChoice2.Opacity = 0.3;
            AngleChoice3.Opacity = 0.3;
            AngleChoice4.Opacity = 0.3;
            AngleChoice5.Opacity = 0.3;

            //read chapter data
            SetupChapters();
        }

        /// <summary>
        /// Starts all videos playing.
        /// </summary>
        public void PlayVid()
        {
            
            
            //Logger.Log("Pressed play.");
            VideoControlGrid.ChangeButtonLabel("Pause");
            playing = true;

            currentVid.Play();
            if (!paused)
            {
                AngleChoice0.Play();
                AngleChoice1.Play();
                AngleChoice2.Play();
                AngleChoice3.Play();
                AngleChoice4.Play();
                AngleChoice5.Play();
            }          
            
            currentVid.PlaybackRate = rate;
            AngleChoice0.PlaybackRate = rate;
            AngleChoice1.PlaybackRate = rate;
            AngleChoice2.PlaybackRate = rate;
            AngleChoice3.PlaybackRate = rate;
            AngleChoice4.PlaybackRate = rate;
            AngleChoice5.PlaybackRate = rate;

            timer.Start();
            //CurrentVideo.PlaybackRate = rate;
        }

        /// <summary>
        /// Pause all videos.
        /// </summary>
        public void PauseVid()
        {
            //Logger.Log("Pressed pause.");
            VideoControlGrid.ChangeButtonLabel("Play");
            playing = false;
            rate = currentVid.PlaybackRate;

            AngleChoice0.Pause();
            AngleChoice1.Pause();
            AngleChoice2.Pause();
            AngleChoice3.Pause();
            AngleChoice4.Pause();
            AngleChoice5.Pause();
            currentVid.Pause();
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
                Logger.Log("Pressed play.");
            }
            else
            {
                PauseVid();
                Logger.Log("Pressed pause.");
            }
        }

        /// <summary>
        /// Changes the video playback rate for all videos.
        /// </summary>
        /// <param name="speed">The new playback rate.</param>
        public void UpdateVideoSpeed(double speed)
        {
            CurrentVideo0.PlaybackRate = speed;
            CurrentVideo1.PlaybackRate = speed;
            CurrentVideo2.PlaybackRate = speed;
            CurrentVideo3.PlaybackRate = speed;
            CurrentVideo4.PlaybackRate = speed;
            CurrentVideo5.PlaybackRate = speed;
            AngleChoice0.PlaybackRate = speed;
            AngleChoice1.PlaybackRate = speed;
            AngleChoice2.PlaybackRate = speed;
            AngleChoice3.PlaybackRate = speed;
            AngleChoice4.PlaybackRate = speed;
            AngleChoice5.PlaybackRate = speed;
        }

        /// <summary>
        /// Updates the position of all videos uniformly by adding/subtracting time from the current position.
        /// </summary>
        /// <param name="change">The number of seconds to jump forward or backward.</param>
        public void UpdatePosition(int change)
        {
            CurrentVideo0.Position += new TimeSpan(0,0,change);
            CurrentVideo1.Position += new TimeSpan(0, 0, change);
            CurrentVideo2.Position += new TimeSpan(0, 0, change);
            CurrentVideo3.Position += new TimeSpan(0, 0, change);
            CurrentVideo4.Position += new TimeSpan(0, 0, change);
            CurrentVideo5.Position += new TimeSpan(0, 0, change);
            AngleChoice0.Position += new TimeSpan(0, 0, change);
            AngleChoice1.Position += new TimeSpan(0, 0, change);
            AngleChoice2.Position += new TimeSpan(0, 0, change);
            AngleChoice3.Position += new TimeSpan(0, 0, change);
            AngleChoice4.Position += new TimeSpan(0, 0, change);
            AngleChoice5.Position += new TimeSpan(0, 0, change);
        }

        /// <summary>
        /// Changes the playback position for all videos uniformly. This change is independent from the current playback position.
        /// </summary>
        /// <param name="pos">The point in the video to jump to, in seconds.</param>
        public void NewVideoPosition(int pos)
        {
            CurrentVideo0.Position = new TimeSpan(0, 0, pos);
            CurrentVideo1.Position = new TimeSpan(0, 0, pos);
            CurrentVideo2.Position = new TimeSpan(0, 0, pos);
            CurrentVideo3.Position = new TimeSpan(0, 0, pos);
            CurrentVideo4.Position = new TimeSpan(0, 0, pos);
            CurrentVideo5.Position = new TimeSpan(0, 0, pos);
            AngleChoice0.Position = new TimeSpan(0, 0, pos);
            AngleChoice1.Position = new TimeSpan(0, 0, pos);
            AngleChoice2.Position = new TimeSpan(0, 0, pos);
            AngleChoice3.Position = new TimeSpan(0, 0, pos);
            AngleChoice4.Position = new TimeSpan(0, 0, pos);
            AngleChoice5.Position = new TimeSpan(0, 0, pos);
        }

        /// <summary>
        /// Creates a preview for the video content at the point on the timeline the user is currently holding down.
        /// </summary>
        /// <param name="pos">The x value of the point pressed by the user.</param>
        public void ShowScrubbingPreview(int pos)
        {
            ScrubbingGrid.Visibility = Visibility.Visible;
            ScrubbingGrid.Margin = new Thickness((double)pos, 0, 0, 88);
            ScrubbingPreview.Play();
            ScrubbingPreview.Position = new TimeSpan(0, 0, pos);
            ScrubbingPreview.Pause();
        }

        /// <summary>
        /// Collapse scrubbing preview when no longer needed.
        /// </summary>
        public void HideScrubbingPreview()
        {
            ScrubbingGrid.Visibility = Visibility.Collapsed;
        }

        public void UpdateLoopBounds(double start, double end)
        {
            loopBounds = new Tuple<double, double>(start, end);
            Logger.Log("Starting loop chapter: " + chapterNames[currentChapterIndex]);
        }

        public void CancelLoop()
        {
            loopBounds = null;
        }

        public int GetCurrentChapterIndex()
        {
            return currentChapterIndex;
        }

        public ChapterMarker GetChapter(int index)
        {
            if (index < markers.Length)
                return markers[index];
            else
                return null;
        }

        public void UpdateLoopLabel(string message)
        {
            LoopStatusLabel.Text = message;
        }

        public void UpdateSpeedLabel(string message)
        {
            SpeedStatusLabel.Text = message;
            Debug.WriteLine(message);
        }

        public void HideBackButton()
        {
            BackButton.Visibility = Visibility.Collapsed;
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
            //CurrentVideo.Pause();

            NoVidMessage.Visibility = Visibility.Collapsed;
            VideoControlGrid.EnableButtons(true);

            //Determines which angle was selected.
            MediaElement selected = null;
            char[] name = { };
            if (sender.GetType().Equals(AngleChoice0.GetType())) //if they clicked the video preview
            {
                selected = (MediaElement)sender;
                name = selected.Name.ToCharArray();
            }
            else if (sender.GetType().Equals(AngleIcon0.GetType())) //if they clicked the camera icon
            {
                name = ((Image)sender).Name.ToCharArray();
                selected = (MediaElement)AngleSelectGrid.FindName("AngleChoice" + name.Last<char>().ToString());
            }

            //set main video
            //CurrentVideo.Source = selected.Source;
            currentVid.Visibility = Visibility.Collapsed;
            currentVid.Stop();

            currentVid = (MediaElement)AngleSelectGrid.FindName("CurrentVideo" + name.Last<char>().ToString());
            //currentVid.Play();
            currentVid.Position = selected.Position;
            

            ScrubbingPreview.Source = selected.Source;
            rate = selected.PlaybackRate;
            PlayVid();
            //position = selected.Position;
            currentVid.Visibility = Visibility.Visible;

            //adjust border colours
            UpdateAngleBorders(name);

            Logger.Log(new string(name) + " selected.");
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
            /*
            if (position != null)
            {
                currentVid.Position = AngleChoice0.Position;
                currentVid.PlaybackRate = rate;
            }

            
            TimeSpan duration = CurrentVideo.NaturalDuration.TimeSpan;
            double minutes = (duration.TotalHours / 60) + duration.TotalMinutes;
            VideoControlGrid.SetDuration((minutes / 60) + duration.TotalSeconds);

            //Play all dimmed angle preview videos
            AngleChoice0.Play();
            AngleChoice1.Play();
            AngleChoice2.Play();
            AngleChoice3.Play();
            AngleChoice4.Play();
            AngleChoice5.Play();
            */

            

            //if this is the first play, set up the chapter markers
            if (!chaptersSet)
            {
                TimeSpan duration = ((MediaElement)sender).NaturalDuration.TimeSpan;
                double minutes = (duration.TotalHours / 60) + duration.TotalMinutes;
                VideoControlGrid.SetDuration((minutes / 60) + duration.TotalSeconds);
                chaptersSet = true;
                CreateChapterMarkers();
            }
            
        }

        /// <summary>
        /// DispatcherTimer tick event. If the video is playing, this increases the length of the video progress bar.
        /// Also checks which chapter video is in.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerTick(object sender, object e)
        {
            if (playing)
            {
                TimeSpan currentPosition = currentVid.Position;
                double minutes = (currentPosition.TotalHours / 60) + currentPosition.TotalMinutes;
                double seconds = (minutes / 60) + currentPosition.TotalSeconds;
                VideoControlGrid.UpdateProgressBar(seconds);

                if (markers != null)
                    CheckUpdateChapter(seconds);
                if (loopBounds != null)
                    CheckRestartLoop(seconds);
            }
        }

        /// <summary>
        /// Stops the timer when the video ends.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CurrentVideo_MediaEnded(object sender, RoutedEventArgs e)
        {
            playing = false;
            timer.Stop();
            Logger.Log("End of video.");
        }

        /// <summary>
        /// Pauses the preview when the scrubbing media opens to show static image of that point in the video.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScrubbingPreview_MediaOpened(object sender, RoutedEventArgs e)
        {
            ScrubbingPreview.Pause();
        }


        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            mainPage.ShowMenu();
            foreach(ChapterMarker m in markers)
            {
                m.Visibility = Visibility.Collapsed;
            }
            markers = null;
            CurrentVideo0.Stop();
            CurrentVideo0.Source = null;
            CurrentVideo1.Stop();
            CurrentVideo1.Source = null;
            CurrentVideo2.Stop();
            CurrentVideo2.Source = null;
            CurrentVideo3.Stop();
            CurrentVideo3.Source = null;
            CurrentVideo4.Stop();
            CurrentVideo4.Source = null;
            CurrentVideo5.Stop();
            CurrentVideo5.Source = null;
            VideoControlGrid.UpdateProgressBar(-1);
            SpeedStatusLabel.Text = "";
            LoopStatusLabel.Text = "";
            timer.Stop();

            chaptersSet = false;
            playing = false;
            paused = false;

            rate = 1;
            chapterTimes = null;
            chapterNames = null;
            currentChapterIndex = 0;
            loopBounds = null;

            NoVidMessage.Visibility = Visibility.Visible;
            VideoControlGrid.ResetFlags();
            UpdateAngleBorders(null);
        }
    }
}
