<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.master" AutoEventWireup="true" CodeFile="ChangePassword.aspx.cs" Inherits="ChangePassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div class="container_full">
    <h2>Change Password</h2>
    
    <asp:TextBox runat="server" ID="txt_currentPassword" TextMode="Password" placeholder="Current Password" /><br />
    <asp:TextBox runat="server" ID="txt_newPassword" TextMode="Password" placeholder="New Password" /><br />
    <asp:TextBox runat="server" ID="txt_newPasswordVerify" TextMode="Password" placeholder="Verify New Password" /><br />
    
    <asp:Button runat="server" ID="btn_SubmitPWChange" OnClick="btn_SubmitPWChange_OnClick" Width="100px" Text="Save" />
    <br />
    <asp:Label runat="server" ID="lbl_FailMessage" ForeColor="Red" Font-Bold="true" /><br />
    <asp:Label runat="server" ID="lbl_Success" />
</div>

</asp:Content>

