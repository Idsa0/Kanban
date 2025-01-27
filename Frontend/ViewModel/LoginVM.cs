﻿using System;
using System.Collections.Generic;
using System.Windows;
using IntroSE.Kanban.Frontend.Model;

namespace IntroSE.Kanban.Frontend.ViewModel
{
    internal class LoginVM : Notifiable
    {
        private BackendController controller;

        public LoginVM(BackendController controller)
        {
            this.controller = controller;
        }

        public LoginVM()
        {
            controller = new BackendController();
        }

        private string errorMessage = "";

        public string ErrorMessage
        {
            get => errorMessage;
            set
            {
                errorMessage = value;
                RaisePropertyChanged("ErrorMessage");
            }
        }

        private string email = "";

        public string Email
        {
            get => email;
            set
            {
                email = value;
                FieldsAreNotEmpty = string.IsNullOrWhiteSpace(value);
            }
        }

        private string password = "";

        public string Password
        {
            get => password;
            set
            {
                password = value;
                FieldsAreNotEmpty = string.IsNullOrWhiteSpace(value);
            }
        }

        internal bool FieldsAreNotEmpty
        {
            get => !(string.IsNullOrWhiteSpace(email) && string.IsNullOrWhiteSpace(password));
            set => RaisePropertyChanged("FieldsAreNotEmpty");
        }

        internal UserModel? Login()
        {
            Tuple<UserModel?, string> t = controller.Login(email, password);
            ErrorMessage = t.Item2;
            return t.Item1;
        }

        internal UserModel? Register()
        {
            Tuple<UserModel?, string> t = controller.Register(email, password);
            ErrorMessage = t.Item2;
            return t.Item1;
        }

        internal List<string> GetUserBoards()
        {
            return controller.GetUserBoards(Email);
        }
    }
}