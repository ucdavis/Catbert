<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.List<Catbert4.Core.Domain.Unit>>" MasterPageFile="~/Views/Shared/Site.Master" %>
<asp:Content runat="server" ID="Title" ContentPlaceHolderID="TitleContent"></asp:Content>
<asp:Content runat="server" ID="Header" ContentPlaceHolderID="HeaderContent"></asp:Content>
<asp:Content runat="server" ID="Main" ContentPlaceHolderID="MainContent">

		
<table id="tblUnits">
    <thead>
        <tr>
            <th>FIS Code</th>
            <th>PPS Code</th>
            <th>Full Name</th>
            <th>Short Name</th>
            <th>School</th>
        </tr>
    </thead>
    <tbody>
        <% foreach (var unit in Model)
           { %>
           <tr>
            <td><%: unit.FisCode %></td>
            <td><%: unit.PpsCode %></td>
            <td><%: unit.FullName %></td>
            <td><%: unit.ShortName %></td>
            <td><%: unit.School.ShortDescription %></td>
           </tr>
        <% } %>
    </tbody>
</table>

</asp:Content>
