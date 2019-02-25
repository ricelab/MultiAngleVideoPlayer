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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace MultiAngleVideoPlayer
{
    public sealed partial class VideoControls : UserControl
    {
        public UserControl viewer { get; set; }

        public VideoControls()
        {
            this.InitializeComponent();
        }

        private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (viewer.GetType() == typeof(EgoView))
            {
                EgoView tempViewer = (EgoView)viewer;
                tempViewer.PlayPause();
            } else
            {
                ExoView tempViewer = (ExoView)viewer;
                tempViewer.PlayPause();
            }
        }

        public void ChangeButtonLabel(String label)
        {
            PlayPauseButton.Content = label;
        }

        public void EnableButton(bool enabled)
        {
            PlayPauseButton.IsEnabled = enabled;
        }
    }
}
