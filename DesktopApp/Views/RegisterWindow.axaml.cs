using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using DesktopApp.ViewModels;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Linq;
using DesktopApp.Views.MessageBoxes;
using DesktopApp.DTOs.Account;

namespace DesktopApp.Views
{
    public partial class RegisterWindow : Window
    {
        private HttpClient client;
        public RegisterWindow()
        {
            InitializeComponent();
            client = new HttpClient();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private async void OnRegisterClick(object sender, RoutedEventArgs e)
        {
            foreach (TextBlock view in ErrorBox.Children)
            {
                view.Text = "";
            }
            ((TextBlock)ErrorBox.Children[0]).Text = "درحال پردازش";
            RegisterResponse? response;
            if (string.IsNullOrEmpty(UsernameInput.Text) || string.IsNullOrEmpty(PasswordInput.Text) || string.IsNullOrEmpty(ConfirmPasswordInput.Text) || string.IsNullOrEmpty(EmailInput.Text))
            {
                foreach (TextBlock view in ErrorBox.Children)
                {
                    view.Text = "";
                }
                ((TextBlock)ErrorBox.Children[0]).Text = "لطفا ورودی ها را به طور کامل پر کنید";
                return;
            }
            var content = JsonContent.Create<RegisterViewModel>(new RegisterViewModel()
            {
                UserName = UsernameInput.Text,
                Password = PasswordInput.Text,
                ConfirmPassword = ConfirmPasswordInput.Text,
                Email = EmailInput.Text,
            });
            try
            {
                HttpResponseMessage res = await client.PostAsync("https://localhost:5001/api/account/register", content);
                response = await res.Content.ReadFromJsonAsync<RegisterResponse>();
                if (!response.Success)
                {
                    ClearAll();
                    var errors = response.Errors.Take(4);
                    int index = 0;
                    foreach (var error in errors)
                    {
                        ((TextBlock)ErrorBox.Children[index]).Text = error;
                        index++;
                    }
                    return;
                }
                var LoginWindow = new MainWindow();
                var msgBox = new SuccessRegisteration();
                await msgBox.ShowDialog(this);
                LoginWindow.Show();
                this.Close();
                return;
            }
            catch (HttpRequestException ex)
            {
                ClearAll();
                ((TextBlock)ErrorBox.Children[0]).Text = "خطا در ارتباط با سرور";
                return;
            }
            catch (Exception ex)
            {
                ClearAll();
                ((TextBlock)ErrorBox.Children[0]).Text = "عملیات با خطا مواجه شد";
                return;
            }
        }
        private void ClearAll()
        {
            PasswordInput.Text = "";
            ConfirmPasswordInput.Text = "";
            EmailInput.Text = "";
            UsernameInput.Text = "";

            foreach (TextBlock view in ErrorBox.Children)
            {
                view.Text = "";
            }
        }
    }
}
