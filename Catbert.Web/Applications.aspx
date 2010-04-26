﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Catbert.master" AutoEventWireup="true" CodeFile="Applications.aspx.cs" Inherits="Applications" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
		body{ font: 62.5% Verdana, sans-serif; margin: 50px;}
		/*demo page css*/
		#dialog_link {padding: .4em 1em .4em 20px;text-decoration: none;position: relative;}
		#dialog_link span.ui-icon {margin: 0 5px 0 0;position: absolute;left: .2em;top: 50%;margin-top: -8px;}
	</style>
    <link href="CSS/fcbklistselection.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
    <script src="JS/jquery.tablesorter.min.js" type="text/javascript"></script>
    <%--<script src="JS/fcbklistselection.js" type="text/javascript"></script>--%>
    <script src="JS/multiselection.js" type="text/javascript"></script>
    <script type="text/javascript">
        var baseUrl = 'Services/CatbertWebService.asmx/';
        var roleList;

        $(document).ready(function() {
             roleList = $('#ulRoles');
            //Sort table
            $("#tblApplications").tablesorter({
                headers: { 0: { sorter: false} },
                sortList: [[1, 0]],
                cssAsc: 'headerSortUp',
                cssDesc: 'headerSortDown',
                cssHeader: 'header',
                widgets: ['zebra']
            });

            //List selection TEST ONLY
            //$.fcbkListSelection(id[ul id],width,height[element height],row[elements in row]);
            //$.fcbkListSelection("#ulPermissions", 400, 50, 2);
        });

        function ShowUserInfo(applicationID, name) {
            var dialog = $("#dialogUserInfo"); //the dialog div

            dialog.dialog({
                autoOpen: true,
                width: 600,
                modal: true,
                title: name,
                buttons: {
                    "Close": function() {
                        $(this).dialog("close");
                    }
                }
            });

            dialog.dialog('option', 'title', name); //Set the title
            
            //TODO: The loading options
            //Clear out the roles list checked options
            
            AjaxCall(
                baseUrl + 'GetApplication',
                { application: name },
                function(data) { PopulateApplication(data); },
                null //TODO: Error method
            );
            
            /*
            var inner = $('<li><input type="checkbox" checked="checked" value="RoleName" /></li>').append('Admin Role');
            var inner2 = $('<li><input type="checkbox" checked="checked" value="RoleName" /></li>').append('Admin Role');
            var inner3 = $('<li><input type="checkbox" checked="checked" value="RoleName" /></li>').append('Admin Role');
            var inner4 = $('<li><input type="checkbox" checked="checked" value="RoleName" /></li>').append('Admin Role');
            
            var roleList = $('#ulRoles'); //roles list
            roleList.append(inner);
            roleList.append(inner2);
            roleList.append(inner3);
            roleList.append(inner4);
            */
            /*
            $("#ulPermissions").mselect(
                {
                    name: "tester"
                }
            );
            */
            
            dialog.dialog('open'); //show
        }
        
        function PopulateApplication(app) {
            //Go through each role and check the corresonding box
            
            for (var i in app.Roles) {
                var roleName = app.Roles[i].Name;
                var roleBox = $("input[value=" + roleName + "]", roleList); //Find the one role with the value of roleName                
                roleBox.attr('checked', 'checked');//Check it
            }
        }
        
    </script>
    
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
            <tr>
                <td>
                    <a href="javascript:;" id="dialog_link" class="ui-state-default ui-corner-all" onclick='ShowUserInfo(<%# Eval("ID") %>, "<%# Eval("Name") %>");'>
                        <span class="ui-icon ui-icon-newwin"></span>
                        Select 
                    </a>
                </td>
                <td>
                    <%# Eval("Name") %>
                </td>
                <td>
                    <%# Eval("Abbr") %>
                </td>
                <td>
                    <%# Eval("Location") %>
                </td>
                <td>
                    <%# !(bool)Eval("Inactive") %>
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
		<p>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.</p>
		<br />

        <div id="Roles">
            Roles:
            <asp:ListView ID="lviewRoles" runat="server" DataSourceID="odsRoles">
                <LayoutTemplate>
                    <ul id="ulRoles">
                        <li id="itemPlaceholder" runat="server"></li>
                    </ul>
                </LayoutTemplate>
                <ItemTemplate>
                    <li>
                        <input type="checkbox" value="<%# Eval("Name") %>" /><%# Eval("Name") %>
                    </li>
                </ItemTemplate>
            </asp:ListView>
            <asp:ObjectDataSource ID="odsRoles" runat="server" 
                OldValuesParameterFormatString="original_{0}" SelectMethod="GetAll" 
                TypeName="CAESDO.Catbert.BLL.RoleBLL">
                <SelectParameters>
                    <asp:Parameter DefaultValue="Name" Name="propertyName" Type="String" />
                    <asp:Parameter DefaultValue="true" Name="ascending" Type="Boolean" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </div>
	</div>
</asp:Content>

