using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MultiAngleVideoPlayer
{
    /// <summary>
    /// 
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //store the paths to the video feeds
        Uri[] vidURIs = new Uri[5];


        // --------------------------------------------------- CONSTRUCTORS ---------------------------------------------------

        /// <summary>
        /// Constructor sets starting visibilities and loads videos
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();

            //version select menu is visible to start
            VersionSelectGrid.Visibility = Visibility.Visible;
            EgoViewGrid.Visibility = Visibility.Collapsed;
            ExoViewGrid.Visibility = Visibility.Collapsed;

            LoadVideos();

            //give the viewer grids a reference to the main page
            EgoViewGrid.SetMainPageReference(this);
        }

        // ------------------------------------------------- PRIVATE METHODS --------------------------------------------------

        /// <summary>
        /// Sets all the video URIs.
        /// </summary>
        private void LoadVideos()
        {
            try
            {
                for (int i = 0; i < vidURIs.Length; i++)
                {
                    vidURIs[i] = new Uri("ms-appx:///Videos/concatNum" + (i + 1) + ".mp4");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        // -------------------------------------------------- PUBLIC METHODS --------------------------------------------------

        /// <summary>
        /// To be called by the viewer grids
        /// </summary>
        /// <returns>A list of URIs pointing to the videos.</returns>
        public Uri[] GetVideos()
        {
            return vidURIs;
        }

        /// <summary>
        /// Allows the viewer grids to make the menu visible
        /// </summary>
        public void ShowMenu()
        {
            VersionSelectGrid.Visibility = Visibility.Collapsed;
        }

        // ------------------------------------------------ UI EVENT HANDLERS -------------------------------------------------

        /// <summary>
        /// Called when the user selects the egocentric viewer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void V1Button_Click(object sender, RoutedEventArgs e)
        {
            VersionSelectGrid.Visibility = Visibility.Collapsed;
            EgoViewGrid.Visibility = Visibility.Visible;
            EgoViewGrid.GridVisible();
        }

        /// <summary>
        /// Called when the user selects the exocentric viewer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void V2Button_Click(object sender, RoutedEventArgs e)
        {
            VersionSelectGrid.Visibility = Visibility.Collapsed;
            ExoViewGrid.Visibility = Visibility.Visible;
        }

        //temp method; this will move to another class later
        private void ExoBackButton_Click(object sender, RoutedEventArgs e)
        {
            ExoViewGrid.Visibility = Visibility.Collapsed;
            VersionSelectGrid.Visibility = Visibility.Visible;
        }

    }
}
