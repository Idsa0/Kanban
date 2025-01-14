﻿using System.Windows;
using IntroSE.Kanban.Frontend.Model;
using IntroSE.Kanban.Frontend.ViewModel;

namespace IntroSE.Kanban.Frontend.View;

public partial class RegisterView : Window
{
    private LoginVM vm;

    internal RegisterView(LoginVM vm)
    {
        InitializeComponent();
        this.vm = vm;
        Grid.DataContext = vm;
    }

    private void BackButton_Click(object sender, RoutedEventArgs e)
    {
        LoginWindow loginWindow = new LoginWindow(vm);
        loginWindow.Show();
        Close();
    }

    private void RegisterButton_Click(object sender, RoutedEventArgs e)
    {
        UserModel? user = vm.Register();
        if (user == null)
        {
            MessageBox.Show(vm.ErrorMessage);
            return;
        }

        MessageBox.Show("Registered successfully");
        UserView uv = new UserView(user);
        uv.Show();
        Close();
    }

    private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
    {
        vm.Password = PasswordBox.Password;
    }
}