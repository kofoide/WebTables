using System;
using System.Data;
using System.Web.UI.WebControls;

public partial class ProcTable : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        GridView1.HeaderRow.TableSection = TableRowSection.TableHeader;
    }

    protected void Insert(object sender, EventArgs e)
    {
        SqlDataSource1.Insert();
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
            if ((e.Row.RowState & DataControlRowState.Edit) > 0)
            {
                DropDownList ddList = (DropDownList)e.Row.FindControl("DropDownList1");

                DataRowView dr = e.Row.DataItem as DataRowView;
                ddList.SelectedValue = dr["Department"].ToString();
            }
    }

    protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView1.EditIndex = e.NewEditIndex;
        GridView1.DataBind();
    }
}