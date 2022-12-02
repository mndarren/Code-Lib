using AAH_AutoSim.Server.Dialogs;
using AAH_AutoSim.TestCase.Constants;
using System.Text.RegularExpressions;

namespace AAH_AutoSim.TestCase.Models
{
    public abstract class BaseCmdObj
    {
        protected string _pattern = "";
        protected string _command = "";
        protected TestCaseDataObj tcObj;
        protected readonly IMessageDialogService _messageDialogService;

        public BaseCmdObj(IMessageDialogService messageDialogService, TestCaseDataObj tcObj)
        {
            _messageDialogService = messageDialogService;
            _command = tcObj.AutoSimFunction;
            this.tcObj = tcObj;
            SetPattern();
            AssignValues();
        }

        // Set command specific pattern
        protected abstract void SetPattern();

        // Assign values to each field
        protected abstract void AssignValues();

        protected Match GetMatch()
        {
            Regex regex = new Regex(_pattern, RegexOptions.IgnoreCase);
            return regex.Match(_command);
        }
    }
}
