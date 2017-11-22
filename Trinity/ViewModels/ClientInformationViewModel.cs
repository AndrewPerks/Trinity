using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using Microsoft.Practices.Unity;
using Trinity.Helpers;
using Trinity.Messages;
using Trinity.Model;
using Trinity.Services.Concrete;
using Trinity.Services.Interfaces;

namespace Trinity.ViewModels
{
    public class ClientInformationViewModel : ValidatableBindableBase
    {
        #region Private Properties
        private Client _selectedClient;
        private int _id;
        private string _clientName;
        private string _website;
        private string _email;
        private bool _isTaxExempt;
        private bool _isActive;
        private string _phoneNumber;
        private string _financeLink;
        private int? _paymentTerms;
        private string _clientCode;
        private DateTime _createdOn;
        private bool _isDeleted;
        private Mode _mode;
        #endregion

        private readonly IClientService _clientService;

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

        public int Id
        {
            get { return _id; }
            set
            {
                SetProperty(ref _id, value);
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        [Required]
        public string ClientName
        {
            get { return _clientName; }
            set
            {
                SetProperty(ref _clientName, value);
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        public string WebSite
        {
            get { return _website; }
            set
            {
                SetProperty(ref _website, value);
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        [EmailAddress]
        public string Email
        {
            get { return _email; }
            set
            {
                SetProperty(ref _email, value);
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        public bool IsTaxExempt
        {
            get { return _isTaxExempt; }
            set
            {
                SetProperty(ref _isTaxExempt, value);
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        public bool Active
        {
            get { return _isActive; }
            set
            {
                SetProperty(ref _isActive, value);
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

        public string FinanceLink
        {
            get { return _financeLink; }
            set
            {
                SetProperty(ref _financeLink, value);
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        public int? PaymentTerms
        {
            get { return _paymentTerms; }
            set
            {
                SetProperty(ref _paymentTerms, value);
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        public string ClientCode
        {
            get { return _clientCode; }
            set
            {
                SetProperty(ref _clientCode, value);
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        public DateTime CreatedOn
        {
            get { return _createdOn; }
            set
            {
                SetProperty(ref _createdOn, value);
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        public bool Deleted
        {
            get { return _isDeleted; }
            set
            {
                SetProperty(ref _isDeleted, value);
                SaveCommand.RaiseCanExecuteChanged();
            }
        }
        #endregion

        public RelayCommand<object> SaveCommand { get; private set; }
        public RelayCommand<object> CloseCommand { get; private set; }

        public ClientInformationViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject())) return;

            _clientService = ContainerHelper.Container.Resolve<ClientService>();

            SelectedClient = new Client();

            Messenger.Default.Register<Client>(this, OnClientReceived);

            LoadCommands();
        }

        private void LoadCommands()
        {
            SaveCommand = new RelayCommand<object>(OnSave, CanSave);
            CloseCommand = new RelayCommand<object>(OnClose);
        }

        private void OnClientReceived(Client client)
        {
            SelectedClient = client;
            Id = SelectedClient.Id;
            ClientName = SelectedClient.ClientName;
            WebSite = SelectedClient.WebSite;
            Email = SelectedClient.Email;
            IsTaxExempt = SelectedClient.IsTaxExempt;
            Active = SelectedClient.Active;
            PhoneNumber = SelectedClient.PhoneNumber;
            FinanceLink = SelectedClient.FinanceLink;
            PaymentTerms = SelectedClient.PaymentTerms;
            ClientCode = SelectedClient.ClientCode;
            CreatedOn = SelectedClient.CreatedOn;
            Deleted = SelectedClient.Deleted;

            Messenger.Default.Unregister(this);
        }

        // Checks the client is valid
        private bool CanSave(object o)
        {
            if (string.IsNullOrWhiteSpace(ClientName) || HasErrors)
            {
                return false;
            }
            return true;
        }

        // Saves the client depending on if add or edit mode
        private void OnSave(object o)
        {
            if (this.Mode == Mode.Add)
            {
                AddClient(o);
            }
            else if (this.Mode == Mode.Edit)
            {
                EditClient();
            }
        }

        private void Bind()
        {
            SelectedClient.ClientName = ClientName;
            SelectedClient.WebSite = WebSite;
            SelectedClient.Email = Email;
            SelectedClient.IsTaxExempt = IsTaxExempt;
            SelectedClient.Active = Active;
            SelectedClient.PhoneNumber = PhoneNumber;
            SelectedClient.FinanceLink = FinanceLink;
            SelectedClient.PaymentTerms = PaymentTerms;
            SelectedClient.ClientCode = ClientCode;
            SelectedClient.Deleted = Deleted;
        }

        // Adds a new client
        private void AddClient(object o)
        {
            try
            {
                var client = _clientService.GetByClientName(ClientName);
                if (client != null)
                {
                    MessageBox.Show("Client already exists.");
                }
                else
                {
                    Bind();
                    SelectedClient.CreatedOn = System.DateTime.Now;

                    _clientService.AddClient(SelectedClient);
                    MessageBox.Show("Client added successfully.");
                    OnClose(o);
                    Messenger.Default.Send<UpdateListMessage>(new UpdateListMessage());
                }     
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //TODO: Auditing
            }
        }

        // Updates an existing client
        private void EditClient()
        {
            try
            {
                Bind();

                _clientService.UpdateClient(SelectedClient);
                MessageBox.Show("Client updated successfully.");
                Messenger.Default.Send<UpdateListMessage>(new UpdateListMessage());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //TODO: Auditing
            }
        }

        // Close the window
        private void OnClose(object o)
        {
            ((Window)o).Close();
        }

    }
}
