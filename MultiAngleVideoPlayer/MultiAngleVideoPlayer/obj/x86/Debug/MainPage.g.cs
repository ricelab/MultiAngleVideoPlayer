﻿#pragma checksum "C:\Users\jessi\Documents\GitHub\MultiAngleVideoPlayer\MultiAngleVideoPlayer\MultiAngleVideoPlayer\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "350273B717F927C58ED352CB15B89328"
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
    partial class MainPage : 
        global::Windows.UI.Xaml.Controls.Page, 
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
            case 1: // MainPage.xaml line 1
                {
                    this.AppPage = (global::Windows.UI.Xaml.Controls.Page)(target);
                }
                break;
            case 2: // MainPage.xaml line 11
                {
                    this.MainGrid = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 3: // MainPage.xaml line 13
                {
                    this.VersionSelectGrid = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 4: // MainPage.xaml line 33
                {
                    this.EgoViewGrid = (global::MultiAngleVideoPlayer.EgoView)(target);
                }
                break;
            case 5: // MainPage.xaml line 34
                {
                    this.SingleViewGrid = (global::MultiAngleVideoPlayer.SingleView)(target);
                }
                break;
            case 6: // MainPage.xaml line 26
                {
                    this.TestVersionButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.TestVersionButton).Click += this.TestVersionButton_Click;
                }
                break;
            case 7: // MainPage.xaml line 27
                {
                    this.RealVersionButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.RealVersionButton).Click += this.RealVersionButton_Click;
                }
                break;
            case 8: // MainPage.xaml line 28
                {
                    this.ParticipantLabel = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 9: // MainPage.xaml line 29
                {
                    this.ParticipantBox = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 10: // MainPage.xaml line 30
                {
                    this.Condition1Flag = (global::Windows.UI.Xaml.Controls.RadioButton)(target);
                    ((global::Windows.UI.Xaml.Controls.RadioButton)this.Condition1Flag).Checked += this.ConditionFlag_Checked;
                }
                break;
            case 11: // MainPage.xaml line 31
                {
                    this.Condition2Flag = (global::Windows.UI.Xaml.Controls.RadioButton)(target);
                    ((global::Windows.UI.Xaml.Controls.RadioButton)this.Condition2Flag).Checked += this.ConditionFlag_Checked;
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

