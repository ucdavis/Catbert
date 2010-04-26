<%@ Page Title="" Language="C#" MasterPageFile="~/Catbert.master" AutoEventWireup="true" CodeFile="SearchTest.aspx.cs" Inherits="SearchTest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <link href="CSS/jquery.autocomplete.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">

    <script src="JS/jquery.ajaxQueue.js" type="text/javascript"></script>
    <script src="JS/jquery.autocomplete.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function() {
            var emails = [
             { name: "Peter Pan", to: "peter@pan.de" },
             { name: "Molly", to: "molly@yahoo.com" },
             { name: "Forneria Marconi", to: "live@japan.jp" },
             { name: "Master <em>Sync</em>", to: "205bw@samsung.com" },
             { name: "Dr. <strong>Tech</strong> de Log", to: "g15@logitech.com" },
             { name: "Don Corleone", to: "don@vegas.com" },
             { name: "Mc Chick", to: "info@donalds.org" },
             { name: "Donnie Darko", to: "dd@timeshift.info" },
             { name: "Quake The Net", to: "webmaster@quakenet.org" },
             { name: "Dr. Write", to: "write@writable.com" }
            ];

            $("#suggest13").autocomplete(emails, {
                minChars: 0,
                width: 310,
                matchContains: true,
                autoFill: false,
                formatItem: function(row, i, max) {
                    return i + "/" + max + ": \"" + row.name + "\" [" + row.to + "]";
                },
                formatMatch: function(row, i, max) {
                    return row.name + " " + row.to;
                },
                formatResult: function(row) {
                    return row.to;
                }
            });


            $("#auto").autocomplete('Services/AutocompleteService.asmx/GetUnitsAuto', {
                width: 260,
                selectFirst: true,
                autoFill: false,
                extraParams: { app: "Catbert" },
                //highlight: function(value, q) { return "SS"; },
                formatItem: function(row, i, max) {
                    //debugger;
                    return row.Name + "<br/>" + row.FIS; //i + "/" + max + ": \"" + row.name + "\" [" + row.to + "]";
                }
            });


            /*
            $("#auto").result(function(event, data, formatted) {
            debugger;
            if (data)
            $(this).parent().next().find("input").val(data[1]);
            });*/
        });

    </script>  
Local: <input type="text" id="suggest13" autocomplete="off" class="ac_input"/>
<br />
Search: <input type="text" id="auto" /><input type="submit" value="Find" />
<br />
Results:
<div id="results">

</div>    

</asp:Content>

