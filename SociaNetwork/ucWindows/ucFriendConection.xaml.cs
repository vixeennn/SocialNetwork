
using DAL.Enteties;
using DAL.Services;
using SociaNetwork.HelperWindows;
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
    /// Interaction logic for ucFriendConection.xaml
    /// </summary>
    public partial class ucFriendConection : UserControl
    {
        UserServices services;
        public ucFriendConection()
        {
            InitializeComponent();
            services = new UserServices();
            List<Person> users = new List<Person>();
            users = services.GetFriendsOfFriend();
            if (users != null && users.Count > 0)
            {
                foreach (var user in users)
                {
                    Grid grid = new Grid();
                    grid.Margin = new Thickness(0,5,0,5);
                    Label lblInformation = new Label();
                    lblInformation.Content = user.Name + "  " + user.Surname + "\n" + user.Mail + "\n" + user.NickName;
                    lblInformation.Background = Brushes.White;
                    lblInformation.FontFamily = new FontFamily("Nirmala UI Semilight");
                    lblInformation.FontSize = 16;
                    lblInformation.FontWeight = FontWeights.Normal;
                    lblInformation.Padding = new Thickness(5, 10, 5, 10);


                    Button btnFollow = new Button();
                    //btnEdit.Content = MaterialDesignThemes.Wpf.PackIconKind.EditOff;
                    btnFollow.Content = "Follow";
                    btnFollow.Click += btnFollow_Click;
                    btnFollow.Width = 150;
                    btnFollow.Margin = new Thickness(150,40, 0, 0);

                    Button btnViewProfile = new Button();
                    btnViewProfile.Content = "View profile";
                    btnViewProfile.Click += btnViewProfile_Click;
                    btnViewProfile.Width = 150;
                    btnViewProfile.Margin = new Thickness(150, 0, 0, 40);


                    Button btnPath = new Button();
                    btnPath.Content = "View path";
                    btnPath.Click += btnPath_Click;
                    btnPath.Width = 150;
                    btnPath.Margin = new Thickness(500, 0, 0, 0);

                    grid.Children.Add(lblInformation);
                    grid.Children.Add(btnFollow);
                    grid.Children.Add(btnViewProfile);
              
                    grid.Children.Add(btnPath);

                    this.stackpanel.Children.Add(grid);
 

                    void btnFollow_Click(object sender, RoutedEventArgs e)
                    {
                        services.AddFollowing(services.NickNameRead(),user.NickName);
                    }

                    void btnViewProfile_Click(object sender, RoutedEventArgs e)
                    {
                        PersonInfo info = new PersonInfo(user.NickName);
                        info.Show();
                    }

                    void btnPath_Click(object sender, RoutedEventArgs e)
                    {
                      
                        ListOfPersonsWindow window = new ListOfPersonsWindow(services.GetConnectingPaths(user.NickName));
                        window.Show();

                    }

                }
            }
            else
            {
                Label lblInformation = new Label();
                lblInformation.Content = "No Recomendation";
                lblInformation.FontSize = 44;
                lblInformation.VerticalAlignment = VerticalAlignment.Center;
                lblInformation.HorizontalAlignment = HorizontalAlignment.Center;
                this.stackpanel.Children.Add(lblInformation);
            }
        }
    }
}
