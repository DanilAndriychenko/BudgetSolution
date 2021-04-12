using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Windows;
using AV.ProgrammingWithCSharp.Budgets.GUI.WPF.Navigation;
using BudgetsWPF.Navigation;
using Models;
using Prism.Commands;
using Services;

namespace BudgetsWPF.Authentication
{
    public class SignUpViewModel : INotifyPropertyChanged, INavigatable<AuthNavigatableTypes>
    {
        private const int MinNumOfSymbols = 3;
        private RegistrationUser _regUser = new RegistrationUser();
        private readonly Action _gotoSignIn;
        private static EmailAddressAttribute _emailValidator = new EmailAddressAttribute();

        public AuthNavigatableTypes Type => AuthNavigatableTypes.SignUp;

        public string Login
        {
            get => _regUser.Login;
            set
            {
                if (_regUser.Login == value) return;
                _regUser.Login = value;
                OnPropertyChanged();
                SignUpCommand.RaiseCanExecuteChanged();
            }
        }

        public string Password
        {
            get => _regUser.Password;
            set
            {
                if (_regUser.Password == value) return;
                _regUser.Password = value;
                OnPropertyChanged();
                SignUpCommand.RaiseCanExecuteChanged();
            }
        }

        public string FirstName
        {
            get => _regUser.FirstName;
            set
            {
                if (_regUser.FirstName == value) return;
                _regUser.FirstName = value;
                OnPropertyChanged();
                SignUpCommand.RaiseCanExecuteChanged();
            }
        }

        public string LastName
        {
            get => _regUser.LastName;
            set
            {
                if (_regUser.LastName != value)
                {
                    _regUser.LastName = value;
                    OnPropertyChanged();
                    SignUpCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string Email
        {
            get => _regUser.Email;
            set
            {
                if (_regUser.Email == value) return;
                _regUser.Email = value;
                OnPropertyChanged();
                SignUpCommand.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand SignUpCommand { get; }
        public DelegateCommand CloseCommand { get; }
        public DelegateCommand SignInCommand { get; }

        public SignUpViewModel(Action gotoSignIn)
        {
            SignUpCommand = new DelegateCommand(SignUp, IsSignUpEnabled);
            CloseCommand = new DelegateCommand(() => Environment.Exit(0));
            _gotoSignIn = gotoSignIn;
            SignInCommand = new DelegateCommand(_gotoSignIn);
        }

        private async void SignUp()
        {
            var authService = new AuthenticationService();
            try
            {
                await authService.RegisterUserAsync(_regUser);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Sign In failed: {ex.Message}");
                return;
            }

            MessageBox.Show($"User successfully registered, please Sign In");
            _gotoSignIn.Invoke();
        }

        private bool IsSignUpEnabled()
        {
            return !String.IsNullOrWhiteSpace(Login) && !String.IsNullOrWhiteSpace(Password) &&
                   !String.IsNullOrWhiteSpace(LastName) && Login.Length >= MinNumOfSymbols &&
                   FirstName.Length >= MinNumOfSymbols && LastName.Length >= MinNumOfSymbols && (new EmailAddressAttribute().IsValid(Email));
        }

        public void ClearSensitiveData()
        {
            _regUser = new RegistrationUser();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}