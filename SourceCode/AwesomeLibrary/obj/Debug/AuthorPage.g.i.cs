﻿#pragma checksum "D:\KodServer\VisualStudioProjeleri\AwesomeLibrary\AwesomeLibrary\AuthorPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "D32F96476EBE9A308BD221CE9150E1E8"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
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
    
    
    public partial class AuthorPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBlock lblAuthorName;
        
        internal System.Windows.Controls.TextBlock lblBookList;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.ListBox lstBooks;
        
        internal System.Windows.Controls.ScrollViewer svBooks;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Awesome%20Library;component/AuthorPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.lblAuthorName = ((System.Windows.Controls.TextBlock)(this.FindName("lblAuthorName")));
            this.lblBookList = ((System.Windows.Controls.TextBlock)(this.FindName("lblBookList")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.lstBooks = ((System.Windows.Controls.ListBox)(this.FindName("lstBooks")));
            this.svBooks = ((System.Windows.Controls.ScrollViewer)(this.FindName("svBooks")));
        }
    }
}

