<%@ Page Title="" Language="C#" MasterPageFile="~/Catbert.master" AutoEventWireup="true" CodeFile="Users.aspx.cs" Inherits="Users" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
    <script src="JS/jquery-ui-personalized-1.5.3.js" type="text/javascript"></script>
    
    <br />
    <%--Main Users Table--%>
    <asp:ListView ID="lviewUsers" runat="server" DataSourceID="odsUsers">
        <LayoutTemplate>
            <table id="tblUsers" class="tablesorter">
                <thead>
                    <tr>
                        <th>
                            &nbsp;
                        </th>
                        <th>
                            Login
                        </th>
                        <th>
                            First Name
                        </th>
                        <th>
                            Last Name
                        </th>
                        <th>
                            Email
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <tr id="itemPlaceholder" runat="server">
                    </tr>
                </tbody>
            </table>
        </LayoutTemplate>
        <ItemTemplate>
            <tr>
                <td>
                    Select
                </td>
                <td>
                    <%# Eval("LoginID") %>
                </td>
                <td>
                    <%# Eval("FirstName") %>
                </td>
                <td>
                    <%# Eval("LastName") %>
                </td>
                <td>
                    <%# Eval("Email") %>
                </td>
            </tr>
        </ItemTemplate>
        <EmptyDataTemplate>
            No Users Found
        </EmptyDataTemplate>
    </asp:ListView>
    <asp:ObjectDataSource ID="odsUsers" runat="server" OldValuesParameterFormatString="original_{0}"
        SelectMethod="GetAll" TypeName="CAESDO.Catbert.BLL.UserBLL">
        <SelectParameters>
            <asp:Parameter DefaultValue="LastName" Name="propertyName" Type="String" />
            <asp:Parameter DefaultValue="true" Name="ascending" Type="Boolean" />
        </SelectParameters>
    </asp:ObjectDataSource>

</asp:Content>