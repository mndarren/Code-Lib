using AAH_AutoSim.TestCase.Communication;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System.IO;
using AAH_AutoSim.Server.Dialogs;
using System.Diagnostics;
using System.Windows.Controls;
using System.Net.Mime;
using NPOI.SS.Converter;

using System.Windows.Input;
using AAH_AutoSim.TestCase.Constants;
using AAH_AutoSim.Model.Models.RainbowModels;

namespace AAH_AutoSim.TestCase.ViewModels
{
    public class ExcelEditorViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IMessageDialogService _messageDialogService;
        IWorkbook book;
        private bool Loaded = false;
        private int index;
        public int Index
        {
            get { return index; }
            set { SetProperty(ref index, value); }
        }
        private string? cmdPar;
        public string? CmdPar
        {
            get { return cmdPar; }
            set { SetProperty(ref cmdPar, value); }
        }
        private string? par1;
        public string? Par1
        {
            get { return par1; }
            set { SetProperty(ref par1, value); }
        }
        private string? par2;
        public string? Par2
        {
            get { return par2; }
            set { SetProperty(ref par2, value); }
        }
        private string? par3;
        public string? Par3
        {
            get { return par3; }
            set { SetProperty(ref par3, value); }
        }
        private string? par4;
        public string? Par4
        {
            get { return par4; }
            set { SetProperty(ref par4, value); }
        }
        private string? range;
        public string? Range
        {
            get { return range; }
            set { SetProperty(ref range, value); }
        }
        private string? select1;
        public string? Select1
        {
            get { return select1; }
            set { SetProperty(ref select1, value); }
        }
        private string? select2;
        public string? Select2
        {
            get { return select2; }
            set { SetProperty(ref select2, value); }
        }
        private string? select3;
        public string? Select3
        {
            get { return select3; }
            set { SetProperty(ref select3, value); }
        }
        private string? testCmd;
        public string? TestCmd
        {
            get { return testCmd; }
            set { SetProperty(ref testCmd, value); }
        }
        private string? mB1Addr;
        public string? MB1Addr
        {
            get { return mB1Addr; }
            set { SetProperty(ref mB1Addr, value); }
        }
        private string? mB1FC;
        public string? MB1FC
        {
            get { return mB1FC; }
            set { SetProperty(ref mB1FC, value); }
        }
        private string? mB1Reg;
        public string? MB1Reg
        {
            get { return mB1Reg; }
            set { SetProperty(ref mB1Reg, value); }
        }
        private string? mb2Addr;
        public string? MB2Addr
        {
            get { return mb2Addr; }
            set { SetProperty(ref mb2Addr, value); }
        }
        private string? mb2FC;
        public string? MB2FC
        {
            get { return mb2FC; }
            set { SetProperty(ref mb2FC, value); }
        }
        private string? mb2Reg;
        public string? MB2Reg
        {
            get { return mb2Reg; }
            set { SetProperty(ref mb2Reg, value); }
        }
        public string? MB1 
        {
            get {return MB1Addr + " " + MB1FC + " " + MB1Reg; } 
        }
        public string? MB2
        {
            get { return MB2Addr + " " + MB2FC + " " + MB2Reg; }
        }
        private int AutoSimNameRow;
        private int AutoSimNameCol;
        private DataTable dt;

        public List<string> Commands { get; set; } = new List<string>();
        public List<string> CommandType { get; set; } = new();
		public List<string> RampCommandType { get; set; } = new();
		public List<string> SetValueType { get; set; } = new();
        public List<string> Operator { get; set; } = new();
        public List<string> ObjectNames { get; set; } = new();
        public DelegateCommand EditorWindowLoaded { get; set; }
        public DelegateCommand EditorWindowClosed { get; set; }
        public DelegateCommand InsertRowClick { get; set; }
        public DelegateCommand DeleteRowClick { get; set; }
        public DelegateCommand InsertCmdClick { get; set; }
        public DelegateCommand CmdSelected { get; set; }
        public DelegateCommand Selected1 { get; set; }
        public DelegateCommand Selected2 { get; set; }
        public DelegateCommand Selected3 { get; set; }
        public DelegateCommand UpdatePar { get; set; }

