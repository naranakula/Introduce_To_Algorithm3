using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils.strings
{
    /// <summary>
    /// 获取当前的语言三字码
    /// </summary>
    public static class CultureUtils
    {
        /*
         * 
         * 多语言开发：
CurrentUICulture获取当前的语言和区域
ThreeLetterISOLanguageName获取当前 CultureInfo 的语言的由三个字母构成的 ISO 639-2 代码。  英语eng 中文zho

定义一种默认语言，每个语言使用自己特定的语言包

         * 
         */

        /// <summary>
        /// 当前语言
        /// </summary>
        private static volatile string cacheCurrentLanguage = null;

        /// <summary>
        /// 获取当前的语言三字码
        /// CurrentUICulture获取当前的语言和区域
        /// ThreeLetterISOLanguageName获取当前 CultureInfo 的语言的由三个字母构成的 ISO 639-2 代码。  英语eng 中文zho
        /// 定义一种默认语言，每个语言使用自己特定的语言包
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentLanguage()
        {
            if (!string.IsNullOrEmpty(cacheCurrentLanguage))
            {
                return cacheCurrentLanguage;
            }

            CultureInfo currentCulture = CultureInfo.CurrentUICulture;
            cacheCurrentLanguage = currentCulture.ThreeLetterISOLanguageName.Trim();
            return cacheCurrentLanguage;
        }

    }
}
