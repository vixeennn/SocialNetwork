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
    /// Interaction logic for ucProfileEdit.xaml
    /// </summary>
    public partial class ucProfileEdit : UserControl
    {


        UserServices services;
        public ucProfileEdit()
        {
            InitializeComponent();
            services = new UserServices();
            FillComboBox();

        }

        private void FillComboBox()
        {
            OptionComboBox.Items.Add("Edit name");
            OptionComboBox.Items.Add("Edit surname");
            OptionComboBox.Items.Add("Edit mail");
        }

        private void OptionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EditNewData.Text = "";
        }

        private void EditNewData_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(OptionComboBox.SelectedItem.ToString() == "Edit name")
            {
                if (services.UpdateField(services.NickNameRead(), "Name", EditNewData.Text))
                {
                    MessageWindow window = new MessageWindow("Done successfully ");
                    window.ShowDialog();
                }
                else
                {
                    MessageWindow window = new MessageWindow("Oooops some problem .. ");
                    window.ShowDialog();
                }
                
            }
            else if (OptionComboBox.SelectedItem.ToString() == "Edit surname")
            {
               
                if (services.UpdateField(services.NickNameRead(), "Surname", EditNewData.Text))
                {
                    MessageWindow window = new MessageWindow("Done successfully ");
                    window.ShowDialog();
                }
                else
                {
                    MessageWindow window = new MessageWindow("Oooops some problem .. ");
                    window.ShowDialog();
                }
            }
            else if (OptionComboBox.SelectedItem.ToString() == "Edit mail")
            {
                
                if (services.UpdateField(services.NickNameRead(), "Mail", EditNewData.Text))
                {
                    MessageWindow window = new MessageWindow("Done successfully ");
                    window.ShowDialog();
                }
                else
                {
                    MessageWindow window = new MessageWindow("Oooops some problem .. ");
                    window.ShowDialog();
                }
            }
            else
            {

            }

        }
    }
}
