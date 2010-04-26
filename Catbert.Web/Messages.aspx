<%@ Page Title="" Language="C#" MasterPageFile="~/CatbertMaster.master" AutoEventWireup="true" CodeFile="Messages.aspx.cs" Inherits="Messages" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

  <script type="text/javascript" src="http://jqueryui.com/latest/ui/ui.tabs.js"></script>
  <script type="text/javascript" src="<%= Request.ApplicationPath + @"/JS/jquery.tablesorter.min.js" %>"></script>

    <script type="text/javascript">

        $(document).ready(function() {
            $("#tabs").tabs();
            //$("odsMessages").tablesorter(); 
            
            //Setup the sorting for the table with the first column initially sorted ascending
            //and the rows striped using the zebra widget
            $("#ListView1").tablesorter({ sortList: [[0, 0]], widgets: ['zebra'] });


            //console.log("HI");
        });
  
  </script>
    
    
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
  <a href="CreateMessage.aspx">[Add New Message]</a><br /><br />
   <div id="tabs">
    <ul>
        <li><a href="#tab-1"><span>All Messages</span></a></li>
        <%--<li><a href="#tab-2"><span>Expired Messages</span></a></li>--%>
    </ul>
    
<%--All Active Messages-------------------------------------------------------%>   
    <div id="tab-1">
    
    <asp:ObjectDataSource ID="odsMessages" runat="server" 
        OldValuesParameterFormatString="original_{0}" SelectMethod="GetActive" 
        TypeName="CAESDO.Catbert.BLL.MessageBLL">
    </asp:ObjectDataSource>    

  <asp:ListView ID="ListView1" runat="server" DataSourceID="odsMessages">
        <EmptyDataTemplate>
            No data was returned.
        </EmptyDataTemplate>
        <ItemTemplate>
            <tr id ="<%# Eval("ID") %>" style="">
                <td><a href='<%# "MessageEdit.aspx?mid=" + Eval("id") %>'><%# Eval("MessageText") %></a></td>
                <td><%# Eval("Application.Name") %></td>
                <td><%#Eval("BeginDisplayDateString")%></td>
                <td><%#Eval("EndDisplayDateString") %></td>
                 
                <td><asp:Button ID="btnDeactivate" OnCommand="BtnDeactivateClick" CommandArgument='<%# Eval("ID") %>' runat="server" Text="Deactivate" BackColor="Silver" BorderColor="Gray" /></td>
            </tr>
       </ItemTemplate>
   
      <LayoutTemplate>
         <table width="100%" id="odsMessages">
                <thead>
                    <tr>
                        <th>Message</th>
                        <th>Application</th>
                        <th>Begin Display</th>
                        <th>End Display</th>
                        <th>Deactivate</th>
                    </tr>
                </thead>
                <tbody>
                    <tr runat='server' id="ItemPlaceHolder"></tr>
                    
                    
                </tbody>
                <tfoot>
                    
                    <tr>
                    </tr>
                    <tr>
                    </tr>
                </tfoot>
            </table>
        </LayoutTemplate>
        </asp:ListView>
  
 </div>
<%----Expired Messages------------------------------------------------------------%>
 <%-- <div id="tab-2">
   <asp:ObjectDataSource ID="odsExpiredMessages" runat="server" 
        OldValuesParameterFormatString="original_{0}" SelectMethod="GetExpired" 
        TypeName="CAESDO.Catbert.BLL.MessageBLL">
    </asp:ObjectDataSource>    

  <asp:ListView ID="ListView2" runat="server" DataSourceID="odsExpiredMessages">
        <EmptyDataTemplate>
            No data was returned.
        </EmptyDataTemplate>
        <ItemTemplate>
            <tr id ="<%# Eval("ID") %>" style="">
                <td><%# Eval("MessageText")%> </td>
                <td><%# Eval("Application.Name") %></td>
                <td><%#Eval("BeginDisplayDateString")%></td>
                <td><%#Eval("EndDisplayDateString") %></td>
            </tr>
      </ItemTemplate>

  
      <LayoutTemplate>
         <table width="100%" id="odsMessages">
                <thead>
                    <tr>
                        <th>Message</th>
                        <th>Application ID</th>
                        <td>Begin Display</td>
                        <td>End Display</td>
                    </tr>
                </thead>
                <tbody>
                    <tr runat='server' id="ItemPlaceHolder"></tr>
                </tbody>
                <tfoot>
                    
                    <tr>
                    </tr>
                    <tr>
                    </tr>
                </tfoot>
            </table>
        </LayoutTemplate>
        </asp:ListView>
  </div>--%>

<%--------------------------------------------------------------%>

</div>
</asp:Content>

