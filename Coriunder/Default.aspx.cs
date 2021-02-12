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

        private string BuildUrlStringToSend(TransactionsData data)
        {
            string sendStr;
            string remoteChargeUrl = WebConfigurationManager.AppSettings["RemoteChargeUrl"];

            StringBuilder sb = new StringBuilder(remoteChargeUrl);
            sb.Append("CompanyNum=" + HttpUtility.UrlEncode(data.CompanyNumber) + "&");
            sb.Append("TransType=" + HttpUtility.UrlEncode(Convert.ToInt32(TransactionType.Debit).ToString()) + "&");
            sb.Append("ClientIP=" + HttpUtility.UrlEncode(data.ClientIp) + "&");
            sb.Append("CardNum=" + HttpUtility.UrlEncode(data.CardNumber) + "&");
            sb.Append("ExpMonth=" + HttpUtility.UrlEncode(data.Month.ToString()) + "&");
            sb.Append("ExpYear=" + HttpUtility.UrlEncode(data.Year.ToString()) + "&");
            sb.Append("Member=" + HttpUtility.UrlEncode(data.CardHolderName.ToString()) + "&");
            sb.Append("TypeCredit=" + HttpUtility.UrlEncode(Convert.ToInt32(TypeCredit.Debit).ToString()) + "&");
            sb.Append("Payments=" + HttpUtility.UrlEncode("1") + "&");         // 1 - for regular transaction
            sb.Append("Amount=" + HttpUtility.UrlEncode(data.Amount.ToString()) + "&");
            sb.Append("Currency=" + HttpUtility.UrlEncode(data.Currency.ToString()) + "&");
            sb.Append("CVV2=" + HttpUtility.UrlEncode(data.Cvv.ToString()) + "&");
            sb.Append("Email=" + HttpUtility.UrlEncode(data.Email) + "&");
            sb.Append("PhoneNumber=" + HttpUtility.UrlEncode(data.Phone) + "&");
            sb.Append("BillingAddress1=" + HttpUtility.UrlEncode(data.BillingAddress) + "&");
            sb.Append("BillingCity=" + HttpUtility.UrlEncode(data.City) + "&");
            sb.Append("BillingZipCode=" + HttpUtility.UrlEncode(data.ZipCode) + "&");
            sb.Append("BillingCountry=" + HttpUtility.UrlEncode(data.CountryCode) + "&");
            //Signature 
            string signature = data.CompanyNumber + TransactionType.ChargeCC.ToString() + ((int)TypeCredit.Refund).ToString() +
                               data.Amount.ToString() + data.Currency.ToString() + data.CardNumber + data.RefTransID + data.PersonalHashKey;
            string shaSignature = Signature.GenerateSHA256(signature);
            string encodedTo64 = Signature.EncodeTo64(shaSignature);
            sb.Append("Signature=" + HttpUtility.UrlEncode(encodedTo64));
            sendStr = sb.ToString();
            return sendStr;
        }

        private void SilentPostCharge(TransactionsData data)
        {
            string sendStr = BuildUrlStringToSend(data);
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
            if (string.IsNullOrEmpty(txtCardNumber.Text) || string.IsNullOrEmpty(txtPhone.Text) ||
                string.IsNullOrEmpty(txtCardholderName.Text))
            {
                return;
            }

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
            data.Month = (ddlExpMonth.SelectedValue != "0") ?  Convert.ToInt32(ddlExpMonth.SelectedValue) : 1;
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