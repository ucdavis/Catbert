<%@ Page Title="" Language="C#" MasterPageFile="~/Catbert.master" AutoEventWireup="true" CodeFile="UserManagement.aspx.cs" Inherits="Management_UserManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
    <script type="text/javascript">
        var baseURL = '../Services/CatbertWebService.asmx/';
        
        $(document).ready(function() {
            var application = $("#app").val();

            PopulateUserTable(application); //Populate the user table
        });

        function PopulateUserTable(application) {
            //Setup the parameters
            var data = { application: application };

            //Call the webservice
            AjaxCall('jqGetUsersByApplication', data, PopulateUserTableSuccess, null);
        }

        function PopulateUserTableSuccess(data) {
            data = data.d; //Grab the inner container of data

            //Render out each row
            $(data.rows).each(RenderRow);

        }

        function RenderRow(index, row) {
            var newrow = $('<tr></tr>');

            newrow.append('<td class="Name">' + row.FirstName + " " + row.LastName + '</td>');
            newrow.append('<td class="Login">' + row.Login + '</td>');
            newrow.append('<td class="Email">' + row.Email + '</td>');

            var units = CreateDomFromUserInfoArray(row.Units);
            var roles = CreateDomFromUserInfoArray(row.Roles);

            newrow.append('<td class="Units">' + units + '</td>');
            newrow.append('<td class="Roles">' + roles + '</td>');

            $("#tblUsers tbody").append(newrow);
            //debugger;
        }

        function CreateDomFromUserInfoArray(array) {
            var dom = "";

            $(array).each(function(index, obj) {
                if (index != 0) //For everything other than the first element, include the comma
                    dom += ", ";

                dom += Trim(obj.Name);
            });

            return dom;
        }

        function Trim(str) {
            return str.replace(/^\s\s*/, '').replace(/\s\s*$/, '');
        }

        function AjaxCall(method, data, onSuccess, onError) {
            //Serialize the data to json format
            data = JSON2.stringify(data);
            //debugger;
            $.ajax({
                type: 'POST',
                url: baseURL + method,
                data: data,
                dataType: 'json', //JSON
                contentType: 'application/json',
                processData: true,
                success: onSuccess,
                error: onError
            });
        }
    </script>
    
<br /><br />

    <table id="tblUsers">
        <tbody>
            <%--Each row is a new person--%>
        </tbody>
    </table>
    
    
</asp:Content>

