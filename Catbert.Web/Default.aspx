<%@ Page Title="" Language="C#" MasterPageFile="~/CatbertMaster.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">

Logged in as: <asp:Label ID="lblLoginID" runat="server" EnableViewState="false"></asp:Label>
<asp:LinkButton ID="btnLogout" runat="server" Text="Log out" 
    onclick="btnLogout_Click"/>

<br /><br />

    <ul>
        <li><a href="Admin/Applications.aspx">Applications</a></li>
        <li><a href="Admin/Units.aspx">Units</a></li>
        <li><a href="Admin/Emulation.aspx">Emulation (Testing Only)</a></li>
        <li><a href="Admin/UserAdministration.aspx">User Management (for all apps)</a></li>
        <li><a href="Admin/UserManagement.aspx">Admin User Management</a></li>
    </ul>

</asp:Content>

