<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage" MasterPageFile="~/Views/Shared/Site.Master" %>
<asp:Content runat="server" ID="Title" ContentPlaceHolderID="TitleContent">Sample User Management Page for <%: ViewData["Application"] %></asp:Content>
<asp:Content runat="server" ID="Header" ContentPlaceHolderID="HeaderContent">
<script type="text/javascript">
    $(function () {
        $("#frame").load(function () {
            $("#loading").hide();
        });
    });
    </script>
</asp:Content>
<asp:Content runat="server" ID="Main" ContentPlaceHolderID="MainContent">

<h2>Sample User Management Page for <%: ViewData["Application"] %></h2>

<div id="loading">
    Loading...
</div>

<div style="width: 100%; height: 800px;" align="center">
    <iframe id="frame"  frameborder="0" 
        src='<%= ViewData["ManagementUrl"] %>' 
        scrolling="auto" name="frame" style="width:100%; height:100%;">
    </iframe> 
</div>

</asp:Content>