        public ExcelEditorViewModel(IMessageDialogService messageDialogService, IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _messageDialogService = messageDialogService;
            book = GetWorkBook();
            EditorWindowLoaded = new DelegateCommand(EditorWindowLoadedCommand);
            EditorWindowClosed = new DelegateCommand(EditorWindowClosedCommand);
            InsertRowClick = new DelegateCommand(InsertRowCommand);
            DeleteRowClick = new DelegateCommand(DeleteRowCommand);
            InsertCmdClick = new DelegateCommand(InsertCmdCommand);
            CmdSelected = new DelegateCommand(CommandSelectionChanged);
            Selected1 = new DelegateCommand (UpdateTestCommand);
            Selected2 = new DelegateCommand(UpdateTestCommand);
            Selected3 = new DelegateCommand(UpdateTestCommand);
            UpdatePar = new DelegateCommand(UpdateTestCommand);
            dt = new DataTable();
            Loaded = false;
            LoadDefaults();
            ObjectNames = NameIdData.RainbowNameIds.Keys.ToList();
            ObjectNames.Sort();
            dt.ColumnChanged += Dt_ColumnChanged;

        }
        private void CommandSelectionChanged()
        {
            Par1 = Par2 = Par3 = Par4 = "";
            UpdateTestCommand();
        }

        private void InsertCmdCommand()
        {
            if (index > AutoSimNameRow)
            {
                DataRow row = dt.Rows[Index];
                row[AutoSimNameCol] = TestCmd;
                index++;
            }
            else
            {
                _messageDialogService.ShowAlertDialog("Please select a row that is after AutoSim Named row " + AutoSimNameRow.ToString() + " to insert Command.", "Incorrect Row", MessageIcon.Error);
            }
        }

        private void UpdateTestCommand()
        {
            switch (CmdPar)
            {
                case "Run":
                    TestCmd = CmdPar + " " + Par1;
                    break;
                case "Compare":
                    if (Select1 == "MBData")
                    {
                        Par1 = MB1;
                    }
                    if (Select3 == "MBData")
                    {
                        Par2 = MB2;
                    }
                    if (Select2 == "+-")
                    {
                        TestCmd = CmdPar + " " + Select1 + " " + Par1 + " " + Select2 + " " + Range + " " + Select3 + " " + Par2;
                    }
                    else
                    {
                        TestCmd = CmdPar + " " + Select1 + " " + Par1 + " " + Select2 + " " + Select3 + " " + Par2;
                    }
                    break;
                case "Set":
                    if (Select1 == "MBData")
                    {
                        Par1 = MB1;
                    }
                    TestCmd = CmdPar + " " + Select1 + " " + Par1 + " to " + Select2 + " " + Par2;
                    break;
                case "Wait":
                    TestCmd = CmdPar + " " + Par1 + "  sec " + Par2;
                    break;
                case "WaitUntil":
                    if (Select1 == "MBData")
                    {
                        Par1 = MB1;
                    }
                    if (Select3 == "MBData")
                    {
                        Par2 = MB2;
                    }
                    if (Select2 == "+-")
                    {
                        TestCmd = CmdPar + " " + Select1 + " " + Par1 + " " + Select2 + " " + Range + " " + Select3 + " " + Par2 + " Wait " + Par3 + " sec";
                    }
                    else
                    {
                        TestCmd = CmdPar + " " + Select1 + " " + Par1 + " " + Select2 + " " + Select3 + " " + Par2 + " Wait " + Par3 + " sec";
                    }
                    break;
                case "Log Start":
                    TestCmd = CmdPar;
                    break;
                case "Log Delta":
                    TestCmd = CmdPar + " " + Par1 + " sec";
                    break;
                case "Log Add":
                    TestCmd = CmdPar + " " + Par1;
                    break;
                case "Log Off":
                    TestCmd = CmdPar;
                    break;
                case "Config":
                    TestCmd = CmdPar + " " + Par1;
                    break;
                case "Pause":
                    TestCmd = CmdPar + " " + Par1;
                    break;
                case "External Test":
                    TestCmd = CmdPar + " " + Par1;
                    break;
                case "SetOOR":
                    TestCmd = CmdPar + " " + Par1 + " to " + Par2;
                    break;
                case "If":
                    TestCmd = CmdPar + " {" + Par1 + "} Then \n{\n" + Par2 + "\n}\nElse\n{\n" + Par3 +"\n}";
                    break;
                case "Status":
                    TestCmd = CmdPar + " " + Par1;
                    break;
                case "Ramp":
                    TestCmd = CmdPar + " " + Select1 +" " + Par1 + " from " + Par2 + " to " + Par3 + " in " + Par4 + " sec";
                    break;
                default:
                    break;
            }
        }
        private void DeleteRowCommand()
        {
            int indexToRemove = index;
            if (index > AutoSimNameRow)
            {              
                try
                {
                    ISheet sheet = book.GetSheet(TestCaseConstants.TestCaseWorksheetName);
                    IRow irow = sheet.GetRow(indexToRemove);                   
                    if (irow != null)
                    {
                        sheet.RemoveRow(irow);
                    }
                    else
                    {
                        int lastIndex = sheet.LastRowNum;
                        sheet.ShiftRows(indexToRemove + 1, lastIndex,-1);
                    }
                    SaveWorkBook(book);
                    dt.Rows.RemoveAt(Index);
                }
                catch (Exception ex)
                {
                    _messageDialogService.ShowAlertDialog(ex.Message, "Exception Error", MessageIcon.Error);
                }
            }
            else
            {
                _messageDialogService.ShowAlertDialog("Please select a row that is after AutoSim Named row " + AutoSimNameRow.ToString() + " to delete.", "Incorrect Row", MessageIcon.Error);
            }
        }

