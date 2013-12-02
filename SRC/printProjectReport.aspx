﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="printProjectReport.aspx.cs" Inherits="printProjectReport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Project Print Report</title>
    <link rel="stylesheet" href="Styles/Site.css" />
    <link rel="stylesheet" href="Styles/StyleSheetV3.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        
        <h1><asp:Label runat="server" ID="lbl_ProjectName" /></h1>
        <h2><asp:Label runat="server" ID="lbl_ProjectDescription" /></h2>

        <asp:Literal runat="server" ID="lit_ProjectTotalComplete" /><br />
        <hr />

        <asp:Literal runat="server" ID="lit_ProjectDetails" />

    </div>
    </form>
</body>
</html>
