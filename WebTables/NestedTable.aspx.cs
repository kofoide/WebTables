﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class NestedTable : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			gvCustomers.DataSource = GetData("SELECT CustomerID, ContactName, City FROM dbo.Customers");
			gvCustomers.DataBind();
		}
	}

	private static DataTable GetData(string query)
	{
		string strConnString = ConfigurationManager.ConnectionStrings["TableSource"].ConnectionString;
		using (SqlConnection con = new SqlConnection(strConnString))
		{
			using (SqlCommand cmd = new SqlCommand())
			{
				cmd.CommandText = query;
				using (SqlDataAdapter sda = new SqlDataAdapter())
				{
					cmd.Connection = con;
					sda.SelectCommand = cmd;
					using (DataSet ds = new DataSet())
					{
						DataTable dt = new DataTable();
						sda.Fill(dt);
						return dt;
					}
				}
			}
		}
	}

	protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
	{
		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			string customerId = gvCustomers.DataKeys[e.Row.RowIndex].Value.ToString();
			GridView gvOrders = e.Row.FindControl("gvOrders") as GridView;
			gvOrders.DataSource = GetData(string.Format("SELECT OrderID, CustomerID, OrderDate, Location FROM dbo.Orders WHERE CustomerId={0}", customerId));
			gvOrders.DataBind();
		}
	}
}