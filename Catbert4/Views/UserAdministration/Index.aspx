<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Catbert4.Models.UserListModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Users
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Users List</h2>

    <div class="ui-widget">
	    <label for="user-search">Search Users: </label>
	    <input id="user-search" />
    </div>

    <table>
        <tr>
            <th></th>
            <th>
                Login
            </th>
            <th>
                FirstName
            </th>
            <th>
                LastName
            </th>
            <th>
                Email
            </th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%: Html.ActionLink("Edit", "Edit", new { id=item.Login }) %>
            </td>
            <td>
                <%: item.Login %>
            </td>
            <td>
                <%: item.FirstName %>
            </td>
            <td>
                <%: item.LastName %>
            </td>
            <td>
                <%: item.Email %>
            </td>
        </tr>
    
    <% } %>

    </table>

    <p>
        <%: Html.ActionLink("Create New", "Create") %>
    </p>

<script id="search-user-template" type="text/x-jquery-tmpl">
    <li data-item.autocomplete="${value}">
        <a>${FirstName} ${LastName} (${value})
            <br/>
            ${Email}
        </a>
    </li>
</script>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">

<%: Catbert4.Helpers.HtmlScriptHelpers.IncludeJqueryTemplate()%>

<script type="text/javascript">
    $(function () {
        $("#user-search").autocomplete({
            source: "<%: Url.Action("SearchUsers") %>",
            minLength: 2,
            select: function (event, ui) {
                this.value = ui.item;

                window.location = "<%: Url.Action("Edit") %>" + "/" + ui.item; 
                return false;
            }
        })
        .data( "autocomplete" )._renderItem = function( ul, item ) {
			var template = $("#search-user-template").tmpl(item);
            
            return template.appendTo(ul);
		};

    });
</script>
    
</asp:Content>