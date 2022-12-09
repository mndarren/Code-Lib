using AAH_AutoSim.Server.Dialogs;
using AAH_AutoSim.TestCase.Communication;
using AAH_AutoSim.TestCase.Models;
using AAH_AutoSim.TestCase.Util;
using Prism.Events;
using System;
using System.Collections.Generic;
using static AAH_AutoSim.Model.Constants.ObjectType4IdName;
using static AAH_AutoSim.TestCase.Constants.CompareConstants;
using static AAH_AutoSim.TestCase.Constants.TestCaseConstants;

namespace AAH_AutoSim.TestCase.Execution
{
    public class CompareCmdExecution : BaseCmdExecution
    {
        private UtilExecution _utilExecution;

        public CompareCmdExecution(IEventAggregator eventAggregator, IMessageDialogService messageDialogService) : base(eventAggregator, messageDialogService)
        {
            _utilExecution = new UtilExecution(eventAggregator, messageDialogService);
        }

        /// <summary>
        /// Run Compare Function if Auto Sim Function starts with "Compare"
        /// Notes:
        /// If type is objectId or objectName, then we use the presentValue to compare.
        /// If type is MbData, then we use the returned value to compare.
        /// If type is MemoryName, then we use the stored value to compare.
        /// If {type} is Value or StringValue, then we use the direct {value} to compare.
        /// </summary>
        /// <param name="tcObj">Test Case Object</param>
        override protected void ExecuteCommand()
		{
            Dictionary<ObjType, bool> isType = new Dictionary<ObjType, bool>() {
                { ObjType.FLOAT, false },
                { ObjType.STRING, false },
                { ObjType.WORD, false },
                { ObjType.BOOL, false },
                { ObjType.ULONG, false },
            };

            bool isPass = false;
            string addedComments = "";

            try
            {
                CompareCmdObj cmpObj = new CompareCmdObj(_messageDialogService, tcObj);
                // If the command syntax error, just update and return.
                if (tcObj.PassFail == "F")
                {
					tcObj.SetTimestamp();
					return;
                }

                // Check if both sides types are the same
                if (!cmpObj.IsAndOrOperatorCMD && !AreBothSidesSameType(ref isType, cmpObj))
                {
                    addedComments = "Both sides are not the same type.";
                    isPass = false;
                }
                else if (cmpObj.IsAndOrOperatorCMD)
                {
                    // Check types
                    cmpObj.CmpTypeB = AutoSimFuncObjTypes.Value;
                    cmpObj.CmpValueB = cmpObj.AndOrOperand1;
                    if (!AreBothSidesSameType(ref isType, cmpObj))
                    {
                        addedComments = "Both sides are not the same type.";
                        isPass = false;
                    }
                    cmpObj.CmpValueB = cmpObj.AndOrOperand2;
                    if (!AreBothSidesSameType(ref isType, cmpObj))
                    {
                        addedComments = "Both sides are not the same type.";
                        isPass = false;
                    }
                    // change them back
                    cmpObj.CmpTypeB = "";
                    cmpObj.CmpValueB = "";
                }
                if (isType[ObjType.FLOAT])
                {
                    if (!cmpObj.IsAndOrOperatorCMD)
                    {
                        isPass = CompareFloatType(cmpObj, ref addedComments);
                    }
                    else if (cmpObj.AndOrOperator.Equals(CompareOperands.or))
                    {
                        cmpObj.CmpTypeB = AutoSimFuncObjTypes.Value;
                        cmpObj.CmpValueB = cmpObj.AndOrOperand1;
                        isPass = CompareFloatType(cmpObj, ref addedComments);
                        if (!isPass)
                        {
                            cmpObj.CmpValueB = cmpObj.AndOrOperand2;
                            isPass = CompareFloatType(cmpObj, ref addedComments);
                        }
                        // change them back
                        cmpObj.CmpTypeB = "";
                        cmpObj.CmpValueB = "";
                    }
                    else if (cmpObj.AndOrOperator.Equals(CompareOperands.and))
                    {
                        float leftValue = _utilExecution.getFloatCmdValue(cmpObj.CmpTypeA.ToLower(), cmpObj.CmpValueA, tcObj);
                        if (leftValue > TCParser.ToFloat(cmpObj.AndOrOperand1) && leftValue < TCParser.ToFloat(cmpObj.AndOrOperand2))
                        {
                            isPass = true;
                            addedComments = $" {leftValue} in ({cmpObj.AndOrOperand1}, {cmpObj.AndOrOperand2})";
                        }
                        else
                        {
                            isPass = false;
                            addedComments = $" {leftValue} NOT in ({cmpObj.AndOrOperand1}, {cmpObj.AndOrOperand2})";
                        }
                    }
                }
                else if (isType[ObjType.STRING])
                {
                    string leftValue = _utilExecution.getStringCmdValue(cmpObj.CmpTypeA.ToLower(), cmpObj.CmpValueA, tcObj);
                    string rightValue = _utilExecution.getStringCmdValue(cmpObj.CmpTypeB.ToLower(), cmpObj.CmpValueB, tcObj);

                    isPass = getCmpResult(leftValue, cmpObj, rightValue);
                    addedComments = " " + $"{leftValue} {cmpObj.CmpOperand} {rightValue}";
                    // Check 2 Objects to the same Value
                    if (!isPass && cmpObj.Is2ObjectIdsCMD)
                    {
                        string ZValue = _utilExecution.getStringCmdValue(cmpObj.CmpTypeZ, cmpObj.CmpValueZ, tcObj);
                        isPass = getCmpResult(ZValue, cmpObj, rightValue);
                        addedComments = addedComments + (isPass ? $" {ZValue} {cmpObj.CmpOperand} {rightValue}"
                        : $" {ZValue} !{cmpObj.CmpOperand} {rightValue}");
                    }
                }
                else if (isType[ObjType.WORD])
                {
                    if (!cmpObj.IsAndOrOperatorCMD)
                    {
                        isPass = CompareWordType(cmpObj, ref addedComments);
                    }
                    else if (cmpObj.AndOrOperator.Equals(CompareOperands.or))
                    {
                        cmpObj.CmpTypeB = AutoSimFuncObjTypes.Value;
                        cmpObj.CmpValueB = cmpObj.AndOrOperand1;
                        isPass = CompareWordType(cmpObj, ref addedComments);
                        if (!isPass)
                        {
                            cmpObj.CmpValueB = cmpObj.AndOrOperand2;
                            isPass = CompareWordType(cmpObj, ref addedComments);
                        }
                        // change them back
                        cmpObj.CmpTypeB = "";
                        cmpObj.CmpValueB = "";
                    }
                    else if (cmpObj.AndOrOperator.Equals(CompareOperands.and))
                    {
                        (int, string) leftValue = _utilExecution.getWordCmdValue(cmpObj.CmpTypeA, cmpObj.CmpValueA, tcObj);
                        if (leftValue.Item1 > TCParser.ToInt(cmpObj.AndOrOperand1) && leftValue.Item1 < TCParser.ToInt(cmpObj.AndOrOperand2))
                        {
                            isPass = true;
                            addedComments = $" {leftValue.Item1} in ({cmpObj.AndOrOperand1}, {cmpObj.AndOrOperand2})";
                        }
                        else
                        {
                            isPass = false;
                            addedComments = $" {leftValue.Item1} NOT in ({cmpObj.AndOrOperand1}, {cmpObj.AndOrOperand2})";
                        }
                    }
                }
                else if (isType[ObjType.BOOL])
                {
                    bool leftValue = _utilExecution.getBoolCmdValue(cmpObj.CmpTypeA, cmpObj.CmpValueA, tcObj);
                    bool rightValue = _utilExecution.getBoolCmdValue(cmpObj.CmpTypeB, cmpObj.CmpValueB, tcObj);

                    isPass = getCmpResult(leftValue, cmpObj, rightValue);
                    addedComments = isPass ? $" {leftValue} {cmpObj.CmpOperand} {rightValue}"
                        : $" {leftValue} !{cmpObj.CmpOperand} {rightValue}";
                    // Check 2 Objects to the same Value
                    if (!isPass && cmpObj.Is2ObjectIdsCMD)
                    {
                        bool ZValue = _utilExecution.getBoolCmdValue(cmpObj.CmpTypeZ, cmpObj.CmpValueZ, tcObj);
                        isPass = getCmpResult(ZValue, cmpObj, rightValue);
                        addedComments = addedComments + (isPass ? $" {leftValue} {cmpObj.CmpOperand} {rightValue}"
                        : $" {leftValue} !{cmpObj.CmpOperand} {rightValue}");
                    }
                }
                else if (isType[ObjType.ULONG])
                {
                    if (!cmpObj.IsAndOrOperatorCMD)
                    {
                        isPass = CompareULongType(cmpObj, ref addedComments);
                    }
                    else if (cmpObj.AndOrOperator.Equals(CompareOperands.or))
                    {
                        cmpObj.CmpTypeB = AutoSimFuncObjTypes.Value;
                        cmpObj.CmpValueB = cmpObj.AndOrOperand1;
                        isPass = CompareULongType(cmpObj, ref addedComments);
                        if (!isPass)
                        {
                            cmpObj.CmpValueB = cmpObj.AndOrOperand2;
                            isPass = CompareULongType(cmpObj, ref addedComments);
                        }
                        // change them back
                        cmpObj.CmpTypeB = "";
                        cmpObj.CmpValueB = "";
                    }
                    else if (cmpObj.AndOrOperator.Equals(CompareOperands.and))
                    {
                        ulong leftValue = _utilExecution.getULongCmdValue(cmpObj.CmpTypeA.ToLower(), cmpObj.CmpValueA, tcObj);
                        if (leftValue > TCParser.ToUlong(cmpObj.AndOrOperand1) && leftValue < TCParser.ToUlong(cmpObj.AndOrOperand2))
                        {
                            isPass = true;
                            addedComments = $" {leftValue} in ({cmpObj.AndOrOperand1}, {cmpObj.AndOrOperand2})";
                        }
                        else
                        {
                            isPass = false;
                            addedComments = $" {leftValue} NOT in ({cmpObj.AndOrOperand1}, {cmpObj.AndOrOperand2})";
                        }
                    }
                }
                else
                {
                    addedComments = "Unexpected object type";
                    isPass = false;
                }
            }
            catch (Exception ex)
            {
                addedComments = " " + ex.Message;
                isPass = false;
            }

			// Add App Comments
			tcObj.AppComments = tcObj.AppComments + addedComments;

			tcObj.PassFail = isPass ? "P" : "F";
			tcObj.SetTimestamp();

		}
        /// <summary>
        /// Compare ULong type points
        /// </summary>
        /// <param name="cmpObj"></param>
        /// <param name="addedComments"></param>
        /// <returns></returns>
        private bool CompareULongType(CompareCmdObj cmpObj, ref string addedComments)
        {
            ulong leftValue = _utilExecution.getULongCmdValue(cmpObj.CmpTypeA.ToLower(), cmpObj.CmpValueA, tcObj);
            ulong rightValue = _utilExecution.getULongCmdValue(cmpObj.CmpTypeB.ToLower(), cmpObj.CmpValueB, tcObj);

            bool isPass = getCmpResult(leftValue, cmpObj, rightValue);
            addedComments = addedComments + (isPass ? $" {leftValue} {cmpObj.CmpOperand} {rightValue}"
                : $" {leftValue} !{cmpObj.CmpOperand} {rightValue}");
            // Check 2 Objects to the same Value
            if (!isPass && cmpObj.Is2ObjectIdsCMD)
            {
                ulong ZValue = _utilExecution.getULongCmdValue(cmpObj.CmpTypeZ, cmpObj.CmpValueZ, tcObj);
                isPass = getCmpResult(ZValue, cmpObj, rightValue);
                addedComments = addedComments + (isPass ? $" {ZValue} {cmpObj.CmpOperand} {rightValue}"
                : $" {ZValue} !{cmpObj.CmpOperand} {rightValue}");
            }
            return isPass;
        }
        /// <summary>
        /// Compare Float type points
        /// </summary>
        /// <param name="cmpObj"></param>
        /// <param name="addedComments"></param>
        /// <returns></returns>
        private bool CompareFloatType(CompareCmdObj cmpObj, ref string addedComments)
        {
            bool isPass = false;
            float leftValue = _utilExecution.getFloatCmdValue(cmpObj.CmpTypeA.ToLower(), cmpObj.CmpValueA, tcObj);
            float rightValue = _utilExecution.getFloatCmdValue(cmpObj.CmpTypeB.ToLower(), cmpObj.CmpValueB, tcObj);

            isPass = getCmpResult(leftValue, cmpObj, rightValue);
            addedComments = addedComments + (isPass ? $" {leftValue} {cmpObj.CmpOperand} {rightValue}"
                : $" {leftValue} !{cmpObj.CmpOperand} {rightValue}");
            // Check 2 Objects to the same Value
            if (!isPass && cmpObj.Is2ObjectIdsCMD)
            {
                float ZValue = _utilExecution.getFloatCmdValue(cmpObj.CmpTypeZ, cmpObj.CmpValueZ, tcObj);
                isPass = getCmpResult(ZValue, cmpObj, rightValue);
                addedComments = addedComments + (isPass ? $" {ZValue} {cmpObj.CmpOperand} {rightValue}"
                : $" {ZValue} !{cmpObj.CmpOperand} {rightValue}");
            }
            return isPass;
        }
        /// <summary>
        /// Compare WORD type points
        /// </summary>
        /// <param name="cmpObj"></param>
        /// <param name="addedComments"></param>
        /// <returns></returns>
        private bool CompareWordType(CompareCmdObj cmpObj, ref string addedComments)
        {
            bool isPass = false;
            (int, string) leftValue = _utilExecution.getWordCmdValue(cmpObj.CmpTypeA.ToLower(), cmpObj.CmpValueA, tcObj);
            (int, string) rightValue = _utilExecution.getWordCmdValue(cmpObj.CmpTypeB.ToLower(), cmpObj.CmpValueB, tcObj);

            if (leftValue.Item1 != ErrorValueInt && rightValue.Item1 != ErrorValueInt)
            {
                isPass = getCmpResult(leftValue.Item1, cmpObj, rightValue.Item1);
                addedComments = addedComments + (isPass ? $" {leftValue.Item1} {cmpObj.CmpOperand} {rightValue.Item1}"
                    : $" {leftValue.Item1} !{cmpObj.CmpOperand} {rightValue.Item1}");
                // Check 2 Objects to the same Value
                if (!isPass && cmpObj.Is2ObjectIdsCMD)
                {
                    (int, string) ZValue = _utilExecution.getWordCmdValue(cmpObj.CmpTypeZ, cmpObj.CmpValueZ, tcObj);
                    isPass = getCmpResult(ZValue.Item1, cmpObj, rightValue.Item1);
                    addedComments = addedComments + (isPass ? $" {ZValue.Item1} {cmpObj.CmpOperand} {rightValue.Item1}"
                    : $" {ZValue.Item1} !{cmpObj.CmpOperand} {rightValue.Item1}");
                }
            }
            else if (leftValue.Item2 != ErrorValueStr && rightValue.Item2 != ErrorValueStr)
            {

                isPass = getCmpResult(leftValue.Item2, cmpObj, rightValue.Item2);
                addedComments = isPass ? $" {leftValue.Item2} {cmpObj.CmpOperand} {rightValue.Item2}"
                    : $" {leftValue.Item2} !{cmpObj.CmpOperand} {rightValue.Item2}";
                // Check 2 Objects to the same Value
                if (!isPass && cmpObj.Is2ObjectIdsCMD)
                {
                    (int, string) ZValue = _utilExecution.getWordCmdValue(cmpObj.CmpTypeZ, cmpObj.CmpValueZ, tcObj);
                    isPass = getCmpResult(ZValue.Item2, cmpObj, rightValue.Item2);
                    addedComments = addedComments + (isPass ? $" {ZValue.Item2} {cmpObj.CmpOperand} {rightValue.Item2}"
                    : $" {ZValue.Item2} !{cmpObj.CmpOperand} {rightValue.Item2}");
                }
            }
            else
            {
                isPass = false;
                addedComments = $" {leftValue} !{cmpObj.CmpOperand} {rightValue}";
            }
            
            return isPass;
        }

