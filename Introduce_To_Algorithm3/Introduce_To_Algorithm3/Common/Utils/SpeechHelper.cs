using NAudio.Wave;
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
        /// <param name="exceptionHandler">异常处理</param>
        public static bool Speech(string text,Action<Exception> exceptionHandler = null)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return true;
            }

            try
            {
                if (WaveOut.DeviceCount < 1)
                {
                    //没有音频设备
                    return false;
                }

                using (SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer())
                {
                    //输出到默认的音频设备
                    speechSynthesizer.SetOutputToDefaultAudioDevice();
                    //同步播放
                    speechSynthesizer.Speak(text);
                }

                return true;
            }catch(Exception ex)
            {
                if(exceptionHandler != null)
                {
                    exceptionHandler(ex);
                }

                return false;
            }
            
        }
    }
}
