<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<%
    if (Request.IsAuthenticated) {
%>
        Welcome <b><%= Html.Encode(Page.User.Identity.Name) %></b>!<br />
        <%= Html.ActionLink("Log out", "LogOut", "Account") %>
<%
    }
    else {
%> 
        | <%= Html.ActionLink("Log in", "LogOn", "Account") %> |
<%
    }
%>