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
	<div id="tabUnits">
	    <br />
		<asp:ListView ID="lviewUnits" runat="server" DataSourceID="odsUnits">
            <LayoutTemplate>
                <table id="tblUnits">
                    <thead>
                        <tr>
                            <th>FIS Code</th>
                            <th>PPS Code</th>
                            <th>Full Name</th>
                            <th>Short Name</th>
                            <th>School</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr id="itemPlaceholder" runat="server"></tr>
                    </tbody>
                </table>
            </LayoutTemplate>
            <ItemTemplate>
                <tr>
                    <td><%# Eval("FISCode") %></td>
                    <td><%# Eval("PPSCode") %></td>
                    <td><%# Eval("FullName")%></td>
                    <td><%# Eval("ShortName")%></td>
                    <td><%# Eval("School.ShortDescription")%></td>
                </tr>
            </ItemTemplate>
        </asp:ListView>
        <asp:ObjectDataSource ID="odsUnits" runat="server" 
            OldValuesParameterFormatString="original_{0}" SelectMethod="GetAll" 
            TypeName="CAESDO.Catbert.BLL.UnitBLL">
            <SelectParameters>
                <asp:Parameter DefaultValue="FullName" Name="propertyName" 
                    Type="String" />
                <asp:Parameter DefaultValue="true" Name="ascending" Type="Boolean" />
            </SelectParameters>
        </asp:ObjectDataSource>
	</div>
	<div id="tabSchools">
	    <br />
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
</div>

</asp:Content>

