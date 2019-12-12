using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using RocketSystem.DbLink;
using RocketSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RocketSystem.Classes
{
    public class ExcelDataHandling
    {
        public static DataTable GetDataTableFromSpreadsheet(Stream MyExcelStream, bool ReadOnly)
        {
            DataTable dt = new DataTable();
            using (SpreadsheetDocument sDoc = SpreadsheetDocument.Open(MyExcelStream, ReadOnly))
            {
                WorkbookPart workbookPart = sDoc.WorkbookPart;
                IEnumerable<Sheet> sheets = sDoc.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();
                string relationshipId = sheets.First().Id.Value;
                WorksheetPart worksheetPart = (WorksheetPart)sDoc.WorkbookPart.GetPartById(relationshipId);
                Worksheet workSheet = worksheetPart.Worksheet;
                SheetData sheetData = workSheet.GetFirstChild<SheetData>();
                IEnumerable<Row> rows = sheetData.Descendants<Row>();

                foreach (Cell cell in rows.ElementAt(0))
                {
                    dt.Columns.Add(GetCellValue(sDoc, cell));
                }

                foreach (Row row in rows) //this will also include your header row...
                {
                    DataRow tempRow = dt.NewRow();

                    for (int i = 0; i < row.Descendants<Cell>().Count(); i++)
                    {
                        tempRow[i] = GetCellValue(sDoc, row.Descendants<Cell>().ElementAt(i));
                    }

                    dt.Rows.Add(tempRow);
                }
            }
            dt.Rows.RemoveAt(0);
            return dt;
        }
        public static string GetCellValue(SpreadsheetDocument document, Cell cell)
        {
            SharedStringTablePart stringTablePart = document.WorkbookPart.SharedStringTablePart;
            string value = cell.CellValue.InnerXml;

            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                return stringTablePart.SharedStringTable.ChildElements[Int32.Parse(value)].InnerText;
            }
            else
            {
                return value;
            }
        }
        public static void InsertDataToDatabase(DataTable dt)
        {
            using (DataAccessLayer db = new DataAccessLayer())
            {
                CsvData csvData = new CsvData();
                foreach (DataRow row in dt.Rows)
                {
                    DateTime date = DateTime.ParseExact(row[0].ToString(), "yyyyMMdd", CultureInfo.InvariantCulture);
                    //csvData.csvDate = DateTime.ParseExact(row[0].ToString(), "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                    csvData.csvDate = date;
                    csvData.transactionDetail = row[1].ToString();
                    csvData.acceptedamount = Convert.ToInt32(row[2]);
                    csvData.outAmount = row[3].ToString();
                    csvData.detailOne = row[4].ToString();
                    csvData.detailTwo = row[5].ToString();
                    csvData.accountBalance = row[6].ToString();
                    csvData.csvStartDate = DateTime.Now;
                    csvData.csvEndDate = DateTime.Now;
                    csvData.status = "pending";
                    db.CsvDatas.Add(csvData);
                    db.SaveChanges();
                }
            }
        }
    }
}