<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.master" AutoEventWireup="true" CodeFile="PSharing.aspx.cs" Inherits="PSharing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div class="fullContainer">
        <div class="row-fluid">
        <asp:LinkButton runat="server" ID="lnk_ReturnToProject" Text="Return to Project" /><br /><br />

        <div class="span5 widget">
        
        <table width="100%">
            <tr><td>
            
            <asp:LinkButton runat="server" ID="lnkbtn_AddbyName" OnClick="lnkbtn_AddbyName_OnClick" CommandArgument="0" Text="[+] Add By Name?" Font-Bold="true" Font-Underline="true" /><br />
            <asp:Panel runat="server" ID="pnl_AddByName" Visible="false">
            <table width="100%">
                <tr>
                    <td colspan="3"><strong><u><i>To add a new person</i></u></strong></td>
                </tr>
                <tr>
                    <td align="center"><strong>First Name</strong><br /><asp:TextBox runat="server" ID="txt_FirstName" style="width:auto"/></td>
                    <td align="center"><strong>Middle Name</strong><br /><asp:TextBox runat="server" ID="txt_MiddleName" style="width:auto"/></td>
                    <td align="center"><strong>Last Name</strong><br /><asp:TextBox runat="server" ID="txt_LastName" style="width:auto"/></td>
                </tr>
                <tr>
                    <td colspan="3" align="center">
                        <asp:Button runat="server" ID="btn_CheckName" Text="Check Name" OnClick="btn_CheckName_OnClick" /><br />
                        <asp:Label runat="server" ID="lbl_checkMessages" ForeColor="Red" Font-Italic="true" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3" align="center">
                        <asp:Table runat="server" ID="tbl_possibleNames" Width="100%" />
                    </td>
                </tr>
            </table>
            </asp:Panel>

            <asp:LinkButton runat="server" ID="lnkbtn_AddbyUserName" OnClick="lnkbtn_AddbyUserName_OnClick" CommandArgument="0" Text="[+] Add By user name?" Font-Bold="true" Font-Underline="true" /><br />
            <asp:Panel runat="server" ID="pnl_AddByUserName" Visible="false">
            <table width="100%">
                <tr>
                    <td colspan="3"><strong><u><i>To add a new person</i></u></strong></td>
                </tr>
                <tr>
                    <td align="center"><strong>User Name</strong><br /><asp:TextBox runat="server" ID="txt_userName" /></td>
                </tr>
                <tr>
                    <td colspan="3" align="center">
                        <asp:Button runat="server" ID="btn_checkuserName" Text="Check UserNames" OnClick="btn_CheckuserName_OnClick" /><br />
                        <asp:Label runat="server" ID="lbl_checkMessages2" ForeColor="Red" Font-Italic="true" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3" align="center">
                        <asp:Table runat="server" ID="tbl_possibleNames2" Width="100%" />
                    </td>
                </tr>
            </table>
            </asp:Panel>
            
            </td></tr>

            <tr><td>
            
            <asp:Panel ID="Panel1" runat="server" Width="100%">
            <h2>Permission Levels</h2>
            <hr />
            <table width="100%">
                <tr>
                    <td align="center" colspan="2"><strong><u>Select Role</u>   &nbsp   </strong>
                        <asp:DropDownList ID="roles_DropDownList" runat="server" AppendDataBoundItems="true" AutoPostBack="True" OnSelectedIndexChanged="roles_DropDownList_SelectedIndexChanged">
                        <asp:ListItem Text="---Select---" Value="-1"></asp:ListItem>
                        <asp:ListItem Text="Project Manager" Value="0"></asp:ListItem>  
                        <asp:ListItem Text="Supervisor" Value="1"></asp:ListItem>  
                        <asp:ListItem Text="Team Member" Value="2"></asp:ListItem>
                        <asp:ListItem Text="Client" Value="3"></asp:ListItem>
                        <asp:ListItem Text="Public Member" Value="4"></asp:ListItem>  
                    </asp:DropDownList>
                    </td> 
                </tr>
                 <tr>
                    <td align="center">
                        <strong><u>These permissions will be applied to this role: </u></strong><br />
                        <asp:Label ID="PermissionLabel" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
            </asp:Panel>
            
            </td></tr>

            <tr><td align="center"><br />
                <asp:Label ID="ValidationLabel" runat="server" ForeColor="Red" Font-Italic="true"></asp:Label>
                <br />
            
                <asp:Button runat="server" ID="btn_AddNewPermission" Text="Add New Permission" OnClick="btn_AddNewPermission_OnClick" Enabled="false" /><br />
            
            </td></tr>

            <tr><td align="center">
                <asp:Panel runat="server" ID="pnl_ListofAdds">
                <hr />
                <strong><u>List of People with Permissions to this Project</u></strong><br />
                <asp:Table runat="server" ID="tbl_List" />
                </asp:Panel>
            </td></tr>

        </table>
        </div>
    </div>
</div>

</asp:Content>

