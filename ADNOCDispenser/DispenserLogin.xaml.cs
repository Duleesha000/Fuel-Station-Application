using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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

namespace ADNOCDispenser
{
    /// <summary>
    /// Interaction logic for DispenserLogin.xaml
    /// </summary>
    public partial class DispenserLogin : Window
    {
        public DispenserLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Clicked(object sender, RoutedEventArgs e)
        {
            if (txtuser.Text.Length == 0 && txtpassword.Password.Length > 0)
            {
                labelerror.Visibility = Visibility.Visible;
                labelerror.Content = "Please Enter Dispenser ID";
                txtuser.Focus();
            }
            else if (txtpassword.Password.Length == 0 && txtuser.Text.Length > 0)
            {
                labelerror.Visibility = Visibility.Visible;
                labelerror.Content = "Please Enter the Password";
                txtpassword.Focus();
            }
            else if (txtuser.Text.Length == 0 && txtpassword.Password.Length == 0)
            {
                labelerror.Visibility = Visibility.Visible;
                labelerror.Content = "Please Enter Dispenser ID & Password";
                txtuser.Focus();
            }
            else
            {
                string dispenserid = txtuser.Text;
                string password = txtpassword.Password.ToString();


                try
                {
                    SqlConnection sqlConnection = new SqlConnection("Data Source=PEHANRANSIKA;Initial Catalog=ADNOC_FillingStation;Integrated Security=True");
                    sqlConnection.Open();
                    string query = "Select count(1) from dispenser where dispenserid= '" + dispenserid + "' and password = '" + password + "' COLLATE SQL_Latin1_General_CP1_CS_AS";

                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.CommandType = CommandType.Text;

                    int count = Convert.ToInt32(sqlCommand.ExecuteScalar());

                    if (count == 1)
                    {
                        Dispenser dispenser = new Dispenser(dispenserid);
                        dispenser.Show();
                        this.Close();
                    }
                    else
                    {
                        labelerror.Visibility = Visibility.Visible;
                        labelerror.Content = "Invalid Username or Password";
                        txtpassword.Focus();
                        txtuser.Focus();

                    }

                }
                catch (SqlException)
                {
                    MessageBox.Show("Database Error", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnMinimize_click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }

}

