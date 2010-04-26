<%@ Page Title="" Language="C#" MasterPageFile="~/Catbert.master" AutoEventWireup="true" CodeFile="WebServiceExamples.aspx.cs" Inherits="WebServiceExamples" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">

    <script type="text/javascript">
        var loginVal, applicationVal, reqData;
        var reqType = 'POST';
        var reqURL = 'Services/CatbertWebService.asmx/GetUsersByApplication';
        var baseURL = 'Services/CatbertWebService.asmx/';
        var reqDataType = 'xml';
        
        $(document).ready(function() {
            //Attach handlers
            $("#btnRunXML").click(runXML);
            $("#btnRunJSON").click(runJSON);
            $("#btnRunJSONASMX").click(runJSONASMX);
            $("#btnCleanJSON").click(runCleanJSON);
        });

        function runXML() {
            getPostVariables();
            //reqType = 'GET';
            reqDataType = 'xml';

            $.ajax({
                type: reqType,
                url: reqURL,
                data: reqData,
                dataType: reqDataType,
                processData: true,
                success: function(data) {
                    showResults(data);
                }
            });
        }

        function runJSON() {
            getPostVariables();
            reqDataType = 'json';

            //Serialize the data to json format
            //reqData = JSON2.stringify(reqData);

            $.ajax({
                type: reqType,
                url: reqURL,
                data: reqData,
                dataType: reqDataType, //JSON
                processData: true,
                success: function(data) {
                    showResults(data);
                },
                error: function(data, textStatus, errorThrown) {
                    alert(textStatus);
                    showResults(data);
                    //showResults(data.responseXML);
                }
            });
        }

        function runJSONASMX() {
            getPostVariables();
            reqDataType = 'json';

            //Serialize the data to json format
            reqData = JSON2.stringify(reqData);

            $.ajax({
                type: reqType,
                url: reqURL,
                data: reqData,
                dataType: reqDataType, //JSON
                contentType: 'application/json',
                processData: true,
                success: function(data) {
                    showResults(data);
                },
                error: function(data, textStatus, errorThrown) {
                    alert(textStatus);
                    showResults(data);
                }
            });
        }

        function runCleanJSON() {
            var data = { login: 'postit', application: 'Catbert' };
            
            AjaxCall('GetUsersByApplication', data, onRunCleanJSONSuccess, null);
        }

        function onRunCleanJSONSuccess(data) {
            showResults(data);
        }

        function getPostVariables() {
            loginVal = $("#login").val();
            applicationVal = $("#application").val();

            reqData = { login: loginVal, application: applicationVal };
            
            //debugger;
        }

        function showResults(data) {
            var string;
            //debugger;
            if (data.contentType == 'text/xml')
                string = (new XMLSerializer()).serializeToString(data);
            else
                string = JSON2.stringify(data);

            $("#spanResult").html(string);
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

<br />
Login: <input id="login" value="postit" /><br />
Application: <input id="application" value="Catbert" /><br /><br />

<input id="btnRunXML" type="button" value="XML Request" />
<input id="btnRunJSON" type="button" value="Regular JSON Request" />
<input id="btnRunJSONASMX" type="button" value="ASMX JSON Request" />
<input id="btnCleanJSON" type="button" value="Clean ASMX JSON" />

<br /><br />
<span id="spanResult"></span>

</asp:Content>

