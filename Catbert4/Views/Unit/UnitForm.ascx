<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<UnitViewModel>" %>

<table>
    <tr>
        <td>
            <div class="editor-label" style="text-align: right;">
                <%: Html.LabelFor(x=>x.Unit.FullName) %>
            </div>
        </td>        
        <td>
            <div class="editor-field">
                <%: Html.EditorFor(x=>x.Unit.FullName) %>
                <%: Html.ValidationMessageFor(x=>x.Unit.FullName, "*")%>
            </div>
        </td>        
    </tr>
    <tr>
        <td>
            <div class="editor-label" style="text-align: right;">
                <%: Html.LabelFor(x=>x.Unit.ShortName) %>
            </div>
        </td>        
        <td>
            <div class="editor-field">
                <%: Html.EditorFor(x=>x.Unit.ShortName) %>
                <%: Html.ValidationMessageFor(x=>x.Unit.ShortName, "*")%>
            </div>
        </td>        
    </tr>
    <tr>
        <td>
            <div class="editor-label" style="text-align: right;">
                <%: Html.LabelFor(x=>x.Unit.PpsCode) %>
            </div>
        </td>        
        <td>
            <div class="editor-field">
                <%: Html.EditorFor(x=>x.Unit.PpsCode) %>
                <%: Html.ValidationMessageFor(x=>x.Unit.PpsCode, "*")%>
            </div>
        </td>        
    </tr>
    <tr>
        <td>
            <div class="editor-label" style="text-align: right;">
                <%: Html.LabelFor(x=>x.Unit.FisCode) %>
            </div>
        </td>        
        <td>
            <div class="editor-field">
                <%: Html.EditorFor(x=>x.Unit.FisCode) %>
                <%: Html.ValidationMessageFor(x=>x.Unit.FisCode, "*")%>
            </div>
        </td>        
    </tr>
    <tr>
        <td>
            <div class="editor-label" style="text-align: right;">
                <%: Html.LabelFor(x=>x.Unit.Type) %>
            </div>
        </td>        
        <td>
            <div class="editor-field">
                <%= this.Select("Unit.Type")
                    .Options(Enum.GetNames(typeof(Catbert4.Core.Domain.UnitType)).ToList()) 
                    .Selected(Model.Unit.Type.ToString()) %>
                <%: Html.ValidationMessageFor(x=>x.Unit.Type, "*")%>
            </div>
        </td>        
    </tr>
    <tr>
        <td>
            <div class="editor-label" style="text-align: right;">
                <%: Html.LabelFor(x=>x.Unit.Parent) %>
            </div>
        </td>
        <td>
            <div class="editor-field">
                <%= this.Select("Unit.Parent")
                    .Options(Model.Units, x=>x.Id, x=>x.ShortName)
                    .Selected(Model.Unit.Parent == null ? 0 : Model.Unit.Parent.Id)
                    .FirstOption("[Optional] Select A Parent Unit") %>
                <%: Html.ValidationMessageFor(x=>x.Unit.Parent, "*")%>
            </div>
        </td>
    </tr><tr>
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