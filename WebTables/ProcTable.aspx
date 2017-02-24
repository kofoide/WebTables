<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProcTable.aspx.cs" Inherits="ProcTable" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="css/styleCopy.css" rel="stylesheet" type="text/css" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:GridView ID="GridView1" runat="server"
        AutoGenerateColumns="False" DataSourceID="SqlDataSource1"
        DataKeyNames="UserId" EmptyDataText="No records has been added." GridLines="Both" BorderWidth="2px" BorderColor="White">
        <Columns>
            <asp:BoundField DataField="UserId" HeaderText="UserId" ReadOnly="True" >
            </asp:BoundField>
            <asp:BoundField DataField="FirstName" HeaderText="FirstName" ReadOnly="True" >
            </asp:BoundField>
            <asp:BoundField DataField="LastName" HeaderText="LastName" ReadOnly="True" >
            </asp:BoundField>
            <asp:BoundField DataField="AdditionalCategory" HeaderText="Additional Category" ReadOnly="True" >
            </asp:BoundField>
            <asp:CheckBoxField DataField="IsTest" HeaderText="IsTest" >
            </asp:CheckBoxField>
            <asp:TemplateField HeaderText="Department" ControlStyle-ForeColor="Black">
                <EditItemTemplate>
                    <asp:DropDownList ID="DropDownList1" runat="server" SelectedValue='<%# Bind("Department") %>'>
                        <asp:ListItem Selected="True">None</asp:ListItem>
                        <asp:ListItem>Sales</asp:ListItem>
                        <asp:ListItem>IT</asp:ListItem>
                        <asp:ListItem>Product</asp:ListItem>
                    </asp:DropDownList>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("Department") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:CommandField ButtonType="Link" ShowEditButton="true" ShowDeleteButton="false" >
            </asp:CommandField>
        </Columns>
    </asp:GridView>

    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:TableSource %>"
        SelectCommand = "
        SELECT
	        U.UserId
        ,	U.FirstName
        ,	U.LastName
        ,	UA.IsTest
        ,	ISNULL(UA.Department, 'None') AS Department
        ,	CASE WHEN UA.UserId IS NULL THEN 'New' ELSE 'Exists' END AS AdditionalCategory
        FROM
			        dbo.[User]			U
        LEFT JOIN	dbo.UserAdditional	UA	ON	U.UserId = UA.UserId"

        InsertCommand = "EXEC dbo.UserAdditionalUpsert @UserId, @IsTest, @Department"
        UpdateCommand = "EXEC dbo.UserAdditionalUpsert @UserId, @IsTest, @Department">
        <InsertParameters>
            <asp:ControlParameter Name="UserId" ControlID="intUserId" Type="Int32" />
            <asp:ControlParameter Name="IsTest" ControlID="bolIsTest" Type="Boolean" />
            <asp:ControlParameter Name="Department" ControlID="DropDownList1" Type="String" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="UserId" Type="Int32" />
            <asp:Parameter Name="IsTest" Type="Boolean" />
            <asp:Parameter Name="Department" Type="String" />
        </UpdateParameters>
    </asp:SqlDataSource>



    </div>
    </form>
</body>
</html>
