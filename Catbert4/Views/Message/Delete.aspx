﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Catbert4.Core.Domain.Message>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Delete Message
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Delete Message</h2>

    <h3>Are you sure you want to delete this?</h3>
    <fieldset>
        <legend>Fields</legend>
        
        <table>
            <%: Html.DisplayForModel("DisplayComplexValues") %>
        </table>

    </fieldset>
    <% using (Html.BeginForm()) { %>
		<%= Html.AntiForgeryToken() %>
        <p>
		    <img class="back" src="<%: Url.Image("delete.png") %>" alt="Delete TODO!" />&nbsp;<input type="submit" value="Delete" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
		    <img class="back" src="<%: Url.Image("back.png") %>" alt="Back to List TODO!" />&nbsp;<%: Html.ActionLink("Back to List", "Index") %>
        </p>
    <% } %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

