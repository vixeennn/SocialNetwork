using DAL.Enteties;
using DAL.Repository;
using DAL.Services;
using SociaNetwork.HelperWindows;
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

namespace SociaNetwork.SearchPeople
{
    /// <summary>
    /// Interaction logic for PersonInfo.xaml
    /// </summary>
    public partial class PersonInfo : Window
    {
     
        string PersonNickName;
        UserRepository repository;
        PostServices postServices;
        UserServices services;
        User user;
        List<Post> posts;
        bool isAnyPosts = false;
        bool tempLike = false;
        bool tempDislike = false;
        Post currentPost;
        int indexOfPost = 0;
        public PersonInfo(string nickname)
        {
            InitializeComponent();
            PersonNickName = nickname;
            //
            repository = new UserRepository();
            services = new UserServices();
            postServices = new PostServices();
 
            //
            user = new User();
            user = repository.GetUser(PersonNickName);
            UserName.Content = user.Name;
            UserSurname.Content = user.Surname;
            UserMail.Content = user.Mail;
            btnConnection.Content = services.GetConnectingPathsNumber(PersonNickName);
            if(btnConnection.Content == " ")
            {
                btnConnection.Content = "No connection";
            }
            //
            currentPost = new Post();
            posts = new List<Post>();
            posts = postServices.GetPosts(PersonNickName);
            if (posts != null && posts.Count > 0)
            {
                currentPost = posts[indexOfPost];
                Main.Content = currentPost.Text;
                isAnyPosts = true;
            }
            else
            {
                Main.Content = "No posts yet";
                //btnDislike.Visibility = Visibility.Hidden;
                //btnLike.Visibility = Visibility.Hidden;
                //btnComment.Visibility = Visibility.Hidden;

                  
            }            
            //
            if (postServices.CheckIfUserDisLikePost(services.NickNameRead(), currentPost.Id))
            {
                btnDislike.Background = Brushes.Red;
                tempDislike = true;
            }
            if (postServices.CheckIfUserLikePost(services.NickNameRead(), currentPost.Id))
            {
                btnLike.Background = Brushes.Green;
                tempLike = true;
            }
            //
            if(services.CheckAlreadyFollow(services.NickNameRead(), PersonNickName))
            {
                btnFollow.Background = Brushes.Green;
            }
            txtDislike.Text = postServices.GetDislikes(currentPost.Id).ToString();
            txtLike.Text = postServices.GetLikes(currentPost.Id).ToString();

           
        }

        private void Follow(object sender, RoutedEventArgs e)
        {
            if (!services.CheckAlreadyFollow(services.NickNameRead(),PersonNickName))
            {
               
                services.AddFollowing(services.NickNameRead(), PersonNickName);
                services.AddFollower(PersonNickName, services.NickNameRead());
                btnFollow.Background = Brushes.Green;
                btnConnection.Content = services.GetConnectingPathsNumber(PersonNickName);
              
            }
           
        }

        private void Unfollow(object sender, RoutedEventArgs e)
        {
            services.UnFollow(services.NickNameRead(), PersonNickName);
            Color color = (Color)ColorConverter.ConvertFromString("#0288d1");
            SolidColorBrush brush = new SolidColorBrush(color);
            btnFollow.Background = brush;
            btnConnection.Content = services.GetConnectingPathsNumber(PersonNickName);
        }


        private void Comment(object sender, RoutedEventArgs e)
        {
            if (isAnyPosts)
            {
                AddCommentWindow main = new AddCommentWindow(currentPost.Id)
                {
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                };
                main.ShowDialog();
            }
        }
       
        private void Like(object sender, RoutedEventArgs e)
        {
            if(tempLike == false)
            {
                btnLike.Background = Brushes.Green;
                tempLike = true;
                postServices.AddLike(services.NickNameRead(),currentPost.Id);
                txtLike.Text = postServices.GetLikes(currentPost.Id).ToString();
            }
            else
            {
                Color color = (Color)ColorConverter.ConvertFromString("#0288d1");
                SolidColorBrush brush = new SolidColorBrush(color);
                btnLike.Background = brush;
                tempLike = false;
                postServices.DismissLike(services.NickNameRead(), currentPost.Id);
                txtLike.Text = postServices.GetLikes(currentPost.Id).ToString();
            }
           
        }

        private void Dislike(object sender, RoutedEventArgs e)
        {
           
            if (tempDislike == false)
            {
                btnDislike.Background = Brushes.Red;
                tempDislike = true;
                postServices.AddDislike(services.NickNameRead(), currentPost.Id);
                txtDislike.Text = postServices.GetDislikes(currentPost.Id).ToString();
            }
            else
            {
                Color color = (Color)ColorConverter.ConvertFromString("#0288d1");
                SolidColorBrush brush = new SolidColorBrush(color);
                btnDislike.Background = brush;
                tempDislike = false;
                postServices.DismissDisLike(services.NickNameRead(), currentPost.Id);
                txtDislike.Text = postServices.GetDislikes(currentPost.Id).ToString();
            }
        }

