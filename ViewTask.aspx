<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ViewTask.aspx.cs" Inherits="ViewTask" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div>
        <a href="Home.aspx">Return to Main</a>
    </div>

    <div class="fullContainer" style="background-color:#303030;">
        <div style="border: 2px ridge black; padding:10px; background-color:#808080; color:White;" >
        <asp:Panel runat="server" ID="pnl_MainHeader">
        <asp:Label runat="server" style="float:right; font-size:xx-large;" ID="lbl_totalPercentComplete" Visible="false" />
        <asp:Label runat="server" CssClass="viewTask_Name" ID="lbl_TaskName" /><br />
        <asp:Label runat="server" CssClass="viewTask_Description" ID="lbl_Description" /><br />
        
        </asp:Panel>
        <asp:Panel runat="server" ID="pnl_Mainheader_Edit" Visible="false">
        <asp:TextBox runat="server" ID="txt_Edit_TaskName" Width="400px" /><br />
        <asp:TextBox runat="server" ID="txt_Edit_TaskDescription" Width="700px" TextMode="MultiLine" Rows="3" /><br />
        <asp:Button runat="server" ID="btn_Update_NameDescription" Text="Update" OnClick="btn_Update_NameDescription_OnClick" /><br />
        </asp:Panel>
        </div>
        <br />
        
        <table>
        
        <tr>
        
            <td width="300px" valign="top">
            
                <asp:Panel ID="pnl_SpecialOptions" runat="server" GroupingText="Special Options" ForeColor="#33FF66">
                <asp:Panel runat="server" ID="btn_markDone"><asp:LinkButton runat="server" OnClick="btn_markDone_OnClick" ForeColor="#33FF66" Font-Bold="true" Font-Size="Medium" Text="Mark Task as Done" /><br /></asp:Panel>
                <asp:Panel runat="server" ID="btn_markWip" Visible="false"><asp:LinkButton runat="server" OnClick="btn_markWip_OnClick" ForeColor="#33FF66" Font-Bold="true" Font-Size="Medium" Text="Re-Open Task" /><br /></asp:Panel>
                <asp:Panel runat="server" ID="btn_startTask" Visible="false"><asp:LinkButton runat="server" OnClick="btn_startTask_OnClick" ForeColor="#33FF66" Font-Bold="true" Font-Size="Medium" Text="Start Task" /><br /></asp:Panel>
                <asp:Panel runat="server" ID="btn_UpgradeSize" Visible="false"><asp:LinkButton runat="server" OnClick="btn_UpgradeSize_OnClick" ForeColor="#33FF66" Font-Bold="true" Font-Size="Medium" Text="Make Project a Large Project" /><br /></asp:Panel>
                </asp:Panel>
            
            </td>

            <td width="300px" valign="top">
                
                <asp:Panel runat="server" ID="Panel2" GroupingText="Project TimeLine Expectations" ForeColor="#33FF66">
                <table>
                
                <tr><td><div style="font-weight:bold;">Expected Start:</div></td><td><asp:Label runat="server" ID="lbl_ExpectedStart" ForeColor="White" /></td></tr>
                <tr><td><div style="font-weight:bold;">Expected Completion:</div></td><td><asp:Label runat="server" ID="lbl_ExpectedStop" ForeColor="White" /></td></tr>
                <tr><td colspan="2"></td></tr>
                <tr><td><div style="font-weight:bold;">Actual Start:</div></td><td><asp:Label runat="server" ID="lbl_ActualStart" ForeColor="White" /></td></tr>
                <tr><td><div style="font-weight:bold;">Actual Completion:</div></td><td><asp:Label runat="server" ID="lbl_ActualStop" ForeColor="White" /></td></tr>

                </table>
                </asp:Panel>
            
            </td>

            <td width="300px" valign="top">
            
                <asp:Panel runat="server" ID="pnl_EditOperations" GroupingText="Edit Operations" ForeColor="#33FF66">
                <asp:LinkButton runat="server" ID="btn_Edit_NameDescription" ForeColor="#33FF66" Font-Bold="true" Font-Size="Small" Text="Edit Project Name/Description" OnClick="btn_Edit_NameDescription_OnClick" /><br />
                <asp:LinkButton runat="server" ID="btn_Edit_Sharing" ForeColor="#33FF66" Font-Bold="true" Font-Size="Small" Text="Edit Project Sharing" OnClick="btn_Edit_Sharing_OnClick" Enabled="false" />
                
                </asp:Panel>
            
            </td>
        
        </tr>
        
        </table>
        
    </div>

    <br />

    <asp:Panel runat="server" ID="pnl_Milestone" Visible="false">
    <div class="fullContainer">
        <div style="font-size:medium;" >Milestones and their sub-Features!</div>
        <i>Highest weighted milestones will be on the left.</i><br /><br />
        <asp:Panel runat="server" ID="pnl_MileStones" /><br /><br /><br />
        <asp:Button runat="server" ID="btn_AddMilestone" Text="Add Milestone" OnClick="btn_AddMilestone_OnClick" />
        <asp:Panel runat="server" ID="pnl_AddMilestone" Visible="false">
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
            <asp:Button runat="server" ID="btn_UpdateMilestone" OnClick="btn_UpdateMilestone_OnClick" Text="Update Milestone" Visible="false" />
        </asp:Panel>
    </div>
    
    <br />
    </asp:Panel>

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

    <asp:Panel runat="server" ID="pnl_Boards" Visible="false">
    <div class="fullContainer" style="background-color:#303030;">
        <div style="border: 2px ridge black; padding:10px; background-color:#808080; color:White;" >
        <div style="font-size:X-large; font-variant:small-caps; font-weight:bold;">Discussion Boards</div><br />
        <i>Select a link below to detail a project or a milestone</i>
        </div><br />
        <asp:Table runat="server" ID="tbl_Boards" Width="100%" />
    </div>
    </asp:Panel>
    

</asp:Content>

