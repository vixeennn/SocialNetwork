using DAL.Enteties;
using DAL.Repository;
using DAL.Services;
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
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        UserServices services;
        public RegisterWindow()
        {
            InitializeComponent();
            services = new UserServices();
        }

        private void LOGIN(object sender, RoutedEventArgs e)
        {
            if(Name.Text != "" && Surname.Text != "" && NickName.Text != "")
            {
                if (services.CheckIndentityOfNickName(NickName.Text))
                {
                    if (Password.Password.ToString() == Password2.Password.ToString() && Password.Password.ToString()!= "")
                    {

                        try
                        {

                            services.InsertUser(Name.Text, Surname.Text, Email.Text, NickName.Text, Password.Password.ToString());
                            services.NickNameWrite(NickName.Text);
                            MainWindow main = new MainWindow()
                            {
                                WindowStartupLocation = WindowStartupLocation.CenterScreen
                            };
                            main.ShowDialog();
                            this.Close();
                        }
                        catch
                        {
                            MessageWindow message = new MessageWindow("Some error");
                            message.Show();
                        }


                    }
                    else
                    {
                        MessageWindow message = new MessageWindow("Password do not match ");
                        message.Show();
                    }
                }
                else
                {
                    MessageWindow message = new MessageWindow("Nick name allready used");
                    message.Show();
                }
               
            }
            else
            {
                MessageWindow message = new MessageWindow("Enter all fields");
                message.Show();
            }
           
            

        }

        private void OFF(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
    }
}
