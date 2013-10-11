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
    <asp:Panel runat="server" ID="pnl_Messages" Visible="false">
    <div class="fullContainer">
    <strong>You have a new message!</strong>
    <asp:Table runat="server" ID="tbl_Messages" Width="100%" />
    </div>
    <hr />
    </asp:Panel>

    <div class="widget">
        <ul id="myTab" class="nav nav-tabs three-tabs fancy">
	        <li class="active"><a href="#Progress" data-toggle="tab">Tasks in Progress(<asp:Literal runat="server" ID="lit_totInProgress" />)</a></li>
	        <li><a href="#Ready" data-toggle="tab">Ready Tasks(<asp:Literal runat="server" ID="lit_totReady" />)</a></li>
	        <li class="dropdown">
		        <a href="#Upcoming" data-toggle="tab">Upcoming Tasks(<asp:Literal runat="server" ID="lit_totUpcoming" />)</a>
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

    <a href="#openModal"><h3>Do you want to add a new Project?</h3></a>
    
    <div id="openModal" class="modalDialog">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" />
        <!-- BEGIN CONTENT TABLE -->
        <!-- CONTENT GOES HERE -->
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
    
	    <div>
		    <a href="#close" title="Close" class="close">X</a>
		    

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
                    <asp:Button runat="server" Text="Cancel" ID="btn_CancelNewTask" PostBackUrl="#close"/>
                    <asp:Label runat="server" ID="lbl_Error" Visible="false" ForeColor="Red" Font-Italic="true" Font-Size="Large" />

	    </div>

        </ContentTemplate>
        </asp:UpdatePanel>
    </div>

</asp:Content>

