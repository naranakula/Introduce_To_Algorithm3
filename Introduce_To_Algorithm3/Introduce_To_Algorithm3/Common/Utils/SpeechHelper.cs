using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Common.Utils
{
    /// <summary>
    /// 声音合成和识别
    /// 需要添加引用：System.Speech
    /// 语音合成使用：SpeechSynthesizer
    /// 语音识别使用：SpeechRecognizer
    /// </summary>
    public static class SpeechHelper
    {
        /// <summary>
        /// 语音合成
        /// </summary>
        /// <param name="text"></param>
        public static void Speech(string text)
        {
            SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer();
            speechSynthesizer.Speak(text);
        }
    }
}
