<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<Catbert4.Services.Wcf.ServiceMessage[]>" MasterPageFile="~/Views/Shared/Site.Master" %>
<asp:Content runat="server" ID="Title" ContentPlaceHolderID="TitleContent">Messages</asp:Content>
<asp:Content runat="server" ID="Main" ContentPlaceHolderID="MainContent">

    <h2>Current Global Messages (server-side via service)</h2>

    <div>
        <%: Model.Length == 0 ? "There are no messages" : string.Format("{0} message(s)", Model.Length) %>
    </div>
    
    <ul>
    <% foreach(var m in Model) { %>
        <li><%: m.Message %></li>
    <% } %>
    </ul>

    <h2>Client Side Proxy</h2>

    <button id="get-messages">Get Messages</button>

    <div id="result"></div>

</asp:Content>
<asp:Content runat="server" ID="Header" ContentPlaceHolderID="HeaderContent">
    <script type="text/javascript">
        $(function () {
            $("#get-messages").click(function (e) {
                e.preventDefault();

                var urlBase = '<%: Url.Content("~/Public/Message.svc") %>';

                $.get(urlBase + '/json/GetMessage',
                    { appName: "AD419" },
                    function (result) { console.log(result.d); },
                    'json'
                );
            });
        });    
    </script>    
</asp:Content>