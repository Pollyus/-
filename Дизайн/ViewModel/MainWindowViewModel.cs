﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using BAL.Interfaces;
using BBL.Interfaces;
using BLL.Models;
using View.Util;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Дизайн.ViewModel;

namespace View.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly IDbCrud _crud;
        private readonly ICategoryService _categoryService;
        private readonly ICatalogService _catalogService;
        private readonly IDialogService _dialogService;
        private readonly IOrderService _orderService;
        private readonly IProfileService _profileService;
        private readonly IFileService _fileService;

        public MainWindowViewModel(IDbCrud dbCrud, ICategoryService categoryService, ICatalogService productCatalogService, IOrderService orderService, IDialogService dialogService, IProfileService profileService, int userId, IFileService fileService)
        {
            _crud = dbCrud;
            _categoryService = categoryService;
            _catalogService = productCatalogService;
            _dialogService = dialogService;
            _orderService = orderService;
            _profileService = profileService;
            _fileService = fileService;

            //fileService.PrintCheque(1070);

            CatalogVM = new CatalogViewModel(dbCrud, categoryService, productCatalogService, dialogService, userId);
            CartVM = new CartViewModel(dbCrud, categoryService, productCatalogService, dialogService, orderService, profileService, userId, fileService);
            ProfileVM = new ProfileViewModel(dbCrud, categoryService, productCatalogService, dialogService, profileService, userId);

            CartVM.OrderCreated += CatalogVM.Update;
            CartVM.OrderCreated += ProfileVM.OrdersVM.Update;
            CartVM.OrderCreated += ProfileVM.ProfileVM.Update;
            CartVM.OrderCreated += ProfileVM.StatsVM.Update;
            CartVM.OrderCreated += ProfileVM.SalesVM.Update;

        }

        public ProfileViewModel ProfileVM { get; set; }
        public CatalogViewModel CatalogVM { get; set; }
        public CartViewModel CartVM { get; set; }

        


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}