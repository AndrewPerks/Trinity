﻿#pragma checksum "..\..\..\Views\ChangePassword.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "7B76279AD8C68FF8CD73C41CE0DE3284"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using Trinity;
using Trinity.Authentication;
using Trinity.Helpers;
using Trinity.ViewModels;


namespace Trinity.Views {
    
    
    /// <summary>
    /// ChangePassword
    /// </summary>
    public partial class ChangePassword : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 2 "..\..\..\Views\ChangePassword.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Trinity.Views.ChangePassword ChangePasswordWindow;
        
        #line default
        #line hidden
        
        
        #line 12 "..\..\..\Views\ChangePassword.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblUsername;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\..\Views\ChangePassword.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtUsername;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\..\Views\ChangePassword.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblOldPassword;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\..\Views\ChangePassword.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox passwordBox;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\..\Views\ChangePassword.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox newPasswordBox;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\..\Views\ChangePassword.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox confirmedPasswordBox;
        
        #line default
        #line hidden
        
        
        #line 45 "..\..\..\Views\ChangePassword.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblConfirmation;
        
        #line default
        #line hidden
        
        
        #line 46 "..\..\..\Views\ChangePassword.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnSave;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\..\Views\ChangePassword.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnCancel;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Trinity;component/views/changepassword.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Views\ChangePassword.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.ChangePasswordWindow = ((Trinity.Views.ChangePassword)(target));
            return;
            case 2:
            this.lblUsername = ((System.Windows.Controls.Label)(target));
            return;
            case 3:
            this.txtUsername = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.lblOldPassword = ((System.Windows.Controls.Label)(target));
            return;
            case 5:
            this.passwordBox = ((System.Windows.Controls.PasswordBox)(target));
            
            #line 28 "..\..\..\Views\ChangePassword.xaml"
            this.passwordBox.PasswordChanged += new System.Windows.RoutedEventHandler(this.passwordBox_PasswordChanged);
            
            #line default
            #line hidden
            return;
            case 6:
            this.newPasswordBox = ((System.Windows.Controls.PasswordBox)(target));
            
            #line 42 "..\..\..\Views\ChangePassword.xaml"
            this.newPasswordBox.PasswordChanged += new System.Windows.RoutedEventHandler(this.passwordBox_PasswordChanged);
            
            #line default
            #line hidden
            return;
            case 7:
            this.confirmedPasswordBox = ((System.Windows.Controls.PasswordBox)(target));
            
            #line 44 "..\..\..\Views\ChangePassword.xaml"
            this.confirmedPasswordBox.PasswordChanged += new System.Windows.RoutedEventHandler(this.passwordBox_PasswordChanged);
            
            #line default
            #line hidden
            return;
            case 8:
            this.lblConfirmation = ((System.Windows.Controls.Label)(target));
            return;
            case 9:
            this.btnSave = ((System.Windows.Controls.Button)(target));
            return;
            case 10:
            this.btnCancel = ((System.Windows.Controls.Button)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

