<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AnyTable.aspx.cs" Inherits="AnyTable" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <style>
        div{ 
            margin:auto; 
            text-align: center;
        } 

        table {
            margin: auto;
        }
    </style>

    <form id="form1" runat="server">
        <div>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="TitleLabel" runat="server" Text="Label" Font-Bold="True" Font-Size="X-Large" Font-Underline="True"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="gv" runat="server" DataSourceID="sds" AutoGenerateColumns="False">
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Table ID="InsertTable" runat="server">
                            <asp:TableRow ID="HeaderRow" runat="server">
                            </asp:TableRow>
                            <asp:TableRow ID="TextBoxRow" runat="server">
                            </asp:TableRow>
                            <asp:TableRow ID="InsertRow" runat="server">
                                <asp:TableCell ID="InsertCell" runat="server">
                                    <asp:Button ID="InsertButton" runat="server" Text="Insert" OnClick="InsertButton_Click" />
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </td>
                </tr>
            </table>
        </div>
        <asp:SqlDataSource ID="sds" runat="server"></asp:SqlDataSource>
        
        <asp:TextBox ID="ErrorBox" runat="server" Height="69px" Visible="False" Width="325px"></asp:TextBox>
        
    </form>
</body>
</html>
