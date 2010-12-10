<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<UnitViewModel>" %>

<table>
    <%: Html.EditorFor(x=>x.Unit) %>
    <tr>
        <td>
            <div class="editor-label" style="text-align: right;">
                <%: Html.LabelFor(x=>x.Unit.School) %>
            </div>
        </td>
        <td>
            <div class="editor-field">
                <%= this.Select("Unit.School")
                    .Options(Model.Schools, x=>x.Id, x=>x.ShortDescription)
                    .Selected(Model.Unit.School == null ? null : Model.Unit.School.Id)
                    .FirstOption("Select A School") %>
                <%: Html.ValidationMessageFor(x=>x.Unit.School, "*")%>
            </div>
        </td>
    </tr>
</table>