        /// <summary>
        /// Check if both sides are the same type
        /// </summary>
        /// <param name="isType"></param>
        /// <param name="cmpObj"></param>
        /// <returns></returns>
        private bool AreBothSidesSameType(ref Dictionary<ObjType, bool> isType, CompareCmdObj cmpObj)
        {
            bool retureV = false;

            if (_utilExecution.IsFloatType(cmpObj.CmpTypeA.ToLower(), cmpObj.CmpValueA, cmpObj.ObjTypeIdA) &&
                _utilExecution.IsFloatType(cmpObj.CmpTypeB.ToLower(), cmpObj.CmpValueB, cmpObj.ObjTypeIdB))
            {
                isType[ObjType.FLOAT] = true;
                retureV = true;
            }
            else if (_utilExecution.IsStringType(cmpObj.CmpTypeA.ToLower(), cmpObj.CmpValueA, cmpObj.ObjTypeIdA) &&
                _utilExecution.IsStringType(cmpObj.CmpTypeB.ToLower(), cmpObj.CmpValueB, cmpObj.ObjTypeIdB))
            {
                isType[ObjType.STRING] = true;
                retureV = true;
            }
            // If TypeA is WORD, TypeB could be Value 1 (Enum)
            else if (_utilExecution.IsWordType(cmpObj.CmpTypeA.ToLower(), cmpObj.CmpValueA, cmpObj.ObjTypeIdA) &&
                _utilExecution.IsWordType(cmpObj.CmpTypeB.ToLower(), cmpObj.CmpValueB, cmpObj.ObjTypeIdB))
            {
                isType[ObjType.WORD] = true;
                retureV = true;
            }
            else if (_utilExecution.IsBoolType(cmpObj.CmpTypeA.ToLower(), cmpObj.CmpValueA, cmpObj.ObjTypeIdA) &&
                _utilExecution.IsBoolType(cmpObj.CmpTypeB.ToLower(), cmpObj.CmpValueB, cmpObj.ObjTypeIdB))
            {
                isType[ObjType.BOOL] = true;
                retureV = true;
            }
            else if (_utilExecution.IsULongType(cmpObj.CmpTypeA.ToLower(), cmpObj.CmpValueA, cmpObj.ObjTypeIdA) &&
                _utilExecution.IsULongType(cmpObj.CmpTypeB.ToLower(), cmpObj.CmpValueB, cmpObj.ObjTypeIdB))
            {
                isType[ObjType.ULONG] = true;
                retureV = true;
            }
            else
            {
                retureV = false;
            }

            return retureV;
        }

