using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace MultiAngleVideoPlayer
{
    public interface IViewable
    {
        void SetupVideos();

        void PlayVid();

        void PauseVid();

        void GridVisible();

        void PlayPause();

        void AngleChoice_Tapped(object sender, TappedRoutedEventArgs e);

        void CurrentVideo_MediaOpened(object sender, RoutedEventArgs e);

        void SetMainPageReference(MainPage page);

    }
}
