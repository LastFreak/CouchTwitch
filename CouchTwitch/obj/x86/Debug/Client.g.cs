﻿#pragma checksum "D:\Entwickeln\Visual Studio\Teamprojekte\CouchTwitch\CouchTwitch\Client.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "1F056E58BFDF03DCC5BB0E32E377D6AA"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CouchTwitch
{
    partial class Client : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                {
                    this.menu = (global::Windows.UI.Xaml.Controls.SplitView)(target);
                }
                break;
            case 2:
                {
                    this.lstChat = (global::Windows.UI.Xaml.Controls.ListView)(target);
                    #line 97 "..\..\..\Client.xaml"
                    ((global::Windows.UI.Xaml.Controls.ListView)this.lstChat).SelectionChanged += this.lstChat_SelectionChanged;
                    #line default
                }
                break;
            case 3:
                {
                    this.mediaAudioStream = (global::Windows.UI.Xaml.Controls.MediaElement)(target);
                }
                break;
            case 4:
                {
                    this.cdIP = (global::Windows.UI.Xaml.Controls.ContentDialog)(target);
                    #line 140 "..\..\..\Client.xaml"
                    ((global::Windows.UI.Xaml.Controls.ContentDialog)this.cdIP).Opened += this.cdIP_Opened;
                    #line default
                }
                break;
            case 5:
                {
                    this.txtIP = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 6:
                {
                    this.ChangePCPhone = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 120 "..\..\..\Client.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.ChangePCPhone).Click += this.ChangePCPhone_Click;
                    #line default
                }
                break;
            case 7:
                {
                    this.PlayPauseButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 126 "..\..\..\Client.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.PlayPauseButton).Click += this.PlayPauseButton_Click;
                    #line default
                }
                break;
            case 8:
                {
                    this.sldVolume = (global::Windows.UI.Xaml.Controls.Slider)(target);
                    #line 135 "..\..\..\Client.xaml"
                    ((global::Windows.UI.Xaml.Controls.Slider)this.sldVolume).ValueChanged += this.sldVolume_ValueChanged;
                    #line default
                }
                break;
            case 9:
                {
                    this.Pause = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 10:
                {
                    this.Play = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 11:
                {
                    this.PCLogo = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 12:
                {
                    this.PhoneLogo = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 13:
                {
                    this.txtToken = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                    #line 116 "..\..\..\Client.xaml"
                    ((global::Windows.UI.Xaml.Controls.TextBox)this.txtToken).KeyDown += this.txtToken_KeyDown;
                    #line default
                }
                break;
            case 14:
                {
                    this.btnLogin = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 92 "..\..\..\Client.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.btnLogin).Click += this.btnLogin_Click;
                    #line default
                }
                break;
            case 15:
                {
                    global::Windows.UI.Xaml.Controls.Button element15 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 94 "..\..\..\Client.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)element15).Click += this.Button_Click;
                    #line default
                }
                break;
            case 16:
                {
                    this.lstFollows = (global::Windows.UI.Xaml.Controls.ListView)(target);
                    #line 157 "..\..\..\Client.xaml"
                    ((global::Windows.UI.Xaml.Controls.ListView)this.lstFollows).SelectionChanged += this.lstFollows_SelectionChanged;
                    #line default
                }
                break;
            case 17:
                {
                    global::Windows.UI.Xaml.Controls.Button element17 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 155 "..\..\..\Client.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)element17).Click += this.Button_Click;
                    #line default
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

