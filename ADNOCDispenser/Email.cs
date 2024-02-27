﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ADNOCDispenser
{
    internal class Email
    {
        public static bool SendBillEmail(string email, string fuelType, double unitPrice, double quantity, double total, string invoiceNo, DateTime dateTime)
        {
            if (sendEmail(email, "ADNOC E-Bill", BuildBillBody(email, fuelType, unitPrice, quantity, total, invoiceNo, dateTime)))
            {
                MessageBox.Show("E-Bill sent Successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                return true;
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("E-Bill not sent. Something went wrong. Do you want to try again?", "Error", MessageBoxButton.YesNo, MessageBoxImage.Error);
                if (result == MessageBoxResult.Yes)
                {
                    return SendBillEmail(email, fuelType, unitPrice, quantity, total, invoiceNo, dateTime);
                }
                else return false;
            }
        }

        private static string BuildBillBody(string email, string fuelType, double unitPrice, double quantity, double total, string invoiceNo, DateTime dateTime)
        {
            string emailBody = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">\r\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n\r\n<head style=\"box-sizing: border-box; font-family: 'Lato', sans-serif; margin: 0; padding: 0;\">\r\n  <title style=\"box-sizing: border-box; font-family: 'Lato', sans-serif; margin: 0; padding: 0;\">Invoice Template Design</title>\r\n\r\n</head>\r\n<body style=\"align-items: center; background: #0A2647; box-sizing: border-box; color: #0A2647; display: flex; font-family: 'Lato', sans-serif; font-size: 14px; justify-content: center; margin: 0; padding: 50px;\">\r\n  <style></style>\r\n\r\n  <div class=\"wrapper\" style=\"background: #fff; box-sizing: border-box; font-family: 'Lato', sans-serif; margin: 0; padding: 30px;\">\r\n    <div class=\"invoice_wrapper\" style=\"border: 3px solid #2C74B3; box-sizing: border-box; font-family: 'Lato', sans-serif; margin: 0; max-width: 100%; padding: 0; width: 700px;\">\r\n      <div class=\"header\" style=\"box-sizing: border-box; font-family: 'Lato', sans-serif; margin: 0; padding: 0;\">\r\n        <div class=\"logo_invoice_wrap\" style=\"box-sizing: border-box; display: flex; font-family: 'Lato', sans-serif; justify-content: space-between; margin: 0; padding: 30px;\">\r\n          <div class=\"logo_sec\" style=\"align-items: center; box-sizing: border-box; display: flex; font-family: 'Lato', sans-serif; margin: 0; padding: 0;\">\r\n            <img src=\"https://drive.google.com/uc?id=1Iuch6LldsgrAJScKmRi5uhghk_BamjCK\" alt=\"adnoc\" style=\"box-sizing: border-box; font-family: 'Lato', sans-serif; margin: 0; padding: 0;\">\r\n            <div class=\"title_wrap\" style=\"box-sizing: border-box; font-family: 'Lato', sans-serif; margin: 0; margin-left: 5px; padding: 0;\">\r\n              <p class=\"title bold\" style=\"box-sizing: border-box; color: #2C74B3; font-family: 'Lato', sans-serif; font-size: 45px; font-weight: 900; margin: 0; padding: 0; text-transform: uppercase;\">adnoc</p>\r\n              <p class=\"sub_title\" style=\"box-sizing: border-box; font-family: 'Lato', sans-serif; font-size: 20px; margin: 0; padding: 0;\">Filling Station</p>\r\n            </div>\r\n          </div>\r\n          <div class=\"invoice_sec\" style=\"box-sizing: border-box; font-family: 'Lato', sans-serif; margin: 0; margin-top: 20px; padding: 0; text-align: left;\">\r\n            <p class=\"invoice bold\" style=\"box-sizing: border-box; color: #2C74B3; font-family: 'Lato', sans-serif; font-size: 25px; font-weight: 900; margin: 0; padding: 0;\">INVOICE</p>\r\n            <p class=\"invoice_no\" style=\"box-sizing: border-box; display: flex; font-family: 'Lato', sans-serif; margin: 0; padding: 0; text-align: left; align-self: left; width: 100%;\">\r\n              <span class=\"bold\" style=\"box-sizing: border-box; font-family: 'Lato', sans-serif; font-weight: 900; margin: 0; padding: 0; text-align: left; width: 70px;\">Invoice: </span>\r\n              <span style=\"box-sizing: border-box; font-family: 'Lato', sans-serif; margin: 0; padding: 0; width: calc(100%-70px);\">" + invoiceNo + "</span>\r\n            </p>\r\n            <p class=\"date\" style=\"box-sizing: border-box; font-family: 'Lato', sans-serif; margin: 0; padding: 0;\">\r\n              <span class=\"bold\" style=\"box-sizing: border-box; font-family: 'Lato', sans-serif; font-weight: 900; margin: 0; padding: 0;\">Date: </span>\r\n              <span style=\"box-sizing: border-box; font-family: 'Lato', sans-serif; margin: 0; padding: 0;\"> " + dateTime.ToString("dd/MM/yyyy") + "</span>\r\n\r\n            </p>\r\n            <p class=\"time\" style=\"box-sizing: border-box; font-family: 'Lato', sans-serif; margin: 0; padding: 0;\">\r\n              <span class=\"bold\" style=\"box-sizing: border-box; font-family: 'Lato', sans-serif; font-weight: 900; margin: 0; padding: 0;\">Time: </span>\r\n              <span style=\"box-sizing: border-box; font-family: 'Lato', sans-serif; margin: 0; padding: 0;\">" + dateTime.ToString("hh:mm:ss") + "</span>\r\n\r\n            </p>\r\n\r\n\r\n          </div>\r\n        </div>\r\n        <div class=\"bill_total_wrap\" style=\"box-sizing: border-box; display: flex; font-family: 'Lato', sans-serif; justify-content: space-between; margin: 0; padding: 30px;\">\r\n          <div class=\"bill_sec\" style=\"box-sizing: border-box; font-family: 'Lato', sans-serif; margin: 0; padding: 0;\">\r\n            <p style=\"box-sizing: border-box; font-family: 'Lato', sans-serif; margin: 0; padding: 0;\">Email To</p>\r\n            <p class=\"bold name\" style=\"box-sizing: border-box; color: #2C74B3; font-family: 'Lato', sans-serif; font-size: 15px; font-weight: 900; margin: 0; padding: 0;\">" + email + "</p>\r\n\r\n          </div>\r\n          <div class=\"total_wrap\" style=\"box-sizing: border-box; font-family: 'Lato', sans-serif; margin: 0; padding: 0; text-align: right;\">\r\n          </div>\r\n        </div>\r\n      </div>\r\n      <div class=\"body\" style=\"box-sizing: border-box; font-family: 'Lato', sans-serif; margin: 0; padding: 0;\">\r\n        <div class=\"main_table\" style=\"box-sizing: border-box; font-family: 'Lato', sans-serif; margin: 0; padding: 0;\">\r\n          <div class=\"table_header\" style=\"background: #2C74B3; box-sizing: border-box; font-family: 'Lato', sans-serif; margin: 0; padding: 0;\">\r\n            <div class=\"row\" style=\"border-bottom: 0px; box-sizing: border-box; color: #fff; display: flex; font-family: 'Lato', sans-serif; font-size: 18px; margin: 0; padding: 0;\">\r\n              <div class=\"col col_des\" style=\" display:flex, align-items:center, box-sizing: border-box; font-family: 'Lato', sans-serif; margin: 0; padding: 30px; width: 63%;\">FUEL TYPE</div>\r\n              <div class=\"col col_price\" style=\" display:flex, align-items:center, box-sizing: border-box; font-family: 'Lato', sans-serif; margin: 0; padding: 30px; text-align: center; width: 24%;\">UNIT PRICE</div>\r\n              <div class=\"col col_qty\" style=\"box-sizing: display:flex, align-items:center, border-box; font-family: 'Lato', sans-serif; margin: 0; padding: 30px; text-align: center; width: 5%;\">QTY</div>\r\n              <div class=\"col col_total\" style=\"box-sizing: display:flex, align-items:center, border-box; font-family: 'Lato', sans-serif; margin: 0; padding: 30px; text-align: right; width: 8%;\">SUB TOTAL</div>\r\n            </div>\r\n          </div>\r\n          <div class=\"table_body\" style=\"box-sizing: border-box; font-family: 'Lato', sans-serif; margin: 0; padding: 0;\">\r\n            <div class=\"row\" style=\"border-bottom: 1px solid #0A2647; box-sizing: border-box; display: flex; font-family: 'Lato', sans-serif; margin: 0; padding: 0;\">\r\n              <div class=\"col col_no\" style=\"box-sizing: border-box; font-family: 'Lato', sans-serif; margin: 0; padding: 30px; width: 5%;\">\r\n\r\n              </div>\r\n              <div class=\"col col_des\" style=\"box-sizing: border-box; font-family: 'Lato', sans-serif; margin: 0; padding: 30px; width: 45%;\">\r\n                <p class=\"bold\" style=\"box-sizing: border-box; font-family: 'Lato', sans-serif; font-weight: 900; margin: 0; padding: 0;\">" + fuelType + "</p>\r\n\r\n              </div>\r\n              <div class=\"col col_price\" style=\"box-sizing: border-box; font-family: 'Lato', sans-serif; margin: 0; padding: 30px; text-align: center; width: 20%;\">\r\n                <p style=\"box-sizing: border-box; font-family: 'Lato', sans-serif; margin: 0; padding: 0;\">" + unitPrice.ToString("0.##") + "</p>\r\n              </div>\r\n              <div class=\"col col_qty\" style=\"box-sizing: border-box; font-family: 'Lato', sans-serif; margin: 0; padding: 30px; text-align: center; width: 10%;\">\r\n                <p style=\"box-sizing: border-box; font-family: 'Lato', sans-serif; margin: 0; padding: 0;\">" + quantity.ToString("0.##") + "L</p>\r\n              </div>\r\n              <div class=\"col col_total\" style=\"box-sizing: border-box; font-family: 'Lato', sans-serif; margin: 0; padding: 30px; text-align: right; width: 20%;\">\r\n                <p style=\"box-sizing: border-box; font-family: 'Lato', sans-serif; margin: 0; padding: 0;\">" + total.ToString("0.##") + "</p>\r\n              </div>\r\n            </div>\r\n\r\n\r\n\r\n            <div class=\"paymethod_grandtotal_wrap\" style=\"align-items: flex-end; box-sizing: border-box; display: flex; font-family: 'Lato', sans-serif; justify-content: space-between; margin: 0; padding: 5px 0 30px;\">\r\n              <div class=\"paymethod_sec\" style=\"box-sizing: border-box; font-family: 'Lato', sans-serif; margin: 0; padding: 0; padding-left: 30px;\">\r\n                <p class=\"bold\" style=\"box-sizing: border-box; font-family: 'Lato', sans-serif; font-weight: 900; margin: 0; padding: 0;\">Payment Method</p>\r\n                <p style=\"box-sizing: border-box; font-family: 'Lato', sans-serif; margin: 0; padding: 0;\">Visa, master Card and We accept Cheque</p>\r\n              </div>\r\n              <div class=\"grandtotal_sec\" style=\"box-sizing: border-box; font-family: 'Lato', sans-serif; margin: 0; padding: 0; width: 30%;\">\r\n\r\n                <p class=\"bold\" style=\"box-sizing: border-box; display: flex; font-family: 'Lato', sans-serif; font-weight: 900; margin: 0; padding: 0; padding-bottom: 5px; width: 100%;\">\r\n                  <span style=\"background: #2C74B3; box-sizing: border-box; color: #fff; font-family: 'Lato', sans-serif; margin: 0; padding: 10px; width: 60%;\">Grand Total</span>\r\n                  <span style=\"background: #2C74B3; box-sizing: border-box; color: #fff; font-family: 'Lato', sans-serif; margin: 0; padding: 10px; text-align: right; width: 40%;\">" + total + "</span>\r\n                </p>\r\n              </div>\r\n            </div>\r\n          </div>\r\n          <div class=\"footer\" style=\"box-sizing: border-box; font-family: 'Lato', sans-serif; margin: 0; padding: 30px;\">\r\n            <p style=\"box-sizing: border-box; color: #2C74B3; font-family: 'Lato', sans-serif; font-size: 18px; margin: 0; padding: 0; padding-bottom: 5px; text-decoration: underline;\">THANK YOU</p>\r\n            <div class=\"terms\" style=\"box-sizing: border-box; font-family: 'Lato', sans-serif; margin: 0; padding: 0;\">\r\n              <p class=\"bold\" style=\"box-sizing: border-box; font-family: 'Lato', sans-serif; font-weight: 900; margin: 0; padding: 0;\">Adress:</p>\r\n              <p style=\"box-sizing: border-box; font-family: 'Lato', sans-serif; margin: 0; padding: 0;\">NO,street name,area1,area2,province. </p>\r\n              <p class=\"bold\" style=\"box-sizing: border-box; font-family: 'Lato', sans-serif; font-weight: 900; margin: 0; padding: 0;\">tel no:</p>\r\n              <p style=\"box-sizing: border-box; font-family: 'Lato', sans-serif; margin: 0; padding: 0;\">+94000000000</p>\r\n              <p class=\"bold\" style=\"box-sizing: border-box; font-family: 'Lato', sans-serif; font-weight: 900; margin: 0; padding: 0;\">email:</p>\r\n              <p style=\"box-sizing: border-box; font-family: 'Lato', sans-serif; margin: 0; padding: 0;\">emai.email@email.com</p>\r\n            </div>\r\n          </div>\r\n        </div>\r\n      </div>\r\n\r\n\r\n    </div>\r\n  </div>\r\n</body>\r\n\r\n</html>";

            return emailBody;
        }

        private static bool sendEmail(string email, string subject, string body)
        {
            bool success = false;
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress("adnocforgadcoursework@outlook.com");
                message.To.Add(new MailAddress(email));
                message.Subject = subject;
                message.IsBodyHtml = true; //to make message body as html
                message.Body = body;
                smtp.Port = 587;
                smtp.Host = "smtp.outlook.com"; //for gmail host
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("adnocforgadcoursework@outlook.com", "#gad123456");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
                success = true;

            }
            catch (Exception)
            {
                success = false;
            }

            return success;
        }
    }
}
