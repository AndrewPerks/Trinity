using System.Security.Permissions;
using System.Windows;

namespace Trinity.Views
{
    /// <summary>
    /// Interaction logic for Users.xaml
    /// </summary>
    [PrincipalPermission(SecurityAction.Demand, Role = "Administrator")]
    public partial class Users : Window
    {
        public Users()
        {
            InitializeComponent();
        }
    }
}
