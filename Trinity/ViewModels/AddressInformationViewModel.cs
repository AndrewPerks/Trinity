using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using Trinity.Extensions;
using Trinity.Helpers;
using Trinity.Model;
using Trinity.Services.Concrete;
using Trinity.Services.Interfaces;
using Microsoft.Practices.Unity;

namespace Trinity.ViewModels
{
    /// <summary>
    /// Client address information view model that manages CRUD operations
    /// </summary>
    public class AddressInformationViewModel : ValidatableBindableBase
    {
        #region Private Properties
        private Client _selectedClient;
        private Address _newAddress;
        private Address _selectedClientAddress;
        private Address _selectedBillingAddress;
        private Address _selectedShippingAddress;
        private int _selectedClientAddressId;
        private int _selectedBillingAddressId;
        private int _selectedShippingAddressId;
        private ObservableCollection<Address> _addresses;
        private string _address1;
        private string _address2;
        private string _city;
        private string _county;
        private string _postalCode;
        private int _countryId;
        private bool _isClientAddress;
        private bool _isBillingAddress;
        private bool _isShippingAddress;
        private Mode _mode;
        #endregion

        private readonly IClientService _clientService;
        private readonly IAddressService _addressService;
        private readonly IClientAddressMappingService _clientAddressMappingService;

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

        public ObservableCollection<Address> Addresses
        {
            get { return _addresses; }
            set { SetProperty(ref _addresses, value); }
        }

        public Client SelectedClient
        {
            get { return _selectedClient; }
            set
            {
                SetProperty(ref _selectedClient, value);
            }
        }

        public Address NewAddress
        {
            get { return _newAddress; }
            set
            {
                SetProperty(ref _newAddress, value);
            }
        }

        public Address SelectedClientAddress
        {
            get { return _selectedClientAddress; }
            set
            {
                SetProperty(ref _selectedClientAddress, value);
            }
        }

        public Address SelectedBillingAddress
        {
            get { return _selectedBillingAddress; }
            set
            {
                SetProperty(ref _selectedBillingAddress, value);
            }
        }

        public Address SelectedShippingAddress
        {
            get { return _selectedShippingAddress; }
            set
            {
                SetProperty(ref _selectedShippingAddress, value);
            }
        }

