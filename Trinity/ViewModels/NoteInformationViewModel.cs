using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using Trinity.Helpers;
using Trinity.Model;

namespace Trinity.ViewModels
{
    public class NoteInformationViewModel : ValidatableBindableBase
    {
        #region Private Properties
        private Client _selectedClient;
        private ClientNote _selectedNote;
        private ObservableCollection<Contact> _contacts;
        private Mode _mode;
        #endregion

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
                _selectedClient = value;
            }
        }

        public ClientNote SelectedNote
        {
            get { return _selectedNote; }
            set
            {
                SetProperty(ref _selectedNote, value);
                OnPropertyChanged("SelectedNote");
                //SaveCommand.RaiseCanExecuteChanged();
            }
        }
        #endregion

        public RelayCommand AddCommand { get; private set; }
        public RelayCommand DeleteCommand { get; private set; } 
        public RelayCommand SaveCommand { get; private set; } 

        public NoteInformationViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject())) return;

            SelectedClient = new Client();

            Messenger.Default.Register<Client>(this, OnClientReceived);

            LoadData();
            LoadCommands();
        }

        private void OnClientReceived(Client client)
        {
            SelectedClient = client;

            Messenger.Default.Unregister(this);
        }

        private void LoadData()
        {
            
        }
        
        private void LoadCommands()
        {
            SaveCommand = new RelayCommand(OnSave, CanSave);
            AddCommand = new RelayCommand(OnAdd, CanAdd);
            DeleteCommand = new RelayCommand(OnDelete, CanDelete);
        }

        // Add a new note against a client
        private void OnAdd()
        {
            if (this.Mode == Mode.Add)
            {
                MessageBox.Show("Add a new client before adding a note.");
            }
            else
            {

            }   
        }

        // Checks that the note is valid
        private bool CanAdd()
        {
            if (HasErrors)
            {
                return false;
            }
            return true;
        }

        // Update an existing note
        private void OnSave()
        {
            
        }

        // Checks that an existing note is selected
        private bool CanSave()
        {
            return SelectedNote != null;
        }

        // Delete a note 
        private void OnDelete()
        {
            
        }

        // Checks that an existing note is selected
        private bool CanDelete()
        {
            return SelectedNote != null;
        }

    }
}
