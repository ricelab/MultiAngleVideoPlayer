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
    /// <summary>
    /// Class representing CameraControl icon/button.
    /// </summary>
    public sealed partial class CameraControl : UserControl
    {
        //The video file associated with this camera angle.
        Uri video;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public CameraControl()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Sets the video file to be associated with this camera angle.
        /// </summary>
        /// <param name="path">The Uri path to the video file.</param>
        public void SetVideoUri(Uri path)
        {
            video = path;
        }

        /// <summary>
        /// Gets the video Uri associated with this camera angle.
        /// </summary>
        /// <returns></returns>
        public Uri GetVideoUri()
        {
            return video;
        }
    }
}
