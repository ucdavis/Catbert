<%@ Page Title="" Language="C#" MasterPageFile="~/CatbertMaster.master" AutoEventWireup="true" CodeFile="UserManagement.aspx.cs" Inherits="Admin_UserManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">

<asp:ListView ID="lviewUser" runat="server" DataSourceID="odsActiveUsers">
    <LayoutTemplate>
        <table>
            <thead>
                <tr>
                    <th style="width: 10%" class="header Login" title="LoginID">
                        Login
                    </th>
                    <th style="width: 10%" class="header" title="FirstName">
                        First Name
                    </th>
                    <th style="width: 10%" class="header headerSortUp" title="LastName">
                        Last Name
                    </th>
                    <th style="width: 20%" class="header" title="Email">
                        Email
                    </th>
                </tr>
            </thead>
            <tbody>
                <tr id="itemPlaceholder" runat="server"></tr>
            </tbody>
        </table>
    </LayoutTemplate>
    <ItemTemplate>
        <tr>
            <td><%# Eval("LoginID") %></td>
            <td><%# Eval("FirstName") %></td>
            <td><%# Eval("LastName") %></td>
            <td><%# Eval("Email") %></td>
        </tr>
    </ItemTemplate>
</asp:ListView>

    <asp:ObjectDataSource ID="odsActiveUsers" runat="server" 
        OldValuesParameterFormatString="original_{0}" SelectMethod="GetAllActive" 
        TypeName="CAESDO.Catbert.BLL.UserBLL"></asp:ObjectDataSource>

</asp:Content>

