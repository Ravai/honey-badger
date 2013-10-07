<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.master" AutoEventWireup="true" CodeFile="PSharing.aspx.cs" Inherits="PSharing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div class="fullContainer">
        <asp:LinkButton runat="server" ID="lnk_ReturnToProject" Text="Return to Project" /><br /><br />

        <table><tr>
        <td width="50%" valign="top">
        
        <table width="100%">
            <tr><td>
            
            <asp:LinkButton runat="server" ID="lnkbtn_AddbyName" OnClick="lnkbtn_AddbyName_OnClick" CommandArgument="0" Text="[+] Add By Name?" Font-Bold="true" Font-Underline="true" /><br />
            <asp:Panel runat="server" ID="pnl_AddByName" Visible="false">
            <table width="100%">
                <tr>
                    <td colspan="3"><strong><u><i>To add a new person</i></u></strong></td>
                </tr>
                <tr>
                    <td align="center"><strong>First Name</strong><br /><asp:TextBox runat="server" ID="txt_FirstName" /></td>
                    <td align="center"><strong>Middle Name</strong><br /><asp:TextBox runat="server" ID="txt_MiddleName" /></td>
                    <td align="center"><strong>Last Name</strong><br /><asp:TextBox runat="server" ID="txt_LastName" /></td>
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
                    <td colspan="2" align="center"><strong><u>Choose Permission Levels</u></strong></td>
                </tr>
                <tr>
                    <td align="center"><strong>Project</strong></td>
                    <td align="center"><strong>Discussion Boards</strong></td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:RadioButton runat="server" ID="radio_Project_Read" Checked="true" GroupName="ProjectGroup" Text="Read Only" />
                        <asp:RadioButton runat="server" ID="radio_Project_Write" Checked="false" GroupName="ProjectGroup" Text="Write" />
                    </td>
                    <td align="center">
                        <asp:RadioButton runat="server" ID="radio_Board_Read" Checked="true" GroupName="BoardGroup" Text="Read Only" />
                        <asp:RadioButton runat="server" ID="radio_Board_Write" Checked="false" GroupName="BoardGroup" Text="Write" />
                    </td>
                </tr>
            </table>
            </asp:Panel>
            
            </td></tr>

            <tr><td align="center">
            
                <asp:Button runat="server" ID="btn_AddNewPermission" Text="Add New Permission" OnClick="btn_AddNewPermission_OnClick" Enabled="false" /><br />
            
            </td></tr>
        </table>
        
        </td>

        <td width="50%" valign="top" align="center">
        
        <asp:Panel runat="server" ID="pnl_ListofAdds">
        
        <strong><u>List of People with Permissions to this Project</u></strong><br />
        <asp:Table runat="server" ID="tbl_List" />
        
        </asp:Panel>

        </td>
        
        </tr></table>

        
        

    
    </div>

</asp:Content>

