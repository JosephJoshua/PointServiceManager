﻿using Caliburn.Micro;
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
    public sealed class SalesViewModel : Screen
    {
        private readonly ILog _logger;

        private readonly IWindowManager _windowManager;
        private readonly ISalesEndpoint _salesEndpoint;

        private bool _isLoading = false;

        private BindableCollection<SalesModel> _sales;
        private SalesModel _selectedSales;

        public bool IsLoading
        {
            get => _isLoading;

            set
            {
                _isLoading = value;

                NotifyOfPropertyChange(() => IsLoading);
                NotifyOfPropertyChange(() => CanAddSales);
                NotifyOfPropertyChange(() => CanDeleteSales);
            }
        }

        public BindableCollection<SalesModel> Sales
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
                NotifyOfPropertyChange(() => CanAddSales);
                NotifyOfPropertyChange(() => CanDeleteSales);
            }
        }

        public bool CanAddSales
        {
            get => !IsLoading;
        }

        public bool CanDeleteSales
        {
            get => !IsLoading && SelectedSales != null;
        }

        public SalesViewModel(IWindowManager windowManager, ISalesEndpoint salesEndpoint)
        {
            DisplayName = "Sales";

            _logger = LogManager.GetLog(typeof(SalesViewModel));
            _windowManager = windowManager;
            _salesEndpoint = salesEndpoint;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            await LoadSales();
        }

        public async Task Search(KeyEventArgs args)
        {
            if ((args.Key == Key.Enter || args.Key == Key.Return) && !IsLoading)
            {
                await LoadSales();
            }
        }

        public async Task AddSales()
        {
            if (await _windowManager.ShowDialogAsync(IoC.Get<AddSalesViewModel>()) == true)
            {
                await LoadSales();
            }
        }

        public async Task DeleteSales()
        {
            if (DXMessageBox.Show("Apakah anda yakin ingin menghapus sales ini?", "Sales", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    await _salesEndpoint.Delete(SelectedSales.Id);
                }
                catch (ApiException ex)
                {
                    // Check the error message to see if it was a foreign key constraint violation or not
                    if (ex.Message.ToLower().Contains("conflicted with the reference constraint \"fk"))
                    {
                        DXMessageBox.Show("Tidak dapat menghapus sales ini karena masih terdapat servisan dengan sales ini.", "Sales", MessageBoxButton.OK);
                        return;
                    }

                    _logger.Error(ex);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);                    
                }
                
                await LoadSales();
            }
        }

        public async Task LoadSales()
        {
            if (IsLoading) return;

            IsLoading = true;

            List<SalesModel> salesList = await _salesEndpoint.GetAll();
            Sales = new BindableCollection<SalesModel>(salesList);

            IsLoading = false;
        }
    }
}
