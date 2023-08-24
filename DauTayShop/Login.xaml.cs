using DauTayShop.Models;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Linq;
using System.Windows;

namespace DauTayShop
{
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }


        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string userName = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            using (var context = new PRN221_ProjectContext())
            {
                var user = context.Accounts.FirstOrDefault(a => a.UserName == userName && a.Password == password);
                if (user != null)
                {
                    if (user.Type == 1)
                    {
                        // Đăng nhập thành công cho admin
                        AdminManager adminWindow = new AdminManager();
                        this.Hide();
                        adminWindow.ShowDialog();
                    }
                    else if (user.Type == 0)
                    {
                        // Đăng nhập thành công cho staff
                        fTableManager fTableWindow = new fTableManager();
                        this.Hide();
                        fTableWindow.ShowDialog();
                    }
                }
                else
                {
                    // Hiển thị thông báo lỗi đăng nhập
                    MessageBox.Show("Invalid username or password.", "Login Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            }

            private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure to exit?", "Exit", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown(); //Close app
            }
        }
    }
}
