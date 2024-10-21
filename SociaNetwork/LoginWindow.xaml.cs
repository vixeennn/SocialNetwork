using DAL;
using DAL.Enteties;
using DAL.Repository;
using DAL.Services;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SociaNetwork
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        UserServices services;
        public LoginWindow()
        {
            InitializeComponent();
            services = new UserServices();

        }
        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
        private void OFF(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void NameLogin_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void LOGIN(object sender, RoutedEventArgs e)
        {
            if (services.CheckPassword(NickName.Text,PasswordLogin.Password.ToString()))
            {

                services.NickNameWrite(NickName.Text);
                MainWindow main = new MainWindow()
                {
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                };
                main.ShowDialog();
                this.Close();
            }
            else
            {
                MessageWindow window = new MessageWindow("Not corect password");
                window.Show();
            }
           
         
        }



        private void Register(object sender, RoutedEventArgs e)
        {
            RegisterWindow main = new RegisterWindow()
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            main.ShowDialog();
            this.Close();

        }
    }
}
