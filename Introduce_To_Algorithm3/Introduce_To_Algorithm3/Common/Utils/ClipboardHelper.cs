using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Introduce_To_Algorithm3.Common.Utils
{
    /// <summary>
    /// 剪贴板辅助类
    /// </summary>
    public class ClipboardHelper
    {
        /// <summary>
        /// 复制文本到剪贴板
        /// </summary>
        /// <param name="text"></param>
        public static void CopyText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return;
            }

            Clipboard.SetData(DataFormats.Text,text);
        }

        public static Tuple<bool, string> PasteText()
        {
            IDataObject idata = Clipboard.GetDataObject();

            if (idata == null)
            {
                return new Tuple<bool, string>(false,"");
            }

            if (idata.GetDataPresent(DataFormats.Text))
            {
                return new Tuple<bool, string>(true, (string) idata.GetData(DataFormats.Text));
            }
            else
            {
                return new Tuple<bool, string>(false,"");
            }
        }


    }
}
