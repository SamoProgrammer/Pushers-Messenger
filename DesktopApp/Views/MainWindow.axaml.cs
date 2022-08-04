using Avalonia.Controls;
using System.Net.Http;
using System;
using System.Net.Http.Json;
using DesktopApp.ViewModels;
using Avalonia.Interactivity;
using DesktopApp.DTOs.Account;

namespace DesktopApp.Views
{
    public partial class MainWindow : Window
    {            
        private HttpClient client;
        public MainWindow()
        {
            InitializeComponent();
            client = new HttpClient();
            LoginButton.Click += OnLoginClick;
            OpenRegisterWindow.Click += (s, e) =>
            {
                RegisterWindow rw = new RegisterWindow()
                {
                    DataContext = new RegisterViewModel(),
                };
                rw.BeginInit();
                rw.InitializeComponent();
                rw.Show();
                this.Close();
            };
        }

        private async void OnLoginClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(UsernameInput.Text) || string.IsNullOrEmpty(PasswordInput.Text))
            {
                LoginErrorTextBlock.Text = "لطفا ورودی ها را به طور کامل پر کنید";
                return;
            }
            LoginErrorTextBlock.Text = "درحال پردازش ...";
            LoginResponse response;

            var content = JsonContent.Create<LoginViewModel>(new LoginViewModel()
            {
                UserName = UsernameInput.Text,
                Password = PasswordInput.Text,
            });
            try
            {
                HttpResponseMessage res = await client.PostAsync("https://localhost:5001/api/account/login", content);
                response = await res.Content.ReadFromJsonAsync<LoginResponse>();
                if (!string.IsNullOrEmpty(response.Error))
                {
                    LoginErrorTextBlock.Text = response.Error;
                }
                return;
            }
            catch (HttpRequestException ex)
            {
                LoginErrorTextBlock.Text = "خطا در ارتباط با سرور";
                ClearInputs();
                return;
            }
            catch (Exception ex)
            {
                LoginErrorTextBlock.Text = "خطا در انجام عملیات";
                ClearInputs();
                return;
            }
        }
        private void ClearInputs()
        {
            PasswordInput.Text = "";
            UsernameInput.Text = "";
        }
    }
}
