<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.master" AutoEventWireup="true" CodeFile="ViewComplete.aspx.cs" Inherits="ViewComplete" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <h1>Tasks that are Completed!</h1>
    <ul class="cards">
    <asp:Literal runat="server" ID="CompletedList" />
    </ul>

</asp:Content>

