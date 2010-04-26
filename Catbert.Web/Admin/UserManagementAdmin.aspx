<%@ Page Title="" Language="C#" MasterPageFile="~/CatbertMaster.master" AutoEventWireup="true" CodeFile="UserManagementAdmin.aspx.cs" Inherits="Admin_UserManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">

<script type="text/javascript">
    var tabs;
    var baseURL = '../Admin/UserInformationService.asmx/';
    
    $(document).ready(function() {
        
        tabs = $('#tabs').tabs();
        
        $('a.ShowUserLink').click(ShowUserInfo);
    });

    function ShowUserInfo() {
        var loginId = $(this).html();

        console.info(loginId);
        //alert(loginId);

        var dialogUserInfo = $("#dialogUserInfo");

        var buttons = {
            "Close": function() {
                //$("#divUserInfo").hide(0);
                $(this).dialog("close");
            }
        }

        tabs.tabs('select', 0); //select the first tab by default when viewing a new user

        OpenDialog(dialogUserInfo, buttons, "User Information", null);

        var url = baseURL + 'GetUserInfo';
        
        AjaxCall(
                url,
                { loginId: loginId },
                function(data) { PopulateUserInfo(data); },
                null //TODO: Error method
            );
    }

    function PopulateUserInfo(data) {
    
        var loginId = data.LoginId;
        var roles = $("#tblPermissions tbody");
        var units = $("#tblUnits tbody");

        //Clear out the old roles and units
        roles.empty();
        units.empty();

        $(data.PermissionAssociations).each(function() {
            var newRoleRow = CreateRoleRow(this.RoleName, loginId, this.ApplicationName);

            roles.append(newRoleRow);
        });

        $(data.UnitAssociations).each(function() {
            var newUnitRow = CreateUnitRow(this.UnitFIS, loginId, this.ApplicationName);

            units.append(newUnitRow);
        });
    }

    function CreateRoleRow(role, login, application) {
        var newrow = $('<tr></tr>');

        var deleteLink = $('<input type="button" value="X" />');
        deleteLink.click(function() { alert('Not Implemented'); }); //TODO
        //deleteLink.click(function() { DeleteRole(login, role, application, newrow); });

        newrow.append('<td>' + application + '</td>');
        newrow.append('<td>' + role + '</td>');

        var deleteCol = $('<td>').append(deleteLink);
        newrow.append(deleteCol);

        return newrow;
    }

    function CreateUnitRow(unit, login, application) {
        var newrow = $('<tr></tr>');

        var deleteLink = $('<input type="button" value="X" />');
        deleteLink.click(function() { alert('Not Implemented'); }); //TODO
        //deleteLink.click(function() { DeleteUnit(login, unitFIS, application, newrow); });

        newrow.append('<td>' + application + '</td>');
        newrow.append('<td>' + unit + '</td>');

        var deleteCol = $('<td>').append(deleteLink);
        newrow.append(deleteCol);

        return newrow;
    }

    function OpenDialog(dialog /*The dialog DIV JQuery object*/, buttons /*Button collection */, title, onClose) {

        dialog.dialog("destroy"); //Reset the dialog to its initial state
        dialog.dialog({
            autoOpen: true,
            closeOnEscape: false,
            width: 600,
            height: 600,
            modal: true,
            title: title,
            buttons: buttons,
            //show: 'fold',
            close: onClose
        });
    }
</script>

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
            <td>
                <a class="ShowUserLink" href="javascript:;"><%# Eval("LoginID") %></a>
            </td>
            <td><%# Eval("FirstName") %></td>
            <td><%# Eval("LastName") %></td>
            <td><%# Eval("Email") %></td>
        </tr>
    </ItemTemplate>
</asp:ListView>

    <asp:ObjectDataSource ID="odsActiveUsers" runat="server" 
        OldValuesParameterFormatString="original_{0}" SelectMethod="GetAllActive" 
        TypeName="CAESDO.Catbert.BLL.UserBLL"></asp:ObjectDataSource>
    
    <div id="dialogUserInfo" title="User Information" style="display: none;">
        <div id="tabs">
            <ul>
                <li><a href="#tabPermissions">Permissions</a></li>
                <li><a href="#tabUnits">Units</a></li>
                <li><a href="#tabInfo">Info</a></li>
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
            
                <p>
                    Proin elit arcu, rutrum commodo, vehicula tempus, commodo a, risus. Curabitur nec
                    arcu. Donec sollicitudin mi sit amet mauris. Nam elementum quam ullamcorper ante.
                    Etiam aliquet massa et lorem. Mauris dapibus lacus auctor risus. Aenean tempor ullamcorper
                    leo. Vivamus sed magna quis ligula eleifend adipiscing. Duis orci. Aliquam sodales
                    tortor vitae ipsum. Aliquam nulla. Duis aliquam molestie erat. Ut et mauris vel
                    pede varius sollicitudin. Sed ut dolor nec orci tincidunt interdum. Phasellus ipsum.
                    Nunc tristique tempus lectus.</p>
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
                <p>
                    Morbi tincidunt, dui sit amet facilisis feugiat, odio metus gravida ante, ut pharetra
                    massa metus id nunc. Duis scelerisque molestie turpis. Sed fringilla, massa eget
                    luctus malesuada, metus eros molestie lectus, ut tempus eros massa ut dolor. Aenean
                    aliquet fringilla sem. Suspendisse sed ligula in ligula suscipit aliquam. Praesent
                    in eros vestibulum mi adipiscing adipiscing. Morbi facilisis. Curabitur ornare consequat
                    nunc. Aenean vel metus. Ut posuere viverra nulla. Aliquam erat volutpat. Pellentesque
                    convallis. Maecenas feugiat, tellus pellentesque pretium posuere, felis lorem euismod
                    felis, eu ornare leo nisi vel felis. Mauris consectetur tortor et purus.</p>
            </div>
            <div id="tabInfo">
                <p>
                    Mauris eleifend est et turpis. Duis id erat. Suspendisse potenti. Aliquam vulputate,
                    pede vel vehicula accumsan, mi neque rutrum erat, eu congue orci lorem eget lorem.
                    Vestibulum non ante. Class aptent taciti sociosqu ad litora torquent per conubia
                    nostra, per inceptos himenaeos. Fusce sodales. Quisque eu urna vel enim commodo
                    pellentesque. Praesent eu risus hendrerit ligula tempus pretium. Curabitur lorem
                    enim, pretium nec, feugiat nec, luctus a, lacus.</p>
                <p>
                    Duis cursus. Maecenas ligula eros, blandit nec, pharetra at, semper at, magna. Nullam
                    ac lacus. Nulla facilisi. Praesent viverra justo vitae neque. Praesent blandit adipiscing
                    velit. Suspendisse potenti. Donec mattis, pede vel pharetra blandit, magna ligula
                    faucibus eros, id euismod lacus dolor eget odio. Nam scelerisque. Donec non libero
                    sed nulla mattis commodo. Ut sagittis. Donec nisi lectus, feugiat porttitor, tempor
                    ac, tempor vitae, pede. Aenean vehicula velit eu tellus interdum rutrum. Maecenas
                    commodo. Pellentesque nec elit. Fusce in lacus. Vivamus a libero vitae lectus hendrerit
                    hendrerit.</p>
            </div>
        </div>
    </div>

</asp:Content>

