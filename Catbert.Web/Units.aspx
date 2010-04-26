<%@ Page Title="" Language="C#" MasterPageFile="~/CatbertMaster.master" AutoEventWireup="true" CodeFile="Units.aspx.cs" Inherits="Units" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">

    <script type="text/javascript">
        $(document).ready(function() {
            $("#tabs").tabs();

            $("#addUnit").click(function() {
                $("#divNewUnit").show('slow');
            });
        });
	</script>

<div id="tabs">
	<ul>
	    <li><a href="#tabUnits">Units</a></li>
		<li><a href="#tabSchools">Schools</a></li>		
	</ul>
	<div id="tabUnits">
        <asp:Panel ID="pnlUnitAdded" runat="server" Visible="false">
            <br />
            <br />
            <span style="color: Red">
                <asp:Literal ID="litUnitMessage" runat="server" EnableViewState="false" Mode="Encode"></asp:Literal>
            </span>
            <br />
            <br />
        </asp:Panel>
	    <br />
	    <a href="javascript:;" id="addUnit" class="dialog_link ui-state-default ui-corner-all">
            <span class="ui-icon ui-icon-plus"></span>Add New Unit
        </a>
        <br />
        <div id="divNewUnit" style="display:none;">
            <br />
            <table>
                <tbody>
                    <tr>
                        <td>
                            FIS Code
                        </td>
                        <td>
                            <asp:TextBox ID="txtFIS" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator id="rvalFIS" ControlToValidate="txtFIS" ErrorMessage="*FIS Code Required" runat="server"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            PPS Code
                        </td>
                        <td><asp:TextBox ID="txtPPS" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>
                            Full Name
                        </td>
                        <td>
                            <asp:TextBox ID="txtFullName" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator id="rvalFullName" ControlToValidate="txtFullName" ErrorMessage="*Full Name Required" runat="server"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Short Name
                        </td>
                        <td>
                            <asp:TextBox ID="txtShortName" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator id="rvalShortName" ControlToValidate="txtShortName" ErrorMessage="*Short Name Required" runat="server"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            School
                        </td>
                        <td>
                            <asp:DropDownList ID="dlistSchool" runat="server" DataSourceID="odsSchools" DataTextField="ShortDescription" DataValueField="ID"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr><td colspan="2">&nbsp;</td></tr>
                    <tr>
                        <td></td>
                        <td><asp:Button ID="btnAddUnit" runat="server" Text="Submit" 
                                CausesValidation="true" onclick="btnAddUnit_Click" /></td>
                    </tr>
                </tbody>
            </table>
        </div>
        <br />
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
                <asp:Parameter DefaultValue="ShortDescription" Name="propertyName" 
                    Type="String" />
                <asp:Parameter DefaultValue="true" Name="ascending" Type="Boolean" />
            </SelectParameters>
        </asp:ObjectDataSource>
	</div>
</div>

</asp:Content>

