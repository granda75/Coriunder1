<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="TransResult.aspx.cs" Inherits="Coriunder.TransResult" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <%-- <br>
    <br>--%>
<div class="jumbotron" style="margin: 50px auto; text-align:center">
        <div class="row">
             
            <asp:Label ID="lblDear" runat="server" Text="Dear customer ! Payment transaction finished with the results" Font-Size="XX-Large"></asp:Label>
        </div>
        <div class="row" style="margin-left:150px; margin-right:150px;text-align:left">
           <%-- <div class="col-md-3"></div>--%>
            <div class="col-md-6">
                <h4>Code: <asp:Label ID="lblCode" runat="server" Text="Label"></asp:Label> </h4>
            </div>
             <div class="col-md-6">
                <h4> Description : <asp:Label ID="lblDescription" runat="server" Text=""></asp:Label> </h4>
             </div>
             <%--<div class="col-md-3"></div>--%>
        </div>
</div>
    
</asp:Content>