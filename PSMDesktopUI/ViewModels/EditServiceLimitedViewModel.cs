using Caliburn.Micro;
using DevExpress.Xpf.Core;
using PSMDesktopUI.Library.Api;
using PSMDesktopUI.Library.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PSMDesktopUI.ViewModels
{
    public class EditServiceLimitedViewModel : Screen
    {
        private readonly IServiceEndpoint _serviceEndpoint;

        private ServiceModel _oldService;
        private int _nomorNota;

        private string _kerusakan;
        private string _noHp;

        private ServiceStatus _serviceStatuses;
        private ServiceStatus _selectedStatus;

        private bool _sudahKonfirmasi;
        private string _isiKonfirmasi;
        private DateTime _tanggalKonfirmasi;

        private double _dp;
        private double _tambahanBiaya;

        public int NomorNota
        {
            get => _nomorNota;

            set
            {
                _nomorNota = value;
                NotifyOfPropertyChange(() => NomorNota);
            }
        }

        public string Kerusakan
        {
            get => _kerusakan;

            set
            {
                _kerusakan = value;
                NotifyOfPropertyChange(() => Kerusakan);
            }
        }

        public string NoHp
        {
            get => _noHp;

            set
            {
                _noHp = value;
                NotifyOfPropertyChange(() => NoHp);
            }
        }

        public ServiceStatus ServiceStatuses
        {
            get => _serviceStatuses;

            set
            {
                _serviceStatuses = value;
                NotifyOfPropertyChange(() => ServiceStatuses);
            }
        }

        public ServiceStatus SelectedStatus
        {
            get => _selectedStatus;

            set
            {
                _selectedStatus = value;
                NotifyOfPropertyChange(() => SelectedStatus);
            }
        }

        public bool SudahKonfirmasi
        {
            get => _sudahKonfirmasi;

            set
            {
                _sudahKonfirmasi = value;
                NotifyOfPropertyChange(() => SudahKonfirmasi);

                if (SudahKonfirmasi && TanggalKonfirmasi.Year == 1753)
                {
                    TanggalKonfirmasi = DateTime.Now;
                }
            }
        }

        public string IsiKonfirmasi
        {
            get => _isiKonfirmasi;

            set
            {
                _isiKonfirmasi = value;
                NotifyOfPropertyChange(() => IsiKonfirmasi);
            }
        }

        public DateTime TanggalKonfirmasi
        {
            get => _tanggalKonfirmasi;

            set
            {
                _tanggalKonfirmasi = value;
                NotifyOfPropertyChange(() => TanggalKonfirmasi);
            }
        }

        public double Dp
        {
            get => _dp;

            set
            {
                _dp = value;
                NotifyOfPropertyChange(() => Dp);
            }
        }

        public double TambahanBiaya
        {
            get => _tambahanBiaya;

            set
            {
                _tambahanBiaya = value;
                NotifyOfPropertyChange(() => TambahanBiaya);
            }
        }

        public EditServiceLimitedViewModel(IServiceEndpoint serviceEndpoint)
        {
            _serviceEndpoint = serviceEndpoint;
        }

        public void SetFieldValues(ServiceModel service)
        {
            _oldService = service;

            Kerusakan = service.Kerusakan;
            NoHp = service.NoHp;
            NomorNota = service.NomorNota;
            SudahKonfirmasi = service.TanggalKonfirmasi.Year != 1753;
            IsiKonfirmasi = service.IsiKonfirmasi;
            TanggalKonfirmasi = service.TanggalKonfirmasi;
            Dp = (double)service.Dp;
            TambahanBiaya = (double)service.TambahanBiaya;

            SelectedStatus = Enum.GetValues(ServiceStatuses.GetType()).Cast<ServiceStatus>().Where((e) => e.Description() == service.StatusServisan).FirstOrDefault();
        }

        public async Task<bool> UpdateService()
        {
            ServiceStatus oldStatus = Enum.GetValues(ServiceStatuses.GetType()).Cast<ServiceStatus>().Where(e => e.Description() ==
                _oldService.StatusServisan).FirstOrDefault();

            bool wasSudahDiambil = oldStatus == ServiceStatus.JadiSudahDiambil || oldStatus == ServiceStatus.TidakJadiSudahDiambil;
            bool belumDiambil = SelectedStatus == ServiceStatus.JadiBelumDiambil || SelectedStatus == ServiceStatus.TidakJadiBelumDiambil;
            bool tidakJadi = SelectedStatus == ServiceStatus.TidakJadiBelumDiambil || SelectedStatus == ServiceStatus.TidakJadiSudahDiambil;

            if (oldStatus == ServiceStatus.JadiSudahDiambil && tidakJadi)
            {
                DXMessageBox.Show("Can't update service from 'Jadi (Sudah diambil)' to 'Tidak jadi'", "Edit service");
                return false;
            }

            if (wasSudahDiambil && belumDiambil)
            {
                DXMessageBox.Show("Can't update service from 'Sudah diambil' to 'Belum diambil'", "Edit service");
                return false;
            }

            if (tidakJadi && (_oldService.Biaya != 0 || TambahanBiaya != 0))
            {
                if (DXMessageBox.Show(
                    "'Biaya' and 'Tambahan biaya' must be 0 if the service is cancelled. Do you want to set 'Biaya' and 'Tambahan biaya' to be 0?", "Edit service",
                    MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    _oldService.Biaya = 0;
                    TambahanBiaya = 0;
                }
                else
                {
                    return false;
                }
            }

            bool sudahDiambil = SelectedStatus == ServiceStatus.JadiSudahDiambil || SelectedStatus == ServiceStatus.TidakJadiSudahDiambil;

            _oldService.Kerusakan = Kerusakan;
            _oldService.NoHp = NoHp;
            _oldService.StatusServisan = SelectedStatus.Description();
            _oldService.IsiKonfirmasi = SudahKonfirmasi ? IsiKonfirmasi : "";
            _oldService.TanggalKonfirmasi = SudahKonfirmasi ? TanggalKonfirmasi : DateTime.MinValue;
            _oldService.TanggalPengambilan = sudahDiambil ? DateTime.Now : DateTime.MinValue;
            _oldService.Dp = (decimal)Dp;
            _oldService.TambahanBiaya = (decimal)TambahanBiaya;

            await _serviceEndpoint.Update(_oldService);
            return true;
        }

        public async Task Save()
        {
            if (await UpdateService())
            {
                TryClose(true);
            }
        }

        public void Cancel()
        {
            TryClose(false);
        }
    }
}
