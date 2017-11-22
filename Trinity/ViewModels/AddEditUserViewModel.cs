using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using Microsoft.Practices.Unity;
using Trinity.Extensions;
using Trinity.Helpers;
using Trinity.Messages;
using Trinity.Model;
using Trinity.Services.Concrete;
using Trinity.Services.Interfaces;

namespace Trinity.ViewModels
{
    /// <summary>
    /// Shared Add and Edit user view model 
    /// </summary>
    public class AddEditUserViewModel : ValidatableBindableBase
    {
        #region Private Properties
        private int _id;
        private string _username;
        private bool _isEnabled;
        private string _firstName;
        private string _lastName;
        private ObservableCollection<Client> _companies = new ObservableCollection<Client>();
        private Client _selectedCompany = new Client();
        private string _phoneNumber;
        private string _mobileNumber;
        private string _emailAddress;
        #endregion

        private readonly IContactService _contactService;
        private readonly IClientService _clientService;
        private readonly IRoleService _roleService;

        #region Properties
        public Mode Mode
        {
            get;
            set;
        }

        public int Id
        {
            get { return _id; }
            set
            {
                SetProperty(ref _id, value);
            }
        }

        [Required]
        public string Username
        {
            get { return _username; }
            set
            {
                SetProperty(ref _username, value);
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                SetProperty(ref _isEnabled, value);
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                SetProperty(ref _firstName, value);
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        [Required]
        public string LastName
        {
            get { return _lastName; }
            set
            {
                SetProperty(ref _lastName, value);
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<Client> Companies
        {
            get { return _companies; }
            set
            {
                SetProperty(ref _companies, value);
            }
        }

        public Client SelectedCompany
        {
            get { return _selectedCompany; }
            set
            {
                SetProperty(ref _selectedCompany, value);
                SaveCommand.RaiseCanExecuteChanged();
            }
        }
        
        [Phone]
        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set
            {
                SetProperty(ref _phoneNumber, value);
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        [Phone]
        public string MobileNumber
        {
            get { return _mobileNumber; }
            set
            {
                SetProperty(ref _mobileNumber, value);
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        [EmailAddress]
        public string Email
        {
            get { return _emailAddress; }
            set
            {
                SetProperty(ref _emailAddress, value);
                SaveCommand.RaiseCanExecuteChanged();
            }
        }
        #endregion

        public RelayCommand<Window> SaveCommand { get; private set; }
        public RelayCommand<Window> CancelCommand { get; private set; }
        public RelayCommand ResetPasswordCommand { get; private set; }

        public AddEditUserViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject())) return;

            _contactService = ContainerHelper.Container.Resolve<ContactService>();
            _clientService = ContainerHelper.Container.Resolve<ClientService>();
            _roleService = ContainerHelper.Container.Resolve<RoleService>();
            
            LoadData();
            LoadCommands();
        }

        private void LoadData()
        {
            Companies = _clientService.GetActiveClients().ToObservableCollection();
        }

        private void LoadCommands()
        {
            SaveCommand = new RelayCommand<Window>(OnSave, CanSave);
            CancelCommand = new RelayCommand<Window>(OnCancel);
            ResetPasswordCommand = new RelayCommand(OnResetPassword, CanResetPassword);
        }

        // Saves or updates a user
        private void OnSave(Window window)
        {
            if (this.Mode == Mode.Add)
            {
                AddUser(window);
            }
            else if (this.Mode == Mode.Edit)
            {
                EditUser(Id);
            }
        }

        // Add a new user
        private void AddUser(Window window)
        {
            try
            {
                // Check if username exists
                var user = _contactService.GetByUsername(Username);
                if (user != null)
                {
                    MessageBox.Show("Username already exists.");                    
                }
                else
                {
                    var role = _roleService.Search("Trinity");
                    var roleCollection = new Collection<Role> {role};

                    var newUser = new Contact
                    {
                        FirstName = FirstName,
                        LastName = LastName,
                        Client_Id = SelectedCompany.Id,
                        Username = Username,
                        PhoneNumber = PhoneNumber,
                        MobileNumber = MobileNumber,
                        Email = Email,
                        PasswordFormat_Id = 1,
                        LoginActive = IsEnabled,
                        FailedLoginCount = 0,
                        Deleted = false,
                        IsSystemAccount = true,
                        CreatedOn = System.DateTime.Now,
                        Roles = roleCollection 
                    };

                    _contactService.AddContact(newUser);     
                    MessageBox.Show("User added successfully.");
                    Messenger.Default.Send<UpdateListMessage>(new UpdateListMessage());
                    if (window != null)
                    {
                        window.Close();
                    }
                }                                    
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //TODO: Auditing
            }
        }

        // Edit an existing user
        private void EditUser(int id)
        {
            try
            {
                var user = _contactService.GetById(id);
                if (user != null)
                {
                    user.FirstName = FirstName;
                    user.LastName = LastName;
                    user.Client_Id = SelectedCompany.Id;
                    user.PhoneNumber = PhoneNumber;
                    user.MobileNumber = MobileNumber;
                    user.Email = Email;
                    user.LoginActive = IsEnabled;

                    _contactService.UpdateContact(user);
                    MessageBox.Show("User updated successfully.");
                    Messenger.Default.Send<UpdateListMessage>(new UpdateListMessage());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //TODO: Auditing
            }
        }

        // Checks if user is valid
        private bool CanSave(Window window)
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(LastName) || HasErrors || SelectedCompany.Id <= 0)
            {
                return false;
            }            
            return true;
        }

        // Close the window
        private void OnCancel(Window window)
        {
            if (window != null)
            {
                window.Close();
            }
        }

        // Resets an existing users password
        private void OnResetPassword()
        {
            try
            {
                // Use a dialog box confirmation
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Reset User Password", System.Windows.MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    var user = _contactService.GetById(Id);
                    if (user != null)
                    {
                        user.PasswordHash = null;
                        user.PasswordSalt = null;
                        _contactService.UpdateContact(user);
                        MessageBox.Show("Password reset successfully.");
                    }
                }                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //TODO: Auditing
            }
        }

        // Checks if reset password is possible
        private bool CanResetPassword()
        {
            if (this.Mode == Mode.Add)
            {
                return false;
            }
            return true;
        }
    }
}
