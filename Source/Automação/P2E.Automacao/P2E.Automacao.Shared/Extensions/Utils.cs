using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace P2E.Automacao.Shared.Extensions
{
    public static class Utils
    {
        public static bool ListObjectToExcel<T>(IList<T> data, string arquivo, string titulo)
        {
            try
            {
                ExcelPackage package = new ExcelPackage();
                ExcelWorksheet excelSheet = package.Workbook.Worksheets.Add("Sheet1");

                excelSheet.Cells[1, 1].Value = "Exportado";
                excelSheet.Cells[1, 2].Value = "Data : " + DateTime.Now.ToShortDateString();

                DataTable dataTable = ConvertToDataTable(data);
                int rowcount = 2;

                foreach (DataRow datarow in dataTable.Rows)
                {
                    rowcount += 1;
                    for (int i = 1; i <= dataTable.Columns.Count; i++)
                    {                        
                        /*if (rowcount == 3)
                        {
                            excelSheet.Cells[2, i].Value = dataTable.Columns[i - 1].ColumnName;
                            excelSheet.Cells.Font.Color = System.Drawing.Color.Black;
                        }*/

                        excelSheet.Cells[rowcount, i].Value = datarow[i - 1].ToString();

                        //for alternate rows
                        /*if (rowcount > 3)
                        {
                            if (i == dataTable.Columns.Count)
                            {
                                if (rowcount % 2 == 0)
                                {
                                    excelCellrange = excelSheet.Range[excelSheet.Cells[rowcount, 1], excelSheet.Cells[rowcount, dataTable.Columns.Count]];
                                    FormattingExcelCells(excelCellrange, "#CCCCFF", System.Drawing.Color.Black, false);
                                }

                            }
                        }*/
                    }
                }

                package.SaveAs(new System.IO.FileInfo(arquivo));
                package.Dispose();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public static DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();

            foreach (PropertyDescriptor prop in properties)
                table
                    .Columns
                    .Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;

                table.Rows.Add(row);
            }

            return table;
        }
    }
}
