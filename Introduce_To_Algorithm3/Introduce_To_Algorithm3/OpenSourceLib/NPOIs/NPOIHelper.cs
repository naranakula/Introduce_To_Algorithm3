using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Introduce_To_Algorithm3.OpenSourceLib.NPOIs
{
    /// <summary>
    /// NPOIExcel辅助类
    /// 不需要预先安装 Microsoft Office suite
    /// </summary>
    public static class NPOIHelper
    {
        /// <summary>
        /// 输出excel数据到指定的文件
        /// </summary>
        /// <param name="excelData"></param>
        /// <param name="excelFilePath">excel文件要保存的地址</param>
        /// <param name="sheetName">页面名称</param>
        public static void ExportExcel(List<List<string>> excelData,string excelFilePath,string sheetName="SheetName")
        {
            if (string.IsNullOrWhiteSpace(excelFilePath) || excelData == null)
            {
                return;
            }

            excelFilePath = excelFilePath.Trim();

            //HSSF (Excel2003及之前 xls)  XSSF(Excel 2007及之后 xlsx)
            IWorkbook workbook = null;

            if (excelFilePath.EndsWith(".xlsx", StringComparison.CurrentCultureIgnoreCase))
            {
                //Excel 2007及之后
                workbook = new XSSFWorkbook();
            }
            else if (excelFilePath.EndsWith(".xls", StringComparison.CurrentCultureIgnoreCase))
            {
                //Excel2003及之前
                workbook = new HSSFWorkbook();
            }
            else
            {
                //默认是Excel2003版本
                workbook = new HSSFWorkbook();
            }

            //指定创建页的名称
            ISheet sheet = workbook.CreateSheet(sheetName);

            for (int rowIndex = 0; rowIndex < excelData.Count; rowIndex++)
            {
                IRow row = sheet.CreateRow(rowIndex);
                for (int colIndex = 0; colIndex < excelData[rowIndex].Count; colIndex++)
                {
                    row.CreateCell(colIndex,CellType.String).SetCellValue(excelData[rowIndex][colIndex]);
                }
            }

            using (FileStream fs = File.Create(excelFilePath))
            {
                workbook.Write(fs);
            }
        }


        /// <summary>
        /// 读取Excel
        /// </summary>
        /// <param name="excelFilePath">要读取的Excel路径</param>
        public static List<List<string>> ImportExcel(string excelFilePath)
        {
            if (string.IsNullOrWhiteSpace(excelFilePath))
            {
                throw new ArgumentNullException("路径不能为空");
            }
            
            excelFilePath = excelFilePath.Trim();

            if (!File.Exists(excelFilePath))
            {
                throw new FileNotFoundException($"{excelFilePath}文件不存在");
            }
            
            IWorkbook workbook = null;

            using (FileStream fs = File.Open(excelFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                if (excelFilePath.EndsWith(".xlsx", StringComparison.CurrentCultureIgnoreCase))
                {
                    //Excel 2007及之后
                    workbook = new XSSFWorkbook(fs);
                }
                else if (excelFilePath.EndsWith(".xls", StringComparison.CurrentCultureIgnoreCase))
                {
                    //Excel2003及之前
                    workbook = new HSSFWorkbook(fs);
                }
                else
                {
                    //默认是Excel2003版本
                    workbook = new HSSFWorkbook(fs);
                }
            }
            
            ISheet sheet = workbook.GetSheetAt(0);

            List<List<string> > excelData = new List<List<string>>();
            if (sheet == null)
            {
                return excelData;
            }
            
            //行索引
            int rowIndex = 0;
            while (true)
            {
                //如果获取不到返回null
                IRow row = sheet.GetRow(rowIndex);
                if (row == null)
                {
                    break;
                }

                List<string> rowData = new List<string>();

                int cellIndex = 0;
                while (true)
                {
                    //如果获取不到，返回null
                    ICell cell = row.GetCell(cellIndex);
                    if (cell == null)
                    {
                        break;
                    }

                    rowData.Add(cell.StringCellValue);
                    cellIndex++;
                }

                rowIndex++;
                excelData.Add(rowData);
            }
            
            return excelData;
        }

    }
}
