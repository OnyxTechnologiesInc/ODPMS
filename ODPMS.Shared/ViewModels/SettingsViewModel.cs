﻿using Microsoft.UI.Xaml.Media.Imaging;
using ODPMS.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Windows.Storage;
using CommunityToolkit.WinUI.UI.Controls;
using Windows.Storage.Pickers;
using WinRT.Interop;

namespace ODPMS.ViewModels
{
    public partial class SettingsViewModel : BaseViewModel
    {
        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(IsNotUserSelected))]
        bool isUserSelected;

        public bool IsNotUserSelected => !IsUserSelected;

        [ObservableProperty]
        Visibility visibleState;

        [ObservableProperty]
        string firstName;

        [ObservableProperty]
        string lastName;

        [ObservableProperty]
        string companyName;

        [ObservableProperty]
        string companyAddress;

        [ObservableProperty]
        string companyEmail;

        [ObservableProperty]
        string companyPhone;

        [ObservableProperty]
        BitmapImage companyLogo;

        [ObservableProperty]
        object selectedUser;

        public ObservableCollection<UserViewModel> Users { get; } = new();
        public ObservableCollection<TicketTypeViewModel> TicketTypes { get; } = new();
        public static ApplicationDataContainer LocalSettings = ApplicationData.Current.LocalSettings;

        public SettingsViewModel()
        {
            Title = "Settings";
            Init();
        }

        private void Init()
        {
            if (App.LoggedInUser.UserType == "admin")
            {
                VisibleState = Visibility.Visible;
                var users = DatabaseHelper.GetUsers();

                if (Users.Count != 0)
                    Users.Clear();

                foreach (var user in users)
                    Users.Add(user);

                var ticketTypes = DatabaseHelper.GetTicketTypeList("All");

                if (TicketTypes.Count != 0)
                    TicketTypes.Clear();

                foreach (var ticketType in ticketTypes)
                    TicketTypes.Add(ticketType);
            }
            else
            {
                VisibleState = Visibility.Collapsed;
            }
            FirstName = App.LoggedInUser.FirstName;
            LastName = App.LoggedInUser.LastName;

            GetCompanyData();
        }

        private void GetCompanyData()
        {
            if (LocalSettings.Values["CompanyName"] != null)
                CompanyName = LocalSettings.Values["CompanyName"] as string;

            if (LocalSettings.Values["CompanyAddress"] != null)
                CompanyAddress = LocalSettings.Values["CompanyAddress"] as string;

            if (LocalSettings.Values["CompanyEmail"] != null)
                CompanyEmail = LocalSettings.Values["CompanyEmail"] as string;

            if (LocalSettings.Values["CompanyPhone"] != null)
                CompanyPhone = LocalSettings.Values["CompanyPhone"] as string;

            if (LocalSettings.Values["CompanyLogo"] != null)
            {
                string clogo = LocalSettings.Values["CompanyLogo"] as string;
                if (File.Exists(ApplicationData.Current.LocalFolder.Path + "\\" + clogo))
                {
                    Uri resourceUri = new Uri(ApplicationData.Current.LocalFolder.Path + "\\" + clogo, UriKind.Relative);
                    CompanyLogo = new BitmapImage(resourceUri);
                }
            }
        }

        [ICommand]
        private void UpdateCompany()
        {
            LocalSettings.Values["CompanyName"] = CompanyName;
            LocalSettings.Values["CompanyAddress"] = CompanyAddress;
            LocalSettings.Values["CompanyEmail"] = CompanyEmail;
            LocalSettings.Values["CompanyPhone"] = CompanyPhone;
        }

        [ICommand]
        private void UpdateUser()
        {
            if (FirstName != App.LoggedInUser.FirstName || LastName != App.LoggedInUser.LastName)
            {
                App.LoggedInUser.FirstName = FirstName;
                App.LoggedInUser.LastName = LastName;
                DatabaseHelper.UpdateUser(App.LoggedInUser);

                var users = DatabaseHelper.GetUsers();

                if (Users.Count != 0)
                    Users.Clear();

                foreach (var user in users)
                    Users.Add(user);
            }
        }

        [ICommand]
        async void ResetPassword()
        {
            // Display the reset password dialog
            ContentDialog resetPasswordDialog = new ResetPasswordContentDialog(null);
            resetPasswordDialog.XamlRoot = (Application.Current as App)?.Window.Content.XamlRoot;
            await resetPasswordDialog.ShowAsync();
        }

        [ICommand]
        async void ResetUserPassword()
        {
            // Display the reset password dialog
            ContentDialog resetPasswordDialog = new ResetPasswordContentDialog((SelectedUser as UserViewModel).Id);
            resetPasswordDialog.XamlRoot = (Application.Current as App)?.Window.Content.XamlRoot;
            await resetPasswordDialog.ShowAsync();
        }

        [ICommand]
        async void AddUser()
        {
            // Display the new user dialog
            ContentDialog newUserDialog = new NewUserContentDialog();
            newUserDialog.XamlRoot = (Application.Current as App)?.Window.Content.XamlRoot;
            await newUserDialog.ShowAsync();

            var users = DatabaseHelper.GetUsers();

            if (Users.Count != 0)
                Users.Clear();

            foreach (var user in users)
                Users.Add(user);
        }

        [ICommand]
        async void AddTicketType()
        {
            // Display the new user dialog
            ContentDialog newTicketType = new NewTicketTypeContentDialog();
            newTicketType.XamlRoot = (Application.Current as App)?.Window.Content.XamlRoot;
            await newTicketType.ShowAsync();

            var ticketTypes = DatabaseHelper.GetTicketTypeList("All");

            if (TicketTypes.Count != 0)
                TicketTypes.Clear();

            foreach (var ticketType in ticketTypes)
                TicketTypes.Add(ticketType);
        }

        [ICommand]
        private async void GetCompanyLogo()
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".png");

            Window window = (Application.Current as App)?.Window;
            var hwnd = WindowNative.GetWindowHandle(window);
            InitializeWithWindow.Initialize(openPicker, hwnd);

            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                File.Copy(file.Path, ApplicationData.Current.LocalFolder.Path + "\\clogo" + file.FileType, true);
                Uri resourceUri = new Uri(ApplicationData.Current.LocalFolder.Path + "\\clogo" + file.FileType, UriKind.Relative);
                CompanyLogo = new BitmapImage(resourceUri);

                LocalSettings.Values["CompanyLogo"] = "clogo" + file.FileType;
            }
            else
            {
                //OutputTextBlock.Text = "Operation cancelled.";
            }
        }
    }
}