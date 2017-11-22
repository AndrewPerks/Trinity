using System;
using System.Security.Permissions;
using System.Threading;
using System.Windows;
using Trinity.Authentication;

namespace Trinity.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [PrincipalPermission(SecurityAction.Demand, Role = "Administrator")]
    [PrincipalPermission(SecurityAction.Demand, Role = "User")]
    [PrincipalPermission(SecurityAction.Demand, Role = "Trinity")]
    public partial class MainWindow : Window
    {

        public MainWindow()
        {        
            InitializeComponent();

            //Get the current principal object
            CustomPrincipal customPrincipal = Thread.CurrentPrincipal as CustomPrincipal;
            if (customPrincipal == null)
                throw new ArgumentException("The application's default thread principal must be set to a CustomPrincipal object on startup.");

            if (customPrincipal.Identity.IsAuthenticated && !customPrincipal.IsInRole("Administrator"))
            {
                AdminMenuItem.Visibility = Visibility.Collapsed;
            }
        }        

    }
}