        public int SelectedClientAddressId
        {
            get { return _selectedClientAddressId; }
            set
            {
                SetProperty(ref _selectedClientAddressId, value);
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        public int SelectedBillingAddressId
        {
            get { return _selectedBillingAddressId; }
            set
            {
                SetProperty(ref _selectedBillingAddressId, value);
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        public int SelectedShippingAddressId
        {
            get { return _selectedShippingAddressId; }
            set
            {
                SetProperty(ref _selectedShippingAddressId, value);
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        [Required]
        public string Address1
        {
            get { return _address1; }
            set
            {
                SetProperty(ref _address1, value);
            }
        }

        public string Address2
        {
            get { return _address2; }
            set
            {
                SetProperty(ref _address2, value);
            }
        }

        public string City
        {
            get { return _city; }
            set
            {
                SetProperty(ref _city, value);
            }
        }

        public string County
        {
            get { return _county; }
            set
            {
                SetProperty(ref _county, value);
            }
        }

        public string PostalCode
        {
            get { return _postalCode; }
            set
            {
                SetProperty(ref _postalCode, value);
            }
        }

        public int CountryId
        {
            get { return _countryId; }
            set
            {
                SetProperty(ref _countryId, value);
            }
        }

        public bool IsClientAddress
        {
            get { return _isClientAddress; }
            set
            {
                SetProperty(ref _isClientAddress, value);
                SelectedClient.ClientAddress_Id = NewAddress.Id;
            }
        }

        public bool IsBillingAddress
        {
            get { return _isBillingAddress; }
            set
            {
                SetProperty(ref _isBillingAddress, value);
                SelectedClient.BillingAddress_Id = NewAddress.Id;
            }
        }

        public bool IsShippingAddress
        {
            get { return _isShippingAddress; }
            set
            {
                SetProperty(ref _isShippingAddress, value);
                SelectedClient.ShippingAddress_Id = NewAddress.Id;
            }
        }

        public string ClientAddress { get; set; }

        public string BillingAddress { get; set; }

        public string ShippingAddress { get; set; }

        #endregion

        public RelayCommand AddCommand { get; private set; }
        public RelayCommand SaveCommand { get; private set; }

        public AddressInformationViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject())) return;

            _clientService = ContainerHelper.Container.Resolve<ClientService>();
            _addressService = ContainerHelper.Container.Resolve<AddressService>();
            _clientAddressMappingService = ContainerHelper.Container.Resolve<ClientAddressMappingService>();

            SelectedClient = new Client();
            NewAddress = new Address();
            SelectedClientAddress = new Address();
            SelectedBillingAddress = new Address();
            SelectedShippingAddress = new Address();

            Messenger.Default.Register<Client>(this, OnClientReceived);

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
            SetAddress();
        }

        private void LoadCommands()
        {
            SaveCommand = new RelayCommand(OnSave, CanSave);
            AddCommand = new RelayCommand(OnAdd, CanAdd);
        }

        // Add a new address
        private void OnAdd()
        {
            if (this.Mode == Mode.Add)
            {
                MessageBox.Show("Add a new client before adding an address.");
            }
            else
            {
                try
                {
                    // Add address to client
                    NewAddress.CreatedOn = DateTime.Now;
                    _addressService.AddAddress(NewAddress);

                    // Add client address mapping
                    _clientAddressMappingService.AddClientAddressMapping(new Client_Address_Mapping { Address_Id = NewAddress.Id, Client_Id = SelectedClient.Id });

                    // Map that address to corresponding bill shipping etc if ticked
                    _clientService.UpdateClient(SelectedClient);
                    MessageBox.Show("Address added to client.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    //TODO: Auditing
                }
            }
        }

        // Checks if address is valid
        private bool CanAdd()
        {
            if (HasErrors)
            {
                return false;
            }
            return true;
        }


        private void SetAddress()
        {
            var mappings = _clientAddressMappingService.GetClientAddressMappingsByClientId(SelectedClient.Id);
            if (mappings != null)
            {
                Addresses = _addressService.GetAddressesByClientId(mappings).ToObservableCollection();
            }

            if (SelectedClient.ClientAddress_Id != null)
                SelectedClientAddressId = (int)SelectedClient.ClientAddress_Id;
            if (SelectedClient.BillingAddress_Id != null)
                SelectedBillingAddressId = (int)SelectedClient.BillingAddress_Id;
            if (SelectedClient.ShippingAddress_Id != null)
                SelectedShippingAddressId = (int)SelectedClient.ShippingAddress_Id;

            ClientAddress = ""; // client service get client address as string
            BillingAddress = ""; // client service get billing address as string
            ShippingAddress = ""; // client service get shipping address as string
        }

        // Updates client, billing and shipping address
        private void OnSave()
        {
            try
            {
                if (SelectedClientAddressId > 0)
                    SelectedClient.ClientAddress_Id = SelectedClientAddressId;
                if (SelectedBillingAddressId > 0)
                    SelectedClient.BillingAddress_Id = SelectedBillingAddressId;
                if (SelectedShippingAddressId > 0)
                    SelectedClient.ShippingAddress_Id = SelectedShippingAddressId;
                _clientService.UpdateClient(SelectedClient);
                MessageBox.Show("Address updated.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //TODO: Auditing
            }

        }

        // Checks if a type of address has been selected
        private bool CanSave()
        {
            if (HasErrors || (SelectedClientAddressId <= 0 && SelectedBillingAddressId <= 0 && SelectedShippingAddressId <= 0))
            {
                return false;
            }
            return true;
        }

    }
}
