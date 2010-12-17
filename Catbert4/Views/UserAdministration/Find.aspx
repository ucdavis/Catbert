<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage" MasterPageFile="~/Views/Shared/Site.Master" %>
<asp:Content runat="server" ID="Title" ContentPlaceHolderID="TitleContent">Find User</asp:Content>
<asp:Content runat="server" ID="Main" ContentPlaceHolderID="MainContent">
<h2>Find User</h2>

<h5>Who would you like to add?</h5>

<% using(Html.BeginForm("Find", "UserAdministration", FormMethod.Get)) { %>
    <div class="ui-widget">
	    <label for="user-search">Kerberos or Email: </label>
	    <input id="user-search" name="searchTerm" value="<%: ViewData["searchTerm"] %>" />
        <input id="submit-search" type="submit" value="Search" />
    </div>
<% } %>

</asp:Content>
<asp:Content runat="server" ID="Header" ContentPlaceHolderID="HeaderContent">

<script type="text/javascript">
    $(function () {
        $("#submit-search").button();
    });
</script>

</asp:Content>