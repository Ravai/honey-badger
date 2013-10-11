<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.master" AutoEventWireup="true" CodeFile="Register.aspx.cs" Inherits="Register" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div class="container_full">
    <h2>Register</h2>
    <div style="color:red; font-size:x-small;">* - denotes Required field</div>
    
    <h3>User Information</h3>
    <asp:TextBox runat="server" ID="txt_UserName" placeholder="Enter a unique user name..." /><font color="red">*</font><br />
    
    <asp:TextBox runat="server" ID="txt_FirstName" placeholder="Your first name..." />
    <asp:TextBox runat="server" ID="txt_MI" placeholder="Middle Initial/Name..." />
    <asp:TextBox runat="server" ID="txt_LastName" placeholder="Last name..." /><font color="red">*</font><br />

    <asp:TextBox runat="server" ID="txt_Email" placeholder="Enter your email address..." /><font color="red">*</font><br />
    <asp:TextBox runat="server" ID="txt_Phone" placeholder="Enter your phone number..." /><br />
    <br />
    <table>
        <tr>
            <td colspan="2"><strong>Display Name:</strong></td>
        </tr>
        <tr>
            <td style="padding:10px;" width="50%">
                Your Full Name&nbsp;<asp:RadioButton runat="server" ID="radio_Name" GroupName="displayname" />
            </td>
            <td style="padding:10px;" width="50%">
                User Name&nbsp;<asp:RadioButton runat="server" ID="radio_UserName" GroupName="displayname" />
            </td>
        </tr>
    </table>
      

    <h3>Associations</h3>
    No associations as of yet...<br />

    <h3>Password Creation</h3>
    <asp:TextBox runat="server" ID="txt_Password1" TextMode="Password" placeholder="Enter A Password..." /><font color="red">*</font><br />
    <asp:TextBox runat="server" ID="txt_Password2" TextMode="Password" placeholder="Enter the password again..." /><font color="red">*</font><br />

    <br />

    <asp:Button runat="server" Text="Submit Registration" ID="btn_Submit" OnClick="btn_Submit_OnClick" /><br />
    <br />
    <asp:Label runat="server" ID="lbl_FailMessage" ForeColor="Red" Font-Bold="true" />
</div>

</asp:Content>

