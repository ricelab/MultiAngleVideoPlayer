using System;
using System.Threading;
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
using System.Diagnostics;
using Windows.UI.Input;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace MultiAngleVideoPlayer
{
    public sealed partial class VideoControls : UserControl
    {
        //UI-related flags
        double duration = 0;
        bool doubleSpeed = false;
        bool halfSpeed = false;
        bool scrubbing = false;
        bool chapterLooping = false;
        //bool customLooping = false;

        //the video viewer which this control is part of
        //this class is leftover from a previous version which also had the option to create an ExoView viewer
        //(should this be changed?)
        public EgoView viewer { get; set; }

        // --------------------------------------------------- CONSTRUCTORS ---------------------------------------------------

        /// <summary>
        /// Generic constructor
        /// </summary>
        public VideoControls()
        {
            this.InitializeComponent();
        }

        // -------------------------------------------------- PRIVATE METHODS -------------------------------------------------

        private void ToggleDoubleSpeed(bool toggleOn)
        {
            doubleSpeed = toggleOn;
            //string message = "";
            if (doubleSpeed)
            {
                TwoSpeedButton.Background = new SolidColorBrush(Windows.UI.Colors.Gray);
                //message = "2x Playback Speed";
            }  
            else
                TwoSpeedButton.Background = new SolidColorBrush(Windows.UI.Colors.LightGray);

            //viewer.UpdateSpeedLabel(message);
        }

        private void ToggleHalfSpeed(bool toggleOn)
        {
            halfSpeed = toggleOn;
            //string message = "";
            if (halfSpeed)
            {
                HalfSpeedButton.Background = new SolidColorBrush(Windows.UI.Colors.Gray);
                //message = "1/2 Playback Speed";
            }   
            else
                HalfSpeedButton.Background = new SolidColorBrush(Windows.UI.Colors.LightGray);

            //viewer.UpdateSpeedLabel(message);
        }

        private void BuildSpeedMessage()
        {
            string message = "";
            if (doubleSpeed)
            {
                message = "2x Playback Speed";
            } else if (halfSpeed)
            {
                message = "1/2 Playback Speed";
            }
            viewer.UpdateSpeedLabel(message);
        }

        private void ToggleLoopChapter(bool toggleOn)
        {
            chapterLooping = toggleOn;
            string message = "";
            if (chapterLooping)
            {
                LoopChapterButton.Background = new SolidColorBrush(Windows.UI.Colors.Gray);
                message = "Looping Current Chapter";
            }              
            else
                LoopChapterButton.Background = new SolidColorBrush(Windows.UI.Colors.LightGray);

            viewer.UpdateLoopLabel(message);
        }

        // -------------------------------------------------- PUBLIC METHODS --------------------------------------------------

        /// <summary>
        /// Called by the viewer to change the label on the PlayPauseButton to "Play" or "Pause"
        /// </summary>
        /// <param name="label">The label to appear on the button</param>
        public void ChangeButtonLabel(String label)
        {
            PlayPauseButton.Content = label;
        }

        /// <summary>
        /// Enables or disables the PlayPause button. At program start, the button is disabled and must be enabled before playing.
        /// </summary>
        /// <param name="enabled">True if enabled, else false.</param>
        public void EnableButtons(bool enabled)
        {
            PlayPauseButton.IsEnabled = enabled;
            TwoSpeedButton.IsEnabled = enabled;
            HalfSpeedButton.IsEnabled = enabled;
            TenBackButton.IsEnabled = enabled;
            TenForwardButton.IsEnabled = enabled;
            LoopChapterButton.IsEnabled = enabled;
        }

        /// <summary>
        /// Set the video duration so this class can calculate video positions visually, or based on user interaction.
        /// </summary>
        /// <param name="seconds">The duration of the video in seconds</param>
        public void SetDuration(double seconds)
        {
            duration = seconds;
        }

        /// <summary>
        /// Sets the position of the scrubbing preview based on a Point selected by the user.
        /// </summary>
        /// <param name="pos">The position selected by the user.</param>
        public void SetPreviewPosition(Point pos)
        {
            double newPosition = pos.X;
            double currentIncrements = duration / 1500 * newPosition*0.75;

            viewer.ShowScrubbingPreview((int)currentIncrements);
        }

        /// <summary>
        /// Places ChapterMarkers along the timeline.
        /// </summary>
        /// <param name="markers">An array of ChapterMarkers to be placed.</param>
        public void SetChapterPositions(ChapterMarker[] markers)
        {
            foreach (ChapterMarker m in markers)
            {
                double time = m.GetStartTime();
                double totalIncrements = 1000 / duration;
                double increments = totalIncrements * time;

                Debug.WriteLine("Position: " + increments);

                Grid.SetColumn(m, 1);
                Grid.SetRow(m, 0);
                m.Margin = new Thickness(increments - 50, -25, 0, 25);
                m.HorizontalAlignment = HorizontalAlignment.Left;
                m.Tapped += ChapterMarker_Click;
                VideoControlGrid.Children.Add(m);
            }
        }

        /// <summary>
        /// Based on a point selected by the user, determine the new video playback position.
        /// </summary>
        /// <param name="pos"></param>
        public void CalculateVideoPosition(Point pos)
        {
            double newPosition = pos.X;
            double currentIncrements = duration / 1000 * newPosition;

            viewer.NewVideoPosition((int)currentIncrements);
        }

        // ------------------------------------------------ UI EVENT HANDLERS -------------------------------------------------

        /// <summary>
        /// Invoked when the user presses the PlayPauseButton. A message is sent to the viewer to handle the play/pause functionality.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            viewer.PlayPause();
            if (PlayPauseButton.Content.Equals("Play"))
            {
                TwoSpeedButton.IsEnabled = false;
                HalfSpeedButton.IsEnabled = false;
                TenBackButton.IsEnabled = false;
                TenForwardButton.IsEnabled = false;
                LoopChapterButton.IsEnabled = false;
            } else
            {
                TwoSpeedButton.IsEnabled = true;
                HalfSpeedButton.IsEnabled = true;
                TenBackButton.IsEnabled = true;
                TenForwardButton.IsEnabled = true;
                LoopChapterButton.IsEnabled = true;
            }
        }

        /// <summary>
        /// Doubles the video playback rate or, if already doubled, returns it to the default rate.
        /// Invoked by user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TwoSpeedButton_Click(object sender, RoutedEventArgs e)
        {
            if (!doubleSpeed)
            {
                viewer.UpdateVideoSpeed(2.0);
                //TwoSpeedButton.Content = "Normal Speed";
                //HalfSpeedButton.Content = "1/2 Speed";
                ToggleDoubleSpeed(true);
                ToggleHalfSpeed(false);
            } else
            {
                viewer.UpdateVideoSpeed(1.0);
                //TwoSpeedButton.Content = "2x Speed";
                ToggleDoubleSpeed(false);
            }
            BuildSpeedMessage();
        }

        /// <summary>
        /// Decreases the playback speed to half or, if already decreased, returns it to the default rate.
        /// Invoked by user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HalfSpeedButton_Click(object sender, RoutedEventArgs e)
        {
            if (!halfSpeed)
            {
                viewer.UpdateVideoSpeed(0.5);
                //HalfSpeedButton.Content = "Normal Speed";
                //TwoSpeedButton.Content = "2x Speed";
                ToggleHalfSpeed(true);
                ToggleDoubleSpeed(false);
            } else
            {
                viewer.UpdateVideoSpeed(1.0);
                //HalfSpeedButton.Content = "1/2 Speed";
                ToggleHalfSpeed(false);
            }
            BuildSpeedMessage();
        }

        /// <summary>
        /// Jump back 10 seconds in all videos.
        /// Invoked by user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TenBackButton_Click(object sender, RoutedEventArgs e)
        {
            viewer.UpdatePosition(-10);
        }

        /// <summary>
        /// Jump forward 10 seconds in all videos.
        /// Invoked by user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TenForwardButton_Click(object sender, RoutedEventArgs e)
        {
            viewer.UpdatePosition(10);
        }

        /// <summary>
        /// Loops the current chapter.
        /// Invoked by user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoopChapterButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!chapterLooping)
            {
                ToggleLoopChapter(true);
                int chapterIndex = viewer.GetCurrentChapterIndex();
                double startLoop = viewer.GetChapter(chapterIndex).GetStartTime();
                ChapterMarker endMarker = viewer.GetChapter(chapterIndex + 1);
                double endLoop = duration - 2;
                if (endMarker != null)
                {
                    endLoop = endMarker.GetStartTime();
                }
                viewer.UpdateLoopBounds(startLoop, endLoop);

            } else
            {
                ToggleLoopChapter(false);
                viewer.CancelLoop();
            }
        }

        /// <summary>
        /// Jump to new video position.
        /// Invoked by user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScrubbingBar_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (duration > 0)
            {
                CalculateVideoPosition(e.GetPosition(ScrubbingBar));
            }
        }

        /// <summary>
        /// Initiate scrubbing features.
        /// Invoked by user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScrubbingBar_Holding(object sender, HoldingRoutedEventArgs e)
        {
            if (!scrubbing)
            {
                scrubbing = true;
            }
            if (duration > 0)
            {
                SetPreviewPosition(e.GetPosition(ScrubbingBar));
            }
        }

        /// <summary>
        /// Cancel scrubbing features.
        /// Invoked by user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScrubbingBar_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (scrubbing)
            {
                scrubbing = false;
                viewer.HideScrubbingPreview();
                CalculateVideoPosition(e.GetCurrentPoint(ScrubbingBar).Position);
            }
        }

        /// <summary>
        /// Perform scrubbing features, e.g. show preview.
        /// Invoked by user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScrubbingBar_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (scrubbing)
            {
                SetPreviewPosition(e.GetCurrentPoint(ScrubbingBar).Position);
            }
        }

        /// <summary>
        /// Jump to chapter start time.
        /// Invoked by user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChapterMarker_Click(object sender, RoutedEventArgs e)
        {
            ChapterMarker marker = (ChapterMarker)sender;
            double jumpToTime = marker.GetStartTime();
            viewer.NewVideoPosition((int)jumpToTime);
        }

        // ---------------------------------------------- NON-UI EVENT HANDLERS ----------------------------------------------

        //Changes the length of the progress bar based on current video position.
        public void UpdateProgressBar(double currentPosition)
        {
            if (duration > 0)
            {
                double totalIncrements = 1000 / duration;
                double currentIncrements = totalIncrements * currentPosition ;

                VideoProgressBar.Width = currentIncrements;
            }
        }
    }
}
