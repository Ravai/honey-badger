<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ViewReady.aspx.cs" Inherits="ViewReady" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div>
        <a href="Home.aspx">Return to Main</a>
    </div>

    <div class="fullContainer">
    <div style="font-size:Medium; font-weight:bold;">Ready Tasks</div> <i>(Task ready to be started)</i><br />
    <asp:Table runat="server" Width="100%" ID="tbl_ReadyTaskList" />
    </div><br />

</asp:Content>

