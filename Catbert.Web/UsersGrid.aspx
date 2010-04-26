﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Catbert.master" AutoEventWireup="true" CodeFile="Users.aspx.cs" Inherits="Users" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<style type="text/css">
		body{ font: 62.5% Verdana, sans-serif; margin: 50px;}
		/*demo page css*/
		#dialog_link {padding: .4em 1em .4em 20px;text-decoration: none;position: relative;}
		#dialog_link span.ui-icon {margin: 0 5px 0 0;position: absolute;left: .2em;top: 50%;margin-top: -8px;}
	</style>
    <link href="CSS/grid.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
    <script src="JS/jquery.jqGrid.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function() {
            $("#tblUsers").jqGrid({
                url: 'Services/CatbertWebService.asmx/jqGetUsersByApplication',
                datatype: 'json',
                mtype: 'POST',
                colNames: ['Login', 'First Name', 'Last Name', 'Email'],
                colModel: [
                    { name: 'Login', index: 'Login', width: 90 },
                    { name: 'FirstName', index: 'FirstName', width: 100 },
                    { name: 'LastName', index: 'LastName', width: 80 },
                    { name: 'Email', index: 'Email', width: 130 }
                    ],
                postData: { login: "postit", application: "Catbert" },
                jsonReader: {
                    records: 'rows',
                    repeatitems: false, 
                    id: "0" 
                }
            });

            /*
            var json = { login: "postit", application: "Catbert" };
            var test = JSON2.stringify(json);
            //debugger;
            $("#tblUsers").flexigrid({
            url: "Services/CatbertWebService.asmx/DataGetUsersByApplication",
            dataType: 'json',
            method: 'POST',
            params: json,
            colModel: [
            { display: 'Login', name: 'login', width: 40, sortable: true, align: 'center' },
            { display: 'FirstName', name: 'firstname', width: 180, sortable: true, align: 'left' },
            { display: 'LastName', name: 'lastname', width: 120, sortable: true, align: 'left' },
            { display: 'Email', name: 'email', width: 130, sortable: true, align: 'left', hide: false },
            ],
            searchitems: [
            { display: 'Any', name: 'any', isdefault: true },
            { display: 'FirstName', name: 'firstname' },
            { display: 'LastName', name: 'lastname' }
            ],
            sortname: "lastname",
            sortorder: "asc",
            usepager: true,
            title: 'Users',
            useRp: true,
            rp: 20,
            showTableToggleBtn: false, //Seems to show anyway
            width: 700,
            height: 200,
            onToggleCol: function() {
            alert("Toggle Just Occurred");
            }
                
            });
            */
        });
    
        function ShowUserInfo(login, name) {
            $("#dialogUserInfo").dialog({
                autoOpen: false,
                width: 600,
                buttons: {
                    "Close": function() {
                        $(this).dialog("close");
                    }
                }
            });

            $("#dialogUserInfo").dialog('open');
        }
    </script>
    
    <table id="tblUsers" class="scroll"></table>
    
    <br /><br />

	<div class="ui-widget">
		<div class="ui-state-error ui-corner-all" style="padding: 0 .7em;"> 
			<p><span class="ui-icon ui-icon-alert" style="float: left; margin-right: .3em;"></span> 
			<strong>Alert:</strong> Page construction in progress.</p>
		</div>
	</div>
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
                    <a href="#" id="dialog_link" class="ui-state-default ui-corner-all" onclick='ShowUserInfo("<%# Eval("LoginID") %>", "<%# Eval("LastName") + ", " + Eval("FirstName") %>");'>
                        <span class="ui-icon ui-icon-newwin"></span>Select
                    </a>
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

    <!-- ui-dialog -->
	<div id="dialogUserInfo" title="User Information" style="display:none;">
		<p>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.</p>
	</div>
	
</asp:Content>