<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<% var message = TempData["Message"] as string; %>
<% if (!string.IsNullOrWhiteSpace(message))
   { %>
<div class="ui-widget">
	<div class="ui-state-highlight ui-corner-all"> 
		<p><span style="float: left; margin-right: 0.3em;" class="ui-icon ui-icon-info"></span>
		<%: message%></p>
	</div>
</div>
<% } %>