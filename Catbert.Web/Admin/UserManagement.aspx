<%@ Page Title="" Language="C#" MasterPageFile="~/CatbertMaster.master" AutoEventWireup="true" EnableViewState="false" CodeFile="UserManagement.aspx.cs" Inherits="Admin_UserManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="../CSS/jquery.autocomplete.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
    <script src="../JS/jquery.ajaxQueue.js" type="text/javascript"></script>
    <script src="../JS/jquery.autocomplete.js" type="text/javascript"></script>
    <%--<script src="../JS/jquery.tablesorter.min.js" type="text/javascript"></script>--%>
    <script src="../JS/UserManagementAdmin.js" type="text/javascript"></script>
       
    <div id="divHeader">
    
    <div class="ui-widget" id="divNewUserNotification" style="display:none;">
        <br />
        <div class="ui-state-highlight ui-corner-all" >
            <p>
                <span class="ui-icon ui-icon-info" style="float:left;"></span>
                User Added Successfully
            </p>
        </div>
    </div>
    <span id="adduserbtn">
        <a href="javascript:;" id="addUser" class="dialog_link ui-state-default ui-corner-all">
        <span class="ui-icon ui-icon-newwin"></span>Add User
        </a>
    </span>
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
    <span class="clear">&nbsp;</span>
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
        
    <div id="dialogFindUser" title="Add a User" style="display: none;">
        <p>&nbsp;</p>
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
                        <option value="">-- Select An Application --</option>
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
            <select id="applicationRoles" disabled="disabled">
                <option>Select An Application</option>
            </select>
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
    
    <div id="dialogUserInfo" title="User Information" style="display: none;">
        <div id="tabs">
            <ul>
                <li><a href="#tabInfo">Info</a></li>
                <li><a href="#tabPermissions">Permissions</a></li>
                <li><a href="#tabUnits">Units</a></li>
            </ul>
            <div id="tabPermissions">
                <table id="tblPermissions">
                    <thead>
                        <tr>
                            <th>Application</th>
                            <th>Role</th>
                            <th>Remove</th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
                <br />
                <table id="tblAddPermission">
                    <tbody>
                        <tr>
                            <td>
                                <asp:ListView ID="lviewApplicationPermissions" runat="server">
                                    <LayoutTemplate>
                                        <select id="applicationsPermissions">
                                            <option value="">-- Select An Application --</option>
                                            <option id="itemPlaceholder" runat="server"></option>
                                        </select>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <option>
                                            <%# Eval("Name") %>
                                        </option>
                                    </ItemTemplate>
                                </asp:ListView>
                            </td>
                            <td>
                                <select id="rolesPermissions" disabled="disabled">
                                    <option value="">Select An Application</option>
                                </select>
                            </td>
                            <td>
                                <input id="btnAddPermission" type="button" value="+" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div id="tabUnits">
                <table id="tblUnits">
                    <thead>
                        <tr>
                            <th>Application</th>
                            <th>Role</th>
                            <th>Remove</th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
                <br />
                <table id="tblAddUnitAssociation">
                    <tbody>
                        <tr>
                            <td>
                                <asp:ListView ID="lviewApplicationUnits" runat="server">
                                    <LayoutTemplate>
                                        <select id="applicationsUnits">
                                            <option id="itemPlaceholder" runat="server"></option>
                                        </select>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <option>
                                            <%# Eval("Name") %>
                                        </option>
                                    </ItemTemplate>
                                </asp:ListView>
                            </td>
                            <td>
                                <asp:ListView ID="lviewUnitsForAssociation" runat="server">
                                    <LayoutTemplate>
                                        <select id="unitsForAssociation">
                                            <option id="itemPlaceholder" runat="server"></option>
                                        </select>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <option value='<%# Eval("FISCode") %>'>
                                            <%# string.Format("{0} ({1})", Eval("ShortName"), Eval("FISCode")) %>
                                        </option>
                                    </ItemTemplate>
                                </asp:ListView>
                            </td>
                            <td>
                                <input id="btnAddUnits" type="button" value="+" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div id="tabInfo">
                <table>
                    <tbody>
                        <tr>
                            <td>Login</td>
                            <td>
                                <span id="UserInfoLogin"></span>
                            </td>
                        </tr>
                        <tr>
                            <td>First</td>
                            <td>
                                <input type="text" id="UserInfoFirstName" maxlength="50" />
                            </td>
                        </tr>
                        <tr>
                            <td>Last</td>
                            <td>
                                <input type="text" id="UserInfoLastName" maxlength="50" />
                            </td>
                        </tr>
                        <tr>
                            <td>Email</td>
                            <td>
                                <input type="text" id="UserInfoEmail" maxlength="50" />
                            </td>
                        </tr>
                        <tr>
                            <td>Phone</td>
                            <td>
                                <input type="text" id="UserInfoPhone" maxlength="50" />
                            </td>
                        </tr>
                        <tr><td colspan="2">&nbsp;</td></tr>
                        <tr>
                            <td></td>
                            <td><input type="button" id="btnUpdateUserInfo" value="Update Information" /></td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <span id="UserInfoMessage"></span>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
        
</asp:Content>

