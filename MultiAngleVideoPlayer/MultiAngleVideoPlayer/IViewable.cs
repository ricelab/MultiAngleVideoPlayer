using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace MultiAngleVideoPlayer
{
    /// <summary>
    /// This interface defines the functionality that needs to be provided by *View classes.
    /// </summary>
    public interface IViewable
    {
        /// <summary>
        /// Enables tap and sets the source URI of all video choices.
        /// </summary>
        void SetupVideos();

        /// <summary>
        /// Play the video(s).
        /// </summary>
        void PlayVid();

        /// <summary>
        /// Pause the video(s).
        /// </summary>
        void PauseVid();

        /// <summary>
        /// Defines what should happen the View becomes visible.
        /// </summary>
        void GridVisible();

        /// <summary>
        /// Chooses whether to play or pause the video based on the current video status
        /// </summary>
        void PlayPause();

        /// <summary>
        /// Defines what should happen when the selected video is opened, e.g. set the position.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CurrentVideo_MediaOpened(object sender, RoutedEventArgs e);

        /// <summary>
        /// Gives the View a reference to the main page for navigation.
        /// </summary>
        /// <param name="page">The main UWP interface page.</param>
        void SetMainPageReference(MainPage page);

    }
}
