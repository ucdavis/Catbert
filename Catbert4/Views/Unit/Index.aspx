<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.List<Catbert4.Core.Domain.Unit>>" MasterPageFile="~/Views/Shared/Site.Master" %>

<asp:Content runat="server" ID="Title" ContentPlaceHolderID="TitleContent">Units</asp:Content>
<asp:Content runat="server" ID="Header" ContentPlaceHolderID="HeaderContent"></asp:Content>
<asp:Content runat="server" ID="Main" ContentPlaceHolderID="MainContent">

    <h2>Units</h2>

<table id="tblUnits">
    <thead>
        <tr>
            <th></th>
            <th>FIS Code</th>
            <th>PPS Code</th>
            <th>Full Name</th>
            <th>Short Name</th>
            <th>Type</th>
            <th>Parent</th>
            <th>School</th>
        </tr>
    </thead>
    <tbody>
        <% foreach (var unit in Model)
           { %>
           <tr>
            <td>
                <%--<%: Html.ActionLink<UnitController>(x=>x.Edit(unit.Id), "Edit") %> |
                <%: Html.ActionLink<UnitController>(x=>x.Delete(unit.Id), "Delete") %>--%>
                <a href="TODO!" class="img"><img src="<%: Url.Image("edit.png") %>" alt="Edit" /></a> | <a href="TODO!" class="img"><img src="<%: Url.Image("delete.png") %>" alt="Delete" /></a>
            </td>
            <td><%: unit.FisCode %></td>
            <td><%: unit.PpsCode %></td>
            <td><%: unit.FullName %></td>
            <td><%: unit.ShortName %></td>
            <td><%: unit.Type %></td>
            <td><%: unit.Parent == null ? "None" : unit.Parent.ShortName %></td>
            <td><%: unit.School.ShortDescription %></td>
           </tr>
        <% } %>
    </tbody>
</table>

    <p>
    <img class="back" src="<%: Url.Image("add.png") %>" alt="Add TODO!" />&nbsp;<%: Html.ActionLink<UnitController>(x=>x.Create(), "Create New Unit") %>
    </p>


</asp:Content>
