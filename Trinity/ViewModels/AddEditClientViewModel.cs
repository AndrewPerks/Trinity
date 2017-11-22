using System.ComponentModel;
using System.Windows;
using Trinity.Helpers;
using Microsoft.Practices.Unity;

namespace Trinity.ViewModels
{
    /// <summary>
    /// Shared Add and Edit client view model
    /// </summary>
    public class AddEditClientViewModel : ValidatableBindableBase
    {
        #region Private Properties
        private BindableBase _CurrentViewModel;
        private ClientInformationViewModel _clientInformationViewModel;
        private ContactInformationViewModel _contactViewModel;
        private AddressInformationViewModel _addressViewModel;
        private NoteInformationViewModel _noteViewModel;
        #endregion

        public RelayCommand<string> NavCommand { get; private set; }
        public RelayCommand<Window> CancelCommand { get; private set; }

        public AddEditClientViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject())) return;

            _clientInformationViewModel = ContainerHelper.Container.Resolve<ClientInformationViewModel>();
            _contactViewModel = ContainerHelper.Container.Resolve<ContactInformationViewModel>();
            _addressViewModel = ContainerHelper.Container.Resolve<AddressInformationViewModel>();
            _noteViewModel = ContainerHelper.Container.Resolve<NoteInformationViewModel>();

            LoadCommands();  

            _CurrentViewModel = _clientInformationViewModel;            
        }

        private void LoadCommands()
        {
            NavCommand = new RelayCommand<string>(OnNav);
            CancelCommand = new RelayCommand<Window>(OnCancel);
        }

        public BindableBase CurrentViewModel
        {
            get { return _CurrentViewModel; }
            set { SetProperty(ref _CurrentViewModel, value); }
        }

        // Sets the child view model
        private void OnNav(string destination)
        {
            switch (destination)
            {                
                case "client":                        
                    CurrentViewModel = _clientInformationViewModel;
                    break;
                case "contact":
                    CurrentViewModel = _contactViewModel;
                    break;
                case "address":
                    CurrentViewModel = _addressViewModel;
                    break;
                case "note":
                    CurrentViewModel = _noteViewModel;                   
                    break;
                default:
                    CurrentViewModel = _clientInformationViewModel;
                    break;
            }            
        }

        // Close the window
        private void OnCancel(Window window)
        {
            if (window != null)
            {
                window.Close();
            }
        }
    }
}