        /// <summary>
        /// Get Compare Function result with FLOAT parameters.
        /// </summary>
        /// <param name="leftValue">Left Value</param>
        /// <param name="cmpObj">Compare Object</param>
        /// <param name="rightValue">Right Value</param>
        /// <returns>True if Pass, False if Failed</returns>
        private bool getCmpResult(float leftValue, CompareCmdObj cmpObj, float rightValue)
        {
            string cmpOperand = cmpObj.CmpOperand;
            // Operand Check
            if (cmpObj.notStandardOperand())
            {
                tcObj.AppComments = tcObj.AppComments + "Not a standard Compare Operand";
                return false;
            }

            switch (cmpOperand)
            {
                case { } when cmpOperand == CompareOperands.eq:
                    return Math.Abs(leftValue - rightValue) < Epsilon;
                case { } when cmpOperand == CompareOperands.ne:
                    return !(Math.Abs(leftValue - rightValue) < Epsilon);
                case { } when cmpOperand == CompareOperands.lt:
                    return leftValue < rightValue;
                case { } when cmpOperand == CompareOperands.gt:
                    return leftValue > rightValue;
                case { } when cmpOperand == CompareOperands.lteq:
                    return leftValue <= rightValue; ;
                case { } when cmpOperand == CompareOperands.gteq:
                    return leftValue >= rightValue; ;
                case { } when cmpOperand.Contains(CompareOperands.range):
                    //  80 +-10 70 should be false Per discussion with Ron
                    if (cmpObj.CmpRange != null)
                    {
                        float unitLow = rightValue - (float)cmpObj.CmpRange;
                        float unitHigh = rightValue + (float)cmpObj.CmpRange;
                        return leftValue > unitLow && leftValue < unitHigh;
                    }
                    return false;
                default:
                    tcObj.AppComments = tcObj.AppComments + $"{cmpOperand} is not allowed Compare Operand";
                    break;
            }
            
            return false;
        }
        
