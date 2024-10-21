using DAL.Enteties;
using DAL.Repository;
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
    /// Interaction logic for myProfile.xaml
    /// </summary>
    public partial class myProfile : UserControl
    {
        bool isAnyPosts = false;
        User user;
        bool tempLike = false;
        bool tempDislike = false;
        int indexOfPost = 0;
        Post currentPost;
        PostRepository postRepository;
        PostServices postServices;
        UserServices services;
        List<Post> posts;
        public myProfile()
        {
            InitializeComponent();
            services = new UserServices();
            postRepository = new PostRepository();
            postServices = new PostServices();
            //
            user = services.GetUser();
            //
            currentPost = new Post();
            posts = new List<Post>();
            posts = postRepository.GetPosts(services.GetUserId());
            if (posts != null && posts.Count > 0)
            {
                currentPost = posts[indexOfPost];
                Main.Content = currentPost.Text;
                isAnyPosts = true;
            }
            else
            {
                Main.Content = "You dont have any posts yet";
            }

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
            txtDislike.Text = postServices.GetDislikes(currentPost.Id).ToString();
            txtLike.Text = postServices.GetLikes(currentPost.Id).ToString();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PersonInfo main = new PersonInfo("ddd")
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            main.ShowDialog();
         

        }

        private void Like(object sender, RoutedEventArgs e)
        {
            if (tempLike == false)
            {
                btnLike.Background = Brushes.Green;
                tempLike = true;
                postRepository.AddLike(services.NickNameRead(), currentPost.Id);
                txtLike.Text = postServices.GetLikes(currentPost.Id).ToString();
            }
            else
            {
                Color color = (Color)ColorConverter.ConvertFromString("#0288d1");
                SolidColorBrush brush = new SolidColorBrush(color);
                btnLike.Background = brush;
                tempLike = false;
                postRepository.DismissLike(services.NickNameRead(), currentPost.Id);
                txtLike.Text = postServices.GetLikes(currentPost.Id).ToString();
            }
        }

        private void Posts(object sender, RoutedEventArgs e)
        {
            posts = postRepository.GetPosts(services.GetUserId());
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
                    else
                    {
                        Color color = (Color)ColorConverter.ConvertFromString("#0288d1");
                        SolidColorBrush brush = new SolidColorBrush(color);
                        btnDislike.Background = brush;
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
                    }
                }
                txtDislike.Text = postServices.GetDislikes(currentPost.Id).ToString();
                txtLike.Text = postServices.GetLikes(currentPost.Id).ToString();
            }
           
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

        private void NextPost(object sender, RoutedEventArgs e)
        {
            posts = postRepository.GetPosts(services.GetUserId());
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
                        btnDislike.Background = brush;
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

        private void Dislike(object sender, RoutedEventArgs e)
        {

            if (tempDislike == false)
            {
                btnDislike.Background = Brushes.Red;
                tempDislike = true;
                postRepository.AddDislike(services.NickNameRead(), currentPost.Id);
                txtDislike.Text = postServices.GetDislikes(currentPost.Id).ToString();
            }
            else
            {
                Color color = (Color)ColorConverter.ConvertFromString("#0288d1");
                SolidColorBrush brush = new SolidColorBrush(color);
                btnDislike.Background = brush;
                tempDislike = false;
                postRepository.DismissDisLike(services.NickNameRead(), currentPost.Id);
                txtDislike.Text = postServices.GetDislikes(currentPost.Id).ToString();
               
            }
        }

        private void AddNewPost(object sender, RoutedEventArgs e)
        {
            AddPostWindow window = new AddPostWindow()
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            window.ShowDialog();

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
            ListOfPersonsWindow main = new ListOfPersonsWindow(services.GetFollowing())
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            main.ShowDialog();
        }

        private void ViewFollowers(object sender, RoutedEventArgs e)
        {
            ListOfPersonsWindow main = new ListOfPersonsWindow(services.GetFollowers())
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            main.ShowDialog();
        }

        private void Refresh(object sender, RoutedEventArgs e)
        {
            posts = postRepository.GetPosts(services.GetUserId());
            if (posts != null && posts.Count > 0)
            {
                currentPost = posts[indexOfPost];
                Main.Content = currentPost.Text;
                isAnyPosts = true;
            }
            else
            {
                Main.Content = "You dont have any posts yet";
            }
        }

        private void EditPost(object sender, RoutedEventArgs e)
        {
            if (isAnyPosts)
            {
                EditPostWindow window = new EditPostWindow(currentPost.Id);
                window.Show();
            }
         
        }

        private void DeletePost(object sender, RoutedEventArgs e)
        {
            if (isAnyPosts)
            {
                bool t = postServices.DeletePost(currentPost.Id);
                if (t)
                {
                    MessageWindow window = new MessageWindow("Done");
                    window.Show();
                }
                else
                {
                    MessageWindow window = new MessageWindow("Ooops some problem");
                    window.Show();
                }
            }
           
        }
    }
}
