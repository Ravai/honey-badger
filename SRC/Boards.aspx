<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.master" AutoEventWireup="true" CodeFile="Boards.aspx.cs" Inherits="Boards" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <asp:LinkButton runat="server" ID="lnk_ReturnToProject" Text="Return to Project" /><br /><br />
    <!--<div class="fullContainer" style="background-color:Black; color:White; font-size:large;">-->
    
    <div style="font-size:medium;">
    <asp:LinkButton ID="projectName" ForeColor="Black" CssClass="google2 blue button" runat="server" /> -> <asp:LinkButton runat="server" ForeColor="Black" CssClass="google2 blue button" ID="boardName" /><br />
    </div>
    <!--/div>-->
    <hr />

    <asp:Literal runat="server" ID="lit_ThreadList" />
    <hr />
    
    <asp:Panel runat="server" ID="pnl_addThread">
        <a href="#addThread" class="google2 button">Create New Thread</a>
    
        <div id="addThread" class="modalDialog">
	        <div>
		        <a href="#close" title="Close" class="close">X</a>

                <h3>Add a new Thread</h3>
                <hr />
                Subject:<br />
                <asp:TextBox runat="server" ID="txt_Subject" Width="100%" /><br />
                Description:<br />
                <asp:TextBox runat="server" ID="txt_Description" Width="100%" TextMode="MultiLine" Rows="3" /><br />
                <asp:Button runat="server" ID="btn_newThread" OnClick="btn_newThread_OnClick" Text="Add New Thread" />
                <asp:Button runat="server" ID="btn_CancelNewThread" Text="Cancel" /><br />
            </div>
        </div>
    </asp:Panel>

</asp:Content>

