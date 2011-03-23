<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage" MasterPageFile="~/Views/Shared/Site.Master" %>
<asp:Content runat="server" ID="Title" ContentPlaceHolderID="TitleContent"></asp:Content>
<asp:Content runat="server" ID="Header" ContentPlaceHolderID="HeaderContent">
<% ViewData["body-class"] = "front-page";%>
<style type="text/css">
.front-page #menucontainer, #catbert {display: none;}
#header h1 {font-size: 13em !important; line-height: 1.25em; padding-top: .3em; margin-bottom: -.2em;}
ul#menu {background: transparent; clear: both; height: 500px; float: none; width: 780px;}
ul#menu li {font-size: 3.5em; display: inline-block; width: 360px; margin: 0px; line-height: .9em; padding: 30px 0;}
ul#menu li a {color: #565353; padding: 10px; text-shadow: 0px 2px 3px #fff;}
.catbert {position: fixed; right: 0px; bottom: 0px; margin-top: 10em; z-index: -1;}
#branding {width: 100%; position: absolute; top: 0px; left: 0px; z-index: 0; height: 82px;}
#branding a.ucdavis img {display: inline-block; margin: 5px -2px 0 6.5em; padding: 0px; width: 130px; height: 41px; position: relative; z-index: 3; cursor: pointer; border: 0px;}
#branding a.caes img {display: inline-block; padding: 0px; width: 220px; height: 41px; position: relative; z-index: 3; cursor: pointer; border: 0px;}
#branding a:hover {border-bottom: 0px;}
</style>

</asp:Content>
<asp:Content runat="server" ID="Main" ContentPlaceHolderID="MainContent">


        <div id="branding">
        <a href="http://ucdavis.edu" class="ucdavis"><img src="<%: Url.Image("logo-ucdavis.jpg") %>" alt="UC Davis" /></a>
        <a href="http://caes.ucdavis.edu" class="caes"><img src="<%: Url.Image("logo-caes.jpg") %>" alt="CA&ES" /></a>
        </div>

                <ul id="menu">              
                    <li><%= Html.ActionLink("1. Home", "Index", "Home")%></li>
                    <li><%= Html.ActionLink("6. Schools", "Index", "School")%></li>
                    <li><%= Html.ActionLink("2. Access Tokens", "Index", "AccessToken")%></li>
                    <li><%= Html.ActionLink("7. Users (Admin)", "Index", "UserAdministration")%></li>
                    <li><%= Html.ActionLink("3. Applications", "Index", "Application")%></li>
                    <li><%= Html.ActionLink("8. Units", "Index", "Unit")%></li>
                    <li><%= Html.ActionLink("4. Messages", "Index", "Message")%></li>
                    <li><%= Html.ActionLink("9. Manage App", "UserManagement", "Home")%></li>
                    <li><%= Html.ActionLink("5. Roles", "Index", "Role")%></li>
                </ul>

                <img class="catbert" src="<%: Url.Image("bg-catbert-med.jpg") %>" alt="Catbert" />

</asp:Content>
