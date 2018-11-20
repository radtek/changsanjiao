using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMShareBLL.Model;


namespace WebUI.Web
{

    public class Pub
    {
        public static Table FindTableControl(Control container)
        {
            if (null == container)
                return null;
            Table theTable = null;
            foreach (Control c in container.Controls)
            {
                if (c.GetType() == typeof(Table))
                {
                    theTable = c as Table;
                    break;
                }
            }
            return theTable;
        }

        /// <summary>
        /// 根据DataTable创建一个Table控件
        /// </summary>
        /// <param name="dataSource"></param>
        /// <returns></returns>
        public static Table CreateTable(DataTable dataSource)
        {
            if (null == dataSource)
                return null;

            Table theTable = new Table();
            TableRow headRow = new TableRow();
            foreach (DataColumn column in dataSource.Columns)
            {
                TableCell cell = new TableCell();
                cell.Text = column.ColumnName;
                headRow.Cells.Add(cell);
            }

            theTable.Rows.Add(headRow);
            int columns = dataSource.Columns.Count;
            foreach (DataRow row in dataSource.Rows)
            {
                TableRow dataRow = new TableRow();
                for (int i = 0; i < columns; i++)
                {
                    TableCell cell = new TableCell();
                    cell.Text = row[i].ToString();
                    dataRow.Cells.Add(cell);
                }
                theTable.Rows.Add(dataRow);
            }

            return theTable;
        }

        public static string FormatDateString(DateTime day)
        {
            return day.Year + "-" + day.Month + "-" + day.Day;
        }

        public static float GetNumLevel(float num)
        {
            if (num >= 1)
                return 1;
            float nl = 1;
            float nc = 10;
            float tempNum = num;
            while (tempNum < 0)
            {
                tempNum = num * nc;
                nc *= 10;
            }
            return (float)((float)nl / (float)nc);
        }

        public static DataRange CalcValueRange(DataRange range, int num, out decimal interval)
        {
            decimal minN = 1m;
            decimal min = range.Min;
            decimal maxN = 1m;
            decimal max = range.Max;

            if (min != 0)
            {
                while (min > (int)min)
                    min *= 10;
                minN = min / range.Min;

                if (min % num != 0)
                    min -= min % num;
                min /= minN;
            }

            if (max != 0)
            {
                while (max > (int)max)
                    max *= 10;
                maxN = max / range.Max;
                if (max % num != 0)
                {
                    max += num;
                    max -= max % num;
                }
                max /= maxN;
            }

            interval = (decimal)Math.Abs((float)(max - min)) / num;
            DataRange theRange = new DataRange(min, max);
            return theRange;
        }

        public static Color ParseColor(string hex)
        {
            try
            {
                Color theColor = Color.FromArgb(Int32.Parse(hex.Substring(1), System.Globalization.NumberStyles.HexNumber));
                return Color.FromArgb(100, theColor.R, theColor.G, theColor.B);
            }
            catch { return Color.Empty; }
        }

        public static DataTable ParseGridView(GridView grid)
        {
            DataTable theTable = new DataTable();
            int columnCount = 0;
            foreach (DataControlField column in grid.Columns)
            {
                theTable.Columns.Add(column.HeaderText, typeof(string));
            }
            columnCount = theTable.Columns.Count;
            if (grid.AutoGenerateColumns)
            {
                foreach (TableCell tc in grid.HeaderRow.Cells)
                    theTable.Columns.Add(tc.Text, typeof(string));
                columnCount += grid.HeaderRow.Cells.Count;
            }

            foreach (GridViewRow row in grid.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    DataRow newRow = theTable.NewRow();
                    for (int i = 0; i < columnCount; i++)
                    {
                        newRow[i] = row.Cells[i].Text;
                    }
                    theTable.Rows.Add(newRow);
                }
            }
            return theTable;
        }


        public static void BindParams(ListControl cblParameters, List<DataParameter> parameters, bool setDefaultChecked, bool appendDataBoundItems)
        {
            if (!appendDataBoundItems)
                cblParameters.Items.Clear();
            foreach (DataParameter param in parameters)
            {
                ListItem item = new ListItem();
                item.Text = param.Name;
                //if (!string.IsNullOrEmpty(param.Byname))
                //    item.Text += "(" + param.Byname + ")";
                item.Value = param.Id.ToString();
                cblParameters.Items.Add(item);
            }
            if (setDefaultChecked && cblParameters.Items.Count > 0)
                cblParameters.Items[0].Selected = true;
        }


        public static void SetChartDefaultProperties(dotnetCHARTING.Chart chart)
        {
            chart.YAxis.GridLine.Color = Color.FromArgb(225, 225, 225);
            chart.XAxis.GridLine.Color = Color.FromArgb(225, 225, 225);
            chart.DefaultChartArea.Background.Color = Color.FromArgb(247, 247, 247);

            chart.DefaultSeries.EmptyElement.Mode = dotnetCHARTING.EmptyElementMode.Fill;
            chart.DefaultSeries.EmptyElement.Line.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            chart.DefaultSeries.EmptyElement.Line.Color = Color.Gray;
            chart.DefaultElement.Marker.Size = 5;

            chart.DefaultElement.Marker.Type = dotnetCHARTING.ElementMarkerType.Circle;

            // chart.PaletteName = dotnetCHARTING.Palette.Five;
        }

        public static int[] SplitString(string str, bool alowSame)
        {
            if (string.IsNullOrEmpty(str))
                return new int[] { };
            List<int> reList = new List<int>();
            string[] strArray = str.Split(new char[] { ',' });
            foreach (string item in strArray)
            {
                int value = 0;
                if (int.TryParse(item, out value) && (alowSame || !reList.Contains(value)))
                    reList.Add(value);
            }
            return reList.ToArray();
        }
        public static int[] SplitString(string str, bool alowSame, bool sort)
        {
            if (string.IsNullOrEmpty(str))
                return new int[] { };
            List<int> reList = new List<int>();
            string[] strArray = str.Split(new char[] { ',' });
            foreach (string item in strArray)
            {
                int value = 0;
                if (int.TryParse(item, out value) && (alowSame || !reList.Contains(value)))
                    reList.Add(value);
            }
            if (sort)
                reList.Sort();
            return reList.ToArray();
        }

        public static DateTime AppendDate(int value, Duration duration, DateTime date)
        {
            switch (duration)
            {
                case Duration.Hour: return date.AddHours(value);
                case Duration.Day: return date.AddDays(value);
                case Duration.Month: return date.AddMonths(value);
                case Duration.Quarter: return date.AddMonths(value * 3);
                case Duration.Year: return date.AddYears(value);
                default: return date;
            }
        }
    }
}