﻿#pragma checksum "D:\KodServer\VisualStudioProjeleri\AwesomeLibrary\AwesomeLibrary\AuthorSettingsPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "02F81383E52CA6286C56ACC81D650842"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace AwesomeLibrary {
    
    
    public partial class AuthorSettingsPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal Microsoft.Phone.Controls.Pivot pvAuthorSettings;
        
        internal Microsoft.Phone.Controls.PivotItem piFont;
        
        internal System.Windows.Controls.StackPanel spFont;
        
        internal System.Windows.Controls.TextBox lblFontFamily;
        
        internal System.Windows.Controls.Button btnFontFamily;
        
        internal System.Windows.Controls.TextBox lblFontSize;
        
        internal System.Windows.Controls.Button btnFontSize;
        
        internal Microsoft.Phone.Controls.PivotItem piOtherSettings;
        
        internal System.Windows.Controls.StackPanel spOtherSettings;
        
        internal System.Windows.Controls.TextBox lblBookOrder;
        
        internal System.Windows.Controls.Button btnBookOrder;
        
        internal System.Windows.Controls.TextBox lblBookOrderStyle;
        
        internal System.Windows.Controls.Button btnBookOrderStyle;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/Awesome%20Library;component/AuthorSettingsPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.pvAuthorSettings = ((Microsoft.Phone.Controls.Pivot)(this.FindName("pvAuthorSettings")));
            this.piFont = ((Microsoft.Phone.Controls.PivotItem)(this.FindName("piFont")));
            this.spFont = ((System.Windows.Controls.StackPanel)(this.FindName("spFont")));
            this.lblFontFamily = ((System.Windows.Controls.TextBox)(this.FindName("lblFontFamily")));
            this.btnFontFamily = ((System.Windows.Controls.Button)(this.FindName("btnFontFamily")));
            this.lblFontSize = ((System.Windows.Controls.TextBox)(this.FindName("lblFontSize")));
            this.btnFontSize = ((System.Windows.Controls.Button)(this.FindName("btnFontSize")));
            this.piOtherSettings = ((Microsoft.Phone.Controls.PivotItem)(this.FindName("piOtherSettings")));
            this.spOtherSettings = ((System.Windows.Controls.StackPanel)(this.FindName("spOtherSettings")));
            this.lblBookOrder = ((System.Windows.Controls.TextBox)(this.FindName("lblBookOrder")));
            this.btnBookOrder = ((System.Windows.Controls.Button)(this.FindName("btnBookOrder")));
            this.lblBookOrderStyle = ((System.Windows.Controls.TextBox)(this.FindName("lblBookOrderStyle")));
            this.btnBookOrderStyle = ((System.Windows.Controls.Button)(this.FindName("btnBookOrderStyle")));
        }
    }
}

