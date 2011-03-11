<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Catbert4.Controllers.MessageViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit Message
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Edit Message</h2>

	<%= Html.ClientSideValidation<Catbert4.Core.Domain.Message>("Message") %>

    <% using (Html.BeginForm()) {%>
        <%= Html.AntiForgeryToken() %>
        <%: Html.ValidationSummary(false) %>
        <%: Html.HiddenFor(x=>x.Message.Id) %>

        <fieldset>
            <legend>Fields</legend>
            
            <% Html.RenderPartial("MessageForm"); %>
            <br />
            <input type="submit" value="Edit" />
            
        </fieldset>

    <% } %>

    <p>
        <img class="back" src="<%: Url.Image("back.png") %>" alt="Back to List TODO!" />&nbsp;<%: Html.ActionLink("Back to List", "Index") %>
    </p>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

