<%@ Page Title="" Language="C#" MasterPageFile="~/CatbertMaster.master" AutoEventWireup="true" CodeFile="UserManagement.aspx.cs" Inherits="Admin_UserManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">

<script type="text/javascript">
    $(document).ready(function() {
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

        OpenDialog(dialogUserInfo, buttons, "User Information", null);
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
        <p>
            Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor
            incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud
            exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.
        </p>
    </div>

</asp:Content>

