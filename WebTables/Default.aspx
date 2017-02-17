<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/ProcTable.aspx">ProcTable</asp:HyperLink>
    <table>
        <tr>
            <td>Schema</td>
            <td>Table</td>
            <td></td>
        </tr>
        <tr>
            <td>
                <asp:TextBox ID="Schema" runat="server"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="Table" runat="server"></asp:TextBox>
            </td>
            <td>
                <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />
            </td>
        </tr>
    </table>
    </div>
    </form>
</body>
</html>
