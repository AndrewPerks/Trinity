using System.Security.Permissions;
using System.Windows;

namespace Trinity.Views
{
    /// <summary>
    /// Interaction logic for Clients.xaml
    /// </summary>
    [PrincipalPermission(SecurityAction.Demand, Role = "Administrator")]
    [PrincipalPermission(SecurityAction.Demand, Role = "User")]
    [PrincipalPermission(SecurityAction.Demand, Role = "Trinity")]
    public partial class Clients : Window
    {

        public Clients()
        {
            InitializeComponent();
        }

    }
}
