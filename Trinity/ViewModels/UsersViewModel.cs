using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using Trinity.Helpers;
using Trinity.Messages;
using Trinity.Model;
using Trinity.Services.Concrete;
using Trinity.Services.Interfaces;
using Microsoft.Practices.Unity;
using Trinity.Views;

namespace Trinity.ViewModels
{
    public class UsersViewModel : BindableBase
    {
        #region Private Properties
        private Contact _selectedUser;
        #endregion

        private readonly IContactService _contactService;

        #region Properties
        public ObservableCollection<Contact> Users { get; set; }

        public Contact SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                DeleteCommand.RaiseCanExecuteChanged();
                EditCommand.RaiseCanExecuteChanged();
            }
        }
        #endregion

        public RelayCommand DeleteCommand { get; private set; }
        public RelayCommand AddCommand { get; private set; }
        public RelayCommand<Contact> EditCommand { get; private set; }
        public RelayCommand<Window> CloseCommand { get; private set; }

        public UsersViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject())) return;
            _contactService = ContainerHelper.Container.Resolve<ContactService>();

            LoadData();
            LoadCommands();

            Messenger.Default.Register<UpdateListMessage>(this, OnUpdateListMessageReceived);
        }

        private void LoadData()
        {
            var userLists = _contactService.GetTrinityUsers();

            var users = new ObservableCollection<Contact>(userLists);
            Users = users;
        }

        private void LoadCommands()
        {
            DeleteCommand = new RelayCommand(OnDelete, CanDelete);
            AddCommand = new RelayCommand(OnAdd);
            EditCommand = new RelayCommand<Contact>(c => OnEdit(SelectedUser), c => CanEdit(SelectedUser));
            CloseCommand = new RelayCommand<Window>(OnClose);
        }

        private void OnUpdateListMessageReceived(UpdateListMessage obj)
        {
            LoadData();
        }

        // Delete a user
        private void OnDelete()
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Delete User", System.Windows.MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                var contact = _contactService.GetById(SelectedUser.Id);
                contact.Deleted = true;
                _contactService.UpdateContact(contact);
                Users.Remove(SelectedUser);
            }  
        }

        // Checks that a user has been selected
        private bool CanDelete()
        {
            return SelectedUser != null;
        }

        // Opens the add user window
        private void OnAdd()
        {
            AddEditUserViewModel viewModel = new AddEditUserViewModel();
            viewModel.Mode = Mode.Add;

            var view = new AddEditUser();
            view.DataContext = viewModel;
            view.Title = "Add User";
            view.Show();
        }

        // Opens the edit user window
        private void OnEdit(Contact user)
        {
            AddEditUserViewModel viewModel = new AddEditUserViewModel();
            viewModel.Mode = Mode.Edit;
            viewModel.Id = user.Id;
            viewModel.Username = user.Username;
            viewModel.IsEnabled = user.LoginActive;
            viewModel.FirstName = user.FirstName;
            viewModel.LastName = user.LastName;
            viewModel.SelectedCompany = user.Client;
            viewModel.PhoneNumber = user.PhoneNumber;
            viewModel.MobileNumber = user.MobileNumber;
            viewModel.Email = user.Email;

            var view = new AddEditUser();
            view.DataContext = viewModel;
            view.Title = "Edit User";
            view.Show();
        }

        // Checks a user can be edited
        private bool CanEdit(Contact user)
        {
            if (user != null)
            {
                return true;
            }
            return false;
        }

        // Close the window
        private void OnClose(Window window)
        {
            if (window != null)
            {
                window.Close();
            }
        }
    }
}
