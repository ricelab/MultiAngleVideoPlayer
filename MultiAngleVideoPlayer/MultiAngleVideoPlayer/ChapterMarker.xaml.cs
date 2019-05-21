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
    public sealed partial class ChapterMarker : UserControl
    {
        //This chapter's start time.
        private double startTime;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ChapterMarker()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="time">Start time for this chapter.</param>
        /// <param name="name">Title of this chapter.</param>
        public void SetChapterAttributes(double time, string name)
        {
            startTime = time;
            ChapterTitle.Text = name;
        }

        /// <summary>
        /// Gets start time of this chapter in seconds.
        /// </summary>
        /// <returns>The start time of the chapter.</returns>
        public double GetStartTime()
        {
            return startTime;
        }
    }
}
