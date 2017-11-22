using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using Trinity.Extensions;
using Trinity.Helpers;
using Trinity.Model;
using Trinity.Services.Concrete;
using Trinity.Services.Interfaces;
using Microsoft.Practices.Unity;
using Trinity.Views;

namespace Trinity.ViewModels
{
    public class ContactInformationViewModel : ValidatableBindableBase
    {
        #region Private Properties
        private Client _selectedClient;
        private Contact _selectedContact;
        private ObservableCollection<Contact> _contacts;
        private ObservableCollection<Title> _titles = new ObservableCollection<Title>();
        private Title _selectedTitle;
        private Mode _mode;
        #endregion

        private readonly IContactService _contactService;
        private readonly ITitleService _titleService;

        #region Properties
        public Mode Mode
        {
            get
            {
                if (_selectedClient.Id <= 0)
                {
                    _mode = Mode.Add;
                }
                else
                {
                    _mode = Mode.Edit;
                }
                return _mode;
            }
        }

        public Client SelectedClient
        {
            get { return _selectedClient; }
            set
            {
                SetProperty(ref _selectedClient, value);
                OnPropertyChanged("SelectedClient");
            }
        }

        public ObservableCollection<Contact> Contacts
        {
            get { return _contacts; }
            set { SetProperty(ref _contacts, value); }
        }

        public Contact SelectedContact
        {
            get { return _selectedContact; }
            set
            {
                _selectedContact = value;
                OnPropertyChanged("SelectedContact");

                if (SelectedContact != null && SelectedContact.Title_Id != null)
                {
                    SelectedTitle = _titleService.GetById((int)SelectedContact.Title_Id);
                }

                SaveCommand.RaiseCanExecuteChanged();
                DeleteCommand.RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<Title> Titles
        {
            get { return _titles; }
            set
            {
                SetProperty(ref _titles, value);
            }
        }

        public Title SelectedTitle
        {
            get
            {
                if (SelectedContact != null && SelectedContact.Title_Id != null)
                {
                    _selectedTitle = _titleService.GetById((int)SelectedContact.Title_Id);
                }
                return _selectedTitle;
            }
            set
            {
                SetProperty(ref _selectedTitle, value);
                SelectedContact.Title_Id = _selectedTitle.Id;
            }
        }
        #endregion

        public RelayCommand AddCommand { get; private set; }
        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand DeleteCommand { get; private set; }

        public ContactInformationViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject())) return;

            _contactService = ContainerHelper.Container.Resolve<ContactService>();
            _titleService = ContainerHelper.Container.Resolve<TitleService>();

            SelectedClient = new Client();         

            Messenger.Default.Register<Client>(this, OnClientReceived);

            LoadData();    
            LoadCommands();
        }

        private void OnClientReceived(Client client)
        {
            SelectedClient = client;
            LoadData();            

            Messenger.Default.Unregister(this);
        }

        private void LoadData()
        {
            Titles = _titleService.Get().ToObservableCollection();

            // Load titles
            if (SelectedContact != null && SelectedContact.Title_Id != null)
            {
                SelectedTitle = _titleService.GetById((int) SelectedContact.Title_Id);
            }

            Contacts = _contactService.GetByClientId(SelectedClient.Id).ToObservableCollection();
        }

        private void LoadCommands()
        {
            AddCommand = new RelayCommand(OnAdd);
            SaveCommand = new RelayCommand(OnSave, CanSave);
            DeleteCommand = new RelayCommand(OnDelete, CanDelete);
        }

        // Add a new contact to a client
        private void OnAdd()
        {
            if (this.Mode == Mode.Add)
            {
                MessageBox.Show("Add a new client before adding a contact.");
            }
            else
            {
                var viewModel = new AddContactViewModel();
                viewModel.SelectedClientId = SelectedClient.Id;

                var view = new AddContact();
                view.DataContext = viewModel;
                view.Show();     
            }   
        }

        // Update an existing contact
        private void OnSave()
        {
            try
            {
                _contactService.UpdateContact(SelectedContact);
                MessageBox.Show("Contact updated successfully.");

                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //TODO: Auditing
            }
        }

        // Checks that a contact is selected
        private bool CanSave()
        {
            return SelectedContact != null;
        }

        // Delete a contact
        private void OnDelete()
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Delete Contact", System.Windows.MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                var contact = _contactService.GetById(SelectedContact.Id);
                contact.Deleted = true;
                _contactService.UpdateContact(contact);
                LoadData();
            }   
        }

        // Checks that a contact is selected
        private bool CanDelete()
        {
            return SelectedContact != null;
        }

    }
}
