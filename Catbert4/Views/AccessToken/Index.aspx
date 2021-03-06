<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.List<Catbert4.Core.Domain.AccessToken>>" MasterPageFile="~/Views/Shared/Site.Master" %>
<asp:Content runat="server" ID="Title" ContentPlaceHolderID="TitleContent">Access Tokens</asp:Content>
<asp:Content runat="server" ID="Header" ContentPlaceHolderID="HeaderContent"></asp:Content>
<asp:Content runat="server" ID="Main" ContentPlaceHolderID="MainContent">
    
    <h2>Access Tokens</h2>

    <p id="pageactions">
        <img class="back" src="<%: Url.Image("add.png") %>" alt="New token! TODO!" /> &nbsp;<%: Html.ActionLink<AccessTokenController>(x=>x.Create(), "Generate new token") %>
    </p>

<table id="tokens">
    <thead>
        <tr>
            <th></th>
            <th>Application</th>
            <th>Token</th>
            <th>Contact Email</th>
            <th>Reason</th>
            <th>Active</th>
        </tr>
    </thead>
    <tbody>
        <% foreach (var accessToken in Model)
           { %>
           <tr>
            <td><a href="<%: Url.Action("Details", new { id = accessToken.Id }) %>" class="img"><img src="<%: Url.Image("edit.png") %>" alt="Edit/Details" /></a> </td>
            <td><%: accessToken.Application.Name %></td>
            <td class="mono"><%: accessToken.Token %></td>
            <td><%: Html.DisplayFor(x=> accessToken.ContactEmail) %></td>
            <td><%: accessToken.Reason %></td>
            <td><%: Html.DisplayFor(x=> accessToken.Active) %></td>
           </tr>
        <% } %>
    </tbody>
</table>

</asp:Content>
