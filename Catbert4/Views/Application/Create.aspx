<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<Catbert4.Controllers.ApplicationViewModel>" MasterPageFile="~/Views/Shared/Site.Master" %>
<asp:Content runat="server" ID="Title" ContentPlaceHolderID="TitleContent">Create</asp:Content>
<asp:Content runat="server" ID="Main" ContentPlaceHolderID="MainContent">
    <h2>Create</h2>

	<%= Html.ClientSideValidation<Catbert4.Core.Domain.Application>("Application") %>

    <% using (Html.BeginForm()) {%>
        <%= Html.AntiForgeryToken() %>
        <%: Html.ValidationSummary(true) %>

        <% Html.RenderPartial("ApplicationForm"); %>

        <p>
            <input id="create" type="submit" value="Create" />
        </p>

    <% } %>

    <p>
        <img class="back" src="<%: Url.Image("back.png") %>" alt="Back to List TODO!" />&nbsp;<%: Html.ActionLink("Back to List", "Index") %>
    </p>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    <% Html.RenderPartial("ApplicationFormHeaderContent"); %>
</asp:Content>