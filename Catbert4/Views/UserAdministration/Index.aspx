<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Users
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Users List</h2>

    <div>
        <a id="add-user" href="<%: Url.Action("Find") %>">Add User</a>
    </div>

    <div class="ui-widget">
	    <label for="user-search">Search Users: </label>
	    <input id="user-search" />
        
        <a id="clear-user-search" href="#">Clear</a>        
    </div>

    <h5><a id="load-users" href="#">View All Users</a></h5>
    <div id="users-list" style="width:60%"></div>

<script id="search-user-template" type="text/x-jquery-tmpl">
    <li data-item.autocomplete="${value}">
        <a>${FirstName} ${LastName} (${value})
            <br/>
            ${Email}
        </a>
    </li>
</script>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">

<% Html.RenderPartial("IncludeDataTables"); %>

<%: Catbert4.Helpers.HtmlHelpers.IncludeJqueryTemplate()%>

<script type="text/javascript">
    $(function () {
        $("#user-search").autocomplete({
            source: "<%: Url.Action("SearchUsers") %>",
            minLength: 2,
            select: function (event, ui) {
                this.value = ui.item;

                window.location = "<%: Url.Action("Edit") %>" + "/" + ui.item; 
                return false;
            }
        })
        .data( "autocomplete" )._renderItem = function( ul, item ) {
                    
            var template = $("#search-user-template").tmpl(item);
            
            return template.appendTo(ul);
		};

        $("#clear-user-search").button({ 
            icons: {
                    primary: "ui-icon-circle-close"
                },
            text: false
        })
        .click(function(e){
            e.preventDefault();
            
            $("#user-search").val("");
        });

        $("#load-users").click(function(e){
            e.preventDefault();

            var button = $(this);

            $("#users-list").load('<%: Url.Action("List") %>', null, function(){ 
                button.hide(); //hide button on success
                
                $("#users").dataTable({
                    "bJQueryUI": true,
                    "sPaginationType": "full_numbers",
                    "aaSorting": [[ 3, "asc" ]]
                });
            });
        });

        $("#add-user").button({
            icons: { primary: "ui-icon-person" }
        });
    });

	    // Helper function for the formatter 
	    var highlightMatch = function(full, snippet, matchindex) { 
	        return full.substring(0, matchindex) +  
	                "<span class='match'>" +  
	                full.substr(matchindex, snippet.length) +  
	                "</span>" + 
	                full.substring(matchindex + snippet.length); 
	    }
</script>
    
</asp:Content>