﻿using System;
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
        public bool singleView = false;
        //bool customLooping = false;

        //the video viewer which this control is part of
        //this class is leftover from a previous version which also had the option to create an ExoView viewer
        //(should this be changed?)

        public EgoView viewer { get; set; }
        public SingleView singleViewer { get; set; }


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


            if (!singleView)
            {
                viewer.UpdateSpeedLabel(message);
                if (message.Equals(""))
                {
                    message = "Reset to normal speed.";
                }
                Logger.Log(message + "," + viewer.GetPosition());
            } else
            {
                singleViewer.UpdateSpeedLabel(message);
                if (message.Equals(""))
                {
                    message = "Reset to normal speed.";
                }
                Logger.Log(message + "," + singleViewer.GetPosition());
            }

        }

        private void ToggleLoopChapter(bool toggleOn)
        {
            chapterLooping = toggleOn;
            string message = "";
            if (chapterLooping)
            {
                LoopChapterButton.Background = new SolidColorBrush(Windows.UI.Colors.Gray);
                message = "Looping Current Chapter";
                
                if (!singleView)
                {
                    Logger.Log(message + "," + viewer.GetPosition());
                } else
                {
                    Logger.Log(message + "," + singleViewer.GetPosition());
                }

                
            }              
            else
            {
                LoopChapterButton.Background = new SolidColorBrush(Windows.UI.Colors.LightGray);

                if (!singleView)
                {
                    Logger.Log("Loop cancelled," + viewer.GetPosition());
                } else
                {
                    Logger.Log("Loop cancelled," + singleViewer.GetPosition());
                }
            }
                
            if (!singleView)
            {
                viewer.UpdateLoopLabel(message);
            } else
            {
                singleViewer.UpdateLoopLabel(message);
            }
            
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
            //double currentIncrements = duration / this.ActualWidth * newPosition * 0.75;
            //double currentIncrements = this.ActualWidth * newPosition * 0.75 / duration;

            //double totalIncrements = this.ActualWidth / duration;
            //double currentIncrements = totalIncrements * newPosition * 0.75;

            if (!singleView)
            {
                viewer.ShowScrubbingPreview((int)newPosition, (int)(duration / ScrubbingBar.ActualWidth * newPosition));
            } else
            {
                singleViewer.ShowScrubbingPreview((int)newPosition, (int)(duration / ScrubbingBar.ActualWidth * newPosition));
            }
            
        }

        /// <summary>
        /// Places ChapterMarkers along the timeline.
        /// </summary>
        /// <param name="markers">An array of ChapterMarkers to be placed.</param>
        public void SetChapterPositions(ChapterMarker[] markers)
        {
            Debug.WriteLine("Duration: " + duration);
            int counter = 1;
            foreach (ChapterMarker m in markers)
            {
                double time = m.GetStartTime();
                //double totalIncrements = 1000 / duration;
                double totalIncrements = ScrubbingBar.ActualWidth / duration;
                double increments = totalIncrements * time;

                Debug.WriteLine("Position: " + increments);

                Grid.SetColumn(m, 1);
                Grid.SetRow(m, 0);
                //m.Margin = new Thickness(increments - 50, -100, 0, 25);
                m.HorizontalAlignment = HorizontalAlignment.Left;
                m.Tapped += ChapterMarker_Click;

                if(counter % 2 == 0)
                {
                    m.Margin = new Thickness(increments - 50, -75, 0, 25);
                } else
                {
                    m.Margin = new Thickness(increments - 50, -40, 0, 25);
                }

                VideoControlGrid.Children.Add(m);
                counter++;
            }
        }

        /// <summary>
        /// Based on a point selected by the user, determine the new video playback position.
        /// </summary>
        /// <param name="pos"></param>
        public int CalculateVideoPosition(Point pos)
        {
            double newPosition = pos.X;
            //double currentIncrements = duration / 1000 * newPosition;
            double currentIncrements = duration / ScrubbingBar.ActualWidth * newPosition;

            if (!singleView)
            {
                viewer.NewVideoPosition((int)currentIncrements);
            } else
            {
                singleViewer.NewVideoPosition((int)currentIncrements);
            }
            

            return (int)currentIncrements;
        }

        public void ResetFlags()
        {
            duration = 0;
            doubleSpeed = false;
            halfSpeed = false;
            scrubbing = false;
            chapterLooping = false;

            PlayPauseButton.Content = "Play";

            TwoSpeedButton.Background = new SolidColorBrush(Windows.UI.Colors.LightGray);
            HalfSpeedButton.Background = new SolidColorBrush(Windows.UI.Colors.LightGray);
            LoopChapterButton.Background = new SolidColorBrush(Windows.UI.Colors.LightGray);
        }

        // ------------------------------------------------ UI EVENT HANDLERS -------------------------------------------------

        /// <summary>
        /// Invoked when the user presses the PlayPauseButton. A message is sent to the viewer to handle the play/pause functionality.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (!singleView)
            {
                viewer.PlayPause();
            } else
            {
                singleViewer.PlayPause();
            }
            
            /*
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
            */
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
                if (!singleView)
                {
                    viewer.UpdateVideoSpeed(2.0);
                } else
                {
                    singleViewer.UpdateVideoSpeed(2.0);
                }
                
                //TwoSpeedButton.Content = "Normal Speed";
                //HalfSpeedButton.Content = "1/2 Speed";
                ToggleDoubleSpeed(true);
                ToggleHalfSpeed(false);
            } else
            {
                if (!singleView)
                {
                    viewer.UpdateVideoSpeed(1.0);
                } else
                {
                    singleViewer.UpdateVideoSpeed(1.0);
                }
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
                if (!singleView)
                {
                    viewer.UpdateVideoSpeed(0.5);
                } else
                {
                    singleViewer.UpdateVideoSpeed(0.5);
                }
                
                //HalfSpeedButton.Content = "Normal Speed";
                //TwoSpeedButton.Content = "2x Speed";
                ToggleHalfSpeed(true);
                ToggleDoubleSpeed(false);
            } else
            {
                if (!singleView)
                {
                    viewer.UpdateVideoSpeed(1.0);
                } else
                {
                    singleViewer.UpdateVideoSpeed(1.0);
                }
                
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
            if (!singleView)
            {
                viewer.UpdatePosition(-10);
                Logger.Log("Back 10 seconds," + viewer.GetPosition());
            } else
            {
                singleViewer.UpdatePosition(-10);
                Logger.Log("Back 10 seconds," + singleViewer.GetPosition());
            }
            
            
        }

        /// <summary>
        /// Jump forward 10 seconds in all videos.
        /// Invoked by user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TenForwardButton_Click(object sender, RoutedEventArgs e)
        {
            if (!singleView)
            {
                viewer.UpdatePosition(10);
                Logger.Log("Forward 10 seconds," + viewer.GetPosition());
            } else
            {
                singleViewer.UpdatePosition(10);
                Logger.Log("Forward 10 seconds," + singleViewer.GetPosition());
            }
            
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
                ChapterMarker endMarker;
                double startLoop;
                ToggleLoopChapter(true);
                if (!singleView)
                {
                    int chapterIndex = viewer.GetCurrentChapterIndex();
                    startLoop = viewer.GetChapter(chapterIndex).GetStartTime();
                    endMarker = viewer.GetChapter(chapterIndex + 1);
                } else
                {
                    int chapterIndex = singleViewer.GetCurrentChapterIndex();
                    startLoop = singleViewer.GetChapter(chapterIndex).GetStartTime();
                    endMarker = singleViewer.GetChapter(chapterIndex + 1);
                }


                double endLoop = duration - 2;
                if (endMarker != null)
                {
                    endLoop = endMarker.GetStartTime();
                }
                if (!singleView)
                {
                    viewer.UpdateLoopBounds(startLoop, endLoop);
                } else
                {
                    singleViewer.UpdateLoopBounds(startLoop, endLoop);
                }
                

            } else
            {
                ToggleLoopChapter(false);
                if (!singleView)
                {
                    viewer.CancelLoop();
                } else
                {
                    singleViewer.CancelLoop();
                }

                
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
                int pos = CalculateVideoPosition(e.GetPosition(ScrubbingBar));
                Logger.Log("Jumped to point: " + new TimeSpan(0,0,pos));
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
                Logger.Log("Started scrubbing," + viewer.GetPosition());
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
                CalculateVideoPosition(e.GetCurrentPoint(ScrubbingBar).Position);
                if (!singleView)
                {
                    viewer.HideScrubbingPreview();
                    Logger.Log("Stopped scrubbing," + viewer.GetPosition());
                } else
                {
                    singleViewer.HideScrubbingPreview();
                    Logger.Log("Stopped scrubbing," + singleViewer.GetPosition());
                }
                
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
            if (!singleView)
            {
                viewer.NewVideoPosition((int)jumpToTime);
                Logger.Log("Jump to chapter: " + marker.GetText() + "," + viewer.GetPosition());
            } else
            {
                singleViewer.NewVideoPosition((int)jumpToTime);
                Logger.Log("Jump to chapter: " + marker.GetText() + "," + singleViewer.GetPosition());
            }
            
        }

        // ---------------------------------------------- NON-UI EVENT HANDLERS ----------------------------------------------

        //Changes the length of the progress bar based on current video position.
        public void UpdateProgressBar(double currentPosition)
        {
            if (currentPosition == -1)
            {
                VideoProgressBar.Width = 0;
            }
            else if (duration > 0)
            {
                //double totalIncrements = 1000 / duration;
                double totalIncrements = ScrubbingBar.ActualWidth / duration;
                double currentIncrements = totalIncrements * currentPosition;

                VideoProgressBar.Width = currentIncrements;
            }
        }
    }
}
