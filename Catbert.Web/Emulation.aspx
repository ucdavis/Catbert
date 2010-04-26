<%@ Page Title="" Language="C#" MasterPageFile="~/Catbert.master" AutoEventWireup="true" CodeFile="Emulation.aspx.cs" Inherits="Emulation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
<br /><br />

Login: 
    <asp:TextBox ID="txtLoginID" runat="server"></asp:TextBox>
    <asp:RequiredFieldValidator id="reqValLogin" ControlToValidate="txtLoginID" ErrorMessage="* LoginID Required" runat="server"/>
    
    <br /><br />
    <asp:Button ID="btnLoginID" runat="server" Text="Emulate!" OnClick="btnLoginID_Click" />
</asp:Content>

