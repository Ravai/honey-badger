<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.master" AutoEventWireup="true" CodeFile="ViewUpcoming.aspx.cs" Inherits="ViewUpcoming" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <h1>Tasks that are Up and Coming!</h1>
    
    <ul class="cards">
    <asp:Literal runat="server" ID="UpcomingList" />
    </ul>

</asp:Content>

