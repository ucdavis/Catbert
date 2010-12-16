<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Catbert4.Controllers.MessageViewModel>" %>

<table>
    <tr>
        <td>
            <div style="text-align: right;" class="editor-label">
                <%: Html.LabelFor(x=>x.Message.Application) %>
            </div>
        </td>
        <td>
            <div class="editor-field">
                <%= this.Select("Message.Application")
                    .Options(Model.Applications, x=>x.Id, x=>x.Name)
                    .Selected(Model.Message.Application == null ? 0 : Model.Message.Application.Id)
                    .FirstOption("[Optional] Select An Application")%>
                <%: Html.ValidationMessageFor(x=>x.Message.Application) %>                            
            </div>
        </td>
    </tr>
    <%: Html.EditorFor(x=>x.Message) %>    
</table>