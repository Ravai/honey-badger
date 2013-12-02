<%@ Page Title="" Debug="true" Language="C#" MasterPageFile="~/Site2.master" AutoEventWireup="true" CodeFile="ViewTask.aspx.cs" Inherits="ViewTask" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

        
        <asp:Panel runat="server" ID="pnl_MainHeader">
        <h1><asp:Label runat="server" ID="lbl_TaskName" /></h1>
        <h2><asp:Label runat="server" ID="lbl_Description" /></h2>

        <asp:Literal runat="server" ID="progress_Header" />
        <%--<asp:Label runat="server" style="float:right; font-size:xx-large;" ID="lbl_totalPercentComplete" Visible="false" />--%>
        
        </asp:Panel>
        
        <hr />
    <div class="widget">
        <ul id="myTab" class="nav nav-tabs three-tabs fancy">
            <li class="active"><a href="#Timelines" data-toggle="tab">Project Details</a></li>
	        <li><a href="#SpecialOptions" data-toggle="tab">Special Operations</a></li>
            <li><a href="#DiscussionBoard" data-toggle="tab">Discussion</a></li>
        </ul>
        <div class="tab-content">
	        <div class="tab-pane fade in active" id="Timelines">
                <asp:Panel runat="server" ID="Panel2">
                <table>
                <tr><td><div style="font-weight:bold;">Project Owner: </div></td><td><asp:Label runat="server" ID="lbl_projectOwner" /></td></tr>
                <tr><td colspan="2"><hr /></td></tr>
                <tr><td><div style="font-weight:bold;">Expected Start:</div></td><td><asp:Label runat="server" ID="lbl_ExpectedStart" /></td></tr>
                <tr><td><div style="font-weight:bold;">Expected Completion:</div></td><td><asp:Label runat="server" ID="lbl_ExpectedStop" /></td></tr>
                <tr><td colspan="2"></td></tr>
                <tr><td><div style="font-weight:bold;">Actual Start:</div></td><td><asp:Label runat="server" ID="lbl_ActualStart" /></td></tr>
                <tr><td><div style="font-weight:bold;">Actual Completion:</div></td><td><asp:Label runat="server" ID="lbl_ActualStop" /></td></tr>
                </table>
                </asp:Panel>
            </div>
            <div class="tab-pane fade" id="SpecialOptions">
                <asp:Panel ID="pnl_SpecialOptions" runat="server">
                <asp:Panel runat="server" ID="btn_markDone"><asp:LinkButton ID="LinkButton1" runat="server" OnClick="btn_markDone_OnClick" CssClass="specialOperationsButton" Text="Mark Task as Done" /></asp:Panel>
                <asp:Panel runat="server" ID="btn_markWip" Visible="false"><asp:LinkButton ID="LinkButton2" runat="server" OnClick="btn_markWip_OnClick" CssClass="specialOperationsButton" Text="Re-Open Task" /></asp:Panel>
                <asp:Panel runat="server" ID="btn_startTask" Visible="false"><asp:LinkButton ID="LinkButton3" runat="server" OnClick="btn_startTask_OnClick" CssClass="specialOperationsButton" Text="Start Task" /></asp:Panel>
                <asp:Panel runat="server" ID="btn_UpgradeSize" Visible="false"><asp:LinkButton ID="LinkButton4" runat="server" OnClick="btn_UpgradeSize_OnClick" CssClass="specialOperationsButton" Text="Make Project a Large Project" /></asp:Panel>
                
                <br />
                <asp:Panel runat="server" ID="pnl_EditOperations">
                <asp:LinkButton runat="server" ID="btn_ViewProjectReport" CssClass="specialOperationsButton floatright" Enabled="true" Text="View Project Report" />
                <asp:LinkButton runat="server" PostBackUrl="#UpdateProjectName" CssClass="specialOperationsButton" Text="Update Project Name and Description" /><br /><br />
                <asp:LinkButton runat="server" ID="btn_Edit_Sharing" CssClass="specialOperationsButton" OnClick="btn_Edit_Sharing_OnClick" Enabled="true" Text="Edit Project Sharing" /><br /><br />
                <asp:LinkButton runat="server" ID="btn_Delete_Project" CssClass="specialOperationsButton" OnClick="btn_Delete_Project_OnClick" OnClientClick="return confirm('Are you sure you want to delete this project?');" Enabled="true" Text="Delete this Project" /><br />
                </asp:Panel>
                </asp:Panel>
            </div>
            <div class="tab-pane fade" id="DiscussionBoard">
                <h2><asp:LinkButton runat="server" ID="lnk_GoToDiscussion" CssClass="specialOperationsButton" Text="Go to Discussion Boards!" /></h2>
            </div>
        </div>
        
    </div>

    <hr />

    <div class="row-fluid">
        <h2>Project Milestones and Objectives!</h2>
        <a href="#addMilestone" class="addNewMilestone">Add a New Milestone</a><br /><br />

        <asp:Literal runat="server" ID="lit_Milestones" />
        
    </div>


    <div id="addMilestone" class="modalDialog">
	    <div>
		    <a href="#close" title="Close" class="close">X</a>
            Name:<br />
            <asp:TextBox runat="server" ID="txt_Milestone_Name" Width="300px" /><br />
            Description:<br />
            <asp:TextBox runat="server" ID="txt_Milestone_Desc" Width="400px" TextMode="MultiLine" Rows="4" /><br />
            Weight (1 is lowest): 
            <asp:DropDownList runat="server" ID="ddl_Milestone_Weight" Width="100px">
                <asp:ListItem Text="1" Value="1" Selected="True" />
                <asp:ListItem Text="2" Value="2" />
                <asp:ListItem Text="3" Value="3" />
                <asp:ListItem Text="4" Value="4" />
                <asp:ListItem Text="5" Value="5" />
            </asp:DropDownList>
            <asp:Button runat="server" ID="btn_AddMilestone_Final" OnClick="btn_AddMilestone_Final_OnClick" Text="Add Milestone" />
            
        </div>
    </div>
    <br />

    <asp:Panel runat="server" ID="pnl_Comments" Visible="false">
    <asp:Panel runat="server" ID="AddComments">
    <div class="fullContainer">
        <strong><u>What's New?</u></strong>
        <asp:TextBox runat="server" ID="txt_addNewComment" Width="400px" Rows="4" />
        <asp:Button runat="server" Text="Add" ID="btn_AddComment" OnClick="btn_AddComment_OnClick" />
    </div>
    </asp:Panel>

    <br />

    <div class="fullContainer">
        <asp:Table runat="server" Width="100%" ID="tbl_Comments" />
    </div>

    <br />
    </asp:Panel>

    <div id="UpdateProjectName" class="modalDialog">
	    <div>
		    <a href="#close" title="Close" class="close">X</a>
            <div class="modalTitles">Update Project Name and Description</div>
            <strong>Name:</strong> <asp:TextBox runat="server" ID="txt_Edit_TaskName" Width="100%" /><br />
            <strong>Description</strong><asp:TextBox runat="server" ID="txt_Edit_TaskDescription" Width="100%" TextMode="MultiLine" Rows="3" /><br />
            <asp:Button runat="server" ID="btn_Update_NameDescription" Text="Update" OnClick="btn_Update_NameDescription_OnClick" /><br />
        </div>
    </div>

    <div id="addChildFeature" class="modalDialog">
	    <div>
		    <a href="#close" title="Close" class="close">X</a>
            <div class="modalTitles">For Adding a Sub-Objective</div>
            <hr />
            <strong>Name: </strong><asp:TextBox runat="server" ID="txt_addChildFeature_Name" placeholder="Enter name..." /><br />
            <strong>Description: </strong><asp:TextBox runat="server" ID="txt_addChildFeature_Description" placeholder="Enter description..." /><br />
            <strong>Weight: </strong><asp:DropDownList runat="server" ID="ddl_addChildFeature_Weight" /><br />
            <br />
            <asp:Button runat="server" ID="btn_addChildFeature" OnClick="btn_addChildFeature_OnClick" Text="Add Child Feature" />
            <asp:Button runat="server" PostBackUrl="#" Text="Cancel" />
        </div>
    </div>
    
    <div id="removeChildFeature" class="modalDialog">
	    <div>
		    <a href="#" title="Close" class="close">X</a>
            <div class="modalTitles">For Removing a Child Feature</div>
            <hr />
            <strong>Name: </strong><asp:Label runat="server" ID="lbl_remChildFeature_Name" /><br />
            <strong>Description: </strong><asp:Label runat="server" ID="lbl_remChildFeature_Description" /><br />
            <strong>% Complete: </strong><asp:Label runat="server" ID="lbl_remChildFeature_PercentComplete" /><br />
            <br />
            Are you sure you want to Delete this Objective?<br />
            <asp:Button ID="btnRemFeature" runat="server" OnClick="btnRem_OnClick" Text="Yes" />
            <asp:Button runat="server" Text="No" PostBackUrl="#" />
        </div>
    </div>

    <div id="editChildFeature" class="modalDialog">
	    <div>
		    <a href="#close" title="Close" class="close">X</a>
            <div class="modalTitles">For Editing a Child Feature</div>
            <hr />
            <strong>Name: </strong><asp:TextBox runat="server" ID="txt_editChildFeature_Name" /><br />
            <strong>Description: </strong><asp:TextBox runat="server" ID="txt_editChildFeature_Description" /><br />
            <strong>Weight: </strong><asp:DropDownList runat="server" ID="ddl_editChildFeature_Weight" /><br />
            <strong>% Complete: </strong><asp:DropDownList runat="server" ID="ddl_editChildFeature_PercentComplete" /><br />
            <br />
            <asp:Button ID="btnEditFeature" runat="server" OnClick="btn_EditChildFeature_OnClick" Text="Update" />
            <asp:Button runat="server" Text="Cancel" PostBackUrl="#" />
        </div>
    </div>

    <div id="quickComplete" class="modalDialog">
        <div>
            <a href="#close" title="Close" class="close">X</a>
            <div class="modalTitles">Quick Complete!</div>
            <hr />
            <strong>Name: </strong><asp:Label runat="server" ID="lbl_quickComplete_Name" /><br />
            <strong>Description: </strong><asp:Label runat="server" ID="lbl_quickComplete_Description" /><br />
            <strong>% Complete: </strong><asp:Label runat="server" ID="lbl_quickComplete_PercentComplete" /><br />
            <br />
            <asp:Button ID="btnQuickComplete" runat="server" OnClick="btnQuickComplete_OnClick" Text="Complete!" />
            <asp:Button ID="Button2" runat="server" Text="Cancel" PostBackUrl="#" />
        </div>
    </div>

</asp:Content>
