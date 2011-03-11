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
        <%: Html.HiddenFor(x=>x.Application.Id) %>

        <% Html.RenderPartial("ApplicationForm"); %>

        <p>
            <input id="edit" type="submit" value="Save" />
        </p>

    <% } %>

    <p>
        <img class="back" src="<%: Url.Image("back.png") %>" alt="Back to List TODO!" />&nbsp;<%: Html.ActionLink("Back to List", "Index") %>
    </p>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    <% Html.RenderPartial("ApplicationFormHeaderContent"); %>
</asp:Content>

