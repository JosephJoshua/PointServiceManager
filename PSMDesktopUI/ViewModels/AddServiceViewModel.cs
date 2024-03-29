﻿using Caliburn.Micro;
using DevExpress.Xpf.Core;
using PSMDesktopUI.Library.Api;
using PSMDesktopUI.Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace PSMDesktopUI.ViewModels
{
    public class AddServiceViewModel : Screen
    {
        private readonly ISalesEndpoint _salesEndpoint;
        private readonly IServiceEndpoint _serviceEndpoint;
        private readonly ITechnicianEndpoint _technicianEndpoint;

        private bool _showSalesGrid = true;
        private bool _isSalesLoading = false;

        private BindingList<SalesModel> _sales;
        private SalesModel _selectedSales;

        private int _nomorNota = -1;

        private string _namaPelanggan;
        private string _noHp;
        private string _tipeHp;
        private string _imei;
        private string _kerusakan;
        private string _yangBelumDicek;
        private string _warna;
        private string _kataSandiPola;
        private string _kondisiHp;
        private string _isiKonfirmasi;

        private int _salesId;

        private double _biaya;
        private int _discount;
        private double _dp;
        private double _tambahanBiaya;

        private bool _isBatteryChecked = false;
        private bool _isSimChecked = false;
        private bool _isMemoryChecked = false;
        private bool _isCondomChecked = false;

        private DateTime _tanggalKonfirmasi = DateTime.Now;
        private bool _sudahKonfirmasi = false;

        private BindingList<TechnicianModel> _technicians;
        private ServiceStatus _serviceStatuses;

        private TechnicianModel _selectedTechnician;
        private ServiceStatus _selectedStatus;

        public bool ShowSalesGrid
        {
            get => _showSalesGrid;

            set
            {
                _showSalesGrid = value;
                NotifyOfPropertyChange(() => ShowSalesGrid);
            }
        }

        public bool IsSalesLoading
        {
            get => _isSalesLoading;

            set
            {
                _isSalesLoading = value;
                NotifyOfPropertyChange(() => IsSalesLoading);
            }
        }

        public BindingList<SalesModel> Sales
        {
            get => _sales;

            set
            {
                _sales = value;
                NotifyOfPropertyChange(() => Sales);
            }
        }

        public SalesModel SelectedSales
        {
            get => _selectedSales;

            set
            {
                _selectedSales = value;
                NotifyOfPropertyChange(() => SelectedSales);
                NotifyOfPropertyChange(() => HasSelectedSales);
            }
        }

        public bool HasSelectedSales
        {
            get => SelectedSales != null;
        }

        public int NomorNota
        {
            get => _nomorNota;

            private set
            {
                _nomorNota = value;
            }
        }

        public string NamaPelanggan
        {
            get => _namaPelanggan;

            set
            {
                _namaPelanggan = value;

                NotifyOfPropertyChange(() => NamaPelanggan);
                NotifyOfPropertyChange(() => CanAdd);
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

        public string TipeHp
        {
            get => _tipeHp;

            set
            {
                _tipeHp = value;

                NotifyOfPropertyChange(() => TipeHp);
                NotifyOfPropertyChange(() => CanAdd);
            }
        }

        public string Imei
        {
            get => _imei;

            set
            {
                _imei = value;
                NotifyOfPropertyChange(() => Imei);
            }
        }

        public string Kerusakan
        {
            get => _kerusakan;

            set
            {
                _kerusakan = value;

                NotifyOfPropertyChange(() => Kerusakan);
                NotifyOfPropertyChange(() => CanAdd);
            }
        }

        public string YangBelumDicek
        {
            get => _yangBelumDicek;

            set
            {
                _yangBelumDicek = value;
                NotifyOfPropertyChange(() => YangBelumDicek);
            }
        }

        public string Warna
        {
            get => _warna;

            set
            {
                _warna = value;
                NotifyOfPropertyChange(() => Warna);
            }
        }

        public string KataSandiPola
        {
            get => _kataSandiPola;

            set
            {
                _kataSandiPola = value;
                NotifyOfPropertyChange(() => KataSandiPola);
            }
        }

        public string KondisiHp
        {
            get => _kondisiHp;

            set
            {
                _kondisiHp = value;
                NotifyOfPropertyChange(() => KondisiHp);
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

        public int SalesId
        {
            get => _salesId;

            set
            {
                _salesId = value;
                NotifyOfPropertyChange(() => SalesId);
            }
        }

        public double Biaya
        {
            get => _biaya;

            set
            {
                _biaya = value;

                NotifyOfPropertyChange(() => Biaya);
                NotifyOfPropertyChange(() => TotalBiaya);
                NotifyOfPropertyChange(() => Sisa);
            }
        }

        public int Discount
        {
            get => _discount;

            set
            {
                _discount = value;

                NotifyOfPropertyChange(() => Discount);
                NotifyOfPropertyChange(() => TotalBiaya);
                NotifyOfPropertyChange(() => Sisa);
            }
        }

        public double Dp
        {
            get => _dp;

            set
            {
                _dp = value;

                NotifyOfPropertyChange(() => Dp);
                NotifyOfPropertyChange(() => Sisa);
            }
        }

        public double TambahanBiaya
        {
            get => _tambahanBiaya;

            set
            {
                _tambahanBiaya = value;

                NotifyOfPropertyChange(() => TambahanBiaya);
                NotifyOfPropertyChange(() => TotalBiaya);
                NotifyOfPropertyChange(() => Sisa);
            }
        }

        public double TotalBiaya
        {
            get => (Biaya - (Biaya * ((double)Discount / 100))) + TambahanBiaya;
        }

        public double Sisa
        {
            get => TotalBiaya - Dp;
        }

        public bool IsBatteryChecked
        {
            get => _isBatteryChecked;

            set
            {
                _isBatteryChecked = value;
                NotifyOfPropertyChange(() => IsBatteryChecked);
            }
        }

        public bool IsSimChecked
        {
            get => _isSimChecked;

            set
            {
                _isSimChecked = value;
                NotifyOfPropertyChange(() => IsSimChecked);
            }
        }

        public bool IsMemoryChecked
        {
            get => _isMemoryChecked;

            set
            {
                _isMemoryChecked = value;
                NotifyOfPropertyChange(() => IsMemoryChecked);
            }
        }

        public bool IsCondomChecked
        {
            get => _isCondomChecked;

            set
            {
                _isCondomChecked = value;
                NotifyOfPropertyChange(() => IsCondomChecked);
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

        public bool SudahKonfirmasi
        {
            get => _sudahKonfirmasi;

            set
            {
                _sudahKonfirmasi = value;
                NotifyOfPropertyChange(() => SudahKonfirmasi);
            }
        }

        public BindingList<TechnicianModel> Technicians
        {
            get => _technicians;

            set
            {
                _technicians = value;
                NotifyOfPropertyChange(() => Technicians);
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

        public TechnicianModel SelectedTechnician
        {
            get => _selectedTechnician;

            set
            {
                _selectedTechnician = value;
                NotifyOfPropertyChange(() => SelectedTechnician);
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

        public bool CanAdd
        {
            get => !string.IsNullOrWhiteSpace(NamaPelanggan) && !string.IsNullOrWhiteSpace(TipeHp) && !string.IsNullOrEmpty(Kerusakan);
        }

        public AddServiceViewModel(ISalesEndpoint salesEndpoint, ITechnicianEndpoint technicianEndpoint, IServiceEndpoint serviceEndpoint)
        {
            _salesEndpoint = salesEndpoint;
            _serviceEndpoint = serviceEndpoint;
            _technicianEndpoint = technicianEndpoint;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            await LoadSales();
            await GetTechnicians();

            if (Technicians.Count > 0)
            {
                SelectedTechnician = Technicians[0];
            }

            NotifyOfPropertyChange(() => SelectedSales);
        }

        public async Task GetTechnicians()
        {
            List<TechnicianModel> technicianList = await _technicianEndpoint.GetAll();
            Technicians = new BindingList<TechnicianModel>(technicianList);
        }

        public void ConfirmSales()
        {
            ShowSalesGrid = false;
            SalesId = SelectedSales.Id;
        }

        public async Task Add()
        {
            if (await AddService())
            {
                await TryCloseAsync(true);
            }
        }

        public async Task Cancel()
        {
            await TryCloseAsync(false);
        }

        public async Task LoadSales()
        {
            if (IsSalesLoading) return;

            IsSalesLoading = true;
            List<SalesModel> salesList = await _salesEndpoint.GetAll();

            IsSalesLoading = false;
            Sales = new BindingList<SalesModel>(salesList);
        }

        public async Task<bool> AddService()
        {
            if (Discount < 0 || Discount > 100)
            {
                DXMessageBox.Show("Discount harus di antara 0 sampai 100", "Tambah servisan");
                return false;
            }

            if ((SelectedStatus == ServiceStatus.TidakJadiSudahDiambil || SelectedStatus == ServiceStatus.TidakJadiBelumDiambil) && Biaya != 0)
            {
                DXMessageBox.Show("Biaya harus 0 jika servisan ini ingin dibatalkan. Tolong ubah biaya menjadi 0", "Tambah servisan");
                return false;
            }

            string kelengkapan = "";

            if (IsBatteryChecked)
            {
                kelengkapan += "Battery ";
            }

            if (IsSimChecked)
            {
                kelengkapan += "SIM ";
            }

            if (IsMemoryChecked)
            {
                kelengkapan += "Memory ";
            }

            if (IsCondomChecked)
            {
                kelengkapan += "Condom ";
            }

            bool sudahDiambil = SelectedStatus == ServiceStatus.JadiSudahDiambil || SelectedStatus == ServiceStatus.TidakJadiSudahDiambil;

            ServiceModel service = new ServiceModel
            {
                NamaPelanggan = NamaPelanggan,
                NoHp = NoHp,
                TipeHp = TipeHp,
                Imei = Imei,
                Kerusakan = Kerusakan,
                KondisiHp = KondisiHp,
                YangBelumDicek = YangBelumDicek,
                Kelengkapan = kelengkapan,
                Warna = Warna,
                KataSandiPola = KataSandiPola,
                TechnicianId = SelectedTechnician.Id,
                SalesId = SalesId,
                StatusServisan = SelectedStatus.Description(),
                TanggalKonfirmasi = SudahKonfirmasi ? TanggalKonfirmasi : DateTime.MinValue,
                IsiKonfirmasi = SudahKonfirmasi ? IsiKonfirmasi : "",
                Biaya = (decimal)Biaya,
                Discount = Discount,
                Dp = (decimal)Dp,
                TambahanBiaya = (decimal)TambahanBiaya,
                HargaSparepart = 0,
                LabaRugi = 0,
                TanggalPengambilan = sudahDiambil ? DateTime.Now : DateTime.MinValue,
            };

            NomorNota = await _serviceEndpoint.Insert(service);
            return true;
        }
    }
}
