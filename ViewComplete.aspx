<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ViewComplete.aspx.cs" Inherits="ViewComplete" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div>
        <a href="Home.aspx">Return to Main</a>
    </div>

    <div class="fullContainer">
    <div style="font-size:Medium; font-weight:bold;">Completed Tasks</div> <i>(Task marked as Done)</i><br />
    <asp:Table runat="server" Width="100%" ID="tbl_CompletedTaskList" />
    </div><br />

</asp:Content>

