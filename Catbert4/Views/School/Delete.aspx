﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Catbert4.Core.Domain.School>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Delete
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Delete</h2>

    <h3>Are you sure you want to delete this?</h3>
    <fieldset>
        <legend>Fields</legend>
        
        <table>
            <%: Html.DisplayForModel() %>
        </table>
        
    </fieldset>
    <% using (Html.BeginForm()) { %>
		<%= Html.AntiForgeryToken() %>
        <p>
		    <input type="submit" value="Delete" /> |
		    <img class="back" src="<%: Url.Image("back.png") %>" alt="Back to List TODO!" />&nbsp;<%: Html.ActionLink("Back to List", "Index") %>
        </p>
    <% } %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

