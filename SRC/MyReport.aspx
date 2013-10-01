<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.master" AutoEventWireup="true" CodeFile="MyReport.aspx.cs" Inherits="MyReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <asp:MultiView ID="mv_Reports" EnableViewState="true" runat="server" ActiveViewIndex="0">
    
    <asp:View runat="server" ID="view_ChooseReport">
        <div class="fullContainer">
        <div style="font-size:Medium; font-weight:bold;">Choose Dates for your Report</div><br />
        <table>
            <tr>
                <td align="center">Start Date</td>
                <td align="center">End Date</td>
            </tr>
            <tr>
                <td align="center"><asp:Calendar runat="server" ID="calendar_date1" /></td>
                <td align="center"><asp:Calendar runat="server" ID="calendar_date2" /></td>
            </tr>
        </table>
        <asp:Button runat="server" Text="View Report" ID="btn_ViewReport" OnClick="btn_ViewReport_OnClick" /><br />
        </div>
    </asp:View>
    <asp:View runat="server" ID="view_Report">
        <div class="fullContainer">
            <a href="MyReport.aspx">Search again?</a>
        </div>
        <div class="fullContainer">
        <div style="font-size:Medium; font-weight:bold;">Task Updates for <asp:Label runat="server" ID="date1" /> to <asp:Label runat="server" ID="date2" /></div><br />
        <asp:Table runat="server" Width="100%" ID="tbl_CurrentMonth" />
        </div><br />
    </asp:View>
    </asp:MultiView>
    
    

</asp:Content>

