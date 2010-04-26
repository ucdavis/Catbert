<%@ Page Title="" Language="C#" MasterPageFile="~/Catbert.master" AutoEventWireup="true" CodeFile="Applications.aspx.cs" Inherits="Applications" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .connectedSortable { list-style-type: none; margin: 0; padding: 0; float: left; margin-right: 10px; background: #eee; padding: 5px; width: 143px;}
	    .connectedSortable li { margin: 5px; padding: 5px; font-size: 1.2em; width: 120px; }
	</style>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
    <script src="JS/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="JS/Applications.js" type="text/javascript"></script>

    <a href="javascript:;" id="addApplication" class="dialog_link ui-state-default ui-corner-all">
        <span class="ui-icon ui-icon-newwin"></span>Add Application
    </a>
    <br /><br />
    <asp:ListView ID="lviewApplications" runat="server" DataSourceID="odsApplications">
        <LayoutTemplate>
            <table id="tblApplications" class="tablesorter">
                <thead>
                    <tr>
                        <th>&nbsp;                            
                        </th>
                        <th>
                            Application Name
                        </th>
                        <th>
                            Abbreviation
                        </th>
                        <th>
                            Location
                        </th>
                        <th>
                            Active
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
            <tr id='<%# Eval("ID") %>' title='<%# Eval("Name") %>'>
                <td title="Select">
                    <a href="javascript:;" class="dialog_link ui-state-default ui-corner-all" title="Select Application">
                        <span class="ui-icon ui-icon-newwin"></span>
                        Select 
                    </a>
                </td>
                <td title="Name">
                    <%# Eval("Name") %>
                </td>
                <td title="Abbr">
                    <%# Eval("Abbr") %>
                </td>
                <td title="Location">
                    <a href='<%# Eval("Location") %>' target='_blank'><%# Eval("Location") %></a>
                </td>
                <td title="Active">
                    <input class="activeIndicator" type="image" style="border-width: 0;" value='<%# Eval("Name") %>'
                        src='<%# (bool)Eval("Inactive") ? "Images/Inactive.gif" : "Images/Active.gif"%>' 
                        alt='<%# (bool)Eval("Inactive") ? "Inactive" : "Active" %>' />
                </td>
            </tr>
        </ItemTemplate>
        <EmptyDataTemplate>
            No Applications Found (How did you do it?)
        </EmptyDataTemplate>
    </asp:ListView>

    <asp:ObjectDataSource ID="odsApplications" runat="server" 
        OldValuesParameterFormatString="original_{0}" SelectMethod="GetAll" 
        TypeName="CAESDO.Catbert.BLL.ApplicationBLL">
        <SelectParameters>
            <asp:Parameter DefaultValue="Name" Name="propertyName" Type="String" />
            <asp:Parameter DefaultValue="true" Name="ascending" Type="Boolean" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <!-- ui-dialog -->
    <div id="dialogUserInfo" title="Application Information" style="display: none;">
        <p>
            Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor
            incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud
            exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.</p>
        <div>
            <span id="spanLoading">Loading....</span>
        </div>
        <div id="divApplicationInfo" style="visibility: hidden">
            <table>
                <tr>
                    <td>
                        Name
                    </td>
                    <td>
                        <input type="text" id="txtApplicationName" size="40" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Abbr
                    </td>
                    <td>
                        <input type="text" id="txtApplicationAbbr" size="40" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Location
                    </td>
                    <td>
                        <input type="text" id="txtApplicationLocation" size="40" />
                    </td>
                </tr>
            </table>
            <br />
            <br />
            <div id="RoleChoice">
                <%--Need three choice boxes for roles--%>
                <ul id="sortableRoles" class="connectedSortable">
                </ul>
                <ul id="nonSortableRoles" class="connectedSortable">
                </ul>
                <asp:ListView ID="lviewAllRoles" runat="server" DataSourceID="odsAllRoles">
                    <LayoutTemplate>
                        <ul id="availableRoles" class="connectedSortable">
                            <li id="itemPlaceholder" runat="server"></li>
                        </ul>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <li class="ui-state-default" id='<%# Eval("Name") %>'>
                            <%# Eval("Name") %>
                        </li>
                    </ItemTemplate>
                </asp:ListView>
                <asp:ObjectDataSource ID="odsAllRoles" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="GetAll" TypeName="CAESDO.Catbert.BLL.RoleBLL">
                    <SelectParameters>
                        <asp:Parameter DefaultValue="Name" Name="propertyName" Type="String" />
                        <asp:Parameter DefaultValue="true" Name="ascending" Type="Boolean" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </div>
        </div>
    </div>
	</div>
</asp:Content>

