using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradesAppWebControls
{
    public class TradesAppDataGrid
    {
        public string SortExpression { get; set; }
        public string SortDirection { get; set; }
        public int CurrentPageNumber { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        public string Title { get; set; }
        public Boolean ReturnStatus { get; set; }
        public string RowSelectionFunction { get; set; }
        public string AjaxFunction { get; set; }

        private string ControlName { get; set; }

        public SortedList columns;
        public SortedList rows;
        public List<Column> rowColumns;

        private int rowIndex;
        private int columnIndex;

        public TableStyle Style;

        public TradesAppDataGrid(string controlName)
        {
            columns = new SortedList();
            rows = new SortedList();
            rowIndex = 0;
            columnIndex = 0;
            TotalPages = 0;
            TotalRecords = 0;
            ReturnStatus = true;
            Style = new TableStyle();
            ControlName = controlName;
        }

        public class TableStyle
        {
            public string Width { get; set; }
        }

        public class Column
        {
            public int ColumnIndex { get; set; }
            public string ColumnName { get; set; }
            public string ColumnValue { get; set; }
            public string HeaderText { get; set; }
            public string TextAlign { get; set; }
            public string Width { get; set; }
            public Boolean Selectable { get; set; }

        }

        public void AddRow()
        {
            rowIndex++;
            rowColumns = new List<Column>();
        }

        public void PopulateRow(string columnName, string columnValue, Boolean selectable)
        {
            Column currentColumn = new Column();
            currentColumn.ColumnName = columnName;
            currentColumn.ColumnValue = columnValue;
            currentColumn.Selectable = selectable;
            rowColumns.Add(currentColumn);

        }

        public void InsertRow()
        {
            rows.Add(rowIndex, rowColumns);
        }

        public void AddColumn(string columnName, string headerText, string width, string textAlign)
        {

            Column column = new Column();

            columnIndex++;

            column.ColumnIndex = columnIndex;
            column.ColumnName = columnName;
            column.HeaderText = headerText;
            column.Width = width;
            column.TextAlign = textAlign;

            columns.Add(columnIndex, column);

        }

        public string CreateControl()
        {
            StringBuilder gridBuilder = new StringBuilder();

            if (TotalPages > 0)
            {
                gridBuilder.Append(BuildGrid());
                gridBuilder.Append(BuildPager());
                gridBuilder.Append(GenerateJavascript());
            }

            return gridBuilder.ToString().Replace("ModelControlName", ControlName);

        }

        private string BuildGrid()
        {

            StringBuilder html = new StringBuilder();
            string javascript = "java" + "script:";

            string tableStyle = String.Empty;

            if (Style.Width != String.Empty)
                tableStyle = " style=\"width:" + Style.Width + ";\"";

            html.Append("<table class=\"DataGridHeader\"" + tableStyle + ">");
            html.Append("<tr><td>" + TotalRecords + "&nbsp;" + Title + "</td><td style=\"text-align: right\">");
            html.Append("Page&nbsp;" + CurrentPageNumber + "&nbsp;of&nbsp;" + TotalPages);
            html.Append("</td></tr></table>");
            html.Append("<table class=\"DataGrid\"" + tableStyle + "><tr>");

            foreach (DictionaryEntry column in columns)
            {
                Column currentColumn =
                    (Column) column.Value;

                html.Append("<th style=\"width: " + currentColumn.Width + "; text-align: " + currentColumn.TextAlign +
                            "\">");
                html.Append("<a style=\"text-decoration: underline; color: Black\" href=\"" + javascript +
                            "ModelControlNameSortGrid('" + currentColumn.ColumnName + "');\">");
                html.Append(currentColumn.HeaderText + "</a>");

                if (SortExpression == currentColumn.ColumnName)
                {
                    if (SortDirection == "DESC")
                    {
                        html.Append(
                            "<img src=\"/Content/Images/UpArrow.gif\" style=\"vertical-align: middle\" alt=\"Asc\" />");
                    }
                    else
                    {
                        html.Append(
                            "<img src=\"/Content/Images/DownArrow.gif\" style=\"vertical-align: middle\" alt=\"Desc\" />");
                    }

                }

                html.Append("</th>");

            }

            html.Append("</tr>");

            if (ReturnStatus == true)
            {
                int i = 0;
                foreach (DictionaryEntry row in rows)
                {
                    string colorCode = i++%2 == 0 ? "White" : "WhiteSmoke";
                    html.Append("<tr style=\"height:25px; color:Black; background-color:" + colorCode + "\">  ");

                    List<Column> rowColumns =
                        (List<Column>) row.Value;

                    foreach (var column in rowColumns)
                    {

                        if (column.Selectable == true)
                        {
                            html.Append(" <td>");
                            html.Append(" <a href=\"" + javascript + RowSelectionFunction + "('" + column.ColumnValue +
                                        "');\">" + column.ColumnValue + "</a>");
                            html.Append(" </td>");
                        }
                        else
                        {
                            html.Append(" <td> " + column.ColumnValue);
                            html.Append(" </td>");
                        }

                    }

                    html.Append("</tr>");

                }

            }

            html.Append("</table>");

            return html.ToString();

        }

        private string BuildPager()
        {
            StringBuilder html = new StringBuilder();
            string javascript = "java" + "script:";

            if (TotalPages == 0) return String.Empty;

            html.Append(" <div class=\"Pager\">   ");

            if (CurrentPageNumber == 1)
            {
                html.Append(" <div style=\"float:left;  width:70px\"><<&nbsp;First</div> ");
            }
            else
            {
                html.Append(" <div style=\"float:left;  width:70px\"><a style=\"text-decoration:none\" href=\"" +
                            javascript + "ModelControlNameFirstPage();\"><<&nbsp;First</a></div> ");
            }

            if (CurrentPageNumber == 1)
            {
                html.Append(" <div style=\"float:left;  width:70px\">< Prev</div> ");
            }
            else
            {
                html.Append(" <div style=\"float:left;  width:70px\"><a style=\"text-decoration:none\" href=\"" +
                            javascript + "ModelControlNamePreviousPage();\"><&nbsp;Prev</a></div>  ");
            }

            if (CurrentPageNumber == TotalPages)
            {
                html.Append(" <div style=\"float:left;  width:70px\">Next ></div> ");
            }
            else
            {
                html.Append(" <div style=\"float:left;  width:70px\"><a style=\"text-decoration:none\" href=\"" +
                            javascript + "ModelControlNameNextPage();\">Next ></a></div> ");
            }

            if (CurrentPageNumber == TotalPages)
            {
                html.Append(" <div style=\"float:left;  width:70px\">Last >></div> ");
            }
            else
            {
                html.Append(" <div style=\"float:left;  width:70px\"><a style=\"text-decoration:none\" href=\"" +
                            javascript + "ModelControlNameLastPage();\">Last >></a></div> ");
            }

            html.Append(" </div>  <div style=\"clear:both;\"></div>     ");

            return html.ToString();

        }

        private string GenerateJavascript()
        {

            StringBuilder html = new StringBuilder();

            html.Append("<script language='javascript' type='text/javascript'> ");

            html.Append(" function ModelControlNameNextPage() { ");
            html.Append("    var currentPageNumber = parseInt($(\"#ModelControlNameCurrentPageNumber\").val()) + 1; ");
            html.Append("    $(\"#ModelControlNameCurrentPageNumber\").val(currentPageNumber); ");
            html.Append("    ModelControlNameExecuteRequest(); ");
            html.Append(" } ");

            html.Append("function ModelControlNamePreviousPage() { ");
            html.Append("    var currentPageNumber = parseInt($(\"#ModelControlNameCurrentPageNumber\").val()) - 1; ");
            html.Append("    $(\"#ModelControlNameCurrentPageNumber\").val(currentPageNumber); ");
            html.Append("    ModelControlNameExecuteRequest(); ");
            html.Append(" } ");

            html.Append(" function ModelControlNameLastPage() { ");
            html.Append("    var currentPageNumber = $(\"#ModelControlNameTotalPages\").val(); ");
            html.Append("    $(\"#ModelControlNameCurrentPageNumber\").val(currentPageNumber);  ");
            html.Append("    ModelControlNameExecuteRequest(); ");
            html.Append(" } ");

            html.Append(" function ModelControlNameFirstPage() { ");
            html.Append("    $(\"#ModelControlNameCurrentPageNumber\").val(\"1\");    ");
            html.Append("    ModelControlNameExecuteRequest(); ");
            html.Append(" } ");

            html.Append(" function ModelControlNameSearch() { ");
            html.Append("    $(\"#ModelControlNameCurrentPageNumber\").val(\"1\"); ");
            html.Append("    $(\"#ModelControlNameSortDirection\").val(\"\"); ");
            html.Append("    $(\"#ModelControlNameSortExpression\").val(\"\"); ");
            html.Append("    ModelControlNameExecuteRequest(); ");
            html.Append(" } ");

            html.Append(" function ModelControlNameSortGrid(sortExpression) { ");
            html.Append("    if ($(\"#ModelControlNameSortExpression\").val() == sortExpression) { ");
            html.Append("        if ($(\"#ModelControlNameSortDirection\").val() == \"ASC\") ");
            html.Append("            $(\"#ModelControlNameSortDirection\").val(\"DESC\"); ");
            html.Append("        else ");
            html.Append("            $(\"#ModelControlNameSortDirection\").val(\"ASC\"); ");
            html.Append("    } ");
            html.Append("    else { ");
            html.Append("        $(\"#ModelControlNameSortDirection\").val(\"ASC\"); ");
            html.Append("    } ");

            html.Append("    $(\"#ModelControlNameSortExpression\").val(sortExpression); ");
            html.Append("    $(\"#ModelControlNameCurrentPageNumber\").val(\"1\"); ");

            html.Append("    ModelControlNameExecuteRequest(); ");
            html.Append(" } ");

            html.Append(" function ModelControlNameExecuteRequest() { ");
            html.Append(AjaxFunction +
                        "($(\"#ModelControlNameCurrentPageNumber\").val(),  $(\"#ModelControlNameSortExpression\").val(), $(\"#ModelControlNameSortDirection\").val()); ");
            html.Append(" } ");

            html.Append("   </script> ");

            html.Append(
                "  <input id=\"ModelControlNameCurrentPageNumber\" name=\"ModelControlNameCurrentPageNumber\" type=\"hidden\" value=\"" +
                CurrentPageNumber + "\" />   ");
            html.Append(
                "  <input id=\"ModelControlNameTotalPages\" name=\"ModelControlNameTotalPages\" type=\"hidden\" value=\"" +
                TotalPages + "\" />   ");
            html.Append(
                "  <input id=\"ModelControlNameSortExpression\" name=\"ModelControlNameSortExpression\" type=\"hidden\" value=\"" +
                SortExpression + "\" />    ");
            html.Append(
                "  <input id=\"ModelControlNameSortDirection\" name=\"ModelControlNameSortDirection\" type=\"hidden\" value=\"" +
                SortDirection + "\" />   ");

            return html.ToString();

        }
    }
}

