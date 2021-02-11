using Coriunder.Models;
using System;
using System.Drawing;

namespace Coriunder
{
    public partial class TransResult : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            TransactionResult transactionResult = (TransactionResult)Session["TransactionResult"];
            if ( transactionResult != null )
            {
                if (transactionResult.Code == "000")
                {
                    lblDear.ForeColor = Color.ForestGreen;
                    lblCode.ForeColor = Color.ForestGreen;
                    lblDescription.ForeColor = Color.ForestGreen; 
                }
                else
                {
                    lblDear.ForeColor = Color.Black;
                    lblCode.ForeColor = Color.Red;
                    lblDescription.ForeColor = Color.Red;
                }
                
                lblCode.Text = transactionResult.Code;
                lblDescription.Text = transactionResult.Description;
            }
            
        }
    }
}