<%@ Page Title="" Language="C#" MasterPageFile="~/Catbert.master" AutoEventWireup="true" CodeFile="GridTest.aspx.cs" Inherits="GridTest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="CSS/grid.css" rel="stylesheet" type="text/css" />
    <link href="CSS/jqModal.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
    <script src="JS/jquery.jqGrid.js" type="text/javascript"></script>
    
    <script type="text/javascript">
        $(document).ready(function() {
            $("#tblUsers").jqGrid({
                url: 'Services/CatbertWebService.asmx/jqGetUsersByApplication',
                datatype: 'json',
                mtype: 'POST',
                colNames: ['Login', 'First Name', 'Last Name', 'Email'],
                colModel: [
                    { name: 'Login', index: 'Login' },
                    { name: 'FirstName', index: 'FirstName' },
                    { name: 'LastName', index: 'LastName' },
                    { name: 'Email', index: 'Email' }
                    ],
                postData: { login: "postit", application: "Catbert" },
                pager: jQuery('#pjmap'),
                rowNum: 10,
                rowList: [10, 20, 30],
                jsonReader: {
                    //records: 'rows',
                    repeatitems: false,
                    id: 'UserID'
                },
                imgpath: 'CSS/images',
                caption: "JSON Mapping"
            }); //.navGrid('#pjmap', { edit: true, search: true, add: true, del: false }).navButtonAdd('#pjmap', { caption: "Something", position: "last" });

            $("#btn").click(function() {
                var tbl = $("#tblUsers");
                debugger;
                //jQuery("#tblUsers").trigger("reloadGrid");
            });
        });
    </script>    
    
    <table id="tblUsers" class="scroll"></table>
    <div id="pjmap" class="scroll" style="text-align:center;"></div> 
    
    <br /><br />
    <input type="button" id="btn" value="click" />
</asp:Content>

