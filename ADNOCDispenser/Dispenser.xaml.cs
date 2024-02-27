using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

using TableDependency.SqlClient.Base.Enums;
using TableDependency.SqlClient;

namespace ADNOCDispenser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Dispenser : Window
    {
        double unitprice, availableQty;
        double price, qty;
        string fuelType, dispenserId, itemId;

        public Dispenser(string dispenserId)
        {
            InitializeComponent();
            this.dispenserId = dispenserId;

            DataTable dataTable = SqlDatabase.getDataTableOf("select item.itemId, itemName, unitPrice, availableQuantity from Dispenser inner join Item on Dispenser.itemId = item.itemId where dispenserId = '" + dispenserId + "'");

            itemId = dataTable.Rows[0]["itemId"].ToString();
            unitprice = Convert.ToDouble(dataTable.Rows[0]["unitPrice"].ToString());
            fuelType = dataTable.Rows[0]["itemName"].ToString();
            availableQty = Convert.ToDouble(dataTable.Rows[0]["availableQuantity"].ToString());


            lbl_price.Content = "Rs." + unitprice.ToString() + "/=";
            lbl_id.Content = dispenserId;
            lbl_type.Content = fuelType;
            lbl_qty.Content = availableQty.ToString();
            rdbtn_price.IsChecked = true;
            txtbox_email1.IsEnabled = false;
            btn_sendemail.IsEnabled = false;

            WatchTable();

        }

        private void realTimeSqlUpdate(double unitPrice, double availableQty)
        {
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    Trace.WriteLine(availableQty.ToString());
                    this.unitprice = unitPrice;
                    this.availableQty = availableQty;
                    lbl_qty.Content = this.availableQty.ToString();
                    lbl_price.Content = "Rs." + this.unitprice.ToString() + "/=";
                });

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void window_load(object sender, RoutedEventArgs e)
        {


        }

        private void btnMinimize_click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }


        private void txtbox_email1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Regex.IsMatch(txtbox_email1.Text, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))
            {
                btn_sendemail.IsEnabled = true;
            }
            else
                btn_sendemail.IsEnabled = false;
        }



        private void rdbtn_price_Checked(object sender, RoutedEventArgs e)
        {
            /*txtbox_qty.IsReadOnly = true;
            txtbox_price.IsReadOnly = false;*/
        }

        private void rdbtn_litres_Checked(object sender, RoutedEventArgs e)
        {
            /*txtbox_price.IsReadOnly = true;
            txtbox_qty.IsReadOnly = false;*/
        }

        private void keypad_btn_Click(object sender, RoutedEventArgs e)
        {
            Button clickedBtn = (Button)sender;
            if (rdbtn_price.IsChecked == true)
            {
                txtbox_price.Text += clickedBtn.Content;
            }

            else if (rdbtn_litres.IsChecked == true)
            {
                txtbox_qty.Text += clickedBtn.Content;
            }

        }
        private void btn_Clear_Click(object sender, RoutedEventArgs e)
        {
            if (rdbtn_price.IsChecked == true && txtbox_price.Text.Length > 0)
                txtbox_price.Text = txtbox_price.Text.Substring(0, txtbox_price.Text.Length - 1);
            else if (rdbtn_litres.IsChecked == true && txtbox_qty.Text.Length > 0)
                txtbox_qty.Text = txtbox_qty.Text.Substring(0, txtbox_qty.Text.Length - 1);
        }

        private void textChange(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (rdbtn_price.IsChecked == true)
                {
                    if (txtbox_price.Text != "")
                    {
                        price = Convert.ToDouble(txtbox_price.Text);
                        qty = price / unitprice;
                        txtbox_qty.Text = (qty).ToString("0.##");
                    }
                    else
                    {
                        price = 0;
                        txtbox_qty.Text = "";
                    }

                }
                else if (rdbtn_litres.IsChecked == true)
                {
                    if (txtbox_qty.Text != "")
                    {
                        qty = Convert.ToDouble(txtbox_qty.Text);
                        price = qty * unitprice;
                        txtbox_price.Text = (price).ToString("0.##");
                    }
                    else
                    {
                        qty = 0;
                        txtbox_price.Text = "";
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Check Values Again", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void btn_enter_Click(object sender, RoutedEventArgs e)
        {

            if (txtbox_price.Text != "" && txtbox_qty.Text != "")
            {
                if (qty < availableQty)
                {
                    try
                    {
                        ShowAsync();
                        MessageBoxResult result = MessageBox.Show("Do you want a E-Bill? ", "E-Bill", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (result == MessageBoxResult.Yes)
                        {
                            txtbox_email1.IsEnabled = true;
                            btn_enter.IsEnabled = false;
                        }
                        else
                        {
                            string saleId = SqlDatabase.getNextSaleId();
                            string saleQuery = "insert into sale values ('" + saleId + "','" + itemId + "','" + dispenserId + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("hh:mm:ss") + "'," + qty.ToString("0.##") + "," + price.ToString("0.##") + ")";
                            string updateItemQuery = "update Item set availableQuantity = availableQuantity - " + qty.ToString() + " where itemId = '" + itemId + "'";
                            if (SqlDatabase.executeNonQuery(saleQuery) > 0 /*&& SqlDatabase.executeNonQuery(updateItemQuery) > 0*/)
                            {
                                MessageBox.Show("Successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                                clearAll();
                            }
                            else
                            {
                                MessageBox.Show("Something went wrong", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Sorry! We don't have sufficient available Stock", "Insufficient Stock", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }


        }

        private void btn_sendemail_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string invoiceNo = SqlDatabase.getNextInvoiceId();
                string saleId = SqlDatabase.getNextSaleId();
                string saleQuery = "insert into sale values ('" + saleId + "','" + itemId + "','" + dispenserId + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("hh:mm:ss") + "'," + qty.ToString("0.##") + "," + price.ToString("0.##") + ")";
                string invoiceQuery = "insert into Invoice values ('" + invoiceNo + "','" + saleId + "','" + txtbox_email1.Text.ToString() + "')";
                string updateItemQuery = "update Item set availableQuantity = availableQuantity - " + qty.ToString() + " where itemId = '" + itemId + "'";
                if (SqlDatabase.executeNonQuery(saleQuery) > 0 /*&& SqlDatabase.executeNonQuery(updateItemQuery) > 0*/)
                {
                    if (Email.SendBillEmail(txtbox_email1.Text, fuelType, unitprice, qty, price, invoiceNo, DateTime.Now))
                    {
                        if (SqlDatabase.executeNonQuery(invoiceQuery) == 0)
                        {
                            MessageBox.Show("Something went wrong", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }

                    }
                    clearAll();
                }
                else
                {
                    if (MessageBox.Show("Something went wrong, Do you want to try again?", "Error", MessageBoxButton.YesNo, MessageBoxImage.Error) == MessageBoxResult.Yes)
                    {
                        btn_sendemail_Click(sender, e);
                    }
                }


                clearAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void clearAll()
        {
            txtbox_price.Text = "";
            txtbox_email1.Text = "";
            txtbox_qty.Text = "";
            price = 0;
            qty = 0;
            txtbox_email1.IsEnabled = false;
            btn_enter.IsEnabled = true;
            rdbtn_price.IsChecked = true;
        }

        public static async Task ShowAsync()
        {
            var timeoutTask = Task.Delay(5000);

            var messageBoxTask = Task.Run(() =>
            {
                MessageBox.Show("PUMPING....", "Pumping");
            });

            await Task.WhenAny(timeoutTask, messageBoxTask);
        }


        public void WatchTable()
        {

            var tableDependency = new SqlTableDependency<Item>(SqlDatabase.sqlConnectionString);

            tableDependency.OnChanged += OnNotificationReceived;
            tableDependency.Start();
        }

        private void OnNotificationReceived(object sender, TableDependency.SqlClient.Base.EventArgs.RecordChangedEventArgs<Item> e)
        {
            if (e.ChangeType == ChangeType.Update)
            {
                if (e.Entity.itemId == itemId)
                {
                    realTimeSqlUpdate(Convert.ToDouble(e.Entity.unitPrice), Convert.ToDouble(e.Entity.availableQuantity));
                }
            }
        }

    }

}

