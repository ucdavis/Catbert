<%@ Page Language="C#" AutoEventWireup="true" CodeFile="IFrameTest.aspx.cs" Inherits="IFrameTest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
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
    </style>
</head>
<body>
    <form id="form1" runat="server">
    
        <iframe id="frame" frameborder="0" src="Management/UserManagement.aspx" scrolling="auto" name="frame" style="width:100%; height:100%;"></iframe>
    
    </form>
</body>
</html>
