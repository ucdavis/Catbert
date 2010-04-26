<%@ Page Title="" Language="C#" MasterPageFile="~/CatbertMaster.master" AutoEventWireup="true" CodeFile="CreateMessage.aspx.cs" Inherits="CreateMessage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <script type="text/javascript">

      var tbMessageID = '<%= tbMessage.ClientID %>';
      var tbBeginDisplayDateID = '<%= tbBeginDisplayDate.ClientID %>';
      var tbEndDisplayDateID = '<%= tbEndDisplayDate.ClientID %>';
      var ddlApplicationsID = '<%= ddlApplications.ClientID %>';

      $(function() {
           $("#" + tbBeginDisplayDateID).datepicker();
           $("#" + tbEndDisplayDateID).datepicker();
      });

      function SubmitProject() {
          var newMessage = new Message();

       }
            
  </script>
    <style type="text/css">
        .style1
        {
            width: 101px;
        }
        .style2
        {
            width: 251px;
        }
    </style>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
    <table>
    <tbody>
        <tr>
            <td class="style1">Message:</td>
            <td class="style2">
                <asp:TextBox ID="tbMessage" runat="server" Height="44px" Width="277px" ></asp:TextBox>
                <asp:RequiredFieldValidator ID="valMessage" ControlToValidate="tbMessage" runat="server" ErrorMessage="Required Field"></asp:RequiredFieldValidator>
            </td>
            <td></td>
            <td></td>
       
        </tr>
            <tr>
            <td class="style1">BeginDisplayDate:</td>
            <td class="style2">
                <asp:TextBox ID="tbBeginDisplayDate" runat="server" ></asp:TextBox>
                <asp:RequiredFieldValidator ID="valBeginDisplayDate" ControlToValidate="tbBeginDisplayDate" runat="server" ErrorMessage="Required Field"></asp:RequiredFieldValidator>
            </td>
            
            
            <td>EndDisplayDate:</td>
            <td>
                <asp:TextBox ID="tbEndDisplayDate" runat="server" ></asp:TextBox>
                <asp:CompareValidator ID="valEndDisplyDate" runat="server" ErrorMessage="End Date must be greater than Start Date" ControlToCompare="tbBeginDisplayDate" ControlToValidate="tbEndDisplayDate" Operator="GreaterThan" ></asp:CompareValidator>
            
            </td>
            </tr>
        <tr>
            <td class="style1"></td>
            <td class="style2"></td>
        </tr>

        
        <tr>
           <td> Application:</td>
           <td class="style2">  
               <asp:DropDownList ID="ddlApplications" runat="server" AppendDataBoundItems="true"
                    DataSourceID="odsApplications" DataTextField="Name" DataValueField="ID" 
                   Height="25px" Width="274px">
                    <asp:ListItem Text="APPLY TO ALL APPLICATIONS" Value="-1"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
           
      <td class="style1"><asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" /></td>
       

    </tbody>
</table>
<%--------------------------------------------------------------%>
        
    <asp:ObjectDataSource ID="odsApplications" runat="server" 
        OldValuesParameterFormatString="original_{0}" SelectMethod="GetAll" 
        TypeName="CAESDO.Catbert.BLL.ApplicationBLL">
        <SelectParameters>
            <asp:Parameter DefaultValue="Name" Name="propertyName" Type="String" />
            <asp:Parameter DefaultValue="true" Name="ascending" Type="Boolean" />
        </SelectParameters>
    </asp:ObjectDataSource>    
        
</asp:Content>

