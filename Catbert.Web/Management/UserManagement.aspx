<%@ Page Title="" Language="C#" MasterPageFile="~/Catbert.master" AutoEventWireup="true" EnableViewState="false" CodeFile="UserManagement.aspx.cs" Inherits="Management_UserManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="../CSS/jquery.autocomplete.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
    <script src="../JS/jquery.ajaxQueue.js" type="text/javascript"></script>
    <script src="../JS/jquery.autocomplete.js" type="text/javascript"></script>
    <%--<script src="../JS/jquery.tablesorter.min.js" type="text/javascript"></script>--%>
    <script src="../JS/UserManagement.js" type="text/javascript"></script>
       
    <a href="javascript:;" id="addUser" class="dialog_link ui-state-default ui-corner-all">
        <span class="ui-icon ui-icon-newwin"></span>Add User
    </a>
    <div class="ui-widget" id="divNewUserNotification" style="display:none;">
        <br />
        <div class="ui-state-highlight ui-corner-all" >
            <p>
                <span class="ui-icon ui-icon-info" style="float:left;"></span>
                User Added Successfully
            </p>
        </div>
    </div>
<br /><br />

    <div id="divHeader">
        <span id="search">
            Search Users: <input type="text" id="txtSearch" /><input type="image" id="imgSearch" title="Clear Search" alt="Clear Search" src="../Images/checked.gif" />
        </span>
    </div>
    <div id="divLoading" style="display:none;">
        Loading...
    </div>
    <table id="tblUsers" class="tablesorter">
        <thead>
            <tr>
                <th ></th>
                <th class="header" title="FirstName">First Name</th>
                <th class="header headerSortUp" title="LastName">Last Name</th>
                <th class="header" title="LoginID">Login</th>
                <th class="header" title="Email">Email</th>
                <th >Departments</th>
                <th >Roles</th>
            </tr>
        </thead>
        <tbody id="tblUsersBody">
            <%--Each row is a new person--%>
        </tbody>
        <tfoot>
            <tr>
                <td colspan="6" align="center">
                    <input id="btnFirst" class="pager" name="First" type="button" value="First" />
                    <input id="btnPrevious" class="pager" name="Previous" type="button" value="Previous" />
                    <span id="spanPageInfo"></span>
                    <input id="btnNext" class="pager" name="Next" type="button" value="Next" />
                    <input id="btnLast" class="pager" name="Last" type="button" value="Last" />
                </td>
            </tr>
        </tfoot>
    </table>
    
    <div id="dialogUserInfo" title="User Information" style="display: none;">
        <p>
            Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor
            incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud
            exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.</p>
        <div id="divUserInfo" style="display:none;">
            <span id="UserInfoName"></span> (<span id="UserInfoLogin"></span>)<br />
            <table id="UserInfoRoles">
                <thead>
                    <tr>
                        <th>
                            Role
                        </th>
                        <th>
                            Remove
                        </th>
                    </tr>
                </thead>
                <tbody>
                </tbody>
            </table>
            <br />
            <asp:ListView ID="lviewUserRoles" runat="server" DataSourceID="odsRoles">
                <LayoutTemplate>
                    <select id="UserRoles">
                        <option id="itemPlaceholder" runat="server"></option>
                    </select>
                </LayoutTemplate>
                <ItemTemplate>
                    <option>
                        <%# Eval("Name") %>
                    </option>
                </ItemTemplate>
            </asp:ListView>
            <input type="button" id="btnAddUserRole" value="Add Role" />
            <br /><br />
            <table id="UserInfoUnits">
                <thead>
                    <tr>
                        <th>
                            Unit
                        </th>
                        <th>
                            FISCode
                        </th>
                        <th>
                            Remove
                        </th>
                    </tr>
                </thead>
                <tbody>
                </tbody>
            </table>
            <br />
            <asp:ListView ID="lviewUserUnits" runat="server" DataSourceID="odsUnits">
                <LayoutTemplate>
                    <select id="UserUnits">
                        <option id="itemPlaceholder" runat="server"></option>
                    </select>
                </LayoutTemplate>
                <ItemTemplate>
                    <option value='<%# Eval("FISCode") %>'>
                        <%# Eval("ShortName") %>
                    </option>
                </ItemTemplate>
            </asp:ListView>
            <input type="button" id="btnAddUserUnit" value="Add Unit" />
        </div>
    </div>
    
    <div id="dialogFindUser" title="Add a User" style="display: none;">
        <p>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.</p>
        Kerberos LoginID: <input type="text" id="txtLoginID" /><input type="button" id="btnSearchUser" value="Search" />
        <span id="spanSearchProgress" style="display:none;">Searching...</span>
        <div id="divSearchResultsSuccess" style="display:none;">
            <span id="spanNewUserFirstName"></span> <span id="spanNewUserLastName"></span> (<span id="spanNewUserLogin"></span>)<br />
            Email: <input type="text" id="txtNewUserEmail" /><br />
            Phone: <input type="text" id="txtNewUserPhone" /><br />
            Role:
            <asp:ListView ID="lviewRoles" runat="server" DataSourceID="odsRoles">
                <LayoutTemplate>
                    <select id="applicationRoles">
                        <option id="itemPlaceholder" runat="server"></option>
                    </select>
                </LayoutTemplate>
                <ItemTemplate>
                    <option>
                        <%# Eval("Name") %>
                    </option>
                </ItemTemplate>
            </asp:ListView>
            <asp:ObjectDataSource ID="odsRoles" runat="server" OldValuesParameterFormatString="original_{0}"
                SelectMethod="GetRolesByApplication" TypeName="CAESDO.Catbert.BLL.RoleBLL">
                <SelectParameters>
                    <asp:QueryStringParameter QueryStringField="app" Name="application" DefaultValue="Catbert" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <br />
            Unit:
            <asp:ListView ID="lviewUnits" runat="server" DataSourceID="odsUnits">
                <LayoutTemplate>
                    <select id="units">
                        <option id="itemPlaceholder" runat="server"></option>
                    </select>
                </LayoutTemplate>
                <ItemTemplate>
                    <option value='<%# Eval("FISCode") %>'>
                        <%# Eval("ShortName")%>
                    </option>
                </ItemTemplate>
            </asp:ListView>
            <asp:ObjectDataSource ID="odsUnits" runat="server" OldValuesParameterFormatString="original_{0}"
                SelectMethod="GetAll" TypeName="CAESDO.Catbert.BLL.UnitBLL">
                <SelectParameters>
                    <asp:Parameter DefaultValue="ShortName" Name="propertyName" Type="String" />
                    <asp:Parameter DefaultValue="true" Name="ascending" Type="Boolean" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <br /><br />
            <input type="button" id="btnAddUser" value="Add" /><span id="spanAddUserProgress" style="display:none;">Processing...</span>
        </div>
    </div>
        
</asp:Content>

