<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HistoryImport.aspx.cs" Inherits="Presentation.Web.Import.HistoryImport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <link href="../Content/bdo/css/bdo.css" rel="stylesheet" />

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h1>Partner Earnings History Import</h1>
        <p>The user can import "Earnings", "Monthly Draw" and "Fixed Income" to the database. Please select from below to continue:</p>

                        <h2>Import Earnings Data</h2><br />
                        <asp:Button ID="btnEarnings" Text="Select Excel File" OnClick="btnEarnings_Click" runat="server" />

                         <h2>Import Monthly Draw Data</h2><br />
                            <asp:Button ID="btnMonthlyDraw" Text="Select Excel File" OnClick="btnMonthlyDraw_Click" runat="server" />
        
                        <h2>Import Fixed Income Data</h2><br />
                            <asp:Button ID="btnFixedIncome" Text="Select Excel File" OnClick="btnFixedIncome_Click" runat="server" />
    </div>
    </form>
</body>
</html>
