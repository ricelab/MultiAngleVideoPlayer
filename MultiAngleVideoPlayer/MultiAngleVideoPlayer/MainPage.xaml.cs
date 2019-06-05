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
using Windows.ApplicationModel.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MultiAngleVideoPlayer
{
    /// <summary>
    /// 
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //store the paths to the video feeds
        Uri[] vidURIs = new Uri[6];
        bool testMode = false;
        bool singleMode = false;
        string participant = "";


        // --------------------------------------------------- CONSTRUCTORS ---------------------------------------------------

        /// <summary>
        /// Constructor sets starting visibilities and loads videos
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();

            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            //coreTitleBar.ExtendViewIntoTitleBar = true;

            //initialize logger
            //Logger.CreateLog("test");

            //set grid visibility
            EgoViewGrid.Visibility = Visibility.Collapsed;
            SingleViewGrid.Visibility = Visibility.Collapsed;
            VersionSelectGrid.Visibility = Visibility.Visible;

            //LoadVideos();

            //give the viewer grid a reference to the main page
            EgoViewGrid.SetMainPageReference(this);
            SingleViewGrid.SetMainPageReference(this);

            //EgoViewGrid.GridVisible();
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
                    if (testMode)
                    {
                        vidURIs[i] = new Uri("ms-appx:///Videos/lego" + (i + 1) + ".avi");
                    } else
                    {
                        vidURIs[i] = new Uri("ms-appx:///Videos/lamp" + (i + 1) + ".mp4");
                    }
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
            VersionSelectGrid.Visibility = Visibility.Visible;
            EgoViewGrid.Visibility = Visibility.Collapsed;
            SingleViewGrid.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Reads the chapters.json file to get all chapter names and chapter start times.
        /// </summary>
        /// <returns>Chapter data read from the chapters.json file.</returns>
        public Chapters GetVideoChapters()
        {
            try
            {
                string json;
                if (testMode)
                {
                    json = File.ReadAllText("EditorOutputData/lego_chapters.json");
                } else
                {
                    json = File.ReadAllText("EditorOutputData/lamp_chapters.json");
                }
                Chapters chapters = JsonConvert.DeserializeObject<Chapters>(json);
                return chapters;
            } catch(Exception e)
            {
                Debug.WriteLine(e);
                return null;
            }
        }

        // ------------------------------------------------ UI EVENT HANDLERS -------------------------------------------------

        /*
        /// <summary>
        /// Called when the user selects the egocentric viewer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void V1Button_Click(object sender, RoutedEventArgs e)
        {
            VersionSelectGrid.Visibility = Visibility.Collapsed;
            if (!singleMode)
            {
                EgoViewGrid.Visibility = Visibility.Visible;
                EgoViewGrid.GridVisible();
            } else
            {
                SingleViewGrid.Visibility = Visibility.Visible;
                SingleViewGrid.GridVisible();
            }
            
        }

        /// <summary>
        /// Called when the user selects the exocentric viewer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void V2Button_Click(object sender, RoutedEventArgs e)
        {
            VersionSelectGrid.Visibility = Visibility.Collapsed;
        }
        */

        private void TestVersionButton_Click(object sender, RoutedEventArgs e)
        {
            testMode = true;
            Logger.log_yes = false;
            LoadVideos();
            participant = ParticipantBox.Text;
            VersionSelectGrid.Visibility = Visibility.Collapsed;
            if (!singleMode)
            {
                EgoViewGrid.Visibility = Visibility.Visible;
                EgoViewGrid.GridVisible();
            } else
            {
                SingleViewGrid.Visibility = Visibility.Visible;
                SingleViewGrid.GridVisible();
            }

            
        }

        private void RealVersionButton_Click(object sender, RoutedEventArgs e)
        {
            testMode = false;
            Logger.log_yes = true;
            participant = ParticipantBox.Text;
            Logger.CreateLog(participant);
            LoadVideos();
            VersionSelectGrid.Visibility = Visibility.Collapsed;
            if (!singleMode)
            {
                EgoViewGrid.Visibility = Visibility.Visible;
                EgoViewGrid.HideBackButton();
                EgoViewGrid.GridVisible();
                Logger.Log("Multi View");
            } else
            {
                SingleViewGrid.Visibility = Visibility.Visible;
                SingleViewGrid.HideBackButton();
                SingleViewGrid.GridVisible();
                Logger.Log("Single View");
            }
            
        }

        private void ConditionFlag_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton b = (RadioButton)sender;
            if (b.Content.Equals("Condition 1"))
            {
                singleMode = true;
            } else
            {
                singleMode = false;
            }
        }
    }
}
