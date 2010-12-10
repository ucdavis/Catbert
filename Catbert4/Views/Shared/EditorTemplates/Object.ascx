<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<% if (ViewData.TemplateInfo.TemplateDepth > 1) { %>
    <%= ViewData.ModelMetadata.SimpleDisplayText %>
<% } else { %>
    <% foreach (var prop in ViewData.ModelMetadata.Properties.Where(pm => pm.ShowForEdit && pm.PropertyName != "Id" && !ViewData.TemplateInfo.Visited(pm)))
       { %>
        <% if (!prop.IsComplexType)
           { %>
            <% if (prop.HideSurroundingHtml)
               { %>
                <%= Html.Editor(prop.PropertyName)%>
            <% }
               else
               { %>
                <tr>
                    <td>
                        <div class="editor-label" style="text-align: right;">
                            <%= prop.IsRequired ? "*" : ""%>
                            <%= Html.Label(prop.PropertyName)%>
                        </div>
                    </td>
                    <td>
                        <div class="editor-field">
                            <%= Html.Editor(prop.PropertyName)%>
                            <%= Html.ValidationMessage(prop.PropertyName, "*")%>
                        </div>
                    </td>
                </tr>
            <% } %>
        <% } %>
    <% } %>
<% } %>
