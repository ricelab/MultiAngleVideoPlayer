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
        double duration = 0;
        bool doubleSpeed = false;
        bool halfSpeed = false;
        bool scrubbing = false;

        public EgoView viewer { get; set; }

        // --------------------------------------------------- CONSTRUCTORS ---------------------------------------------------

        /// <summary>
        /// Generic constructor
        /// </summary>
        public VideoControls()
        {
            this.InitializeComponent();
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
        }

        public void SetDuration(double seconds)
        {
            duration = seconds;
            //Debug.WriteLine("Duration set to " + duration + " seconds in VideoControlGrid.");
        }

        public void SetPreviewPosition(Point pos)
        {
            double newPosition = pos.X;
            double currentIncrements = duration / 1500 * newPosition*0.75;

            viewer.ShowScrubbingPreview((int)currentIncrements);
        }

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
                m.Margin = new Thickness(increments - 60, 0, 0, 0);
                m.HorizontalAlignment = HorizontalAlignment.Left;
                m.Tapped += ChapterMarker_Click;
                VideoControlGrid.Children.Add(m);
            }
        }

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
            } else
            {
                TwoSpeedButton.IsEnabled = true;
                HalfSpeedButton.IsEnabled = true;
                TenBackButton.IsEnabled = true;
                TenForwardButton.IsEnabled = true;
            }
        }

        private void TwoSpeedButton_Click(object sender, RoutedEventArgs e)
        {
            if (!doubleSpeed)
            {
                viewer.UpdateVideoSpeed(2.0);
                TwoSpeedButton.Content = "Normal Speed";
                HalfSpeedButton.Content = "1/2 Speed";
                doubleSpeed = true;
                halfSpeed = false;
            } else
            {
                viewer.UpdateVideoSpeed(1.0);
                TwoSpeedButton.Content = "2x Speed";
                doubleSpeed = false;
            }
            
        }

        private void HalfSpeedButton_Click(object sender, RoutedEventArgs e)
        {
            if (!halfSpeed)
            {
                viewer.UpdateVideoSpeed(0.5);
                HalfSpeedButton.Content = "Normal Speed";
                TwoSpeedButton.Content = "2x Speed";
                halfSpeed = true;
                doubleSpeed = false;
            } else
            {
                viewer.UpdateVideoSpeed(1.0);
                HalfSpeedButton.Content = "1/2 Speed";
                halfSpeed = false;
            }
            
        }

        private void TenBackButton_Click(object sender, RoutedEventArgs e)
        {
            viewer.UpdatePosition(-10);
        }

        private void TenForwardButton_Click(object sender, RoutedEventArgs e)
        {
            viewer.UpdatePosition(10);
        }

        private void ScrubbingBar_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (duration > 0)
            {
                CalculateVideoPosition(e.GetPosition(ScrubbingBar));
            }
        }

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

        private void ScrubbingBar_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (scrubbing)
            {
                scrubbing = false;
                viewer.HideScrubbingPreview();
                CalculateVideoPosition(e.GetCurrentPoint(ScrubbingBar).Position);
            }
        }

        private void ScrubbingBar_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (scrubbing)
            {
                SetPreviewPosition(e.GetCurrentPoint(ScrubbingBar).Position);
            }
        }

        private void ChapterMarker_Click(object sender, RoutedEventArgs e)
        {
            ChapterMarker marker = (ChapterMarker)sender;
            double jumpToTime = marker.GetStartTime();
            viewer.NewVideoPosition((int)jumpToTime);
        }

        // ---------------------------------------------- NON-UI EVENT HANDLERS ----------------------------------------------

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
