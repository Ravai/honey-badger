<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div class="container_full">
    Please Log In below:<br />
    <table>
        <tr>
            <td align="right" style="color:Black;">UserName:</td>
            <td align="center"><asp:TextBox runat="server" ID="txt_UserName" /></td>
        </tr>
        <tr>
            <td align="right" style="color:Black;">Password:</td>
            <td align="center"><asp:TextBox runat="server" ID="txt_password" TextMode="Password" /></td>
        </tr>
    </table>
    <asp:Button runat="server" ID="btn_Submit" OnClick="btn_Submit_OnClick" Text="Login" /><br />
    <br />
    <asp:Label runat="server" ID="lbl_FailMessage" ForeColor="Red" Font-Bold="true" /><br />

    <br /><br />
    If you have not yet REGISTERED, please do so <a href="Register.aspx">HERE</a>.<br />
</div>

</asp:Content>

