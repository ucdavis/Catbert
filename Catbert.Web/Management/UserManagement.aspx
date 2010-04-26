<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserManagement.aspx.cs" Inherits="Management_UserManagement" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="theme/ui.all.css" rel="stylesheet" type="text/css" />
	<style type="text/css">
		body{ font: 62.5% Verdana, sans-serif; margin: 50px;}
		/*demo page css*/
		#dialog_link {padding: .4em 1em .4em 20px;text-decoration: none;position: relative;}
		#dialog_link span.ui-icon {margin: 0 5px 0 0;position: absolute;left: .2em;top: 50%;margin-top: -8px;}
		ul#icons {margin: 0; padding: 0;}
		ul#icons li {margin: 2px; position: relative; padding: 4px 0; cursor: pointer; float: left;  list-style: none;}
		ul#icons span.ui-icon {float: left; margin: 0 4px;}
	</style>
</head>
<body>
    <form id="form1" runat="server">
    <AjaxControlToolkit:ToolkitScriptManager ID="scriptManager" runat="server">
        <Scripts>
            <asp:ScriptReference Path="~/JS/jquery-1.3.1.min.js" />
            <asp:ScriptReference Path="~/JS/jquery.tablesorter.min.js" />
            <asp:ScriptReference Path="~/JS/jquery.quicksearch.js" />
            <asp:ScriptReference Path="~/JS/jquery.ui.all.js" />
        </Scripts>
    </AjaxControlToolkit:ToolkitScriptManager>
    <% if (false) //Load the vsdoc for intellisense only
       { %>

    <script src="../JS/jquery-1.3.1-vsdoc.js" type="text/javascript"></script>

    <% } %>
    <script type="text/javascript">
        $(document).ready(function() {
            $("#dialog").dialog({
                autoOpen: false,
                width: 600,
                buttons: {
                    "Ok": function() {
                        $(this).dialog("close");
                    },
                    "Cancel": function() {
                        $(this).dialog("close");
                    }
                }
            });

            $("#dialog_link").click(function() {
                $("#dialog").dialog('open');
                return false;
            });
        })
    </script>
    <div>
    
    <a href="#" id="dialog_link" class="ui-state-default ui-corner-all"><span class="ui-icon ui-icon-newwin"></span>Open Dialog</a>
    <br /><br />
	<!-- ui-dialog -->
	<div id="dialog" title="Dialog Title" style="display:none;">
		<p>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.</p>
	</div>
    
    			<div class="ui-widget">
				<div class="ui-state-error ui-corner-all" style="padding: 0 .7em;"> 
					<p><span class="ui-icon ui-icon-alert" style="float: left; margin-right: .3em;"></span> 
					<strong>Alert:</strong> Sample ui-state-error style.</p>
				</div>
			</div>
    </div>
    </form>
</body>
</html>
