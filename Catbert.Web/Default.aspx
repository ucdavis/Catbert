<%@ Page Title="" Language="C#" MasterPageFile="~/Catbert.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">

Logged in as: <asp:Label ID="lblLoginID" runat="server" EnableViewState="false"></asp:Label>
<asp:LinkButton ID="btnLogout" runat="server" Text="Log out" 
    onclick="btnLogout_Click"/>

<br /><br />
<a href="Applications.aspx">Applications</a><br /><br />
<a href="Units.aspx">Units</a><br /><br />
<a href="Emulation.aspx">Emulation (Testing Only)</a><br /><br />
<a href="Management/UserManagement.aspx">User Management (For external apps)</a>

</asp:Content>

