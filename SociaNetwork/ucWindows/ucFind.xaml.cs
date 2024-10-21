using DAL.Enteties;
using DAL.Repository;
using DAL.Services;
using SociaNetwork.SearchPeople;
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
    /// Interaction logic for ucFind.xaml
    /// </summary>
    public partial class ucFind : UserControl
    {
        User user;
        UserRepository repository;
        UserServices services;
        public ucFind()
        {
            InitializeComponent();
            repository = new UserRepository();
            services = new UserServices();
            user = repository.GetUser(services.NickNameRead());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (services.CheckIsUserInDatabase(PersonNickName.Text))
            {
                PersonInfo main = new PersonInfo(PersonNickName.Text)
                {
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                };
                main.ShowDialog();
            }
            else
            {
                MessageWindow window = new MessageWindow("No such user");
                window.Show();
            }
            


        }
    }
}
