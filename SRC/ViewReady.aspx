<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.master" AutoEventWireup="true" CodeFile="ViewReady.aspx.cs" Inherits="ViewReady" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <h1>Tasks that are Ready to Go!</h1>
    
    <ul class="cards">
    <asp:Literal runat="server" ID="ReadyList" />
    </ul>

</asp:Content>

