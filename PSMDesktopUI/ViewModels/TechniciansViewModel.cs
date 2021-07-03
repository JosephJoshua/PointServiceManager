using Caliburn.Micro;
using DevExpress.Xpf.Core;
using PSMDesktopUI.Library.Api;
using PSMDesktopUI.Library.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PSMDesktopUI.ViewModels
{
    public sealed class TechniciansViewModel : Screen
    {
        private readonly ILog _logger;

        private readonly IWindowManager _windowManager;
        private readonly ITechnicianEndpoint _technicianEndpoint;

        private bool _isLoading = false;

        private BindableCollection<TechnicianModel> _technicians;
        private TechnicianModel _selectedTechnician;

        public bool IsLoading
        {
            get => _isLoading;

            set
            {
                _isLoading = value;

                NotifyOfPropertyChange(() => IsLoading);
                NotifyOfPropertyChange(() => CanAddTechnician);
                NotifyOfPropertyChange(() => CanDeleteTechnician);
            }
        }

        public BindableCollection<TechnicianModel> Technicians
        {
            get => _technicians;

            set
            {
                _technicians = value;
                NotifyOfPropertyChange(() => Technicians);
            }
        }

        public TechnicianModel SelectedTechnician
        {
            get => _selectedTechnician;

            set
            {
                _selectedTechnician = value;

                NotifyOfPropertyChange(() => SelectedTechnician);
                NotifyOfPropertyChange(() => CanAddTechnician);
                NotifyOfPropertyChange(() => CanDeleteTechnician);
            }
        }

        public bool CanAddTechnician
        {
            get => !IsLoading;
        }

        public bool CanDeleteTechnician
        {
            get => !IsLoading && SelectedTechnician != null;
        }

        public TechniciansViewModel(IWindowManager windowManager, ITechnicianEndpoint technicianEndpoint)
        {
            DisplayName = "Teknisi";

            _logger = LogManager.GetLog(typeof(TechniciansViewModel));
            _windowManager = windowManager;
            _technicianEndpoint = technicianEndpoint;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            await LoadTechnicians();
        }

        public async Task Search(KeyEventArgs args)
        {
            if ((args.Key == Key.Enter || args.Key == Key.Return) && !IsLoading)
            {
                await LoadTechnicians();
            }
        }

        public async Task AddTechnician()
        {
            if (await _windowManager.ShowDialogAsync(IoC.Get<AddTechnicianViewModel>()) == true)
            {
                await LoadTechnicians();
            }
        }

        public async Task DeleteTechnician()
        {
            if (DXMessageBox.Show("Apakah anda yakin ingin menghapus teknisi ini?", "Teknisi", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    await _technicianEndpoint.Delete(SelectedTechnician.Id);
                }
                catch (ApiException ex)
                {
                    // Check the error message to see if it was a foreign key constraint violation or not
                    if (ex.Message.ToLower().Contains("conflicted with the reference constraint \"fk"))
                    {
                        DXMessageBox.Show("Tidak dapat menghapus teknisi ini karena masih terdapat servisan dengan teknisi ini.", "Teknisi", MessageBoxButton.OK);
                        return;
                    }

                    _logger.Error(ex);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                }

                await LoadTechnicians();
            }
        }

        public async Task LoadTechnicians()
        {
            if (IsLoading) return;

            IsLoading = true;

            List<TechnicianModel> technicianList = await _technicianEndpoint.GetAll();
            Technicians = new BindableCollection<TechnicianModel>(technicianList);

            IsLoading = false;
        }
    }
}
