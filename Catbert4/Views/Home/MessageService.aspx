<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<Catbert4.Services.Wcf.ServiceMessage[]>" MasterPageFile="~/Views/Shared/Site.Master" %>
<asp:Content runat="server" ID="Title" ContentPlaceHolderID="TitleContent">Messages</asp:Content>
<asp:Content runat="server" ID="Main" ContentPlaceHolderID="MainContent">

    <h3>Using the following service <%: ViewData["serviceUrl"] %>.  Change it by passing a baseUrl parameter.</h3>
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

    <div id="result">
        <ul id="messages">
        
        </ul>
    </div>

</asp:Content>
<asp:Content runat="server" ID="Header" ContentPlaceHolderID="HeaderContent">
    <script type="text/javascript">
        $(function () {
            $("#get-messages").click(function (e) {
                e.preventDefault();

                var urlBase = '<%: ViewData["serviceUrl"] %>';

                $.get(urlBase + '/json/GetMessages',
                    null,
                    function (result) { ShowMessages(result.d); },
                    'json'
                );
            });
        });

        function ShowMessages(messages) {
            var messageList = $("#messages");
            messageList.html(null); //clear out any old messages

            for (var i = 0; i < messages.length; i++) {
                var newMessage = $("<li>").html(messages[i].Message);

                messageList.append(newMessage);
            }
        }
    </script>    
</asp:Content>