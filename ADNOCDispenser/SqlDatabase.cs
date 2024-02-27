using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ADNOCDispenser
{
    internal class SqlDatabase
    {
        public static string sqlConnectionString = "Data Source=PEHANRANSIKA;Initial Catalog=ADNOC_FillingStation;Integrated Security=True";
        private static SqlConnection sqlConnection = new SqlConnection(sqlConnectionString);

        public static int executeNonQuery(string query)
        {
            int affectedRows = 0;
            try
            {
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlConnection.Open();
                affectedRows = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                sqlConnection.Close();
            }
            return affectedRows;
        }

        public static DataTable getDataTableOf(string query)
        {
            DataTable dataTable = new DataTable();
            SqlDataAdapter sqlDataAdapter;
            try
            {
                sqlConnection.Open();
                sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);
                sqlDataAdapter.Fill(dataTable);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                sqlConnection.Close();
            }
            return dataTable;
        }

        public static double executeScalar(string query)
        {
            double data = 0;
            try
            {
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlConnection.Open();
                data = Convert.ToDouble(sqlCommand.ExecuteScalar());
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                sqlConnection.Close();
            }
            return data;
        }

        public static string executeScalarForString(string query)
        {
            string data = "";
            try
            {
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlConnection.Open();
                data = Convert.ToString(sqlCommand.ExecuteScalar());
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                sqlConnection.Close();
            }
            return data;
        }

        public static string getemployeeid()
        {

            string query = "select max(employeeId) from employee";
            string empid = "E001";

            try
            {
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlConnection.Open();
                var data = sqlCommand.ExecuteScalar();
                empid = data.ToString().Replace("E", "");
                sqlConnection.Close();
                int empno = int.Parse(empid) + 1;
                if (empno < 10) empid = "E00" + empno.ToString();
                else if (empno < 100) empid = "E0" + empno.ToString();
                else empid = "E" + empno.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                sqlConnection.Close();
            }
            return empid;


        }

        public static string getNextSaleId()
        {

            string query = "select max(saleId) from sale";
            string empid = "S001";

            try
            {
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlConnection.Open();
                var data = sqlCommand.ExecuteScalar();
                empid = data.ToString().Replace("S", "");
                sqlConnection.Close();
                int empno = int.Parse(empid) + 1;
                if (empno < 10) empid = "S00" + empno.ToString();
                else if (empno < 100) empid = "S0" + empno.ToString();
                else empid = "S" + empno.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                sqlConnection.Close();
            }
            return empid;


        }

        public static string getNextInvoiceId()
        {

            string query = "select max(invoiceId) from invoice";
            Trace.WriteLine(query);
            string invoiceId = "IV001";

            try
            {
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlConnection.Open();
                var data = sqlCommand.ExecuteScalar();
                invoiceId = data.ToString().Replace("IV", "");
                sqlConnection.Close();
                int invoiceNo = int.Parse(invoiceId) + 1;
                if (invoiceNo < 10) invoiceId = "IV00" + invoiceNo.ToString();
                else if (invoiceNo < 100) invoiceId = "IV0" + invoiceNo.ToString();
                else invoiceId = "IV" + invoiceNo.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                sqlConnection.Close();
            }
            return invoiceId;


        }
    }
}
