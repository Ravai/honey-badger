<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.master" AutoEventWireup="true" CodeFile="UserProfile.aspx.cs" Inherits="UserProfile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="container_full">
    <asp:Label runat="server" ID="lbl_userName" /><br />

    <table>
        <tr>
            <td align="right" style="color:Black;">Name: </td>
            <!-- <td align="center"><asp:Label runat="server" ID="lbl_name" /></asp:Label></td> -->
        </tr>
        <tr>
            <td align="right" style="color:black;">Email: </td>
            <td align="center"><asp:Label runat="server" ID="lbl_email" /></asp:Label></td>
        </tr>
        <tr>
            <td align="right" style="color:black;">Phone number: </td>
            <td align="center"><asp:Label runat="server" ID="lbl_phone" /></asp:Label></td>
        </tr>
        <tr>
            <td align="right" style="color:black;">Last online: </td>
            <td align="center"><asp:Label runat="server" ID="lbl_lastOnline" /></asp:Label></td>
        </tr>
    </table>
    
</div>
</asp:Content>