        private void InsertRowCommand()
        {
            if (index > AutoSimNameRow)
            {
                try
                {
                    ISheet sheet = book.GetSheet(TestCaseConstants.TestCaseWorksheetName);
                    IRow newRow = sheet.CreateRow(index);
                    for (int i = 0; i < AutoSimNameCol; i++)
                    {
                        ICell cell = newRow.CreateCell(i);
                        cell.SetCellValue("");
                    }
                    SaveWorkBook(book);
                    DataRow row = dt.NewRow();
                    dt.Rows.InsertAt(row, index);
                }
                catch (Exception ex)
                {
                    _messageDialogService.ShowAlertDialog(ex.Message, "Exception Error", MessageIcon.Error);
                }
            }
            else
            {
                _messageDialogService.ShowAlertDialog("Please select a row that is after AutoSim Named row " + AutoSimNameRow.ToString() + " to delete.", "Incorrect Row", MessageIcon.Error);
            }
        }
        private static IWorkbook GetWorkBook()
        {
            IWorkbook book;
            using (FileStream fs = new FileStream(TestCaseDataShop.FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                book = new XSSFWorkbook(fs);
                book.MissingCellPolicy = MissingCellPolicy.RETURN_NULL_AND_BLANK;
                fs.Close();
            }
            return book;
        }
        private void SaveWorkBook(IWorkbook book)
        {
            File.Delete(TestCaseDataShop.FileName);
            using (FileStream file = new FileStream(TestCaseDataShop.FileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
            {
                book.Write(file, true);
                file.Close();
            }
        }
        private void LoadDefaults()
        {
            Commands.Add("Run");
            Commands.Add("Compare");
            Commands.Add("Set");
            Commands.Add("Wait");
            Commands.Add("WaitUntil");
            Commands.Add("Log Start");
            Commands.Add("Log Delta");
            Commands.Add("Log Add");
            Commands.Add("Log Off");
            Commands.Add("Config");
            Commands.Add("Pause");
            Commands.Add("External Test");
            Commands.Add("SetOOR");
            Commands.Add("If");
            Commands.Add("Status");
            Commands.Add("Ramp");

            
            CommandType.Add("objectId");
            CommandType.Add("objectName");
            CommandType.Add("MBData");
            CommandType.Add("MemoryName");
            CommandType.Add("Value");
            CommandType.Add("StringValue");

			RampCommandType.Add("objectId");
			RampCommandType.Add("objectName");
			RampCommandType.Add("MBData");
			RampCommandType.Add("MemoryName");

			SetValueType = new();
            SetValueType.Add("Value");
            SetValueType.Add("StringValue");
            SetValueType.Add("MemoryName");

            Operator = new();
            Operator.Add(CompareConstants.CompareOperands.eq);
            Operator.Add(CompareConstants.CompareOperands.gt);
            Operator.Add(CompareConstants.CompareOperands.gteq);
            Operator.Add(CompareConstants.CompareOperands.lt);
            Operator.Add(CompareConstants.CompareOperands.lteq);
            Operator.Add(CompareConstants.CompareOperands.ne);
            Operator.Add(CompareConstants.CompareOperands.range);

        }

        private void Dt_ColumnChanged(object sender, DataColumnChangeEventArgs e)
        {
            if (Loaded)
            {
                int columnIdx = dt.Columns.IndexOf(e.Column);
                int rowIdx = dt.Rows.IndexOf(e.Row);
                string newValue = (string)e.ProposedValue;
                try
                {
                    ISheet sheet = book.GetSheet(TestCaseConstants.TestCaseWorksheetName);
                    IRow row = sheet.GetRow(rowIdx) ?? sheet.CreateRow(rowIdx);
                    ICell cell = row.GetCell(columnIdx) ?? row.CreateCell(columnIdx);
                    cell.SetCellValue(newValue);

                    File.Delete(TestCaseDataShop.FileName);
                    using (FileStream file = new FileStream(TestCaseDataShop.FileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
                    {

                        book.Write(file, true);
                        file.Close();
                    }
                }
                catch (Exception ex)
                {
                    _messageDialogService.ShowAlertDialog(ex.Message, "Exception Error", MessageIcon.Error);
                }
            }
        }
        private void EditorWindowLoadedCommand()
        {
            Loaded = true;
        }
        private void EditorWindowClosedCommand()
        {
            Loaded = false;
        }

        public DataView Data
        {
            get
            {
                try
                {
                    ISheet sheet = book.GetSheet(TestCaseConstants.TestCaseWorksheetName);
                    dt.Columns.Add("A");
                    dt.Columns.Add("B");
                    dt.Columns.Add("C");
                    dt.Columns.Add("D");
                    dt.Columns.Add("E");
                    dt.Columns.Add("F");
                    dt.Columns.Add("G");
                    dt.Columns.Add("H");
                    dt.Columns.Add("I");
                    dt.Columns.Add("J");

                    AutoSimNameRow = new CellReference(book.GetName(TestCaseConstants.AutoSimCellId).RefersToFormula).Row;
                    AutoSimNameCol = new CellReference(book.GetName(TestCaseConstants.AutoSimCellId).RefersToFormula).Col;                    

                    for (int i = 0; i <= sheet.LastRowNum; i++)
                    {
                        DataRow dr = dt.NewRow();
                        if (sheet.GetRow(i) != null)
                        {
                            IRow row = sheet.GetRow(i);
                            for (int j = 0; j < row.Cells.Count(); j++)
                            {
                                if (row.Cells.Count > dt.Columns.Count)
                                {
                                    dt.Columns.Add();
                                }
                                var celldata = string.Empty;
                                switch (row.Cells[j].CellType)
                                {
                                    case CellType.Numeric:
                                        celldata = row.Cells[j].NumericCellValue.ToString();
                                        break;
                                    case CellType.String:
                                        celldata = row.Cells[j].StringCellValue;
                                        break;
                                    case CellType.Formula:
                                        switch (row.Cells[j].CachedFormulaResultType)
                                        {
                                            case CellType.Numeric:
                                                celldata = row.Cells[j].NumericCellValue.ToString();
                                                break;
                                            case CellType.String:
                                                celldata = row.Cells[j].StringCellValue;
                                                break;
                                            case CellType.Formula:
                                                celldata = row.Cells[j].ToString();
                                                break;
                                            default:
                                                celldata = row.Cells[j].ToString();
                                                break;
                                        }
                                        break;
                                    default:
                                        celldata = row.Cells[j].ToString();
                                        break;
                                }
                                if (celldata != null)
                                {
                                    dr[j] = celldata.ToString();
                                }
                                else
                                {
                                    dr[j] = "";
                                }
                            }
                        }
                        dt.Rows.Add(dr);
                        dt.AcceptChanges();
                    }
                    return dt.DefaultView;
                }
                catch (Exception ex)
                {
                    _messageDialogService.ShowAlertDialog(ex.Message, "Exception Error", MessageIcon.Error);
                }
                return dt.DefaultView;
            }
        }
    }
}
