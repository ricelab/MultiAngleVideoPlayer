﻿#pragma checksum "C:\Users\jessi\Documents\GitHub\MultiAngleVideoPlayer\MultiAngleVideoPlayer\MultiAngleVideoPlayer\EgoView.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "6903AF1A361BD2A0B50286CB431643FE"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MultiAngleVideoPlayer
{
    partial class EgoView : 
        global::Windows.UI.Xaml.Controls.UserControl, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.17.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1: // EgoView.xaml line 1
                {
                    this.Ego_View = (global::Windows.UI.Xaml.Controls.UserControl)(target);
                }
                break;
            case 2: // EgoView.xaml line 12
                {
                    this.EgoViewGrid = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 3: // EgoView.xaml line 24
                {
                    this.AngleSelectGrid = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 4: // EgoView.xaml line 48
                {
                    this.NoVidMessage = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 5: // EgoView.xaml line 49
                {
                    this.CurrentVideo = (global::Windows.UI.Xaml.Controls.MediaElement)(target);
                    ((global::Windows.UI.Xaml.Controls.MediaElement)this.CurrentVideo).MediaOpened += this.CurrentVideo_MediaOpened;
                }
                break;
            case 6: // EgoView.xaml line 50
                {
                    this.VideoControlGrid = (global::MultiAngleVideoPlayer.VideoControls)(target);
                }
                break;
            case 7: // EgoView.xaml line 51
                {
                    this.AngleIconGrid = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 8: // EgoView.xaml line 68
                {
                    this.ScrubbingGrid = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 9: // EgoView.xaml line 72
                {
                    this.LoopStatusLabel = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 10: // EgoView.xaml line 73
                {
                    this.SpeedStatusLabel = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 11: // EgoView.xaml line 69
                {
                    this.ScrubbingSquare = (global::Windows.UI.Xaml.Shapes.Rectangle)(target);
                }
                break;
            case 12: // EgoView.xaml line 70
                {
                    this.ScrubbingPreview = (global::Windows.UI.Xaml.Controls.MediaElement)(target);
                    ((global::Windows.UI.Xaml.Controls.MediaElement)this.ScrubbingPreview).MediaOpened += this.ScrubbingPreview_MediaOpened;
                }
                break;
            case 13: // EgoView.xaml line 60
                {
                    this.IconBackground = (global::Windows.UI.Xaml.Shapes.Rectangle)(target);
                }
                break;
            case 14: // EgoView.xaml line 61
                {
                    this.AngleIcon0 = (global::Windows.UI.Xaml.Controls.Image)(target);
                    ((global::Windows.UI.Xaml.Controls.Image)this.AngleIcon0).Tapped += this.AngleChoice_Tapped;
                }
                break;
            case 15: // EgoView.xaml line 62
                {
                    this.AngleIcon1 = (global::Windows.UI.Xaml.Controls.Image)(target);
                    ((global::Windows.UI.Xaml.Controls.Image)this.AngleIcon1).Tapped += this.AngleChoice_Tapped;
                }
                break;
            case 16: // EgoView.xaml line 63
                {
                    this.AngleIcon2 = (global::Windows.UI.Xaml.Controls.Image)(target);
                    ((global::Windows.UI.Xaml.Controls.Image)this.AngleIcon2).Tapped += this.AngleChoice_Tapped;
                }
                break;
            case 17: // EgoView.xaml line 64
                {
                    this.AngleIcon3 = (global::Windows.UI.Xaml.Controls.Image)(target);
                    ((global::Windows.UI.Xaml.Controls.Image)this.AngleIcon3).Tapped += this.AngleChoice_Tapped;
                }
                break;
            case 18: // EgoView.xaml line 65
                {
                    this.AngleIcon4 = (global::Windows.UI.Xaml.Controls.Image)(target);
                    ((global::Windows.UI.Xaml.Controls.Image)this.AngleIcon4).Tapped += this.AngleChoice_Tapped;
                }
                break;
            case 19: // EgoView.xaml line 66
                {
                    this.AngleIcon5 = (global::Windows.UI.Xaml.Controls.Image)(target);
                    ((global::Windows.UI.Xaml.Controls.Image)this.AngleIcon5).Tapped += this.AngleChoice_Tapped;
                }
                break;
            case 20: // EgoView.xaml line 33
                {
                    this.AngleChoice0 = (global::Windows.UI.Xaml.Controls.MediaElement)(target);
                    ((global::Windows.UI.Xaml.Controls.MediaElement)this.AngleChoice0).Tapped += this.AngleChoice_Tapped;
                }
                break;
            case 21: // EgoView.xaml line 34
                {
                    this.AngleChoice1 = (global::Windows.UI.Xaml.Controls.MediaElement)(target);
                    ((global::Windows.UI.Xaml.Controls.MediaElement)this.AngleChoice1).Tapped += this.AngleChoice_Tapped;
                }
                break;
            case 22: // EgoView.xaml line 35
                {
                    this.AngleChoice2 = (global::Windows.UI.Xaml.Controls.MediaElement)(target);
                    ((global::Windows.UI.Xaml.Controls.MediaElement)this.AngleChoice2).Tapped += this.AngleChoice_Tapped;
                }
                break;
            case 23: // EgoView.xaml line 36
                {
                    this.AngleChoice3 = (global::Windows.UI.Xaml.Controls.MediaElement)(target);
                    ((global::Windows.UI.Xaml.Controls.MediaElement)this.AngleChoice3).Tapped += this.AngleChoice_Tapped;
                }
                break;
            case 24: // EgoView.xaml line 37
                {
                    this.AngleChoice4 = (global::Windows.UI.Xaml.Controls.MediaElement)(target);
                    ((global::Windows.UI.Xaml.Controls.MediaElement)this.AngleChoice4).Tapped += this.AngleChoice_Tapped;
                }
                break;
            case 25: // EgoView.xaml line 38
                {
                    this.AngleChoice5 = (global::Windows.UI.Xaml.Controls.MediaElement)(target);
                    ((global::Windows.UI.Xaml.Controls.MediaElement)this.AngleChoice5).Tapped += this.AngleChoice_Tapped;
                }
                break;
            case 26: // EgoView.xaml line 39
                {
                    this.Borders = (global::Windows.UI.Xaml.Controls.Canvas)(target);
                }
                break;
            case 27: // EgoView.xaml line 40
                {
                    this.Border0 = (global::Windows.UI.Xaml.Controls.Border)(target);
                }
                break;
            case 28: // EgoView.xaml line 41
                {
                    this.Border1 = (global::Windows.UI.Xaml.Controls.Border)(target);
                }
                break;
            case 29: // EgoView.xaml line 42
                {
                    this.Border2 = (global::Windows.UI.Xaml.Controls.Border)(target);
                }
                break;
            case 30: // EgoView.xaml line 43
                {
                    this.Border3 = (global::Windows.UI.Xaml.Controls.Border)(target);
                }
                break;
            case 31: // EgoView.xaml line 44
                {
                    this.Border4 = (global::Windows.UI.Xaml.Controls.Border)(target);
                }
                break;
            case 32: // EgoView.xaml line 45
                {
                    this.Border5 = (global::Windows.UI.Xaml.Controls.Border)(target);
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        /// <summary>
        /// GetBindingConnector(int connectionId, object target)
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.17.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

