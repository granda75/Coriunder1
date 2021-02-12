<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Coriunder._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
   <script src="Scripts/Validations.js"></script>
   
       <br>
        <div style="margin auto; text-align:center">
            <asp:Label ID="lblDetailsReview" runat="server" Text="Details review" Font-Size="XX-Large" ForeColor="#009933"></asp:Label>
        </div>

            <div id="paymentsId">
            <div class="container" style="width:450px">
                <div class="row">
                  <asp:ValidationSummary ID="ValidationSummary1" runat="server" BackColor="#FFCECE" DisplayMode="SingleParagraph" />
                </div>
                <div class="row">
                    <div class="col-md-12">
                        Cardholder Name: *
                    </div>
                    <div class="col-md-12">
                        <asp:TextBox ID="txtCardholderName" style="width:100%" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtCardholderName" ErrorMessage="*Cardholder name " Display="None"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row">
                     <div class="col-md-12">
                        Email Address: 
                     </div>
                    <div class="col-md-12">
                        <asp:TextBox ID="txtEmail" style="width:100%"  runat="server"></asp:TextBox>
                    </div>
                </div>
               
                <div class="row">
                    <div class="col-md-12">
                         Phone Number: *
                    </div>
                    <div class="col-md-12">
                         <asp:TextBox ID="txtPhone" style="width:100%" runat="server"></asp:TextBox>
                         <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtPhone" ErrorMessage="*Phone Number " Display="None"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <hr>

                <div class="row">
                    <div class="col-md-12">
                        Card Number: *
                    </div>
                    <div class="col-md-12">
                        <asp:TextBox ID="txtCardNumber" style="width:100%" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtCardNumber" ErrorMessage="*Card Number " Display="None"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <table>
                            <tr>
                                <td> <asp:Label ID="Label1" runat="server" Text="Exp Date: *"></asp:Label> </td>
                                <td><asp:Label ID="Label2" runat="server" Text="CVV:" style="margin-left:4.5em"></asp:Label> </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>   <asp:DropDownList id="ddlExpMonth" runat="server"></asp:DropDownList> 
                                       <asp:DropDownList id="ddlExpYear" runat="server"></asp:DropDownList>  
                                        &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlExpMonth" ErrorMessage=" *Card expiration month " Display="None"></asp:RequiredFieldValidator>
                                       <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlExpYear" ErrorMessage=" *Card expiration year " Display="None"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCvv" runat="server" style="margin-left:4.5em"></asp:TextBox>
                                </td>
                                <td></td>
                            </tr>
                        </table>
                    </div>
                   
                </div>   
                <hr>
                <div class="row">
                    <div class="col-md-12">
                        Address 1:
                    </div>
                    <div class="col-md-12">
                        <asp:TextBox ID="txtAddress" style="width:100%" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <table>
                            <tr>
                                <td><asp:Label ID="lblCity" runat="server" Text="City:"></asp:Label> </td>
                            </tr>
                            <tr>
                                <td><asp:TextBox ID="txtCity" runat="server"></asp:TextBox> </td>
                            </tr>
                        </table>
                    </div>

                    <%--<div class="col-md-6">
                    </div>
      
                    <div class="col-md-6">
                    </div>--%>

                    <div class="col-md-6">
                        <table>
                            <tr>
                                <td><asp:Label ID="lblZipCode" runat="server" Text="Zip Code:"></asp:Label>  </td>  
                            </tr>
                            <tr>
                                <td> <asp:TextBox ID="txtZipCode" runat="server"></asp:TextBox> </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="row">
                     <div class="col-md-12">
                         <asp:Label ID="lblCountry" runat="server" Text="Country:"></asp:Label>
                     </div>
                     <div class="col-md-6">
                        
                         <asp:DropDownList ID="ddlCountry" runat="server" style="width:100%"></asp:DropDownList>
                     </div>
                    
                </div>
          
               
                <div class="row">
                    <div class="col-md-12">  
                        <asp:Button id="btnContinue"   runat="server" Text="CONTINUE"  OnClientClick="return Validate()" OnClick="btnContinue_Click" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <asp:Button ID="btnBack" class="btnBackpay" runat="server" Text="BACK" Visible="False" OnClick="btnBack_Click" /> 
                    </div>
                    <div class="col-md-6">
                        <asp:Button ID="btnPay" class="btnBackpay" runat="server" Text="PAY" Visible="False" OnClick="btnPay_Click"  OnClientClick="return confirm('Are you sure do you want to pay?')"  />
                    </div>     
                </div>
            </div>
       </div>

</asp:Content>
