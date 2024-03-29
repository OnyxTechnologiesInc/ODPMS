﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI;
using ODPMS.Models;
using ODPMS.Helpers;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ODPMS.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
            Window window = (Application.Current as App)?.Window;
            window.ExtendsContentIntoTitleBar = true;
            window.SetTitleBar(appTitleBar_grid);
        }

        private void Login_Clicked(object sender, RoutedEventArgs e)
        {
            App.IsUserLoggedIn = Login(username_txt.Text, password_txt.Password);
            if (App.IsUserLoggedIn)
            {
                Frame rootFrame = new Frame();
                rootFrame.Navigate(typeof(MainPage));
                Window window = (Application.Current as App)?.Window;
                window.Content = rootFrame;
            } else
            {
                statusMessage_txtBlock.Foreground = new SolidColorBrush(Colors.Red);
                statusMessage_txtBlock.Text = "Invalid login information. Please try again";
            }
        }

        private void Password_Changed(object sender, RoutedEventArgs e)
        {
            statusMessage_txtBlock.Foreground = new SolidColorBrush(Colors.Black);
            statusMessage_txtBlock.Text = "";
        }

        private void Key_Pressed(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                Login_Clicked(sender, e);
            }
        }

        private bool Login(string username, string password)
        {
            List<User> users = DatabaseHelper.UserLogin(username, password);            

            if (users.Count > 0)
            {
                App.LoggedInUser = users[0];
                return true;
            } else
            {
                return false;
            }
        }
    }
}
