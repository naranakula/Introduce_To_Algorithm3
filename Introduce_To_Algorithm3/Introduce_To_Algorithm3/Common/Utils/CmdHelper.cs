using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Introduce_To_Algorithm3.Common.Utils
{
    public class CmdHelper
    {

        private Process proc = null;
        /// <summary> 
        /// ���췽�� 
        /// </summary> 
        public CmdHelper()
        {
            proc = new Process();
        }
        /// <summary> 
        /// ִ��CMD��� 
        /// </summary> 
        /// <param name="cmd">Ҫִ�е�CMD����</param> 
        public void RunCmd(string cmd)
        {
            try
            {
                proc.StartInfo.CreateNoWindow = true;
                proc.StartInfo.FileName = "cmd.exe";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardError = true;
                proc.StartInfo.RedirectStandardInput = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.Start();
                proc.StandardInput.WriteLine(cmd);
                proc.StandardInput.WriteLine("exit");
                Console.WriteLine("ִ��ֹͣwebservice�����"+proc.StandardOutput.ReadToEnd());
            }
            finally
            {
                if (proc != null)
                {
                    proc.Close();
                }
            }
        }
        /// <summary> 
        /// �������ִ������ 
        /// </summary> 
        /// <param name="programName">���·�������ƣ�.exe�ļ���</param> 
        /// <param name="cmd">Ҫִ�е�����</param> 
        public void RunProgram(string programName, string cmd)
        {
            Process proc = new Process();
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.FileName = programName;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.Start();
            if (cmd.Length != 0)
            {
                proc.StandardInput.WriteLine(cmd);
            }
            proc.Close();
        }
        /// <summary> 
        /// ����� 
        /// </summary> 
        /// <param name="programName">���·�������ƣ�.exe�ļ���</param> 
        public void RunProgram(string programName)
        {
            this.RunProgram(programName, "");
        }
    }


}

