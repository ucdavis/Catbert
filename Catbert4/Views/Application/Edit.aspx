<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Catbert4.Controllers.ApplicationViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Edit</h2>

	<%= Html.ClientSideValidation<Catbert4.Core.Domain.Application>("Application") %>

    <% using (Html.BeginForm()) {%>
        <%= Html.AntiForgeryToken() %>
        <%: Html.ValidationSummary(true) %>

        <fieldset>
            <legend>Application Details</legend>
            
             <table>
                <%: Html.EditorFor(x=>x.Application) %>
             </table>

        </fieldset>

        <fieldset>
            <legend>Roles</legend>
            <h3>Ordered:</h3>
            <ul id="ordered-roles" class="connectedSortable">
                <% foreach (var role in Model.Application.ApplicationRoles.Where(x=>x.Level != null).OrderBy(x=>x.Level)) { %>
                    <li class="ui-state-default" id="<%: role.Role.Name %>">
                        (<%: role.Level %>): <%: role.Role.Name %>
                    </li>
                <% } %>
            </ul>

            <h3>Unordered:</h3>
            <ul id="unordered-roles" class="connectedSortable">
                <% foreach (var role in Model.Application.ApplicationRoles.Where(x=>x.Level == null)) { %>
                    <li class="ui-state-default" id="<%: role.Role.Name %>">
                        <%: role.Role.Name %>
                    </li>
                <% } %>
            </ul>

            <h3>Available:</h3>

            <ul class="connectedSortable">
                <% foreach (var role in Model.GetAvailableRoles()) { %>
                    <li class="ui-state-default" id="<%: role.Name %>">
                        <%: role.Name %>
                    </li>
                <% } %>
            </ul>
    </fieldset>

            <p>
                <input id="edit" type="submit" value="Edit" />
            </p>

    <% } %>

    <div>
        <%: Html.ActionLink("Back to List", "Index") %>
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
<style type="text/css">
    .connectedSortable { list-style-type: none; margin: 0; padding: 0; margin-right: 10px; background: #eee; padding: 5px; width: 220px;}
	.connectedSortable li { margin: 5px; padding: 5px; font-size: 1.2em;  }
		
</style>

<script type="text/javascript">
    $(function () {
        $(".connectedSortable").sortable({
            connectWith: ".connectedSortable"
        }).disableSelection();

        $("form").submit(function (e) {
            e.preventDefault();

            var data = $(this).serializeArray();

            CollectRoleData(data);

            var jsonEditUrl = '<%: Url.Action("Edit", new { id = Model.Application.Id}) %>';

            $.post(
                jsonEditUrl,
                data,
                function (result) {
                    if (result.success) {
                        window.location = '<%: Url.Action("Index") %>';
                    }
                },
                'json');
        });
    });

    //Get all of the roles, serialize them into string arrays and then push them into the data
    function CollectRoleData(data) {
        var ordered = $('#ordered-roles').sortable('toArray');
        var unordered = $('#unordered-roles').sortable('toArray');

        $(ordered).each(function (index, value) {
            data.push({name:'orderedRoles', value: value});
        });

        $(unordered).each(function (index, value) {
            data.push({ name: 'unorderedRoles', value: value });
        });
    }
</script>

</asp:Content>

