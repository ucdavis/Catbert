<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Catbert4.Controllers.UnitViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Edit</h2>

	<%= Html.ClientSideValidation<Catbert4.Core.Domain.Unit>("Unit") %>

    <% using (Html.BeginForm()) {%>
        <%= Html.AntiForgeryToken() %>
        <%: Html.ValidationSummary(false) %>
        <%: Html.HiddenFor(x=>x.Unit.Id) %>

        <fieldset>
            <legend>Fields</legend>
            
            <% Html.RenderPartial("UnitForm"); %>

            <br />
                <input type="submit" value="Edit" />
            
        </fieldset>

    <% } %>

    <p>
        <img class="back" src="<%: Url.Image("back.png") %>" alt="Back TODO!" />&nbsp;<%: Html.ActionLink("Back to List", "Index") %>
    </p>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

