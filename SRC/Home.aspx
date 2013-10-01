<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.master" AutoEventWireup="true" CodeFile="Home.aspx.cs" Inherits="Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<script type="text/javascript">
    function PopupPicker(ctl, w, h) {
        var PopupWindow = null;
        settings = 'width=' + w + ',height=' + h + ',location=no,directories=no,menubar=no,toolbar=no,status=no,scrollbars=no,resizable=no,dependent=no';
        PopupWindow = window.open('DatePicker.aspx?Ctl=' + ctl, 'DatePicker', settings);
        PopupWindow.focus();
    }
</script>
    
    <div class="widget">
        <ul id="myTab" class="nav nav-tabs three-tabs fancy">
	        <li class="active"><a href="#Progress" data-toggle="tab">Tasks in Progress</a></li>
	        <li><a href="#Ready" data-toggle="tab">Ready Tasks</a></li>
	        <li class="dropdown">
		        <a href="#Upcoming" data-toggle="tab">Upcoming Tasks</a>
	        </li>
        </ul>
        <div class="tab-content">
	        <div class="tab-pane fade in active" id="Progress">
                <ul class="cards">
                    <asp:Literal runat="server" ID="ProgressList" />
                </ul>
	        </div>
	        <div class="tab-pane fade" id="Ready">

	            <ul class="cards">
                    <asp:Literal runat="server" ID="ReadyList" />
	            </ul>
            </div>
            <div class="tab-pane fade" id="Upcoming">
	            <ul class="cards">
                    <asp:Literal runat="server" ID="UpcomingList" />
	            </ul>
            
            </div>
        </div>
    </div>


    <asp:Panel runat="server" ID="pnl_Messages" Visible="false">
    <div class="fullContainer">
    <strong>You have a new message!</strong>
    <asp:Table runat="server" ID="tbl_Messages" Width="100%" />
    </div>
    </asp:Panel>

