﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.master" AutoEventWireup="true" CodeFile="Threads.aspx.cs" Inherits="Threads" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <asp:LinkButton runat="server" ID="lnk_ReturnToBoard" Text="Return to Board" /><br /><br />
    
    <div style="font-size:medium;">
    <asp:LinkButton ID="projectName" ForeColor="Black" CssClass="google2 blue button" runat="server" /> -> <asp:LinkButton runat="server" ForeColor="Black" CssClass="google2 blue button" ID="boardName" /> -> <asp:LinkButton runat="server" CssClass="google2 blue button" ForeColor="Black" ID="lbl_ThreadName" /><br />
    </div>
    <hr />
    <asp:Literal runat="server" ID="lit_Posts"/><br />
    <hr />

    <asp:Panel runat="server" ID="pnl_AddPost">
        <a href="#addPost" class="google2 button">Create New Post</a>
    
        <div id="addPost" class="modalDialog">
	        <div>
		        <a href="#close" title="Close" class="close">X</a>

                <h3>Add a new Post</h3>
                <hr />
                <asp:TextBox runat="server" ID="txt_newPost" TextMode="MultiLine" Rows="4" Width="100%" /><br />
                <asp:Button runat="server" ID="btn_newPost" Text="Submit New Post" OnClick="btn_newPost_OnClick" />
                <asp:Button runat="server" ID="btn_CancelPost" Text="Cancel" />
            </div>
        </div>
    </asp:Panel>

</asp:Content>

