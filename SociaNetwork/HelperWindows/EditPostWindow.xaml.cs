using DAL.Services;
using MongoDB.Bson;
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

namespace SociaNetwork.HelperWindows
{
    /// <summary>
    /// Interaction logic for EditPostWindow.xaml
    /// </summary>
    public partial class EditPostWindow : Window
    {
        ObjectId postId;
        PostServices postServices;
        public EditPostWindow(ObjectId postId)
        {
            InitializeComponent();
            this.postId = postId;
            postServices = new PostServices();
            Text.Text = postServices.GetPost(postId).Text;
        }
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        private void OFF(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void NameLogin_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }


        private void ADD(object sender, RoutedEventArgs e)
        {
            postServices.EditPost(Text.Text,postId);
            this.Close();
        }

    }
}
