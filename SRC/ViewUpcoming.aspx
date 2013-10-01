<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.master" AutoEventWireup="true" CodeFile="ViewUpcoming.aspx.cs" Inherits="ViewUpcoming" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div>
        <a href="Home.aspx">Return to Main</a>
    </div>

    <div class="fullContainer">
    <div style="font-size:Medium; font-weight:bold;">Upcoming Tasks</div> <i>(Expected start is in the future)</i><br />
    <asp:Table runat="server" Width="100%" ID="tbl_UpcomingTaskList" />
    </div><br />

</asp:Content>