        private void NextPost(object sender, RoutedEventArgs e)
        {
            if (isAnyPosts)
            {
                indexOfPost++;
                if (indexOfPost < posts.Count)
                {
                    currentPost = posts[indexOfPost];
                    Main.Content = currentPost.Text;
                    if (postServices.CheckIfUserDisLikePost(services.NickNameRead(), currentPost.Id))
                    {
                        btnDislike.Background = Brushes.Red;
                        tempDislike = true;
                    }
                    else
                    {
                        Color color = (Color)ColorConverter.ConvertFromString("#0288d1");
                        SolidColorBrush brush = new SolidColorBrush(color);
                        btnDislike.Background = brush;
                        tempDislike = false;
                    }
                    if (postServices.CheckIfUserLikePost(services.NickNameRead(), currentPost.Id))
                    {
                        btnLike.Background = Brushes.Green;
                        tempLike = true;
                    }
                    else
                    {
                        Color color = (Color)ColorConverter.ConvertFromString("#0288d1");
                        SolidColorBrush brush = new SolidColorBrush(color);
                        btnLike.Background = brush;
                        tempLike = false;
                    }
                }
                else
                {
                    indexOfPost--;
                    currentPost = posts[indexOfPost];
                    Main.Content = currentPost.Text;

                }
                txtDislike.Text = postServices.GetDislikes(currentPost.Id).ToString();
                txtLike.Text = postServices.GetLikes(currentPost.Id).ToString();
            }
          

        }
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        private void OFF(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void PreviusPost(object sender, RoutedEventArgs e)
        {
            if (isAnyPosts)
            {
                if (indexOfPost > 0)
                {
                    indexOfPost--;
                    currentPost = posts[indexOfPost];
                    Main.Content = currentPost.Text;
                    if (postServices.CheckIfUserDisLikePost(services.NickNameRead(), currentPost.Id))
                    {
                        btnDislike.Background = Brushes.Red;
                        tempDislike = true;
                    }
                    else
                    {
                        Color color = (Color)ColorConverter.ConvertFromString("#0288d1");
                        SolidColorBrush brush = new SolidColorBrush(color);
                        btnDislike.Background = brush;
                        tempDislike = false;
                    }
                    if (postServices.CheckIfUserLikePost(services.NickNameRead(), currentPost.Id))
                    {
                        btnLike.Background = Brushes.Green;
                        tempLike = true;
                    }
                    else
                    {
                        Color color = (Color)ColorConverter.ConvertFromString("#0288d1");
                        SolidColorBrush brush = new SolidColorBrush(color);
                        btnLike.Background = brush;
                        tempLike = false;
                    }
                }
                else
                {
                    currentPost = posts[indexOfPost];
                    Main.Content = currentPost.Text;
                    if (postServices.CheckIfUserDisLikePost(services.NickNameRead(), currentPost.Id))
                    {
                        btnDislike.Background = Brushes.Red;
                        tempDislike = true;
                    }
                    if (postServices.CheckIfUserLikePost(services.NickNameRead(), currentPost.Id))
                    {
                        btnLike.Background = Brushes.Green;
                        tempLike = true;
                    }
                }
                txtDislike.Text = postServices.GetDislikes(currentPost.Id).ToString();
                txtLike.Text = postServices.GetLikes(currentPost.Id).ToString();
            }
          
        }

        private void PersonsWhoLikes(object sender, RoutedEventArgs e)
        {
            ListOfPersonsWindow main = new ListOfPersonsWindow(postServices.GetPersonsWhoLiked(currentPost.Id))
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            main.ShowDialog();
        }

        private void PersonsWhoComment(object sender, RoutedEventArgs e)
        {
            ListOfCommentsWindow main = new ListOfCommentsWindow(postServices.GetPersonWhoComment(currentPost.Id))
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            main.ShowDialog();
        }

        private void PersonsWhoDislike(object sender, RoutedEventArgs e)
        {
            ListOfPersonsWindow main = new ListOfPersonsWindow(postServices.GetPersonsWhoDisLiked(currentPost.Id))
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            main.ShowDialog();
        }

        private void ViewFollowing(object sender, RoutedEventArgs e)
        {
           
            ListOfPersonsWindow main = new ListOfPersonsWindow(repository.GetFollowing(user.NickName))
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            main.ShowDialog();
        }

        private void ViewFollowers(object sender, RoutedEventArgs e)
        {
            ListOfPersonsWindow main = new ListOfPersonsWindow(repository.GetFollowers(user.NickName))
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            main.ShowDialog();
        }

        private void CommonFriends(object sender, RoutedEventArgs e)
        {
            var temp = services.GetCommonFriends(PersonNickName);
            if(temp!=null && temp.Count > 0)
            {
                ListOfPersonsWindow window = new ListOfPersonsWindow(temp);
                window.Show();
            }
            else
            {
                MessageWindow window = new MessageWindow("No comon friends");
                window.Show();
            }
        }

        private void ViewConnection(object sender, RoutedEventArgs e)
        {
            var temp = services.GetConnectingPaths(PersonNickName);
            if(temp != null && temp.Count > 0)
            {
                ListOfPersonsWindow window = new ListOfPersonsWindow(services.GetConnectingPaths(PersonNickName));
                window.Show();
            }
           
        }
    }
}
