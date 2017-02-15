using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Table : System.Web.UI.Page
{
	// List of the textboxes created for inserts
	//	this is needed to clear the boxes out after the insert is done
	List<TextBox> boxes = new List<TextBox>();

	protected void Page_Load(object sender, EventArgs e)
	{
		//Setup the page
		SetupPage();

		if (!IsPostBack)
		{
			TitleLabel.Text = Request.QueryString["Schema"] + "." + Request.QueryString["Table"];

			sds.DataBind();
		}
	}

	protected void SetupPage()
	{
		// Clear the Grid before resetting it up
		gv.Columns.Clear();

		sds.ConnectionString = ConfigurationManager.ConnectionStrings["TableSource"].ConnectionString;

		string fullTableName = Request.QueryString["Schema"] + "." + Request.QueryString["Table"];
		string delSQL = "DELETE FROM " + fullTableName + " WHERE 1=1 ";

		string updateSQL = "UPDATE " + fullTableName;
		string updateSetSQL = " SET ";
		string updateWhereSQL = " WHERE 1=1 ";

		string selectSQL = "SELECT * FROM " + fullTableName;
		
		string insertSQL = "INSERT INTO " + fullTableName + "(";
		string insertColumnsSQL = "";
		string valuesSQL = ") VALUES(";
		string valuesColumnsSQL = "";

		List<string> primaryKeyColumns = new List<string>();

		bool isPrimarySet = false;
		int insertRowColumnSpan = 0;

		#region Get Single row from table to determine real column Datatypes
		string top1SQL = "SELECT TOP 1 * FROM " + fullTableName;
		DataSet top1DS = new DataSet();
		string connString = ConfigurationManager.ConnectionStrings["TableSource"].ConnectionString;
		SqlConnection conn = new SqlConnection(connString);
		SqlCommand comm = new SqlCommand(top1SQL, conn);
		SqlDataAdapter da = new SqlDataAdapter(comm);
		conn.Open();
		da.Fill(top1DS); 
		#endregion

		#region typeMap
		var typeMap = new Dictionary<Type, DbType>();
		typeMap[typeof(byte)] = DbType.Byte;
		typeMap[typeof(sbyte)] = DbType.SByte;
		typeMap[typeof(short)] = DbType.Int16;
		typeMap[typeof(ushort)] = DbType.UInt16;
		typeMap[typeof(int)] = DbType.Int32;
		typeMap[typeof(uint)] = DbType.UInt32;
		typeMap[typeof(long)] = DbType.Int64;
		typeMap[typeof(ulong)] = DbType.UInt64;
		typeMap[typeof(float)] = DbType.Single;
		typeMap[typeof(double)] = DbType.Double;
		typeMap[typeof(decimal)] = DbType.Decimal;
		typeMap[typeof(bool)] = DbType.Boolean;
		typeMap[typeof(string)] = DbType.String;
		typeMap[typeof(char)] = DbType.StringFixedLength;
		typeMap[typeof(Guid)] = DbType.Guid;
		typeMap[typeof(DateTime)] = DbType.DateTime;
		typeMap[typeof(DateTimeOffset)] = DbType.DateTimeOffset;
		typeMap[typeof(byte[])] = DbType.Binary;
		typeMap[typeof(byte?)] = DbType.Byte;
		typeMap[typeof(sbyte?)] = DbType.SByte;
		typeMap[typeof(short?)] = DbType.Int16;
		typeMap[typeof(ushort?)] = DbType.UInt16;
		typeMap[typeof(int?)] = DbType.Int32;
		typeMap[typeof(uint?)] = DbType.UInt32;
		typeMap[typeof(long?)] = DbType.Int64;
		typeMap[typeof(ulong?)] = DbType.UInt64;
		typeMap[typeof(float?)] = DbType.Single;
		typeMap[typeof(double?)] = DbType.Double;
		typeMap[typeof(decimal?)] = DbType.Decimal;
		typeMap[typeof(bool?)] = DbType.Boolean;
		typeMap[typeof(char?)] = DbType.StringFixedLength;
		typeMap[typeof(Guid?)] = DbType.Guid;
		typeMap[typeof(DateTime?)] = DbType.DateTime;
		typeMap[typeof(DateTimeOffset?)] = DbType.DateTimeOffset; 
		#endregion

		// Get the metadata of the table
		DataSet dsMetaData = GetTableMetaData();
		//int numRows = dsMetaData.Tables[0].Rows.Count;

		#region Loop Through Columns
		foreach (DataRow row in dsMetaData.Tables[0].Rows)
		{
			string columnName = row[0].ToString();
			int ordinalPosition = (int)row[1];
			string isNullable = row[2].ToString();
			//string dataType = row[3].ToString();
			int isPrimary = (int)row[4];
			int isIdentity = (int)row[5];
			//Gets the true SQL Datatype of the column
			var trueDataType = typeMap[top1DS.Tables[0].Columns[ordinalPosition - 1].DataType];

			// Create the Grid field
			BoundField field = new BoundField();
			field.HeaderText = columnName;
			field.DataField = columnName;

			#region Non Identity Column
			if (isIdentity != 1)
			{
				TextBox tb = new TextBox();
				tb.ID = "tb" + columnName;
				boxes.Add(tb);

				TableCell cellHeader = new TableCell();
				cellHeader.Text = columnName + ":";
				cellHeader.HorizontalAlign = HorizontalAlign.Left;
				cellHeader.Font.Bold = true;
				HeaderRow.Cells.Add(cellHeader);
				TableCell cellBox = new TableCell();
				cellBox.Controls.Add(tb);
				TextBoxRow.Cells.Add(cellBox);
				

				insertRowColumnSpan++;

				ControlParameter parm = new ControlParameter(columnName, trueDataType, "tb" + columnName, "Text");
				sds.InsertParameters.Add(parm);

				// Insert statement creation
				if (insertColumnsSQL == "")
				{
					insertColumnsSQL = columnName;
					valuesColumnsSQL = "@" + columnName;
				}
				else
				{
					insertColumnsSQL = insertColumnsSQL + ", " + columnName;
					valuesColumnsSQL = valuesColumnsSQL + ", @" + columnName;
				}
			}
			#endregion

			#region If Column Is Primary Key
			if (isPrimary == 1)
			{
				// Add this column to the list of primary keys
				primaryKeyColumns.Add(columnName);

				// Since this field is primary key, it isn't updatable
				field.ReadOnly = true;

				// If we haven't already done so
				//	turn on row editing
				if (!isPrimarySet)
				{
					// Because there is a primary key the rows can be updated/deleted
					// Add Edit/Delete Buttons to the grid
					CommandField edit = new CommandField();
					edit.ShowEditButton = true;
					CommandField del = new CommandField();
					del.ShowDeleteButton = true;
					gv.Columns.Add(edit);
					gv.Columns.Add(del);

					isPrimarySet = true;
				}

				// Setup the Delete, Update, & Insert sql
				Parameter parm = new Parameter(columnName, trueDataType);

				// Add as Delete Parameter in the WHERE clause of DELETE
				sds.DeleteParameters.Add(parm);
				delSQL = delSQL + " AND " + columnName + " = @" + columnName;

				// Add as Update Parameter in the WHERE clause of UPDATE
				sds.UpdateParameters.Add(parm);
				updateWhereSQL = updateWhereSQL + " AND " + columnName + " = @" + columnName;

				// What to do with Insert???
			}
			#endregion
			#region Else Column is not Primary Key
			else
			{
				Parameter parm = new Parameter(columnName, trueDataType);
				sds.UpdateParameters.Add(parm);

				// Create the SET part of the UPDATE statement
				// First SET value
				if (updateSetSQL.Equals(" SET "))
				{
					updateSetSQL = updateSetSQL + columnName + " = @" + columnName;
				}
				else
				{
					updateSetSQL = updateSetSQL + " , " + columnName + " = @" + columnName;
				}
			}
			#endregion

			gv.Columns.Add(field);
		}
		#endregion


		sds.DeleteCommand = delSQL;
		sds.UpdateCommand = updateSQL + updateSetSQL + updateWhereSQL;
		sds.SelectCommand = selectSQL;
		sds.InsertCommand = insertSQL + insertColumnsSQL + valuesSQL + valuesColumnsSQL + ")";

		// Set the InsertRow of the table to span X columns
		InsertCell.ColumnSpan = insertRowColumnSpan;

		// Add primary key names to grid
		gv.DataKeyNames = primaryKeyColumns.ToArray();
	}

	protected DataSet GetTableMetaData()
	{
		// Query to retrieve meta-data about the selected table
		string sql = @"
SELECT
	C.COLUMN_NAME
,	C.ORDINAL_POSITION
,	C.IS_NULLABLE
,	C.DATA_TYPE
,	CASE WHEN K.COLUMN_NAME IS NOT NULL THEN 1 ELSE 0 END AS IsPrimary
,	COLUMNPROPERTY(OBJECT_ID(@Schema + '.' + @Table), C.COLUMN_NAME, 'IsIdentity') AS IsIdentity

FROM
			INFORMATION_SCHEMA.COLUMNS	C
LEFT JOIN	INFORMATION_SCHEMA.KEY_COLUMN_USAGE	K	ON	C.TABLE_SCHEMA = K.TABLE_SCHEMA
													AND	C.TABLE_NAME = K.TABLE_NAME
													AND	C.COLUMN_NAME = K.COLUMN_NAME
WHERE
	C.TABLE_SCHEMA = @Schema
AND	C.TABLE_NAME = @Table
";

		DataSet ds = new DataSet();

		string connString = ConfigurationManager.ConnectionStrings["TableSource"].ConnectionString;

		SqlConnection conn = new SqlConnection(connString);
		SqlCommand comm = new SqlCommand(sql, conn);
		comm.Parameters.Add("@Schema", SqlDbType.VarChar);
		comm.Parameters["@Schema"].Value = Request.QueryString["Schema"];
		comm.Parameters.Add("@Table", SqlDbType.VarChar);
		comm.Parameters["@Table"].Value = Request.QueryString["Table"];

		SqlDataAdapter da = new SqlDataAdapter(comm);

		conn.Open();

		da.Fill(ds);

		return ds;
	}

	protected void InsertButton_Click(object sender, EventArgs e)
	{
		try
		{
			sds.Insert();

			// Clear out the insert textboxes
			foreach (TextBox x in boxes)
			{
				x.Text = String.Empty;
			}
		}
		catch (Exception ex)
		{
			ErrorBox.Text = ex.Message;
			ErrorBox.Visible = true;
		}
	}
}