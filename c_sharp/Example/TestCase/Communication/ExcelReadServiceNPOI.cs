using AAH_AutoSim.Server.Dialogs;
using AAH_AutoSim.TestCase.Constants;
using AAH_AutoSim.TestCase.Models;
using AAH_AutoSim.Model.Models;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace AAH_AutoSim.TestCase.Communication
{
    /// <summary>
    /// This class is used to load Test Case Excel file and Test Moduel Excel file.
    /// NPOI Package reference:
    /// https://www.thecodebuzz.com/read-and-write-excel-file-in-net-core-using-npoi/
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ExcelReadServiceNPOI
    {
        private readonly IMessageDialogService _messageDialogService;

        public ExcelReadServiceNPOI(IMessageDialogService messageDialogService)
        {
            _messageDialogService = messageDialogService;
        }

        /// <summary>
        /// Load Test Case data from Excel file
        /// </summary>
        public void LoadTestCaseDataObjs()
        {
            IWorkbook xssWorkbook;
            ISheet sheet;

            try
            {
                using (var stream = new FileStream(TestCaseDataShop.FileName, FileMode.Open))
                {
                    stream.Position = 0;
                    xssWorkbook = new XSSFWorkbook(stream);
                    sheet = xssWorkbook.GetSheet(TestCaseConstants.TestCaseWorksheetName);
                    int AutoSimNameRow = new CellReference(xssWorkbook.GetName(TestCaseConstants.AutoSimCellId).RefersToFormula).Row;
                    IRow headerRow = sheet.GetRow(AutoSimNameRow);
                    int cellCount = headerRow.LastCellNum - TestCaseConstants.StepColumnNum;

                    for (int i = (AutoSimNameRow + 1); i <= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue;
                        if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                        TestCaseDataObj testCase = new TestCaseDataObj();

                        for (int j = row.FirstCellNum + TestCaseConstants.StepColumnNum; j < row.LastCellNum; j++)
                        {
                            // Using StringCellValue will get string value from Formular cell
                            string? cellValue = row.GetCell(j) != null ? row.GetCell(j).StringCellValue : "";
                            if (!string.IsNullOrEmpty(cellValue) && !string.IsNullOrWhiteSpace(cellValue))
                            {
                                BulidTestCaseObj(testCase, j, cellValue);
                            }
                        }
                        TestCaseDataShop.testCaseDataObjs.Add(testCase);
                    }
                }
            }
            catch (Exception ex)
            {
                _messageDialogService.ShowAlertDialog(ex.Message, "Load Test Case File", MessageIcon.Error);
            }
        }

        /// <summary>
        /// Find the location of the cell with a specific cell value
        /// We use xssWorkbook.GetName([cell Id]) to find the location now, but please keep this method for now
        /// </summary>
        /// <param name="workSheet">Worksheet</param>
        /// <param name="cellName">a cell value</param>
        /// <returns>Return the specific location if found it; otherwise return (-1, -1)</returns>
        private (int, int) getLocationCell(ISheet workSheet, String cellName)
        {
            for (int i = 0; i < workSheet.LastRowNum; i++)
            {
                IRow row = workSheet.GetRow(i);
                if (row == null) continue;
                if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                //Traverse all columns of specific Row
                for (int j = row.FirstCellNum; j < row.LastCellNum; j++)
                {
                    //Get the values
                    var val = row.GetCell(j);
                    if (val != null && val.ToString().Equals(cellName))
                    {
                        return (i, j);
                    }
                }
            }
            return (-1, -1);
        }

        /// <summary>
        /// Load Test Module data from Excel file
        /// </summary>
        /// <param name="modulePath">the path to Test Module Excel file</param>
        public void LoadTestModuleDataObjs(string modulePath)
        {
            IWorkbook xssWorkbook;
            ISheet sheet;

            try
            {
                using (var stream = new FileStream(modulePath, FileMode.Open))
                {
                    stream.Position = 0;
                    xssWorkbook = new XSSFWorkbook(stream);
                    for (int sNum = 0; sNum < xssWorkbook.NumberOfSheets; sNum++)
                    {
                        sheet = xssWorkbook.GetSheetAt(sNum);
                        string sheetName = xssWorkbook.GetSheetName(sNum);
                        int AutoSimNameRow = new CellReference(xssWorkbook.GetName(TestCaseConstants.AutoSimCellId).RefersToFormula).Row;
                        ObservableCollection<TestCaseDataObj> testModuleDataobjs = new();

                        for (int i = (AutoSimNameRow + 1); i <= sheet.LastRowNum; i++)
                        {
                            IRow row = sheet.GetRow(i);
                            int cellCount = row.LastCellNum;
                            if (row == null) continue;
                            if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                            TestCaseDataObj testCase = new();

                            for (int j = row.FirstCellNum + 3; j < cellCount; j++)
                            {
                                if (row.GetCell(j) != null)
                                {
                                    // Using StringCellValue will get string value from Formular cell
                                    string cellValue = row.GetCell(j).StringCellValue;
                                    if (!string.IsNullOrEmpty(cellValue) && !string.IsNullOrWhiteSpace(cellValue))
                                    {
                                        BulidTestCaseObj(testCase, j, cellValue);
                                    }
                                }
                            }
                            testModuleDataobjs.Add(testCase);
                        }
                        TestCaseDataShop.testModuleDataObjs[sheetName] = testModuleDataobjs;
                    }
                }
            }
            catch (Exception ex)
            {
                _messageDialogService.ShowAlertDialog(ex.Message, "Load Test Module File", MessageIcon.Error);
            }
        }

        /// <summary>
        /// Build Test Case Data object
        /// </summary>
        /// <param name="testCaseDataObj">TestCaseDataObj</param>
        /// <param name="cellIndex">Cell Index</param>
        /// <param name="cValue">Cell Value</param>
        private void BulidTestCaseObj(TestCaseDataObj testCaseDataObj, int cellIndex, string cValue)
        {
            switch (cellIndex)
            {
                case 3: testCaseDataObj.Step = cValue; break;
                case 4: testCaseDataObj.Task = cValue; break;
                case 5: testCaseDataObj.ExpectedResult = cValue; break;
                case 6: testCaseDataObj.PassFail = cValue; break;
                case 7: testCaseDataObj.Note = cValue; break;
                case 8: testCaseDataObj.AppComments = cValue; break;
                case 9: testCaseDataObj.AutoSimFunction = cValue; break;
            }
        }

        /// <summary>
        /// Load Test Log Excel file data
        /// </summary>
        public void LoadTestLogDataObjs()
        {
            IWorkbook xssWorkbook;
            ISheet sheet;
            
            // If Log file opens, after a while AutoSim will notify user to close the Log file
            int counter = 0;
            bool fileLocked = ExcelWriteServiceNPOI.IsFileLocked(new FileInfo(TestLogConfigObj.LogPath));
            while (fileLocked)
            {
                Thread.Sleep(1000);
                counter++;
                if (counter >= 5)
                {
                    _messageDialogService.ShowAlertDialog("The Log File is being used by other process!", "Log File Opening Alert", MessageIcon.Question);
                    counter = 0;
                }
                fileLocked = ExcelWriteServiceNPOI.IsFileLocked(new FileInfo(TestLogConfigObj.LogPath));
            }

            try
            {
                using (var stream = new FileStream(TestLogConfigObj.LogPath, FileMode.Open))
                {
                    stream.Position = 0;
                    xssWorkbook = new XSSFWorkbook(stream);
                    sheet = xssWorkbook.GetSheet(LogConstants.WorksheetName);
                    IRow row = sheet.GetRow(LogConstants.StartRowNum);  // Start from row 2 with object Ids

                    for (int j = row.FirstCellNum + LogConstants.NumOfIgnoreColumns; j < row.LastCellNum; j++)
                    {
                        // Using StringCellValue will get string value from Formular cell
                        string? cellValue = row.GetCell(j) != null ? row.GetCell(j).StringCellValue : "";
                        if (cellValue != "")
                        {
                            TestCaseDataShop.testLogObjectIds.Add(cellValue);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _messageDialogService.ShowAlertDialog(ex.Message, "Load Test Log File", MessageIcon.Error);
            }
        }
    }
}
