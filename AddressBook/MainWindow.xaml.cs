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

namespace AddressBook
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            try
            {
                InitializeComponent();

                DataContext = new ApplicationViewModel();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string msg = "Сохранить изменения в адресах перед закрытием приложения?";

            MessageBoxResult result =
              MessageBox.Show(
                msg,
                "Data App",
                MessageBoxButton.YesNoCancel,
                MessageBoxImage.Question);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    ApplicationViewModel.SaveInFile();
                    break;
                case MessageBoxResult.No:
                    break;
                case MessageBoxResult.Cancel:
                    e.Cancel = true;
                    break;
            }
        }

        private void EnabledButton_Click(object sender, RoutedEventArgs e)
        {
            SurnameTextBox.IsEnabled = true;

            NameTextBox.IsEnabled = true;

            PatronymicTextBox.IsEnabled = true;

            PhoneTextBox.IsEnabled = true;
        }

        private void DisabledButton_Click(object sender, RoutedEventArgs e)
        {
            SurnameTextBox.IsEnabled = false;

            NameTextBox.IsEnabled = false;

            PatronymicTextBox.IsEnabled = false;

            PhoneTextBox.IsEnabled = false;
        }
    }
}
