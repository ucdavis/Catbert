<%@ Page Title="" Language="C#" MasterPageFile="~/CatbertMaster.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
<div class="menu">
<p>Welcome to my world, <asp:Label ID="lblLoginID" runat="server" EnableViewState="false"></asp:Label>. 
I've got my eye on you.
Maybe you should save yourself the trouble and 
<asp:LinkButton ID="btnLogout" runat="server" Text="log out" onclick="btnLogout_Click"/> now.</p>


    <ul>
        <li class="m1"><a href="Admin/Applications.aspx">Applications</a></li>
        <li class="m2"><a href="Admin/Units.aspx">Units</a></li>
        <li class="m3"><a href="Admin/Emulation.aspx">Emulation (Testing Only)</a></li>
        <li class="m4"><a href="Admin/UserAdministration.aspx">User Management (for all apps)</a></li>
        <li class="m5"><a href="Admin/UserManagement.aspx">Admin User Management</a></li>
    </ul>
</div>
</asp:Content>

