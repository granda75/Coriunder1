using Coriunder.Models;
using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Coriunder
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblDetailsReview.Visible = false;
            if (!Page.IsPostBack)
            {
                FillsMonthDropDown();
                FillCountryDropDown();
                FillYearsDropDown();
            }
        }

        private void FillYearsDropDown()
        {
            ddlExpYear.Items.Add(new ListItem("2021"));
            ddlExpYear.Items.Add(new ListItem("2022"));
            ddlExpYear.Items.Add(new ListItem("2023"));
            ddlExpYear.Items.Add(new ListItem("2024"));
        }

        private void FillsMonthDropDown()
        {
            var months = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;
            for (int i = 0; i < months.Length; i++)
            {
                ddlExpMonth.Items.Add(new ListItem(months[i], i.ToString()));
            }
        }

        private void FillCountryDropDown()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("Name");

            DataRow row = dt.NewRow();
            row["Id"] = "IL";
            row["Name"] = "Israel";
            dt.Rows.Add(row);
            row = dt.NewRow();
            row["Id"] = "RU";
            row["Name"] = "Russia";
            dt.Rows.Add(row);
            row = dt.NewRow();
            row["Id"] = "AT";
            row["Name"] = "Austria";
            dt.Rows.Add(row);

            ddlCountry.DataSource = dt;
            ddlCountry.DataTextField = "Name";
            ddlCountry.DataValueField = "Id";
            ddlCountry.DataBind();
        }

        
        public string GetIPAddress()
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName()); // `Dns.Resolve()` method is deprecated.
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            return ipAddress.ToString();
        }

        private void SilentPostCharge(TransactionsData data)
        {
            //------------- building url string to send
            String sendStr;
            sendStr = "https://process.coriunder.cloud/member/remote_charge.asp?";
            sendStr += "CompanyNum=" + HttpUtility.UrlEncode(data.CompanyNumber) + "&";
            sendStr += "TransType=" + HttpUtility.UrlEncode(Convert.ToInt32(TransactionType.Debit).ToString()) + "&";
            sendStr += "ClientIP=" + HttpUtility.UrlEncode(data.ClientIp) + "&";
            sendStr += "CardNum=" + HttpUtility.UrlEncode(data.CardNumber) + "&";
            sendStr += "ExpMonth=" + HttpUtility.UrlEncode(data.Month.ToString()) + "&";
            sendStr += "ExpYear=" + HttpUtility.UrlEncode(data.Year.ToString()) + "&";
            sendStr += "Member=" + HttpUtility.UrlEncode(data.CardHolderName.ToString()) + "&";
            sendStr += "TypeCredit=" + HttpUtility.UrlEncode(Convert.ToInt32(TypeCredit.Debit).ToString()) + "&";
            sendStr += "Payments=" + HttpUtility.UrlEncode("1") + "&";            // 1 - for regular transaction

            sendStr += "Amount=" + HttpUtility.UrlEncode(data.Amount.ToString()) + "&";
            sendStr += "Currency=" + HttpUtility.UrlEncode(data.Currency.ToString()) + "&";
            sendStr += "CVV2=" + HttpUtility.UrlEncode(data.Cvv.ToString()) + "&";
            sendStr += "Email=" + HttpUtility.UrlEncode(data.Email) + "&";
            sendStr += "PhoneNumber=" + HttpUtility.UrlEncode(data.Phone) + "&";
                        
            sendStr += "BillingAddress1=" + HttpUtility.UrlEncode(data.BillingAddress) + "&";
            sendStr += "BillingCity=" + HttpUtility.UrlEncode(data.City) + "&";
            sendStr += "BillingZipCode=" + HttpUtility.UrlEncode(data.ZipCode) + "&";
            sendStr += "BillingCountry=" + HttpUtility.UrlEncode(data.CountryCode) + "&";

            //Signature 
            string signature = data.CompanyNumber + TransactionType.ChargeCC.ToString() + ((int)TypeCredit.Refund).ToString() +
                               data.Amount.ToString() + data.Currency.ToString() + data.CardNumber + data.RefTransID + data.PersonalHashKey;
            string shaSignature = Signature.GenerateSHA256(signature);
            string encodedTo64 = Signature.EncodeTo64(shaSignature);
            sendStr += "Signature=" + HttpUtility.UrlEncode(encodedTo64);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(sendStr);
            webReq.Method = "GET";
           
            try
            {
                HttpWebResponse webRes = (HttpWebResponse)webReq.GetResponse();
                StreamReader sr = new StreamReader(webRes.GetResponseStream());
                string resStr = sr.ReadToEnd();
                
                if (!string.IsNullOrEmpty(resStr))
                {
                    TransactionResult transResult = new TransactionResult();
                    string[] resParts = resStr.Split('&');
                    if (resParts != null && resParts.Length > 0)
                    {
                        string reply = resParts.ToList().Where(l => l.StartsWith("Reply")).Select(l => l).FirstOrDefault();
                        string[] replyParts = reply.Split('=');
                        if ( replyParts?.Length > 1 )
                        {
                            transResult.Code = replyParts[1];
                        }
                        string replyDesc = resParts.ToList().Where(l => l.StartsWith("ReplyDesc")).Select(l => l).FirstOrDefault();
                        string[] descParts = replyDesc.Split('=');
                        if (descParts?.Length > 1)
                        {
                            transResult.Description = descParts[1];
                        }
                    }
                    Session["TransactionResult"] = transResult;
                }
               
                Response.Redirect("TransResult.aspx");
               
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void btnContinue_Click(object sender, EventArgs e)
        {
            txtCardholderName.Enabled = false;
            txtEmail.Enabled = false;
            txtCardNumber.Enabled = false;
            ddlExpMonth.Enabled = false;
            ddlExpYear.Enabled = false;
            txtCvv.Enabled = false;
            txtAddress.Enabled = false;
            txtCity.Enabled = false;
            txtZipCode.Enabled = false;
            ddlCountry.Enabled = false;
            txtPhone.Enabled = false;

            this.btnContinue.Visible = false;
            btnBack.Visible = true;
            btnPay.Visible = true;
            lblDetailsReview.Visible = true;

        }

        private TransactionsData FillTransactionsData()
        {
            string merchantId = WebConfigurationManager.AppSettings["MerchantId"];
            string personalHashKey = WebConfigurationManager.AppSettings["PersonalHashKey"];

            TransactionsData data = new TransactionsData();
            data.CompanyNumber = merchantId;    //merchant id
            data.CardHolderName = txtCardholderName.Text;
            data.Email = txtEmail.Text;
            data.CardNumber = txtCardNumber.Text;
            data.Month = Convert.ToInt32(ddlExpMonth.SelectedValue);
            data.Year = this.ddlExpYear.SelectedValue;
            data.Cvv = txtCvv.Text;
            data.BillingAddress = txtAddress.Text;
            data.City = txtCity.Text;
            data.ZipCode = txtZipCode.Text;
            data.CountryCode = ddlCountry.SelectedValue;
            data.Phone = txtPhone.Text;
            data.Amount = 2.00m;          //Hardcoded for the exam, the value must be transferred from Order details screen
            data.Currency = "ILS";        //ILS (Israel New Shekel)
            data.PersonalHashKey = personalHashKey;
            data.ClientIp = "1.2.3.4";
            data.RefTransID = "1234";
            return data;
        }

        protected void btnPay_Click(object sender, EventArgs e)
        {
            TransactionsData data = FillTransactionsData();
            SilentPostCharge(data);
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            txtCardholderName.Enabled = true;
            txtEmail.Enabled = true;
            txtCardNumber.Enabled = true;
            ddlExpMonth.Enabled = true;
            ddlExpYear.Enabled = true;
            txtCvv.Enabled = true;
            txtAddress.Enabled = true;
            txtCity.Enabled = true;
            txtZipCode.Enabled = true;
            ddlCountry.Enabled = true;
            txtPhone.Enabled = true;

            this.btnContinue.Visible = true;
            btnBack.Visible = false;
            btnPay.Visible = false;
        }
    }
}