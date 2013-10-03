<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div class="container_full">
    <h2>Login</h2>
    
    <asp:TextBox runat="server" ID="txt_UserName" placeholder="Enter your user name..." /><br />
    <asp:TextBox runat="server" ID="txt_password" TextMode="Password" placeholder="Enter your password..." /><br />
    
    <asp:Button runat="server" ID="btn_Submit" OnClick="btn_Submit_OnClick" Width="100px" Text="Login" />
    <asp:Button runat="server" PostBackUrl="~/Register.aspx" Width="100px" Text="Register" /><br />
    <asp:LinkButton runat="server" ID="btn_ForgotPass" Width="205px" Text="Forget Your Password?" />
    <br />
    <asp:Label runat="server" ID="lbl_FailMessage" ForeColor="Red" Font-Bold="true" /><br />
</div>

</asp:Content>

