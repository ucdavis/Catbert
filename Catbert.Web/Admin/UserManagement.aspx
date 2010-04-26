<%@ Page Title="" Language="C#" MasterPageFile="~/CatbertMaster.master" AutoEventWireup="true" EnableViewState="false" CodeFile="UserManagement.aspx.cs" Inherits="Management_UserManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="../CSS/jquery.autocomplete.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
    <script src="../JS/jquery.ajaxQueue.js" type="text/javascript"></script>
    <script src="../JS/jquery.autocomplete.js" type="text/javascript"></script>
    <%--<script src="../JS/jquery.tablesorter.min.js" type="text/javascript"></script>--%>
    <script src="../JS/UserManagementAdmin.js" type="text/javascript"></script>
       
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
        <span id="search" style="float:left;">
            <label>Search Users:</label> <input type="text" id="txtSearch" /><input type="image" id="imgSearch" title="Clear Search" alt="Clear Search" src="../Images/clear-left.png" style="height: 15px;" />
        </span>
        
        <span id="filter" style="float:right;">
            
            <asp:ListView ID="lviewFilterApplications" runat="server">
                <LayoutTemplate>
                    <select id="filterApplications">
                        <option value="">-- Filter By Application --</option>
                        <option id="itemPlaceholder" runat="server"></option>
                    </select>
                </LayoutTemplate>
                <ItemTemplate>
                    <option>
                        <%# Eval("Name") %>
                    </option>
                </ItemTemplate>
            </asp:ListView>
            
        </span>
    </div>
    <div id="divLoading" style="display:none; clear: left;">
        Loading...
    </div>
    <table id="tblUsers" class="tablesorter">
        <thead>
            <tr>
                <th style="width: 20%" class="header Login" title="LoginID">Login</th>
                <th style="width: 20%" class="header" title="FirstName">First Name</th>
                <th style="width: 20%" class="header headerSortUp" title="LastName">Last Name</th>
                <th style="width: 40%" class="header" title="Email">Email</th>
            </tr>
        </thead>
        <tbody id="tblUsersBody">
            <%--Each row is a new person--%>
        </tbody>
        <tfoot>
            <tr>
                <td colspan="6" align="center">
                    <input id="btnFirst" class="pager" name="First" type="button" value="First" title="First" />
                    <input id="btnPrevious" class="pager" name="Previous" type="button" value="Previous" title="Previous" />
                    <span id="spanPageInfo"></span>
                    <input id="btnNext" class="pager" name="Next" type="button" value="Next" title="Next" />
                    <input id="btnLast" class="pager" name="Last" type="button" value="Last" title="Last" />
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
            <h2><span id="UserInfoName"></span> (<span id="UserInfoLogin"></span>)</h2>
            <br /><br />
            <table id="UserInfoRoles">
                <thead>
                    <tr>
                        <th>Role</th>
                        <th>Remove</th>
                    </tr>
                </thead>
                <tbody>
                </tbody>
            </table>
            <br />
            <asp:ListView ID="lviewUserRoles" runat="server">
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
            <br /><br /><br /><br />
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
            <asp:ListView ID="lviewUserUnits" runat="server">
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
        Kerberos or Email: <input type="text" id="txtLoginID" /><input type="button" id="btnSearchUser" value="Search" />
        <span id="spanSearchProgress" style="display:none;">Searching...</span>
        <div id="divSearchResultsSuccess" style="display:none;">
            <h2><span id="spanNewUserFirstName"></span> <span id="spanNewUserLastName"></span> (<span id="spanNewUserLogin"></span>)</h2>
            <label>Email:</label> <input type="text" id="txtNewUserEmail" /><br />
            <label>Phone:</label> <input type="text" id="txtNewUserPhone" /><br />
            <br />
            <label>Application:</label>
            <asp:ListView ID="lviewApplications" runat="server">
                <LayoutTemplate>
                    <select id="applications">
                        <option id="itemPlaceholder" runat="server"></option>
                    </select>
                </LayoutTemplate>
                <ItemTemplate>
                    <option>
                        <%# Eval("Name") %>
                    </option>
                </ItemTemplate>
            </asp:ListView>
            <br />
            <label>Role:</label>
            <asp:ListView ID="lviewRoles" runat="server">
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
            <br />
            <label>Unit:</label>
            <asp:ListView ID="lviewUnits" runat="server">
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
<%--            <asp:ObjectDataSource ID="odsUnits" runat="server" OldValuesParameterFormatString="original_{0}"
                SelectMethod="GetVisibleByUser" TypeName="CAESDO.Catbert.BLL.UnitBLL">
                <SelectParameters>
                    <asp:QueryStringParameter QueryStringField="app" Name="application" DefaultValue="Catbert" />
                </SelectParameters>
            </asp:ObjectDataSource>--%>
            <br />
            <input type="button" id="btnAddUser" value="Add User" /><span id="spanAddUserProgress" style="display: none;">Processing...</span>
        </div>
    </div>
        
</asp:Content>

