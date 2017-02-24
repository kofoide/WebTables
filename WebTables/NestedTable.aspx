<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NestedTable.aspx.cs" Inherits="NestedTable" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Nested Grids</title>
    <link href="css/GridView.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript">
        $("[src*=plus]").live("click", function () {
            $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
            $(this).attr("src", "images/minus.png");
        });
        $("[src*=minus]").live("click", function () {
            $(this).attr("src", "images/plus.png");
            $(this).closest("tr").next().remove();
        });
    </script>

    <form id="form1" runat="server">
    <div>

        <asp:GridView ID="gvCustomers" runat="server" AutoGenerateColumns="false" CssClass="Grid" 
            DataKeyNames="CustomerID" OnRowDataBound="OnRowDataBound">
            <AlternatingRowStyle BackColor="#CCCCCC" />
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <img alt = "" style="cursor: pointer" src="images/plus.png" />
                        <asp:Panel ID="pnlOrders" runat="server" Style="display: none">
                            <asp:GridView ID="gvOrders" runat="server" AutoGenerateColumns="false" CssClass = "ChildGrid" 
                                HeaderStyle-BackColor="#D6DBE9" AlternatingRowStyle-BackColor="#FDF6E3">
                                <Columns>
                                    <asp:BoundField ItemStyle-Width="150px" DataField="OrderId" HeaderText="Order Id" />
                                    <asp:BoundField ItemStyle-Width="150px" DataField="OrderDate" HeaderText="Date" />
                                    <asp:BoundField ItemStyle-Width="150px" DataField="Location" HeaderText="Location" />
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:BoundField ItemStyle-Width="150px" DataField="ContactName" HeaderText="Contact Name" >
                    <ItemStyle Width="150px"></ItemStyle>
                </asp:BoundField>

                <asp:BoundField ItemStyle-Width="300px" DataField="City" HeaderText="City" >
                    <ItemStyle Width="300px"></ItemStyle>
                </asp:BoundField>
            </Columns>
            <HeaderStyle BackColor="Gray" />
        </asp:GridView>
        
    </div>
    </form>
</body>
</html>
