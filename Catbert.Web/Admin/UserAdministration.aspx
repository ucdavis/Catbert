<%@ Page Title="" Language="C#" MasterPageFile="~/CatbertMaster.master" AutoEventWireup="true" CodeFile="UserAdministration.aspx.cs" Inherits="UserAdministration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        html
        {
            height: 100%;
        }
        body
        {
            height: 100%;
            overflow: hidden;
        }
        form 
        {
            height:100%;
        }
        #bodyDiv
        {
            height:100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
    
    <asp:DropDownList ID="dlistApplications" runat="server" AutoPostBack="true" AppendDataBoundItems="true" DataSourceID="odsApplications" DataTextField="Name" DataValueField="Name">
        <asp:ListItem Text="Select An Application" Value=""></asp:ListItem>
    </asp:DropDownList>
    
    <asp:ObjectDataSource ID="odsApplications" runat="server" 
        OldValuesParameterFormatString="original_{0}" SelectMethod="GetAll" 
        TypeName="CAESDO.Catbert.BLL.ApplicationBLL">
        <SelectParameters>
            <asp:Parameter DefaultValue="Name" Name="propertyName" 
                Type="String" />
            <asp:Parameter DefaultValue="true" Name="ascending" Type="Boolean" />
        </SelectParameters>
    </asp:ObjectDataSource>

    <asp:Panel ID="pnlShowUserManagement" runat="server" Visible="false" Width="100%" Height="100%">
        <iframe id="frame" runat="server" frameborder="0" src="../Management/UserManagement.aspx" scrolling="auto" name="frame" style="width:100%; height:100%;"></iframe>
    </asp:Panel>
</asp:Content>

