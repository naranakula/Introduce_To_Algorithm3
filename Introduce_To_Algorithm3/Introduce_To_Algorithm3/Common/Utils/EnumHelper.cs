using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Utility.Commons
{
    /// <summary>
    /// 枚举帮助类
    /// </summary>
    public class EnumHelper
    {
        /// <summary>
        /// 将枚举转换为整型
        /// </summary>
        /// <param name="e"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryParseToInt(Color e,out int result)
        {
            try
            {
                result = (int)e;
                return true;
            }
            catch (Exception)
            {
                result = -1;
                return false;
            }
        }


        public static bool TryParse(string s,out Color e)
        {
            return  Enum.TryParse(s, true, out e);
        }


        public static bool TryParse(int i, out Color e)
        {
            return Enum.TryParse(i.ToString(), true, out e);
        }

    }

    public enum Color
    {
        Blue,
        Red,
        Black
    }
}
