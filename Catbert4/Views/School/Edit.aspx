﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Catbert4.Controllers.SchoolViewModel>" %>
<%@ Import Namespace="Catbert4.Core.Domain" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Edit</h2>

	<%= Html.ClientSideValidation<School>("School") %>

    <% using (Html.BeginForm()) {%>
        <%= Html.AntiForgeryToken() %>
        <%: Html.ValidationSummary(true) %>

        <fieldset>
            <legend>Fields</legend>
    
            <table>
                <%: Html.EditorFor(x=>x.School) %>
            </table>

            <br />
                <input type="submit" value="Save" />
            
        </fieldset>

    <% } %>

    <p>
        <img class="back" src="<%: Url.Image("back.png") %>" alt="Back to List TODO!" />&nbsp;<%: Html.ActionLink("Back to List", "Index") %>
    </p>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

