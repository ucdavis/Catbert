<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<Catbert4.Controllers.UserManagementViewModel>" MasterPageFile="~/Views/Shared/Site.Master" %>
<asp:Content runat="server" ID="Title" ContentPlaceHolderID="TitleContent">Catbert: User Management</asp:Content>
<asp:Content runat="server" ID="Main" ContentPlaceHolderID="MainContent">

<div class="ui-widget" id="message" style="display: none;">
	<div class="ui-state-highlight ui-corner-all"> 
		<p><span style="float: left; margin-right: 0.3em;" class="ui-icon ui-icon-info"></span>
		<span id="message-text">User Added Successfully</span></p>
	</div>
</div>
<br />
<div>
	<a href="#" id="add-user">Add User</a>    
</div>
<br />
<div style="width: 100%">
	<div>
		<%= this.Select("roles").Class("user-filter").Attr("data-filter-column", 5).Options(Model.Roles, x=>x.Value, x=>x.Value).FirstOption("-- Filter By Role --") %>
		<%= this.Select("units").Class("user-filter").Attr("data-filter-column", 4).Options(Model.Units.OrderBy(x => x.Value), x => x.Value, x => x.Value).FirstOption("-- Filter By Unit --")%>
	</div>
	<table id="users" class="display">
		<thead>
			<tr>
				<th>
					Login
				</th>
				<th>
					FirstName
				</th>
				<th>
					LastName
				</th>
				<th>
					Email
				</th>
				<th>
					Departments
				</th>
				<th>
					Roles
				</th>
			</tr>
		</thead>
		<tbody>
			<% foreach (var item in Model.UserShowModel)
			   { %>
			<tr>
				<td>
					<a href="#" id="<%: item.Login %>" class="modify-user" title="Modify <%: item.FullNameAndLogin %>"><%: item.Login %></a>
				</td>
				<td>
					<%: item.FirstName %>
				</td>
				<td>
					<%: item.LastName %>
				</td>
				<td>
					<%: item.Email %>
				</td>
				<td>
					<%: string.Join(", ", item.UnitAssociations.OrderBy(x=>x.UnitName).Select(x=>x.UnitName.Trim())) %>
				</td>
				<td>
					<%: string.Join(", ", item.Permissions.OrderBy(x=>x.RoleName).Select(x=>x.RoleName.Trim())) %>
				</td>
			</tr>
			<% } %>
		</tbody>
	</table>
</div>

<% Html.RenderPartial("NewUserDialog"); %>

<% Html.RenderPartial("ModifyUserDialog"); %>

