<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Projects.aspx.cs" Inherits="Projects" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Honey Badger</title>
    <meta charset="UTF-8" />
    <link rel="Stylesheet" href="Styles/reset.css" />
    <link rel="Stylesheet" href="Styles/newSite.css" />
</head>
<body>

    <header>
        <h1>My Tracking Tool</h1>
    </header>

    <section>
        <section class="fullContainer">
            <summary><h2>WIP Tasks</h2></summary>
            <details><asp:Panel runat="server" ID="pnl_WipTasks" /></details>
        </section>
        <section class="fullContainer">
            <summary><h2>Ready Tasks</h2></summary>
            <details><asp:Panel runat="server" ID="pnl_ReadyTasks" /></details>
        </section>
    </section>

    <footer>
        <asp:Label ID="templateLastUpdated" Font-Size="9px" ForeColor="black" runat="server" /><br />
    </footer>
</body>
</html>
