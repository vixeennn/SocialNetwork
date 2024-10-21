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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SociaNetwork.ucWindows
{
    /// <summary>
    /// Interaction logic for ucChangePassword.xaml
    /// </summary>
    public partial class ucChangePassword : UserControl
    {

        UserServices services;

        public ucChangePassword()
        {
            InitializeComponent();
            services = new UserServices();
        }

        private void OldPassword_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void NewPassword_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bool temp = services.UpdatePassword(OldPassword.Password,NewPassword.Password);
            if(temp == true)
            {
                MessageWindow window = new MessageWindow("Done successfully");
                window.Show();
            }
            else
            {
                MessageWindow window = new MessageWindow("Ooops some problem, "+"\n"+"check is corect your old password");
                window.Show();
            }
        }
    }
}