</asp:Content>
<asp:Content runat="server" ID="Header" ContentPlaceHolderID="HeaderContent">
	<% Html.RenderPartial("IncludeDataTables"); %>
	<%: Catbert4.Helpers.HtmlHelpers.IncludeJqueryTemplate() %>

	<style type="text/css">
		.dataTables_length {
			float: right;
			text-align: right;
			width: 40%;
		}
		.dataTables_filter {
			float: left;
			text-align: left;            
			width: 50%;
		}
	</style>
	<script type="text/javascript">
		var Catbert = { Services: { }, Indicators: { } };
		Catbert.Services.FindUser = "<%: Url.Action("FindUser") %>";
		Catbert.Services.InsertNewUser = "<%: Url.Action("InsertNewUser", new { application = Model.Application }) %>";
		Catbert.Services.LoadUser = "<%: Url.Action("LoadUser", new { application = Model.Application }) %>";
		Catbert.Services.RemoveUnit = "<%: Url.Action("RemoveUnit", new { application = Model.Application }) %>";
		Catbert.Services.RemovePermission = "<%: Url.Action("RemovePermission", new { application = Model.Application }) %>";
		Catbert.Services.AddUnit = "<%: Url.Action("AddUnit", new { application = Model.Application }) %>";
		Catbert.Services.AddPermission = "<%: Url.Action("AddPermission", new { application = Model.Application }) %>";

		$(function () {

			CreateButtons();

			AssignIndicators(); //Find and assign loading indicators

			AssignVerificationToken();

			$("#insert-new-user").validate();

			Catbert.UserTable = $("#users").dataTable({
				"bJQueryUI": true,
				"iDisplayLength": 25,
				"sPaginationType": "full_numbers",
				"aoColumns": [null, null, null, null, { "bSortable": false }, { "bSortable": false}], //Don't sort the last two cols
				"aaSorting": [[2, "asc"]]
			});

			$(".user-filter").change(function () {
				var element = $(this);
				Catbert.UserTable.fnFilter(element.val(), element.data("filter-column"));
			});

			$("#add-user").click(function (e) {
				e.preventDefault();

				$("#find-user").dialog({ modal: true, width: '50%' });
			});

			$("#search-user").click(function (e) {
				e.preventDefault();

				Catbert.Indicators.SearchProgress.show(0); //Show the loading dialog
				$("#search-results").hide(0); //Hide the content

				var data = { searchTerm: $("#search-login").val() };

				Log(data);
				Log(Catbert.Services.FindUser);

				$.getJSON(Catbert.Services.FindUser, data, SearchNewUserSuccess);
			});

			$("#search-login").keypress(function (e) {
				if (e.keyCode == 13) {
					$("#search-user").click();
				}
			});

			$("#add-new-user").click(function(e) {
				e.preventDefault();

				Catbert.Indicators.AddNewUserProgress.show(0);

				var form = $("#insert-new-user");

				if (!form.valid()) return;
				
				$.post(Catbert.Services.InsertNewUser, form.serialize(), function(result) { AddNewUserSuccess(result); }, 'json');
			});

			$(".modify-user").live("click", function(e){
				e.preventDefault();

				Catbert.Indicators.UserInfoProgress.show(0);

				//Load the user info
				var login = this.id;

				Log(login);

				$.getJSON(Catbert.Services.LoadUser,
					{ login: login },
					function (data) {
						PopulateUserInfo(data);
						Catbert.Indicators.UserInfoProgress.hide(0);
					}
				);

				$("#manage-user").dialog({ 
					modal: true, 
					width: '50%', 
					position: 'top',
					beforeClose: function(event, ui) {
						$("#user-info").remove();
					},
					buttons: {
						"Close": function() {
							$(this).dialog("close");
						}
					}
				});

				$(".remove-link").live("click", function(e){
					e.preventDefault();

                    RemoveAssociation($(this));
				});

				$(".add-link").live("click", function(e) {
					e.preventDefault();

                    AddAssociation($(this));
				});
			});
		});

		function CreateButtons() {
			$("#add-user").button({
				icons: {
					primary: "ui-icon-person"
				}
			});

			$("#search-user").button({
				icons: {
					primary: "ui-icon-search"
				}
			});

			$("#add-new-user").button({
				icons: {
					primary: "ui-icon-plus"
				}
			});
		}

		function AssignIndicators(){
			Catbert.Indicators.SearchProgress = $("#search-progress");
			Catbert.Indicators.AddNewUserProgress = $("#add-new-user-progress");
			Catbert.Indicators.UserInfoProgress = $("#user-info-progress");
		}

		function AssignVerificationToken() {
			Catbert.VerificationToken = $("input[name=__RequestVerificationToken]").val();
		}

		function SearchNewUserSuccess(data){
			Log(data);

			Catbert.Indicators.SearchProgress.hide(0);

			if (data == null){
				alert("No Users Found");
			}
			else {
				var searchResults = $("#search-results");

				$("#new-user-first-name-display", searchResults).html(data.FirstName);
				$("#new-user-last-name-display", searchResults).html(data.LastName);
				$("#new-user-login-display", searchResults).html(data.Login);
				$("#new-user-first-name", searchResults).val(data.FirstName);
				$("#new-user-last-name", searchResults).val(data.LastName);
				$("#new-user-login", searchResults).val(data.Login);
				$("#new-user-email", searchResults).val(data.Email);
				$("#new-user-phone", searchResults).val(data.Phone);

				searchResults.show(0);
			}
		}

		function AddNewUserSuccess(data){
			var userTable = Catbert.UserTable;

			//First find if the given login is already in the table
			var userRow = $("#" + data.Login).parent().parent();

			if (userRow.length != 0) { //if we are already in the table, remove the row first                
				var position = userTable.fnGetPosition(userRow[0]);

				userTable.fnDeleteRow(position);
			}

			var userLink = "<a href=\"#\" id=\""+ data.Login + "\" class=\"manage-user\" title=\""+ "Modify " + data.FullNameAndLogin +"\">"+ data.Login + "</a>";

			userTable.fnAddData([
				userLink,
				data.FirstName,
				data.LastName,
				data.Email,
				data.Units,
				data.Roles
			]);            

			Catbert.Indicators.AddNewUserProgress.hide(0);
			$("#search-results").hide(0);
			$("#find-user").dialog("close"); //Close the dialog            
			$("#message-text").html(data.FullNameAndLogin + " has been added successfully");
			$("#message").show("slow"); //Show the new user notification
		}

		function PopulateUserInfo(data){
			var userInfo = $("#user-info-template").tmpl(data);
			$("#manage-user").append(userInfo);

            StyleUserInfoButtons();
		}

        function RemoveAssociation(removeLink){
            var link = removeLink;

			var row = link.parent().parent();

			row.fadeOut("slow").remove();

			var data = { login: link.data("login"), id: link.data("id"), __RequestVerificationToken: Catbert.VerificationToken };
			Log(data);

			var url = link.data("type") == "permission" ? Catbert.Services.RemovePermission : Catbert.Services.RemoveUnit;

			$.post(url, data, null, null);
        }

        function AddAssociation(addLink){
            var link = addLink;

			var type = link.data("type");
				
			var url, list, table;

			if (type == "permission"){
				url = Catbert.Services.AddPermission;
				list = $("#userRoles");
				table = $("#user-info-roles");
			}
			else {
				url = Catbert.Services.AddUnit;
				list = $("#userUnits");
				table = $("#user-info-units");
			}

            var login = link.data("login");
            var id = list.val();

			var selectedText = list.find("option:selected").text();

			var existingColumnMatch = table.find("tbody tr td:contains(" + selectedText + ")").filter(
				function() {
					if ($(this).text() == selectedText)
						return true;
					else
						return false;
				}
			);

			if (existingColumnMatch.size() != 0) {
				alert("The user already has the " + type + " " + selectedText);
				return;
			}

            var rowValues = { value: selectedText, login: login, type: type, id: id };
            //Add a new row
            var newRow = $("#user-info-row-template").tmpl(rowValues);
            table.append(newRow);

            list.effect("transfer", { to: newRow }, "slow");
            newRow.effect("highlight", {}, 'slow');

            StyleUserInfoButtons();

            var data = { login: login, id: id, __RequestVerificationToken: Catbert.VerificationToken };
			Log(data);

			//$.post(url, data, null, null);
        }

        function StyleUserInfoButtons(){
        	//style the buttons
			$(".remove-link").button({
				icons: {
					primary: "ui-icon-trash"
				}
			});

			$(".add-link").button({
				icons: {
					primary: "ui-icon-plusthick"
				}
			});
        }

		function Log(text){
			if (typeof console != "undefined") {
				console.info(text);
			}
		}
	</script>
</asp:Content>
