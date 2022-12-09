using AAH_AutoSim.Server.Dialogs;
using AAH_AutoSim.TestCase.Constants;
using AAH_AutoSim.TestCase.Models;
using AAH_AutoSim.Model.Models;
using Newtonsoft.Json;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using AAH_AutoSim.Server.Config;
using System.Windows;

namespace AAH_AutoSim.TestCase.Communication
{
    /// <summary>
    /// This class is used to generate Run test cases result Excel file.
    /// NPOI Package reference:
    /// https://www.thecodebuzz.com/read-and-write-excel-file-in-net-core-using-npoi/
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ExcelWriteServiceNPOI
    {
        private readonly IMessageDialogService _messageDialogService;

		private const int StepIndex = 0;
		private const int TaskIndex = 1;
		private const int ExpectedResultIndex = 2;
		private const int PassFailIndex = 3;
		public const int NoteIndex = 4;
		private const int AppCommentsIndex = 5;
		private const int AutoSimFunctionIndex = 6;
        private const int TimestampIndex = 7;

        private const int factor = 30;

        public ExcelWriteServiceNPOI(IMessageDialogService messageDialogService)
        {
            _messageDialogService = messageDialogService;
        }

		/// <summary>
		/// Create run test case result Excel file
		/// </summary>
		public void CreateTestResultFile(TestCaseResults results)
		{
            try
            {
                // Lets converts our object data to Datatable for a simplified logic.
                // Datatable is most easy way to deal with complex datatypes for easy reading and formatting.
                DataTable table = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(TestCaseDataShop.testCaseDataObjs), (typeof(DataTable)));
                var memoryStream = new MemoryStream();
                string origFileName = TestCaseDataShop.FileName.Substring(0, TestCaseDataShop.FileName.Length - 5);
                string savedFileName = origFileName + "_Result_" + DateTime.Now.ToString("MM.dd.yyyy_HH.mm.ss") + "_" + TestCaseDataShop.TestStatus + ".xlsx";

                using (var fs = new FileStream(savedFileName, FileMode.Create, FileAccess.Write))
                {
                    IWorkbook workbook = new XSSFWorkbook();
                    // Add the original worksheets from the Test Case Excel file
                    AddWorksheets2ResultFile(workbook);
                    ISheet excelSheet = workbook.CreateSheet(TestCaseConstants.ResultWorksheetName);

					// Add Summary To The Test Results File
					IFont font = workbook.CreateFont();
					font.Color = XSSFFont.DEFAULT_FONT_COLOR;
					font.IsBold = true;
					font.IsItalic = false;

					ICellStyle boldStyle = workbook.CreateCellStyle();
					boldStyle.SetFont(font);

                    ICellStyle wrapTextStyle = workbook.CreateCellStyle();
                    wrapTextStyle.WrapText = true;

                    IFont redFont = workbook.CreateFont();
                    redFont.Color = IndexedColors.Red.Index;
                    redFont.IsBold = true;
                    ICellStyle redStyle = workbook.CreateCellStyle();
                    redStyle.SetFont(redFont);
                    IFont greenFont = workbook.CreateFont();
                    greenFont.Color = IndexedColors.Green.Index;
                    greenFont.IsBold = true;
                    ICellStyle greenStyle = workbook.CreateCellStyle();
                    greenStyle.SetFont(greenFont);

                    int rowIndex = 0;
                    Dictionary<string, string> resultReports = new Dictionary<string, string>() 
                    {
                        {"Total Test Commands:", results.totalRunCmdCount.ToString() },
                        {"Execution Time:",      results.executionTime },
                        {"Passed Commands:",     results.passedCmdCount.ToString()},
                        {"Failed Commands:",     results.failedCmdCount.ToString()},
                        {"Skipped Commands:",    results.getSkippedCmdCount().ToString()}
                    };
                    foreach (KeyValuePair<string, string> entry in resultReports)
                    {
                        IRow completedCommandsRow = excelSheet.CreateRow(rowIndex++);
					    completedCommandsRow.CreateCell(0).SetCellValue(entry.Key);
					    completedCommandsRow.GetCell(0).CellStyle = boldStyle;
					    completedCommandsRow.CreateCell(1).SetCellValue(entry.Value);
                    }

					// Create empty row to separate summary from the test results table
					IRow emptyRow = excelSheet.CreateRow(rowIndex++);

					List<string> columns = new List<string>();  // Contain column names
                    IRow row = excelSheet.CreateRow(rowIndex++);
                    int columnIndex = 0;
                    // Create the header row
                    foreach (DataColumn column in table.Columns)
                    {
                        columns.Add(column.ColumnName);
                        row.CreateCell(columnIndex).SetCellValue(column.ColumnName);
						row.GetCell(columnIndex).CellStyle = boldStyle;
						columnIndex++;
                    }

					foreach (DataRow dsrow in table.Rows)
                    {
                        row = excelSheet.CreateRow(rowIndex++);
                        int cellIndex = 0;
                        // Generate new row for new Excel file, and update Test Result Data
                        TestCaseDataObj testCaseDataObj = new TestCaseDataObj();
                        foreach (String col in columns)
                        {
                            string cellValue = dsrow[col].ToString();
                            row.CreateCell(cellIndex).SetCellValue(cellValue);
                            BulidTestCaseObj(testCaseDataObj, cellIndex, cellValue);
							if (cellIndex == PassFailIndex)
							{
								if (cellValue == "P")
								{
									row.GetCell(cellIndex).CellStyle = greenStyle;
								}
								else if (cellValue == "F")
								{
									row.GetCell(cellIndex).CellStyle = redStyle;
								}
							}
							else if (cellIndex == TaskIndex || cellIndex == AppCommentsIndex)
							{
								row.GetCell(cellIndex).CellStyle = wrapTextStyle;
							}

							cellIndex++;
						}

                        // Add Test Module test result if Auto Sim Function contains "Run"
                        string command = dsrow[columns[AutoSimFunctionIndex]].ToString();
                        if (command.ToLower().Trim().StartsWith(TestCaseConstants.TestCaseTypeKeyword.RunModule))
                        {
                            string moduleName = command.Substring(command.IndexOf(" ") + 1).Trim();
                            ObservableCollection<TestCaseDataObj> testModuleDataObjs = TestCaseDataShop.testModuleDataObjs[moduleName];
                            DataTable moduleTable = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(testModuleDataObjs), (typeof(DataTable)));

                            foreach (DataRow tmRow in moduleTable.Rows)
                            {
                                row = excelSheet.CreateRow(rowIndex++);
                                int tmCellIndex = 0;
                                testCaseDataObj = new TestCaseDataObj();
                                foreach (String col in columns)
                                {
                                    string cValue = tmRow[col].ToString();
                                    row.CreateCell(tmCellIndex).SetCellValue(cValue);
                                    BulidTestCaseObj(testCaseDataObj, tmCellIndex, cValue);
									if (tmCellIndex == PassFailIndex)
									{
										if (cValue == "P")
										{
											row.GetCell(tmCellIndex).CellStyle = greenStyle;
										}
										else if (cValue == "F")
										{
											row.GetCell(tmCellIndex).CellStyle = redStyle;
										}
									}
									else if (tmCellIndex == TaskIndex || tmCellIndex == AppCommentsIndex)
									{
										row.GetCell(tmCellIndex).CellStyle = wrapTextStyle;
									}
									tmCellIndex++;
                                }
                            }
                        }
                    }

                    // Format columns
					excelSheet.AutoSizeColumn(StepIndex);
					excelSheet.SetColumnWidth(TaskIndex, 450 * factor);
					excelSheet.SetColumnWidth(ExpectedResultIndex, 200 * factor);
					excelSheet.AutoSizeColumn(PassFailIndex);
					excelSheet.SetColumnWidth(NoteIndex, 200 * factor);
					excelSheet.SetColumnWidth(AppCommentsIndex, 450 * factor);
					excelSheet.AutoSizeColumn(AutoSimFunctionIndex);
					excelSheet.AutoSizeColumn(TimestampIndex);

					workbook.Write(fs, true);
                }
            }
            catch (Exception ex)
            {
                _messageDialogService.ShowAlertDialog(ex.Message, "Generate Test Result File", MessageIcon.Error);
            }
        }
		
        /// <summary>
		/// Copy all worksheets to the new test result Excel file.
		/// </summary>
		/// <param name="workbookMerged"></param>
		private void AddWorksheets2ResultFile(IWorkbook workbookMerged)
        {
            using (FileStream fs = new FileStream(TestCaseDataShop.FileName, FileMode.Open, FileAccess.Read))
            {
                XSSFWorkbook workbook = new XSSFWorkbook(fs);

                for (int i = 0; i < workbook.NumberOfSheets; i++)
                {
                    // Copy worksheet without formula
                    ((XSSFSheet)workbook.GetSheetAt(i)).CopyTo(workbookMerged, workbook.GetSheetName(i), true, false);
                }
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
				case StepIndex: testCaseDataObj.Step = cValue; break;
				case TaskIndex: testCaseDataObj.Task = cValue; break;
				case ExpectedResultIndex: testCaseDataObj.ExpectedResult = cValue; break;
				case PassFailIndex: testCaseDataObj.PassFail = cValue; break;
				case NoteIndex: testCaseDataObj.Note = cValue; break;
				case AppCommentsIndex: testCaseDataObj.AppComments = cValue; break;
				case AutoSimFunctionIndex: testCaseDataObj.AutoSimFunction = cValue; break;
				case TimestampIndex: testCaseDataObj.Timestamp = cValue; break;
			}
        }

        /// <summary>
        /// Write out Log data into Log Excel file
        /// </summary>
        /// <param name="logData2Write"></param>
        public void WriteLogData(Dictionary<string, List<string>> logData2Write)
        {
            XSSFWorkbook wb = null;
            CheckFileLock();

            try
            {
                using (FileStream fs = new FileStream(TestLogConfigObj.LogPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    fs.Position = 0;
                    wb = new XSSFWorkbook(fs);
                    wb.MissingCellPolicy = MissingCellPolicy.RETURN_NULL_AND_BLANK;
                    fs.Close();
                }

                // We must delete the old file otherwise the log Excel file cannot open with format changed error
                File.Delete(TestLogConfigObj.LogPath);
                using (FileStream fs = new FileStream(TestLogConfigObj.LogPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
                {
                    fs.Position = 0; 
                    if (wb != null)
                    {
                        ISheet ws = wb.GetSheet(LogConstants.WorksheetName);
                        if (ws != null)
                        {
                            foreach (KeyValuePair<string, List<string>> entry in logData2Write)
                            {
                                int rowIndex = ws.LastRowNum + 1;
                                IRow row = ws.GetRow(rowIndex) ?? ws.CreateRow(rowIndex);
                                ICell cell0 = row.GetCell(0) ?? row.CreateCell(0);
                                cell0.SetCellValue(entry.Key);
                                int i = 1;
                                foreach (string IdValue in entry.Value)
                                {
                                    ICell aCell = row.CreateCell(i);
                                    aCell.SetCellValue(IdValue);
                                    i++;
                                }
                            }
                            wb.Write(fs);
                        }
                    }
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                _messageDialogService.ShowAlertDialog($"{ex.Message}", "Write Test Log Data", MessageIcon.Error);
            }
            TestCaseDataShop.ClearLogData();
        }

        /// <summary>
        /// Add Object Name and Object Id in the Log file
        /// </summary>
        /// <param name="objectName">Object Name</param>
        /// <param name="objectId">Object Id</param>
        public void AddPoint2Log(string objectName, string objectId)
        {
            XSSFWorkbook wb = null;
            CheckFileLock();

            try
            {
                using (FileStream fs = new FileStream(TestLogConfigObj.LogPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    fs.Position = 0;
                    wb = new XSSFWorkbook(fs);
                    wb.MissingCellPolicy = MissingCellPolicy.RETURN_NULL_AND_BLANK;
                    fs.Close();
                }

                // We must delete the old file otherwise the log Excel file cannot open with format changed error
                File.Delete(TestLogConfigObj.LogPath);
                using (FileStream fs = new FileStream(TestLogConfigObj.LogPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
                {
                    fs.Position = 0;
                    if (wb != null)
                    {
                        ISheet ws = wb.GetSheet(LogConstants.WorksheetName);
                        if (ws != null)
                        {
                            // Add Object Name
                            IRow row = ws.GetRow(0);
                            ICell lastCell = row.CreateCell(row.LastCellNum);
                            lastCell.SetCellValue(objectName);
                            // Add Object Id
                            row = ws.GetRow(1);
                            lastCell = row.CreateCell(row.LastCellNum);
                            lastCell.SetCellValue(objectId);
                            
                            wb.Write(fs);
                        }
                    }
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                _messageDialogService.ShowAlertDialog($"{ex.Message}", "Add Object Id to Log File", MessageIcon.Error);
            }
        }
        /// <summary>
        /// Check if the Log file is locked by other process
        /// </summary>
        public void CheckFileLock()
        {
            // If Log file opens, after a while AutoSim will notify user to close the Log file
            int counter = 0;
            bool fileLocked = IsFileLocked(new FileInfo(TestLogConfigObj.LogPath));
            while (fileLocked)
            {
                Thread.Sleep(1000);
                counter++;
                if (counter >= 5)
                {
                    _messageDialogService.ShowAlertDialog("The Log File is being used by other process!", "Log File Opening Alert", MessageIcon.Question);
                    counter = 0;
                }
                fileLocked = IsFileLocked(new FileInfo(TestLogConfigObj.LogPath));
            }
        }
        /// <summary>
        /// Check if the file is being used currently
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool IsFileLocked(FileInfo file)
        {
            try
            {
                using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    stream.Close();
                }
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }

            //file is not locked
            return false;
        }
    }
}
