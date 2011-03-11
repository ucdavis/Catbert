<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<Catbert4.Controllers.UnitViewModel>" MasterPageFile="~/Views/Shared/Site.Master" %>
<asp:Content runat="server" ID="Title" ContentPlaceHolderID="TitleContent"></asp:Content>
<asp:Content runat="server" ID="Header" ContentPlaceHolderID="HeaderContent"></asp:Content>
<asp:Content runat="server" ID="Main" ContentPlaceHolderID="MainContent">

    <h2>Create</h2>

	<%= Html.ClientSideValidation<Catbert4.Core.Domain.Unit>("Unit") %>

    <% using (Html.BeginForm()) {%>
        <%= Html.AntiForgeryToken() %>
        <%: Html.ValidationSummary(false) %>
        
        <fieldset>
            <legend>Fields</legend>
            
            <% Html.RenderPartial("UnitForm"); %>

            
            <br />
                <input type="submit" value="Create" />
            
        </fieldset>

    <% } %>

    <p>
        <img class="back" src="<%: Url.Image("back.png") %>" alt="Back TODO!" />&nbsp;<%: Html.ActionLink("Back to List", "Index") %>
    </p>

</asp:Content>
