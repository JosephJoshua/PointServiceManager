using Caliburn.Micro;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using PSMDesktopUI.Library.Api;
using PSMDesktopUI.Library.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PSMDesktopUI.ViewModels
{
    public sealed class ServicesViewModel : Screen
    {
        private readonly IWindowManager _windowManager;
        private readonly IApiHelper _apiHelper;

        private readonly IServiceEndpoint _serviceEndpoint;
        private readonly ITechnicianEndpoint _technicianEndpoint;
        private readonly ISalesEndpoint _salesEndpoint;
        private readonly ISparepartEndpoint _sparepartEndpoint;

        private bool _isLoading = false;

        private BindableCollection<ServiceModel> _services;
        private ServiceModel _selectedService;

        private SparepartModel _selectedSparepart;

        private string _searchText;
        private SearchType _searchTypes;
        private SearchType _selectedSearchType;

        private DateTime _startDate;
        private DateTime _endDate;

        public delegate void OnRefreshEventHandler();
        public event OnRefreshEventHandler OnRefresh;

        public bool IsLoading
        {
            get => _isLoading;

            set
            {
                _isLoading = value;

                NotifyOfPropertyChange(() => IsLoading);
                NotifyOfPropertyChange(() => CanAddService);
                NotifyOfPropertyChange(() => CanAddSparepart);
                NotifyOfPropertyChange(() => CanEditService);
                NotifyOfPropertyChange(() => CanDeleteService);
                NotifyOfPropertyChange(() => CanPrintService);
            }
        }

        public BindableCollection<ServiceModel> Services
        {
            get => _services;

            set
            {
                _services = value;
                NotifyOfPropertyChange(() => Services);
            }
        }

        public ServiceModel SelectedService
        {
            get => _selectedService;

            set
            {
                _selectedService = value;

                NotifyOfPropertyChange(() => SelectedService);
                NotifyOfPropertyChange(() => SelectedServiceTechnician);
                NotifyOfPropertyChange(() => SelectedServiceSales);
                NotifyOfPropertyChange(() => CanAddSparepart);
                NotifyOfPropertyChange(() => CanEditService);
                NotifyOfPropertyChange(() => CanDeleteService);
                NotifyOfPropertyChange(() => CanPrintService);
                NotifyOfPropertyChange(() => ShowInfo);
            }
        }

        public string SelectedServiceTechnician
        {
            get
            {
                if (SelectedService == null) return null;

                return _technicianEndpoint.GetById(SelectedService.TechnicianId).Result.Nama;
            }
        }

        public string SelectedServiceSales
        {
            get
            {
                if (SelectedService == null) return null;

                return _salesEndpoint.GetById(SelectedService.SalesId).Result.Nama;
            }
        }

        public SparepartModel SelectedSparepart
        {
            get => _selectedSparepart;

            set
            {
                _selectedSparepart = value;

                NotifyOfPropertyChange(() => SelectedSparepart);
                NotifyOfPropertyChange(() => CanAddSparepart);
                NotifyOfPropertyChange(() => CanDeleteSparepart);
            }
        }

        public string SearchText
        {
            get => _searchText;

            set
            {
                _searchText = value;
                NotifyOfPropertyChange(() => SearchText);
            }
        }

        public SearchType SearchTypes
        {
            get => _searchTypes;

            set
            {
                _searchTypes = value;
                NotifyOfPropertyChange(() => SearchTypes);
            }
        }

        public SearchType SelectedSearchType
        {
            get => _selectedSearchType;

            set
            {
                _selectedSearchType = value;
                NotifyOfPropertyChange(() => SelectedSearchType);
            }
        }

        public DateTime StartDate
        {
            get => _startDate;

            set
            {
                _startDate = value;
                NotifyOfPropertyChange(() => StartDate);
            }
        }

        public DateTime EndDate
        {
            get => _endDate;

            set
            {
                _endDate = value;
                NotifyOfPropertyChange(() => EndDate);
            }
        }

        public bool CanAddService
        {
            get => !IsBuyer && !IsLoading;
        }

        public bool CanAddSparepart
        {
            get => !IsCustomerService && !IsLoading && (SelectedService != null || SelectedSparepart != null);
        }

        public bool CanEditService
        {
            get => !IsBuyer && !IsLoading && SelectedService != null;
        }

        public bool CanDeleteService
        {
            get => IsAdmin && !IsLoading && SelectedService != null;
        }

        public bool CanDeleteSparepart
        {
            get => IsAdmin && !IsLoading && SelectedSparepart != null;
        }

        public bool CanPrintService
        {
            get => !IsBuyer && !IsLoading && SelectedService != null;
        }

        public bool ShowInfo
        {
            get => SelectedService != null;
        }

        public bool IsAdmin
        {
            get => _apiHelper.LoggedInUser.Role == "Admin";
        }

        public bool IsCustomerService
        {
            get => _apiHelper.LoggedInUser.Role == "Customer Service";
        }

        public bool IsBuyer
        {
            get => _apiHelper.LoggedInUser.Role == "Buyer";
        }

        public ServicesViewModel(IApiHelper apiHelper, IWindowManager windowManager, IServiceEndpoint serviceEndpoint,
                                 ITechnicianEndpoint technicianEndpoint, ISalesEndpoint salesEndpoint,
                                 ISparepartEndpoint sparepartEndpoint)
        {
            DisplayName = "Servisan";

            _windowManager = windowManager;
            _apiHelper = apiHelper;
            _serviceEndpoint = serviceEndpoint;
            _technicianEndpoint = technicianEndpoint;
            _salesEndpoint = salesEndpoint;
            _sparepartEndpoint = sparepartEndpoint;

            DateTime today = DateTime.Today;

            // Set start and end date to the start and end of the month, respectively.
            _startDate = new DateTime(today.Year, today.Month, 1);
            _endDate = _startDate.AddMonths(1).AddTicks(-1);
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            await LoadServices();
        }

        public async Task OnMasterRowExpanding(RowAllowEventArgs args)
        {
            ServiceModel service = (ServiceModel)args.Row;

            if (service.Spareparts == null)
            {
                service.Spareparts = await _sparepartEndpoint.GetByNomorNota(service.NomorNota);
            }
        }

        public async Task Search(KeyEventArgs args)
        {
            if ((args.Key == Key.Enter || args.Key == Key.Return) && !IsLoading)
            {
                await LoadServices();
            }
        }

        public async Task AddService()
        {
            AddServiceViewModel addServiceVM = IoC.Get<AddServiceViewModel>();

            if (await _windowManager.ShowDialogAsync(addServiceVM) == true)
            {
                if (addServiceVM.NomorNota != -1 &&
                    DXMessageBox.Show("Apakah anda ingin mencetak servisan ini?", "Tambah servisan", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    ServiceModel newService = await _serviceEndpoint.GetByNomorNota(addServiceVM.NomorNota);
                    await PrintService(newService);
                }

                await LoadServices();
            }
        }

        public async Task AddSparepart()
        {
            int nomorNota = SelectedService != null ? SelectedService.NomorNota : SelectedSparepart.NomorNota;

            AddSparepartViewModel addSparepartVM = IoC.Get<AddSparepartViewModel>();
            addSparepartVM.SetNomorNota(nomorNota);

            if (await _windowManager.ShowDialogAsync(addSparepartVM) == true)
            {
                await LoadServices();
            }
        }

        public async Task EditService()
        {
            ServiceModel service = SelectedService;

            EditServiceViewModel editServiceVM = IoC.Get<EditServiceViewModel>();
            editServiceVM.SetFieldValues(service);

            if (await _windowManager.ShowDialogAsync(editServiceVM) == true)
            {
                await LoadServices();
            }
        }

        public async Task EditServiceLimited()
        {
            ServiceModel service = SelectedService;

            EditServiceLimitedViewModel editServiceVM = IoC.Get<EditServiceLimitedViewModel>();
            editServiceVM.SetFieldValues(service);

            if (await _windowManager.ShowDialogAsync(editServiceVM) == true)
            {
                await LoadServices();
            }
        }

        public async Task DeleteService()
        {
            if (DXMessageBox.Show("Apakah anda yakin ingin menghapus servisan ini?", "Servisan", MessageBoxButton.YesNo) == MessageBoxResult.Yes &&
                DXMessageBox.Show("Apakah anda YAKIN ingin menghapus servisan ini?", "Servisan", MessageBoxButton.YesNo) == MessageBoxResult.Yes &&
                DXMessageBox.Show("Apakah anda SANGAT YAKIN ingin menghapus servisan ini?", "Servisan", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                await _serviceEndpoint.Delete(SelectedService.NomorNota);
                await LoadServices();
            }
        }

        public async Task DeleteSparepart()
        {
            if (DXMessageBox.Show("Apakah anda yakin ingin menghapus sparepart ini?", "Servisan", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                await _sparepartEndpoint.Delete(SelectedSparepart.Id);
                await LoadServices();
            }
        }

        public async Task LoadServices()
        {
            if (IsLoading) return;

            IsLoading = true;

            // Make sure the start date's time is set to the start of the day (at 00:00:00).
            StartDate = StartDate.Date;

            // Make sure the end date's time is set to the end of the day (at 23:59:59).
            EndDate = EndDate.Date.AddDays(1).AddTicks(-1);

            List<ServiceModel> serviceList = await _serviceEndpoint.GetAll();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                string searchText = SearchText.ToLower();

                switch (SelectedSearchType)
                {
                    case SearchType.NomorNota:
                        serviceList = serviceList.Where(s => s.NomorNota.ToString().ToLower().Contains(searchText)).ToList();
                        break;

                    case SearchType.NamaPelanggan:
                        serviceList = serviceList.Where(s => s.NamaPelanggan.ToLower().Contains(searchText)).ToList();
                        break;

                    case SearchType.NomorHp:
                        serviceList = serviceList.Where(s => s.NoHp.ToLower().Contains(searchText)).ToList();
                        break;

                    case SearchType.TipeHp:
                        serviceList = serviceList.Where(s => s.TipeHp.ToLower().Contains(searchText)).ToList();
                        break;

                    case SearchType.Status:
                        serviceList = serviceList.Where(s => s.StatusServisan.ToLower().Contains(searchText)).ToList();
                        break;
                }
            }
            else
            {
                // Only filter servisan based on the date range if the user isn't searching.
                serviceList = serviceList.Where(s => s.Tanggal >= StartDate && s.Tanggal <= EndDate).ToList();
            }

            // Don't unnecessarily update the grid if the collections are the same.
            var serviceCollection = new BindableCollection<ServiceModel>(serviceList);
            if (Services != null && serviceCollection.SequenceEqual(Services))
            {
                IsLoading = false;
                return;
            }

            Services = serviceCollection;
            IsLoading = false;

            OnRefresh?.Invoke();
        }

        public async Task PrintSelectedService()
        {
            if (SelectedService == null) return;
            await PrintService(SelectedService);
        }

        private async Task PrintService(ServiceModel service)
        {
            string kelengkapan = service.Kelengkapan?.Trim().Replace(" ", ", ");

            if (!string.IsNullOrWhiteSpace(kelengkapan) && kelengkapan.Length > 1)
            {
                kelengkapan = kelengkapan[0].ToString() + kelengkapan.Substring(1).ToLower();

                // Make "SIM" uppercase so it looks better; there's probably a more efficient way of doing this but I don't care.
                for (int i = 0; i < kelengkapan.Length; i++)
                {
                    if (kelengkapan[i].ToString().ToLower() == "s" && kelengkapan.Length - i - 1 >= 2 && kelengkapan[i + 1] == 'i' &&
                        kelengkapan[i + 2] == 'm')
                    {
                        kelengkapan = kelengkapan.Substring(0, i) + "SIM" + kelengkapan.Substring(i + 3);
                        break;
                    }
                }
            }

            ServiceInvoicePreviewViewModel invoicePreviewVM = IoC.Get<ServiceInvoicePreviewViewModel>();
            invoicePreviewVM.SetInvoiceModel(new ServiceInvoiceModel
            {
                NomorNota = service.NomorNota.ToString(),
                NamaPelanggan = service.NamaPelanggan,
                NoHp = service.NoHp,
                TipeHp = service.TipeHp,
                Imei = service.Imei,
                Kerusakan = service.Kerusakan,
                TotalBiaya = service.TotalBiaya,
                Dp = service.Dp,
                Sisa = service.Sisa,
                Kelengkapan = kelengkapan,
                KondisiHp = service.KondisiHp,
                YangBelumDicek = service.YangBelumDicek,
                Tanggal = service.Tanggal.ToString(DateTimeFormatInfo.CurrentInfo.LongDatePattern),
            });

            await _windowManager.ShowDialogAsync(invoicePreviewVM);
        }
    }
}
