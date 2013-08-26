<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Boards.aspx.cs" Inherits="Boards" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <asp:LinkButton runat="server" ID="lnk_ReturnToProject" Text="Return to Project" /><br /><br />
    <!--<div class="fullContainer" style="background-color:Black; color:White; font-size:large;">-->
    
    <div style="font-size:medium;">
    <asp:LinkButton ID="projectName" ForeColor="Black" runat="server" /> -> <asp:LinkButton runat="server" ForeColor="Black" ID="boardName" /><br />
    </div>
    <!--/div>-->

    <asp:Table runat="server" ID="tbl_Threads" Width="100%" /><br />
    
    <br />
    <asp:Panel runat="server" ID="pnl_AddThread1">
        <asp:LinkButton runat="server" ID="lnkbtn_ShowAddThread" Text="[+] Do you want to add a new Thread?" CssClass="ShowAdd_Button" OnClick="lnkbtn_ShowAddThread_OnClick" />
    </asp:Panel>
    <asp:Panel runat="server" ID="pnl_AddThread2" Visible="false">
        <strong>Add a new Thread</strong><br />
        Subject:<br />
        <asp:TextBox runat="server" ID="txt_Subject" Width="400px" /><br />
        Description:<br />
        <asp:TextBox runat="server" ID="txt_Description" Width="600px" TextMode="MultiLine" Rows="3" /><br />
        <asp:Button runat="server" ID="btn_newThread" OnClick="btn_newThread_OnClick" Text="Add New Thread" />
        <asp:Button runat="server" ID="btn_CancelNewThread" OnClick="btn_CancelNewThread_OnClick" Text="Cancel" /><br />
    </asp:Panel>

</asp:Content>

