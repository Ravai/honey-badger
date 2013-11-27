<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.master" AutoEventWireup="true" CodeFile="ForgotPassword.aspx.cs" Inherits="ForgotPassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container_full">
        <asp:Panel ID="formField" runat="server">
            <h2>Please Enter the Username and the Email associated with it.</h2><br />
            <asp:TextBox ID="username" runat="server" placeholder="UserName"></asp:TextBox>
            <br />
            <asp:TextBox ID="email" runat="server" placeholder="Email"></asp:TextBox>
            <br />
            <asp:Button ID="btn_submit" runat="server" Text="Submit" OnClick="btn_submit_Click" />
        </asp:Panel>
        
        <br />
        <asp:Label ID="statusMessage" runat="server"></asp:Label>
        <br />
        <asp:Button ID="btn_return" Text="Return" runat="server" PostBackUrl="~/Login.aspx" Visible="False"/>
    </div>
</asp:Content>

