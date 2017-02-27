using System;
using System.Data;
using System.Web.UI.WebControls;

public partial class ProcTable2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Insert(object sender, EventArgs e)
    {
        SqlDataSource1.Insert();
    }

    protected void roundedcorner_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
            if ((e.Row.RowState & DataControlRowState.Edit) > 0)
            {
                DropDownList ddList = (DropDownList)e.Row.FindControl("DropDownList1");

                DataRowView dr = e.Row.DataItem as DataRowView;
                ddList.SelectedValue = dr["Department"].ToString();
            }
    }

    protected void roundedcorner_RowEditing(object sender, GridViewEditEventArgs e)
    {
        roundedcorner.UseAccessibleHeader = true;
        roundedcorner.HeaderRow.TableSection = TableRowSection.TableHeader;
        roundedcorner.FooterRow.TableSection = TableRowSection.TableFooter;
        roundedcorner.EditIndex = e.NewEditIndex;
        roundedcorner.DataBind();
    }

    protected void roundedcorner_PreRender(object sender, EventArgs e)
    {
        if (roundedcorner.Rows.Count > 0)
        {
            //This replaces <td> with <th> and adds the scope attribute
            roundedcorner.UseAccessibleHeader = true;

            //This will add the <thead> and <tbody> elements
            roundedcorner.HeaderRow.TableSection = TableRowSection.TableHeader;

            //This adds the <tfoot> element. 
            //Remove if you don't have a footer row
            roundedcorner.FooterRow.TableSection = TableRowSection.TableFooter;
        }
    }
}