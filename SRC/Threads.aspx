<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.master" AutoEventWireup="true" CodeFile="Threads.aspx.cs" Inherits="Threads" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <asp:LinkButton runat="server" ID="lnk_ReturnToBoard" Text="Return to Board" /><br /><br />
    <!--<div class="fullContainer" style="background-color:Green; color:White; font-size:large;">-->
    
    <div style="font-size:medium;">
    <asp:LinkButton ID="projectName" ForeColor="Black" runat="server" /> -> <asp:LinkButton runat="server" ForeColor="Black" ID="boardName" /> -> <asp:LinkButton runat="server" ForeColor="Black" ID="lbl_ThreadName" /><br />
    </div>

    <!--</div>-->

    <asp:Table runat="server" ID="tbl_Posts" Width="100%" /><br />
    <br />
    <asp:Panel runat="server" ID="pnl_AddPost1">
        <asp:LinkButton runat="server" ID="lnkbtn_ShowAddPost" Text="[+] Do you want to add a new Post?" CssClass="ShowAdd_Button" OnClick="lnkbtn_ShowAddPost_OnClick" />
    </asp:Panel>
    <asp:Panel runat="server" ID="pnl_AddPost2" Visible="false">
        Submit a new Post:<br />
        <asp:TextBox runat="server" ID="txt_newPost" TextMode="MultiLine" Rows="4" Width="700px" /><br />
        <asp:Button runat="server" ID="btn_newPost" Text="Submit New Post" OnClick="btn_newPost_OnClick" />
        <asp:Button runat="server" ID="btn_CancelPost" Text="Cancel" OnClick="btn_CancelNewPost_OnClick" />
    </asp:Panel>

</asp:Content>