        /// <summary>
        /// Override Get compare reselt with STRING parameters
        /// </summary>
        /// <param name="leftValue"></param>
        /// <param name="cmpObj"></param>
        /// <param name="rightValue"></param>
        /// <returns>True if it's true, otherwise false</returns>
        private bool getCmpResult(string leftValue, CompareCmdObj cmpObj, string rightValue)
        {
            string cmpOperand = cmpObj.CmpOperand;
            bool returnV = false;

            if (cmpOperand != CompareOperands.eq && cmpOperand != CompareOperands.ne)
            {
                tcObj.AppComments = tcObj.AppComments + "The operand only can be '=' or '<>' for STRING type";
                return false;
            }
            if (cmpOperand == CompareOperands.eq)
            {
                returnV = leftValue.ToLower().Equals(rightValue.ToLower());
            }
            else if (cmpOperand == CompareOperands.ne)
            {
                returnV = !leftValue.ToLower().Equals(rightValue.ToLower());
            }

            return returnV;
        }
        
        /// <summary>
        /// Override Get compare result with BOOL type parameters
        /// </summary>
        /// <param name="leftValue"></param>
        /// <param name="cmpObj"></param>
        /// <param name="rightValue"></param>
        /// <returns></returns>
        private bool getCmpResult(bool leftValue, CompareCmdObj cmpObj, bool rightValue)
        {
            string cmpOperand = cmpObj.CmpOperand;
            bool returnV = false;

            if (cmpOperand != CompareOperands.eq && cmpOperand != CompareOperands.ne)
            {
                tcObj.AppComments = tcObj.AppComments + "The operand only can be '=' or '<>' for BOOL type";
                return false;
            }
            if (cmpOperand == CompareOperands.eq)
            {
                returnV = leftValue.Equals(rightValue);
            }
            else if (cmpOperand == CompareOperands.ne)
            {
                returnV = !leftValue.Equals(rightValue);
            }
            return returnV;
        }
    }
}
