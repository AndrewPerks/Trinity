using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using Trinity.Extensions;
using Trinity.Helpers;
using Trinity.Messages;
using Trinity.Model;
using Trinity.Services.Concrete;
using Trinity.Services.Interfaces;
using Microsoft.Practices.Unity;
using Trinity.Views;

namespace Trinity.ViewModels
{
    public class ClientsViewModel : ValidatableBindableBase
    {        
        #region Private Properties
        private string _surname;
        private string _companyName;
        private string _postcode;
        private ObservableCollection<Client> _clients;
        private Client _selectedClient;
        #endregion

        private readonly IClientService _clientService;

        #region Properties
        public ObservableCollection<Client> Clients
        {
            get { return _clients; }
            set { SetProperty(ref _clients, value); }
        }

        public Client SelectedClient
        {
            get { return _selectedClient; }
            set
            {
                _selectedClient = value;
                EditClientCommand.RaiseCanExecuteChanged();
                DeleteClientCommand.RaiseCanExecuteChanged();
            }
        }

        public string CompanyName
        {
            get { return _companyName; }
            set
            {
                SetProperty(ref _companyName, value);
                SearchCommand.RaiseCanExecuteChanged();
            }
        }

        public string Surname
        {
            get { return _surname; }
            set
            {
                SetProperty(ref _surname, value);
                SearchCommand.RaiseCanExecuteChanged();
            }
        }

        public string Postcode
        {
            get { return _postcode; }
            set
            {
                SetProperty(ref _postcode, value);
                SearchCommand.RaiseCanExecuteChanged();
            }
        }
        #endregion

        public RelayCommand SearchCommand { get; private set; }
        public RelayCommand ResetCommand { get; private set; }
        public RelayCommand AddClientCommand { get; private set; }
        public RelayCommand<Client> EditClientCommand { get; private set; }
        public RelayCommand DeleteClientCommand { get; private set; }
        public RelayCommand<Window> CloseCommand { get; private set; }

        public ClientsViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject())) return;

            _clientService = ContainerHelper.Container.Resolve<ClientService>();            

            LoadData();
            LoadCommands();

            Messenger.Default.Register<UpdateListMessage>(this, OnUpdateListMessageReceived);
        }

        private void LoadData()
        {
            Clients = _clientService.GetActiveClients().ToObservableCollection();            
        }

        private void LoadCommands()
        {
            SearchCommand = new RelayCommand(Search, CanSearch);
            ResetCommand = new RelayCommand(Reset);
            CloseCommand = new RelayCommand<Window>(OnClose);
            AddClientCommand = new RelayCommand(AddClient);
            EditClientCommand = new RelayCommand<Client>(c => OnEdit(SelectedClient), c => CanEdit(SelectedClient));
            DeleteClientCommand = new RelayCommand(OnDelete, CanDelete);
        }

        private void OnUpdateListMessageReceived(UpdateListMessage obj)
        {
            LoadData();
        }

        // Open the add client window
        private void AddClient()
        {
            AddEditClientViewModel viewModel = new AddEditClientViewModel();
            
            var view = new AddEditClient();
            view.DataContext = viewModel;
            view.Title = "Add New Client";
            view.Show();
        }

        // Open the edit client window
        private void OnEdit(Client client)
        {            
            AddEditClientViewModel viewModel = new AddEditClientViewModel();            
            Messenger.Default.Send<Client>(client);

            var view = new AddEditClient();
            view.DataContext = viewModel;
            view.Title = "Edit Client - " + client.ClientName;            
            view.Show();            
        }

        // Checks that a client is selected
        private bool CanEdit(Client client)
        {
            if (client != null)
            {
                return true;
            }
            return false;
        }

        // Delete a client
        private void OnDelete()
        {
            try
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Delete Client", System.Windows.MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    var client = _clientService.GetById(SelectedClient.Id);
                    client.Deleted = true;
                    _clientService.UpdateClient(client);
                    Messenger.Default.Send<UpdateListMessage>(new UpdateListMessage());
                }   
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //TODO: Auditing
            }
        }

        // Checks that a client is selected
        private bool CanDelete()
        {
            return SelectedClient != null;
        }

        // Resets the search parameters
        private void Reset()
        {
            CompanyName = "";
            Surname = "";
            Postcode = "";

            Clients = new ObservableCollection<Client>(new List<Client>()); ;
        }

        // Checks the search criteria is valid
        private bool CanSearch()
        {                                    
            if (String.IsNullOrWhiteSpace(CompanyName) && String.IsNullOrWhiteSpace(Surname) && String.IsNullOrWhiteSpace(Postcode))
            {
                return false;
            }
            return true;
        }

        // Search for a company with a matching name, surname or postcode
        private void Search()
        {
            Clients = _clientService.Search(CompanyName, Surname, Postcode).ToObservableCollection();
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
