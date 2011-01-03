<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<% if (Model == null) { %>
    <%= ViewData.ModelMetadata.NullDisplayText %>
<% } else if (ViewData.TemplateInfo.TemplateDepth > 1) { %>
    <%= ViewData.ModelMetadata.SimpleDisplayText %>
<% } else { %>
    <% foreach (var prop in ViewData.ModelMetadata.Properties.Where(pm => pm.ShowForDisplay && pm.PropertyName != "Id" && !ViewData.TemplateInfo.Visited(pm))) { %>
        <% if (!prop.IsComplexType)
           { %>
            <% if (prop.HideSurroundingHtml)
               { %>
                <%= Html.Display(prop.PropertyName)%>
            <% }
               else
               { %>
                <tr>
                    <td>
                        <div class="display-label" style="text-align: right;">
                            <%= prop.GetDisplayName()%>
                        </div>
                    </td>
                    <td>
                        <div class="display-field">
                            <%= Html.Display(prop.PropertyName)%>
                        </div>
                    </td>
                </tr>
            <% } %>
        <% } %>
    <% } %>
<% } %>