<%--    <div style="font-size:large; font-variant:small-caps; font-weight:bold;"><br />My Tasks<br /></div>
    <table width="100%">
    <tr>
        <td>
        
        <div class="fullContainer" style="background-color:#00FF00;">
        <div style="font-size:Medium; font-weight:bold;">Completed Tasks</div> <i>(Task marked as Done)</i><br />
        <asp:Table runat="server" Width="100%" ID="tbl_CompletedTaskList" Height="300px" />
        <div style="float:right;">
            <asp:LinkButton runat="server" ID="btn_gotoComplete" PostBackUrl="~/ViewComplete.aspx" Font-Bold="true" Font-Size="Large" Text="View All Completed" Visible="false" />
        </div>
        </div>
        
        </td>
    
    </tr>
    </table>--%>

    <asp:Panel runat="server" ID="pnl_SharedTasks">
        <div style="font-size:medium; font-variant:small-caps; font-weight:bold;"><br />Tasks that have been shared with me<br /></div>
        <table width="100%">
        <tr>

            <td width="25%">
        
            <div class="fullContainer" style="background-color:#D8D8D8;">
            <div style="font-size:Medium; font-weight:bold;">Upcoming Tasks</div> <i>(Expected start is in the future)</i><br />
            <asp:Table runat="server" Width="100%" ID="tbl_Shared_Upcoming" Height="300px" />
            <div style="float:right;">
                <asp:LinkButton runat="server" ID="LinkButton1" PostBackUrl="~/ViewUpcoming.aspx" Font-Bold="true" Font-Size="Large" Text="View All Upcoming" Visible="false" />
            </div>
            </div>
        
            </td>

            <td width="25%">
        
            <div class="fullContainer" style="background-color:#FFCC00;">
            <div style="font-size:Medium; font-weight:bold;">Ready Tasks</div> <i>(Task ready to be started)</i><br />
            <asp:Table runat="server" Width="100%" ID="tbl_Shared_Ready" Height="300px" />
            <div style="float:right;">
                <asp:LinkButton runat="server" ID="LinkButton2" PostBackUrl="~/ViewReady.aspx" Font-Bold="true" Font-Size="Large" Text="View All Ready" Visible="false" />
            </div>
            </div>
        
            </td>
    
            <td width="25%">
        
            <div class="fullContainer" style="background-color:#FFFF00;">
            <div style="font-size:Medium; font-weight:bold;">Tasks in WIP</div> <i>(Task has been started)</i><br />
            <asp:Table runat="server" Width="100%" ID="tbl_Shared_WIP" Height="300px" />
            <div style="float:right;">
                <asp:LinkButton runat="server" ID="LinkButton3" PostBackUrl="~/ViewWip.aspx" Font-Bold="true" Font-Size="Large" Text="View All Wip" Visible="false" />
            </div>
            </div>
        
            </td>

            <td width="25%">
        
            <div class="fullContainer" style="background-color:#00FF00;">
            <div style="font-size:Medium; font-weight:bold;">Completed Tasks</div> <i>(Task marked as Done)</i><br />
            <asp:Table runat="server" Width="100%" ID="tbl_Shared_Completed" Height="300px" />
            <div style="float:right;">
                <asp:LinkButton runat="server" ID="LinkButton4" PostBackUrl="~/ViewComplete.aspx" Font-Bold="true" Font-Size="Large" Text="View All Completed" Visible="false" />
            </div>
            </div>
        
            </td>
    
        </tr>
        </table>
    </asp:Panel>


    <div class="fullContainer">

    <asp:Panel runat="server" ID="pnl_AddTask1">
        <asp:LinkButton runat="server" ID="lnkbtn_ShowAddTask" Text="[+] Do you want to add a new task/project?" CssClass="ShowAdd_Button" OnClick="lnkbtn_ShowAddTask_OnClick" />
    </asp:Panel>
    <asp:Panel runat="server" ID="pnl_AddTask2" Visible="false">
        <div style="font-size:large; font-variant:small-caps; font-weight:bold; text-decoration:underline;">Add New Task</div><br />
        <div style="font-size:medium; font-variant:small-caps; font-weight:bold;">Name: <i>(100 characters)</i></div>
        <asp:TextBox runat="server" ID="txt_addNew_Task_Name" Width="200px" /><asp:Label runat="server" ID="lbl_NameError" Text="*" Font-Size="Medium" ForeColor="Red" Visible="false" /><br />
        <div style="font-size:medium; font-variant:small-caps; font-weight:bold;">Description: <i>(2000 characters)</i></div>
        <asp:TextBox runat="server" ID="txt_addNew_Task_Description" Width="400px" TextMode="MultiLine" Rows="4" /><asp:Label runat="server" ID="lbl_DescError" Text="*" Font-Size="Medium" ForeColor="Red" Visible="false" /><br />
        
        <table>
        <tr>
            <td align="center"><div style="font-size:medium; font-variant:small-caps; font-weight:bold;">Expected Start</div></td>
            <td align="center"><div style="font-size:medium; font-variant:small-caps; font-weight:bold;">Expected Stop</div></td>
        </tr>
        <tr>
            <td align="center">
                <asp:Calendar ID="Calendar_ExpectedStart"   
                    runat="server"   
                    DayNameFormat="FirstLetter"  
                    Font-Names="Arial"   
                    Font-Size="11px"   
                    NextMonthText="»"   
                    PrevMonthText="«"  
                    SelectMonthText="»"   
                    SelectWeekText="›"  
                    CssClass="myCalendar"  
                    BorderStyle="None"   
                    CellPadding="1">  
  
                    <OtherMonthDayStyle ForeColor="Gray" />  
  
                    <DayStyle CssClass="myCalendarDay" />  
  
                    <SelectedDayStyle Font-Bold="True" Font-Size="12px" />  
  
                    <SelectorStyle CssClass="myCalendarSelector" />  
  
                    <NextPrevStyle CssClass="myCalendarNextPrev" />  
  
                    <TitleStyle CssClass="myCalendarTitle" />  
                </asp:Calendar>
            </td>
            <td align="center">
                <asp:Calendar ID="Calendar_ExpectedStop"   
                    runat="server"   
                    DayNameFormat="FirstLetter"  
                    Font-Names="Arial"   
                    Font-Size="11px"   
                    NextMonthText="»"   
                    PrevMonthText="«"  
                    SelectMonthText="»"   
                    SelectWeekText="›"  
                    CssClass="myCalendar"  
                    BorderStyle="None"   
                    CellPadding="1">  
  
                    <OtherMonthDayStyle ForeColor="Gray" />  
  
                    <DayStyle CssClass="myCalendarDay" />  
  
                    <SelectedDayStyle Font-Bold="True" Font-Size="12px" />  
  
                    <SelectorStyle CssClass="myCalendarSelector" />  
  
                    <NextPrevStyle CssClass="myCalendarNextPrev" />  
  
                    <TitleStyle CssClass="myCalendarTitle" />  
                </asp:Calendar>
            </td>
        </tr>
        </table>

        <asp:Button runat="server" Text="Add" ID="btn_AddNewTask" OnClick="btn_AddNewTask_OnClick" />
        <asp:Button runat="server" Text="Cancel" ID="btn_CancelNewTask" OnClick="btn_CancelNewTask_OnClick" />
        <asp:Label runat="server" ID="lbl_Error" Visible="false" ForeColor="Red" Font-Italic="true" Font-Size="Large" />
    </asp:Panel>
    
    </div>
</asp:Content>

