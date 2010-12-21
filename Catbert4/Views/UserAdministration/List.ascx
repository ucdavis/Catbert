<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<System.Collections.Generic.List<Catbert4.Models.UserListModel>>" %>
<table id="users" class="display">
        <thead>
            <tr>
                <th>
                </th>
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
        </thead>
        <tbody>
            <% foreach (var item in Model)
               { %>
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
        </tbody>
    </table>