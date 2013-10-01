<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.master" AutoEventWireup="true" CodeFile="Register.aspx.cs" Inherits="Register" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div class="container_full">
    Thank you for wishing to register with this Tracking Tool<br />
    <br />
    Please fill out the below info to get yourself registered.<br />
    <br />
    * Indicates required field
    <table style="color:Black;">
        <tr>
            <td>
                Choose a UserName *: 
            </td>
            <td>
                <asp:TextBox runat="server" ID="txt_UserName" />
            </td>
        </tr>
        <tr>
            <td>
                Choose a Display Name: 
            </td>
            <td>
                <asp:TextBox runat="server" ID="txt_DisplayName" />
            </td>
        </tr>
        <tr>
            <td>
                Create a Password: * 
            </td>
            <td>
                <asp:TextBox runat="server" ID="txt_Password1" TextMode="Password" />
            </td>
        </tr>
        <tr>
            <td>
                Validate the Password: * 
            </td>
            <td>
                <asp:TextBox runat="server" ID="txt_Password2" TextMode="Password" />
            </td>
        </tr>
        <tr>
            <td>
                Email address:
            </td>
            <td>
                <asp:TextBox runat="server" ID="txt_EmailAddress" />
            </td>
        </tr>
    </table>
    <asp:Button runat="server" Text="Submit Registration" ID="btn_Submit" OnClick="btn_Submit_OnClick" /><br />
    <br />
    <asp:Label runat="server" ID="lbl_FailMessage" ForeColor="Red" Font-Bold="true" />
</div>

</asp:Content>

