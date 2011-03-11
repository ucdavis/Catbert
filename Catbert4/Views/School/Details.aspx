<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Catbert4.Controllers.SchoolViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Details</h2>

    <fieldset>
        <legend>School Info:</legend>
        
        <table>
            <%: Html.DisplayFor(x=>x.School) %>
        </table>

    </fieldset>

    <fieldset>
        <legend>Associated Units:</legend>

        <ul>
            <% foreach (var unit in Model.Units) { %>
                <li>
                    <%: unit.ShortName %> (<%: unit.FisCode %>)
                </li>                
            <%} %>
        </ul>
    </fieldset>

    <p>
        <img class="back" src="<%: Url.Image("edit.png") %>" alt="Back to List TODO!" />&nbsp;<%: Html.ActionLink("Edit", "Edit", new { id = Model.School.Id }) %>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <img class="back" src="<%: Url.Image("back.png") %>" alt="Back to List TODO!" />&nbsp;<%: Html.ActionLink("Back to List", "Index") %>
    </p>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

