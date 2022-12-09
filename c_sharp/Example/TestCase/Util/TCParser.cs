using AAH_AutoSim.TestCase.Constants;
using AAH_AutoSim.TestCase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NPOI.SS.Formula.Functions.Countif;

namespace AAH_AutoSim.TestCase.Util
{
    public class TCParser
    {
        /// <summary>
        /// For WORD enum value converted from string
        /// </summary>
        /// <param name="numStr"></param>
        /// <param name="numInt"></param>
        /// <param name="tcObj"></param>
        public static int ToInt(string numStr)
        {
            int numInt = TestCaseConstants.ErrorValueInt;
            if (!Int32.TryParse(numStr, out numInt))
            {
                throw new Exception($" Error to convert [{numStr}] to Int");
            }
            return numInt;
        }

        /// <summary>
        /// For FLOAT value converted from string
        /// </summary>
        /// <param name="numStr"></param>
        /// <param name="numFloat"></param>
        /// <param name="tcObj"></param>
        public static float ToFloat(string numStr)
        {
            float numFloat = TestCaseConstants.ErrorValueFloat;
            if (!float.TryParse(numStr, out numFloat))
            {
                throw new Exception($" Error to convert [{numStr}] to FLOAT");
            }
            return numFloat;
        }
        /// <summary>
        /// For ULONG value converted from string
        /// </summary>
        /// <param name="numStr"></param>
        /// <param name="numUlong"></param>
        /// <param name="tcObj"></param>
        public static ulong ToUlong(string numStr)
        {
            ulong numUlong = TestCaseConstants.ErrorValueULong;
            if (!ulong.TryParse(numStr, out numUlong))
            {
                throw new Exception($" Error to convert [{numStr}] to ULONG");
            }
            return numUlong;
        }
        /// <summary>
        /// For BOOL value converted from string
        /// </summary>
        /// <param name="numStr"></param>
        /// <param name="numUlong"></param>
        /// <exception cref="Exception"></exception>
        public static bool ToBool(string numStr)
        {
            bool numBool;
            if (!bool.TryParse(numStr, out numBool))
            {
                throw new Exception($" Error to convert [{numStr}] to BOOL");
            }
            return numBool;
        }

    }
}
