<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ViewWip.aspx.cs" Inherits="ViewWip" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div>
        <a href="Home.aspx">Return to Main</a>
    </div>

    <div class="fullContainer">
    <div style="font-size:Medium; font-weight:bold;">Tasks in WIP</div> <i>(Task has been started)</i><br />
    <asp:Table runat="server" Width="100%" ID="tbl_WipTaskList" />
    </div><br />

</asp:Content>

