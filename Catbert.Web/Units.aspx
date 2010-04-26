<%@ Page Title="" Language="C#" MasterPageFile="~/Catbert.master" AutoEventWireup="true" CodeFile="Units.aspx.cs" Inherits="Units" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">

    <script type="text/javascript">
        $(document).ready(function() {
            $("#tabs").tabs();
        });
	</script>

<div id="tabs">
	<ul>
	    <li><a href="#tabUnits">Units</a></li>
		<li><a href="#tabSchools">Schools</a></li>		
	</ul>
	<div id="tabSchools">
        <asp:ListView ID="lviewSchools" runat="server" DataSourceID="odsSchools">
            <LayoutTemplate>
                <table id="tblSchools">
                    <thead>
                        <tr>
                            <th>SchoolCode</th>
                            <th>Abbreviation</th>
                            <th>Short Description</th>
                            <th>Long Description</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr id="itemPlaceholder" runat="server"></tr>
                    </tbody>
                </table>
            </LayoutTemplate>
            <ItemTemplate>
                <tr>
                    <td><%# Eval("ID") %></td>
                    <td><%# Eval("Abbreviation") %></td>
                    <td><%# Eval("ShortDescription")%></td>
                    <td><%# Eval("LongDescription")%></td>
                </tr>
            </ItemTemplate>
        </asp:ListView>
        <asp:ObjectDataSource ID="odsSchools" runat="server" 
            OldValuesParameterFormatString="original_{0}" SelectMethod="GetAll" 
            TypeName="CAESDO.Catbert.BLL.SchoolBLL">
            <SelectParameters>
                <asp:Parameter DefaultValue="ID" Name="propertyName" 
                    Type="String" />
                <asp:Parameter DefaultValue="true" Name="ascending" Type="Boolean" />
            </SelectParameters>
        </asp:ObjectDataSource>
	</div>
	<div id="tabUnits">
		<p>Morbi tincidunt, dui sit amet facilisis feugiat, odio metus gravida ante, ut pharetra massa metus id nunc. Duis scelerisque molestie turpis. Sed fringilla, massa eget luctus malesuada, metus eros molestie lectus, ut tempus eros massa ut dolor. Aenean aliquet fringilla sem. Suspendisse sed ligula in ligula suscipit aliquam. Praesent in eros vestibulum mi adipiscing adipiscing. Morbi facilisis. Curabitur ornare consequat nunc. Aenean vel metus. Ut posuere viverra nulla. Aliquam erat volutpat. Pellentesque convallis. Maecenas feugiat, tellus pellentesque pretium posuere, felis lorem euismod felis, eu ornare leo nisi vel felis. Mauris consectetur tortor et purus.</p>
	</div>
</div>



</asp:Content>

