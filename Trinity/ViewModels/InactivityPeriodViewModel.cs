using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using Trinity.Helpers;
using Trinity.Services.Concrete;
using Trinity.Services.Interfaces;
using Microsoft.Practices.Unity;

namespace Trinity.ViewModels
{
    public class InactivityPeriodViewModel : ValidatableBindableBase
    {
        #region Private Properties
        private string _amountOfHours;
        #endregion

        private readonly IApplicationSettingService _applicationSettingService;

        #region Properties
        [Required]
        public string AmountOfHours
        {
            get { return _amountOfHours; }
            set
            {
                SetProperty(ref _amountOfHours, value);
                //SaveCommand.RaiseCanExecuteChanged();
            }
        }
        #endregion        

        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand<Window> CancelCommand { get; private set; }

        public InactivityPeriodViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject())) return;

            _applicationSettingService = ContainerHelper.Container.Resolve<ApplicationSettingService>();
            
            LoadData();
            LoadCommands();
        }

        private void LoadData()
        {
            var applicationSetting = _applicationSettingService.GetTimeOutLength();

            AmountOfHours = applicationSetting.Value;
        }

        private void LoadCommands()
        {
            SaveCommand = new RelayCommand(OnSave, CanSave);
            CancelCommand = new RelayCommand<Window>(OnCancel);
        }

        // Update the inactivity period time
        private void OnSave()
        {
            try
            {
                int i;
                if (!int.TryParse(AmountOfHours, out i))
                {
                    MessageBox.Show("Must enter a number.");
                    return;
                }
                if(i <= 0)
                {
                    MessageBox.Show("Value must be greater than 0.");
                    return;
                }

                var applicationSetting = _applicationSettingService.GetTimeOutLength();
                if (applicationSetting != null)
                {
                    applicationSetting.Value = AmountOfHours;
                    _applicationSettingService.UpdateApplicationSetting(applicationSetting);

                    MessageBox.Show("Timeout period set to " + AmountOfHours + " hour/s");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //TODO: Auditing
            } 
        }

        // Checks amount of hours is valid
        private bool CanSave()
        {
            if (string.IsNullOrWhiteSpace(AmountOfHours) || HasErrors)
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
