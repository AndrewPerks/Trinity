using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using Trinity.Helpers;
using Trinity.Model;
using Trinity.Services.Concrete;
using Trinity.Services.Interfaces;
using Microsoft.Practices.Unity;

namespace Trinity.ViewModels
{
    /// <summary>
    /// Add contact view model 
    /// </summary>
    public class AddContactViewModel : ValidatableBindableBase
    {
        #region Private Properties
        private string _firstName;
        private string _lastName;
        private int _selectedClientId;
        #endregion

        private readonly IContactService _contactService;

        #region Properties
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

        public int SelectedClientId
        {
            get { return _selectedClientId; }
            set
            {
                SetProperty(ref _selectedClientId, value);
                SaveCommand.RaiseCanExecuteChanged();
            }
        }
        #endregion     

        public RelayCommand<Window> SaveCommand { get; private set; }
        public RelayCommand<Window> CancelCommand { get; private set; }

        public AddContactViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject())) return;

            _contactService = ContainerHelper.Container.Resolve<ContactService>();

            LoadCommands();
        }

        private void LoadCommands()
        {
            SaveCommand = new RelayCommand<Window>(OnSave, CanSave);
            CancelCommand = new RelayCommand<Window>(OnCancel);
        }

        // Add a new contact
        private void OnSave(Window window)
        {
            try
            {
                var contact = new Contact
                {
                    FirstName = FirstName, 
                    LastName = LastName,
                    Client_Id = SelectedClientId,
                    PasswordFormat_Id = 1,
                    LoginActive = true,
                    CreatedOn = DateTime.Now
                };

                _contactService.AddContact(contact);
                MessageBox.Show("Contact added successfully.");
                if (window != null)
                {
                    window.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //TODO: Auditing
            }
        }

        // Checks if contact is valid
        private bool CanSave(Window window)
        {
            if (string.IsNullOrWhiteSpace(LastName) || HasErrors)
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
    }
}
