using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

namespace Introduce_To_Algorithm3.OpenSourceLib.NPOIs
{
    /// <summary>
    /// NPOIExcel辅助类
    /// 不需要预先安装 Microsoft Office suite
    /// 
    /// 官方地址   http://npoi.codeplex.com/
    /// Github: https://github.com/tonyqus/npoi
    /// nuget:npoi
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
            try
            {
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
                //设置默认的列宽和行高
                sheet.DefaultColumnWidth = 36;
                sheet.DefaultRowHeightInPoints = 32;

                //标题行合并单元格 指定合并的第一行 最后一行 第一行 最后一列
                sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 4)); //第一行，前5列合并为一个单元格

                //// 设置列宽,excel列宽每个像素是1/256
                //sheet1.SetColumnWidth(0, 18 * 256);
                //sheet1.SetColumnWidth(1, 18 * 256);

                //单元格样式
                ICellStyle cellStyle = workbook.CreateCellStyle();
                cellStyle.Alignment = HorizontalAlignment.Center;
                cellStyle.VerticalAlignment = VerticalAlignment.Center;
                //设置字体
                IFont font = workbook.CreateFont();
                font.FontHeightInPoints = 20;
                font.Color = HSSFColor.Lime.Index;
                cellStyle.SetFont(font);


                for (int rowIndex = 0; rowIndex < excelData.Count; rowIndex++)
                {
                    IRow row = sheet.CreateRow(rowIndex);


                    //设置行高
                    //row.HeightInPoints = 32;
                    for (int colIndex = 0; colIndex < excelData[rowIndex].Count; colIndex++)
                    {
                        ICell cell = row.CreateCell(colIndex, CellType.String);

                        if (rowIndex == 0)
                        {
                            cell.CellStyle = cellStyle;
                        }

                        cell.SetCellValue(excelData[rowIndex][colIndex]);
                    }
                }


                //输出图片数据， 使用memorystream读取图片
                byte[] pictureData = File.ReadAllBytes(@"D:\1.bmp");

                //返回the index to this picture (1 based). 需要根据实际情况指定图片格式
                int pictureIndex = workbook.AddPicture(pictureData, PictureType.BMP);
                //Create the drawing container
                IDrawing drawing = sheet.CreateDrawingPatriarch();
                //Create an anchor point
                int row1 = 4;
                int col1 = 0;
                //处理照片位置， xss使用xss hss使用hss
                //照片的左上角 (col1,row1)  右下角(col1+4,row1+10) 
                /*
                 * dx1：起始单元格的x偏移量，如例子中的255表示直线起始位置距A1单元格左侧的距离；
                dy1：起始单元格的y偏移量，如例子中的125表示直线起始位置距A1单元格上侧的距离；
    dx2：终止单元格的x偏移量，如例子中的1023表示直线起始位置距C3单元格左侧的距离；
    dy2：终止单元格的y偏移量，如例子中的150表示直线起始位置距C3单元格上侧的距离；
    col1：起始单元格列序号，从0开始计算；
    row1：起始单元格行序号，从0开始计算，如例子中col1 = 0,row1 = 0就表示起始单元格为A1；
    col2：终止单元格列序号，从0开始计算；
    row2：终止单元格行序号，从0开始计算，如例子中col2 = 2,row2 = 2就表示起始单元格为C3；*/
                XSSFClientAnchor anchor = new XSSFClientAnchor(0, 0, 0, 0, col1, row1, col1 + 4, row1 + 10);

                IPicture picture = drawing.CreatePicture(anchor, pictureIndex);

                //picture.Resize(); // 这句话一定不要，这是用图片原始大小来显示

                using (FileStream fs = File.Create(excelFilePath))
                {
                    workbook.Write(fs);
                }
            }
            finally
            {
                workbook?.Dispose();
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
           FileStream fs = null;
            try
            {
                fs = File.Open(excelFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
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

                //sheet没有dispose
                ISheet sheet = workbook.GetSheetAt(0);
                
                List<List<string>> excelData = new List<List<string>>();
                if (sheet == null)
                {
                    return excelData;
                }

                //行索引
                int rowIndex = 0;
                while (true)
                {
                    //如果获取不到返回null,row没有dispose
                    IRow row = sheet.GetRow(rowIndex);
                    if (row == null)
                    {
                        break;
                    }

                    List<string> rowData = new List<string>();

                    int cellIndex = 0;
                    while (true)
                    {
                        //如果获取不到，返回null cell没有dispose
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
            finally
            {
                if (fs != null)
                {
                    fs.Dispose();
                }

                if (workbook != null)
                {
                    workbook.Dispose();
                }
            }
        }




    }
}
