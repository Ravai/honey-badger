<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.master" AutoEventWireup="true" CodeFile="UserProfile.aspx.cs" Inherits="UserProfile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="container_full">
    <asp:Label runat="server" ID="lbl_DisplayName" />
    <section style="display:table">
        <section style="display:inline-block; vertical-align:top;">
            <asp:Image runat="server" ID="img_avatar" BorderColor="#0860a8" BorderWidth="2"/>
        </section>

        <section style="display:inline-block; vertical-align:top; padding-left:10px;">
        <strong>Username: </strong>
        <asp:Label runat="server" ID="lbl_alias" />
        <br />
        <strong>Name: </strong>
        <asp:Label runat="server" ID="lbl_name" />
        <br />
        <strong>Email: </strong>
        <asp:Label runat="server" ID="lbl_email" />
        <br /> 
        <strong>Phone number: </strong>
        <asp:Label runat="server" ID="lbl_phone" />
        <br />
        </section>
    </section>
    <hr />

    <!-- <strong>Associations</strong><br />
    No associations as of yet...<br /> -->

    <asp:Panel runat="server" ID="pnl_PersonalProjects">
    <h2>Personal Projects</h2>
    <asp:Table runat="server" ID="tbl_Projects" Width="50%" />
    </asp:Panel>

    <br />
    <h2>Shared Projects</h2>
    <asp:Table runat="server" ID="tbl_sharedProjects" Width="50%" />

    <br />
    <hr />
    <strong>Date Joined:  </strong>
    <asp:Label runat="server" ID="lbl_dateJoined" />
    <br />

    <asp:Panel runat="server" ID="pln_Edit">
        <div class="pull-left">
            <a href="#openEdit">Edit</a>
        </div>
    </asp:Panel>

    <div id="openEdit" class="modalDialog">
        <div>
            <a href="#close" title="Close" class="close">X</a>
                
                <div style="font-size:large; font-variant:small-caps; font-weight:bold; text-decoration:underline;">Edit Profile</div><br />
                <div style="font-size:medium; font-variant:small-caps; font-weight:bold;">First Name: 
                    <asp:TextBox runat="server" ID="txt_FirstName" /><asp:Label runat="server" ID="lbl_FirstNameError" Text="*" Font-Size="Medium" ForeColor="Red" Visible="false" />
                </div>

                <div style="font-size:medium; font-variant:small-caps; font-weight:bold;">Middle Name: 
                    <asp:TextBox runat="server" ID="txt_MiddleName" />
                </div>
                
                <div style="font-size:medium; font-variant:small-caps; font-weight:bold;">Last Name: 
                    <asp:TextBox runat="server" ID="txt_LastName" /><asp:Label runat="server" ID="lbl_LastNameError" Text="*" Font-Size="Medium" ForeColor="Red" Visible="false" />
                </div>
                
                <hr />
                <div style="font-size:medium; font-variant:small-caps; font-weight:bold;">Display Name: <i>(Select one)</i> </div>
                Username <asp:RadioButton runat="server" ID="rdbt_UserName" Checked="true" GroupName="DisplayName" />
                 Your name <asp:RadioButton runat="server" ID="rdbt_Name" GroupName="DisplayName" />

                <hr />
                <div style="font-size:medium; font-variant:small-caps; font-weight:bold;">Email Address: 
                    <asp:TextBox runat="server" ID="txt_EmailAddress" /><asp:Label runat="server" ID="lbl_EmailError" Text="*" Font-Size="Medium" ForeColor="Red" Visible="false" />
                </div>
                

                <div style="font-size:medium; font-variant:small-caps; font-weight:bold;">Phone Number: 
                    <asp:TextBox runat="server" ID="txt_PhoneNumber" />
                </div>
                

                <div style="font-size:medium; font-variant:small-caps; font-weight:bold;">Display Image: <i>(Image scaled to 100x100)</i></div>
                <asp:TextBox runat="server" ID="txt_DisplayImage" />
            
                <br />
                <asp:Button runat="server" Text="Apply" ID="btn_ApplyChanges" OnClick="btn_ApplyChanges_OnClick" />
                <asp:Button runat="server" Text="Cancel" ID="btn_CancelNewTask" PostBackUrl="#close" />
                <asp:Label runat="server" ID="lbl_Error" Visible="false" ForeColor="Red" Font-Italic="true" Font-Size="Large" />
                <br />
        </div>
    </div>
    
</div>
</asp:Content>
