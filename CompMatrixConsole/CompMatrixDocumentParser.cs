using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;
using System.Reflection;
using ToolBox.Model;

namespace ToolBox.Utilities
{
    public class CompMatrixDocumentParser
    {
        private bool _considerNewLine = false;

        public CompMatrixDocumentParser()
        {
        }

        public List<CompMatrixModel> Parse(Stream fileStream)
        {
            List<CompMatrixModel> compMatrixList = Parse(fileStream, null);
            return compMatrixList;
        }

        public List<CompMatrixModel> Parse(Stream fileStream, params string[] args)
        {
            if (fileStream == null) return null;

            string[] columnNames = args != null ? args.ToArray() : null;

            List<CompMatrixModel> compMatrixList = new List<CompMatrixModel>();

            try
            {
                Dictionary<string, string> columnMapper = new Dictionary<string, string>();

                WorkbookPart workbookPart = null; List<Row> rows = null;

                SpreadsheetDocument excdocument = SpreadsheetDocument.Open(fileStream, false);
                workbookPart = excdocument.WorkbookPart;

                var sheets = workbookPart.Workbook.Descendants<Sheet>();
                var sheet = sheets.First();

                var workSheet = ((WorksheetPart)workbookPart.GetPartById(sheet.Id)).Worksheet;
                var columns = workSheet.Descendants<Columns>().FirstOrDefault();

                var sheetData = workSheet.Elements<SheetData>().First();
                rows = sheetData.Elements<Row>().ToList();

                // Read the header
                if (rows.Count > 0)
                {
                    Dictionary<string, string> columnNameInfos = new Dictionary<string, string>();
                    if (columnNames != null) columnNames.ToList().ForEach(a => columnNameInfos.Add(a.GetTrimmedColumnName(), a));

                    var row = rows[0];
                    var cellEnumerator = GetExcelCellEnumerator(row);
                    while (cellEnumerator.MoveNext())
                    {
                        var cell = cellEnumerator.Current;
                        var text = ReadExcelCell(excdocument, cell, workbookPart).Trim();

                        if (columnNameInfos.Count > 0)
                        {
                            text = text.GetTrimmedColumnName();
                            string keyMatch = columnNameInfos.Keys.SingleOrDefault(cn => cn == text);
                            if (keyMatch != null)
                            {
                                string columnIndexName = GetColumnName(cell.CellReference);
                                string columnName = columnNameInfos[keyMatch];
                                if (!_considerNewLine) columnName = columnName.Replace("\n", " ");
                                columnMapper.Add(columnIndexName, columnName);
                            }
                        }
                        else
                        {
                            string columnIndexName = GetColumnName(cell.CellReference);
                            if (!_considerNewLine) text = text.Replace("\n", " ");
                            columnMapper.Add(columnIndexName, text);
                        }
                    }
                }

                // Read the sheet data
                if (rows.Count > 1)
                {
                    for (var i = 1; i < rows.Count; i++)
                    {
                        CompMatrixModel modelInstance = new CompMatrixModel();
                        Type modelType = typeof(CompMatrixModel);
                        PopulateModel(excdocument, columnMapper, workbookPart, modelInstance, modelType, rows[i]);
                        compMatrixList.Add(modelInstance);
                    }
                }
            }
            catch (OpenXmlPackageException e)
            {
                throw new Exception(e.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return compMatrixList;
        }

        private void PopulateModel(SpreadsheetDocument doc, Dictionary<string, string> columnMapper, WorkbookPart workbookPart, CompMatrixModel modelInstance, Type modelType, Row row)
        {
            var cellEnumerator = GetExcelCellEnumerator(row);
            while (cellEnumerator.MoveNext())
            {
                var cell = cellEnumerator.Current;
                if (cell.CellReference == null) continue;

                string columnIndexName = GetColumnName(cell.CellReference);

                string key = columnMapper.Keys.SingleOrDefault(k => k == columnIndexName);
                if (key != null)
                {
                    var text = ReadExcelCell(doc, cell, workbookPart).Trim();
                    string propertyDescription = columnMapper[key];

                    PropertyInfo property = GetPropertyByDescription(modelType, propertyDescription);
                    if (property != null)
                    {
                        if (property.PropertyType.Namespace == "System.Collections.Generic")
                        {
                            if (string.IsNullOrEmpty(text))
                            {
                                property.SetValue(modelInstance, null);
                            }
                            else
                            {
                                if (!_considerNewLine) text = text.Replace("\n", "");
                                string[] textList = text.Split(',');
                                property.SetValue(modelInstance, textList.ToList());
                            }
                        }
                        else
                        {
                            if(!_considerNewLine) text = text.Replace("\n", "");
                            property.SetValue(modelInstance, text);
                        }
                    }
                }
            }
        }

        private PropertyInfo GetPropertyByDescription(Type classType, string propertyName)
        {
            PropertyInfo result = null;
            PropertyInfo[] props = classType.GetProperties();
            foreach (PropertyInfo prop in props)
            {
                var attrs = prop.GetCustomAttributes(typeof(ExtendedDescription), true);
                if (attrs.Length > 0)
                {
                    var attrMatch = attrs.SingleOrDefault(a => (a as ExtendedDescription).Values.Contains(propertyName));
                    if (attrMatch != null)
                    {
                        result = prop;
                        break;
                    }
                    //else
                    //{
                    //    attrs = prop.GetCustomAttributes(typeof(ExtraDescriptionAttribute), false);
                    //    if (attrs.Length > 0)
                    //    {
                    //        attrMatch = attrs.SingleOrDefault(a => (a as ExtraDescriptionAttribute).Description == propertyName || (a as ExtraDescriptionAttribute).ExtraInfo == propertyName);
                    //        if (attrMatch != null)
                    //        {
                    //            result = prop;
                    //            break;
                    //        }
                    //    }
                    //}
                }
                //else
                //{
                //    attrs = prop.GetCustomAttributes(typeof(ExtraDescriptionAttribute), false);
                //    if (attrs.Length > 0)
                //    {
                //        var attrMatch = attrs.SingleOrDefault(a => (a as ExtraDescriptionAttribute).Description == propertyName || (a as ExtraDescriptionAttribute).ExtraInfo == propertyName);
                //        if (attrMatch != null)
                //        {
                //            result = prop;
                //            break;
                //        }
                //    }
                //}
            }
            return result;
        }

        private string GetColumnName(string cellReference)
        {
            var regex = new Regex("[A-Za-z]+");
            var match = regex.Match(cellReference);

            return match.Value;
        }

        private int ConvertColumnNameToNumber(string columnName)
        {
            var alpha = new Regex("^[A-Z]+$");
            if (!alpha.IsMatch(columnName)) throw new ArgumentException();

            char[] colLetters = columnName.ToCharArray();
            Array.Reverse(colLetters);

            var convertedValue = 0;
            for (int i = 0; i < colLetters.Length; i++)
            {
                char letter = colLetters[i];
                // ASCII 'A' = 65
                int current = i == 0 ? letter - 65 : letter - 64;
                convertedValue += current * (int)Math.Pow(26, i);
            }

            return convertedValue;
        }

        private IEnumerator<Cell> GetExcelCellEnumerator(Row row)
        {
            int currentCount = 0;
            foreach (Cell cell in row.Descendants<Cell>())
            {
                string columnName = GetColumnName(cell.CellReference);

                int currentColumnIndex = ConvertColumnNameToNumber(columnName);

                for (; currentCount < currentColumnIndex; currentCount++)
                {
                    var emptycell = new Cell()
                    {
                        DataType = null, 
                        CellValue = new CellValue(string.Empty)
                    };
                    yield return emptycell;
                }

                yield return cell;
                currentCount++;
            }
        }

        private string ReadExcelCell(SpreadsheetDocument doc, Cell cell, WorkbookPart workbookPart)
        {
            string value = cell.CellValue.InnerText;
            //Object result;
            //if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            //{
            //    switch (cell.DataType.Value)
            //    {
            //        case CellValues.SharedString:
            //            {
            //                workbookPart = doc.WorkbookPart;

            //                //var sheets = workbookPart.Workbook.Descendants<Sheet>();
            //                //var sheet = sheets.First();

            //                //var workSheet = ((WorksheetPart)workbookPart.GetPartById(sheet.Id)).Worksheet;

            //                var stringTable = workbookPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();
            //                if (stringTable != null)
            //                {
            //                    string value01 = stringTable.SharedStringTable.ElementAt(int.Parse(cell.InnerText)).InnerText;
            //                }

            //                result = doc.WorkbookPart.SharedStringTablePart.SharedStringTable.ChildElements[Convert.ToInt32(cell.CellValue.Text)].InnerText;
            //                //result = doc.WorkbookPart.SharedStringTablePart.SharedStringTable.ChildElements.GetItem(int.Parse(value)).InnerText;
            //            }
            //            break;
            //        case CellValues.Boolean:
            //            result = cell.CellValue.Text == "1" ? true : false;
            //            break;
            //        case CellValues.Date:
            //            result = DateTime.FromOADate(Convert.ToDouble(cell.CellValue.Text));
            //            break;
            //        case CellValues.Number:
            //            result = Convert.ToDecimal(cell.CellValue.Text);
            //            break;
            //        default:
            //            if (cell.CellValue != null)
            //                result = cell.CellValue.Text;
            //            result = string.Empty;
            //            break;
            //    }
            //}
            //else
            //{
            //    WorkbookStylesPart workbookStylesPart = doc.WorkbookPart.GetPartsOfType<WorkbookStylesPart>().First();
            //    CellFormats cellFormats = (CellFormats)workbookStylesPart.Stylesheet.CellFormats;

            //    CellFormat cf = cellFormats.Descendants<CellFormat>().ElementAt<CellFormat>(Convert.ToInt32(cell.StyleIndex.Value));
            //    if (cf.NumberFormatId >= 0 && cf.NumberFormatId <= 13) // This is a number
            //        result = Convert.ToDecimal(cell.CellValue.Text);
            //    else if (cf.NumberFormatId >= 14 && cf.NumberFormatId <= 22) // This is a date
            //        result = DateTime.FromOADate(Convert.ToDouble(cell.CellValue.Text));
            //    else
            //        result = cell.CellValue.Text;
            //}

            var cellValue = cell.CellValue;
            var text = (cellValue == null) ? cell.InnerText : cellValue.Text;
            if ((cell.DataType != null) && (cell.DataType == CellValues.SharedString))
            {
                text = workbookPart.SharedStringTablePart.SharedStringTable
                    .Elements<SharedStringItem>().ElementAt(
                        Convert.ToInt32(cell.CellValue.Text)).InnerText;
            }

            try
            {
                if (cell.StyleIndex == 5 && !string.IsNullOrEmpty(text))
                {
                    double p;
                    bool isNumeric = double.TryParse(text, out p);
                    if(isNumeric)
                    {
                        double percentage = Convert.ToDouble(text) * 100.0;
                        text = string.Format("{0}%", percentage);
                    }
                }
            }
            catch(Exception ee)
            {

            }
            return (text ?? string.Empty).Trim();
        }
    }

    public static class ColumnNameExtension
    {
        public static string GetTrimmedColumnName(this string columnName)
        {
            if (columnName != null)
            {
                columnName = columnName.Replace(" ", "").Replace("’", "").Replace("'", "").Replace("/", "").Replace(",", "").Trim().ToLower();
                return columnName;
            }
            else return string.Empty;
        }
    }
}
