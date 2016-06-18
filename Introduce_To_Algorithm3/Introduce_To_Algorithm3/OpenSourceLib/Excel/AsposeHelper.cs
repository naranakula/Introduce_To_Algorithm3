using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aspose.Cells;

namespace Introduce_To_Algorithm3.OpenSourceLib.Excel
{
    public static class AsposeHelper
    {
                /// <summary>
        /// 读取指定的excel文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="isSuccess">是否读取成功</param>
        /// <returns></returns>
        public static List<List<string>> ReadExcelToList(string fileName,out bool isSuccess)
        {
            isSuccess = false;
            //文件不存在
            if (!File.Exists(fileName))
            {
                return null;
            }

            try
            {
                List<List<string>> resultList = new List<List<string>>();
                using (Workbook workBook = new Workbook(fileName))
                {
                    Cells cells = workBook.Worksheets[0].Cells;
                    for (int i = 0; i <= cells.MaxRow; i++)
                    {
                        List<string> rowList = new List<string>();
                        for (int j = 0; j <= cells.MaxColumn; j++)
                        {
                            rowList.Add(cells.Rows[i][j].StringValue ?? string.Empty);
                        }
                        resultList.Add(rowList);
                    }
                }
                isSuccess = true;
                return resultList;
            }
            catch (Exception)
            {
                isSuccess = false;
                return null;
            }
        }

        /// <summary>
        /// 读取指定的excel文件到指定的数组中
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="isSuccess">是否读取成功</param>
        /// <returns></returns>
        public static string[,] ReadExcelToArray(string fileName,out bool isSuccess)
        {
            isSuccess = false;
            //文件不存在
            if (!File.Exists(fileName))
            {
                return null;
            }

            try
            {
                using (Workbook workBook = new Workbook())
                {
                    Cells cells = workBook.Worksheets[0].Cells;
                    object[,] objects = cells.ExportArray(0, 0, cells.MaxRow + 1, cells.MaxColumn + 1);
                    int rowCount = objects.GetLength(0);
                    int columnCount = objects.GetLength(1);
                    string[,] resultArray = new string[rowCount, columnCount];
                    for (int i = 0; i < rowCount; i++)
                    {
                        for (int j = 0; j < columnCount; j++)
                        {
                            resultArray[i, j] = objects[i, j] == null ? string.Empty : objects[i, j].ToString();
                        }
                    }

                    isSuccess = true;
                    return resultArray;
                }
            }
            catch (Exception)
            {
                isSuccess = false;
                return null;
            }
        }


        /// <summary>
        /// 写excel文件，如果文件不存在，将创建新文件；如果文件存在，则覆盖该文件
        /// </summary>
        /// <param name="fileName">要保存的文件名</param>
        /// <param name="workSheetName">一页的名称 </param>
        /// <param name="excelList">保存excel文件的数据</param>
        /// <returns></returns>
        public static bool WriteExcel(string fileName,string workSheetName,List<List<string>> excelList)
        {
            try
            {
                using (Workbook workbook = new Workbook())
                {
                    //清空页，因为默认情况下，创建一个Workbook会创建一页
                    workbook.Worksheets.Clear();
                    workbook.Worksheets.Add(workSheetName ?? string.Empty);

                    //行和列计数器
                    int rowCount = 0;
                    foreach (List<string> rowList in excelList)
                    {
                        int columnCount = 0;
                        foreach (string column in rowList)
                        {
                            workbook.Worksheets[0].Cells.Rows[rowCount][columnCount].PutValue(column);
                            columnCount++;
                        }
                        rowCount++;
                    }
                    //workbook.Save(fileName,SaveFormat.Auto);
                    workbook.Save(fileName);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 写excel文件，如果文件不存在，将创建新文件；如果文件存在，则覆盖该文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="workSheetName"></param>
        /// <param name="excelArray"></param>
        /// <returns></returns>
        public static bool WriteExcel(string fileName,string workSheetName,string[,] excelArray)
        {
            try
            {
                using (Workbook workbook = new Workbook())
                {
                    //清空页，因为默认情况下，创建一个Workbook会创建一页
                    workbook.Worksheets.Clear();
                    workbook.Worksheets.Add(workSheetName ?? string.Empty);
                    workbook.Worksheets[0].Cells.ImportArray(excelArray, 0, 0);
                    workbook.Save(fileName);
                }
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

    }
}
