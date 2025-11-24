using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTools.Serialize.Server
{
    public class ExcelOperation
    {
        public bool OpenExcel(string filePath, ref string err)
        {
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    IWorkbook workbook = null;
                    if (filePath.EndsWith(".xls"))
                    {
                        workbook = new HSSFWorkbook(fs);
                    }
                    else if (filePath.EndsWith(".xlsx"))
                    {
                        workbook = new XSSFWorkbook(fs);
                    }
                    ISheet sheet = workbook.GetSheetAt(0);

                }
                return true;
            }
            catch (Exception ex)
            {
                err = ex.Message;
            }

            return false;
        }



        public KocosData GetResult(string filePath)
        {
            KocosData resultData = new KocosData();
            合闸数据 合闸 = new 合闸数据();
            分闸数据 分闸 = new 分闸数据();

            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    IWorkbook workbook = null;
                    if (filePath.EndsWith(".xls"))
                    {
                        workbook = new HSSFWorkbook(fs);
                    }
                    else if (filePath.EndsWith(".xlsx"))
                    {
                        workbook = new XSSFWorkbook(fs);
                    }
                    ISheet sheet = workbook.GetSheetAt(0);
                    resultData.合闸 = 合闸;
                    resultData.分闸 = 分闸;
                    resultData.出厂编号 = sheet.GetRow(0).GetCell(1)?.ToString() ?? "";
                    resultData.操作电压 = sheet.GetRow(1).GetCell(1)?.ToString() ?? "";
                    resultData.断路器型号 = sheet.GetRow(2).GetCell(1)?.ToString() ?? "";
                    resultData.真空灭弧室厂家 = sheet.GetRow(3).GetCell(1)?.ToString() ?? "";
                    resultData.真空灭弧室型号 = sheet.GetRow(4).GetCell(1)?.ToString() ?? "";
                    resultData.真空灭弧室编号A = sheet.GetRow(5).GetCell(1)?.ToString() ?? "";
                    resultData.真空灭弧室编号B = sheet.GetRow(6).GetCell(1)?.ToString() ?? "";
                    resultData.真空灭弧室编号C = sheet.GetRow(7).GetCell(1)?.ToString() ?? "";
                    //合闸数据
                    resultData.合闸.A相电流 = sheet.GetRow(11).GetCell(1).NumericCellValue.ToString("0.00", CultureInfo.InvariantCulture) ?? "";
                    resultData.合闸.A相动作时间 = sheet.GetRow(12).GetCell(1).NumericCellValue.ToString("0.00", CultureInfo.InvariantCulture) ?? "";
                    resultData.合闸.B相动作时间 = sheet.GetRow(12).GetCell(2).NumericCellValue.ToString("0.00", CultureInfo.InvariantCulture) ?? "";
                    resultData.合闸.C相动作时间 = sheet.GetRow(12).GetCell(3).NumericCellValue.ToString("0.00", CultureInfo.InvariantCulture) ?? "";
                    resultData.合闸.A相速度 = sheet.GetRow(13).GetCell(1).NumericCellValue.ToString("0.00", CultureInfo.InvariantCulture) ?? "";
                    resultData.合闸.B相速度 = sheet.GetRow(13).GetCell(2).NumericCellValue.ToString("0.00", CultureInfo.InvariantCulture) ?? "";
                    resultData.合闸.C相速度 = sheet.GetRow(13).GetCell(3).NumericCellValue.ToString("0.00", CultureInfo.InvariantCulture) ?? "";
                    resultData.合闸.A相总行程 = sheet.GetRow(14).GetCell(1).NumericCellValue.ToString("0.00", CultureInfo.InvariantCulture) ?? "";
                    resultData.合闸.B相总行程 = sheet.GetRow(14).GetCell(2).NumericCellValue.ToString("0.00", CultureInfo.InvariantCulture) ?? "";
                    resultData.合闸.C相总行程 = sheet.GetRow(14).GetCell(3).NumericCellValue.ToString("0.00", CultureInfo.InvariantCulture) ?? "";
                    resultData.合闸.A相超行程 = sheet.GetRow(15).GetCell(1).NumericCellValue.ToString("0.00", CultureInfo.InvariantCulture) ?? "";
                    resultData.合闸.B相超行程 = sheet.GetRow(15).GetCell(2).NumericCellValue.ToString("0.00", CultureInfo.InvariantCulture) ?? "";
                    resultData.合闸.C相超行程 = sheet.GetRow(15).GetCell(3).NumericCellValue.ToString("0.00", CultureInfo.InvariantCulture) ?? "";
                    resultData.合闸.A相开距 = sheet.GetRow(16).GetCell(1)?.NumericCellValue.ToString("0.00", CultureInfo.InvariantCulture) ?? "";
                    resultData.合闸.B相开距 = sheet.GetRow(16).GetCell(2)?.NumericCellValue.ToString("0.00", CultureInfo.InvariantCulture) ?? "";
                    resultData.合闸.C相开距 = sheet.GetRow(16).GetCell(3)?.NumericCellValue.ToString("0.00", CultureInfo.InvariantCulture) ?? "";
                    resultData.合闸.A相过冲 = sheet.GetRow(17).GetCell(1)?.NumericCellValue.ToString("0.00", CultureInfo.InvariantCulture) ?? "";
                    resultData.合闸.B相过冲 = sheet.GetRow(17).GetCell(2)?.NumericCellValue.ToString("0.00", CultureInfo.InvariantCulture) ?? "";
                    resultData.合闸.C相过冲 = sheet.GetRow(17).GetCell(3)?.NumericCellValue.ToString("0.00", CultureInfo.InvariantCulture) ?? "";
                    resultData.合闸.A相反弹 = sheet.GetRow(18).GetCell(1)?.NumericCellValue.ToString("0.00", CultureInfo.InvariantCulture) ?? "";
                    resultData.合闸.B相反弹 = sheet.GetRow(18).GetCell(2)?.NumericCellValue.ToString("0.00", CultureInfo.InvariantCulture) ?? "";
                    resultData.合闸.C相反弹 = sheet.GetRow(18).GetCell(3)?.NumericCellValue.ToString("0.00", CultureInfo.InvariantCulture) ?? "";
                    resultData.合闸.A相弹跳 = sheet.GetRow(19).GetCell(1)?.NumericCellValue.ToString("0.00", CultureInfo.InvariantCulture) ?? "";
                    resultData.合闸.B相弹跳 = sheet.GetRow(19).GetCell(2)?.NumericCellValue.ToString("0.00", CultureInfo.InvariantCulture) ?? "";
                    resultData.合闸.C相弹跳 = sheet.GetRow(19).GetCell(3)?.NumericCellValue.ToString("0.00", CultureInfo.InvariantCulture) ?? "";
                    resultData.合闸.同期 = sheet.GetRow(20).GetCell(1)?.NumericCellValue.ToString("0.00", CultureInfo.InvariantCulture) ?? "";
                    //分闸数据
                    resultData.分闸.A相电流 = sheet.GetRow(23).GetCell(1)?.NumericCellValue.ToString("0.00", CultureInfo.InvariantCulture) ?? "";
                    resultData.分闸.A相动作时间 = sheet.GetRow(24).GetCell(1)?.NumericCellValue.ToString("0.00", CultureInfo.InvariantCulture) ?? "";
                    resultData.分闸.B相动作时间 = sheet.GetRow(24).GetCell(2)?.NumericCellValue.ToString("0.00", CultureInfo.InvariantCulture) ?? "";
                    resultData.分闸.C相动作时间 = sheet.GetRow(24).GetCell(3)?.NumericCellValue.ToString("0.00", CultureInfo.InvariantCulture) ?? "";
                    resultData.分闸.A相速度 = sheet.GetRow(25).GetCell(1)?.NumericCellValue.ToString("0.00", CultureInfo.InvariantCulture) ?? "";
                    resultData.分闸.B相速度 = sheet.GetRow(25).GetCell(2)?.NumericCellValue.ToString("0.00", CultureInfo.InvariantCulture) ?? "";
                    resultData.分闸.C相速度 = sheet.GetRow(25).GetCell(3)?.NumericCellValue.ToString("0.00", CultureInfo.InvariantCulture) ?? "";
                    resultData.分闸.A相总行程 = sheet.GetRow(26).GetCell(1)?.NumericCellValue.ToString("0.00", CultureInfo.InvariantCulture) ?? "";
                    resultData.分闸.B相总行程 = sheet.GetRow(26).GetCell(2)?.NumericCellValue.ToString("0.00", CultureInfo.InvariantCulture) ?? "";
                    resultData.分闸.C相总行程 = sheet.GetRow(26).GetCell(3)?.NumericCellValue.ToString("0.00", CultureInfo.InvariantCulture) ?? "";
                    resultData.分闸.A相超行程 = sheet.GetRow(27).GetCell(1)?.NumericCellValue.ToString("0.00", CultureInfo.InvariantCulture) ?? "";
                    resultData.分闸.B相超行程 = sheet.GetRow(27).GetCell(2)?.NumericCellValue.ToString("0.00", CultureInfo.InvariantCulture) ?? "";
                    resultData.分闸.C相超行程 = sheet.GetRow(27).GetCell(3)?.NumericCellValue.ToString("0.00", CultureInfo.InvariantCulture) ?? "";
                    resultData.分闸.A相开距 = sheet.GetRow(28).GetCell(1)?.NumericCellValue.ToString("0.00", CultureInfo.InvariantCulture) ?? "";
                    resultData.分闸.B相开距 = sheet.GetRow(28).GetCell(2)?.NumericCellValue.ToString("0.00", CultureInfo.InvariantCulture) ?? "";
                    resultData.分闸.C相开距 = sheet.GetRow(28).GetCell(3)?.NumericCellValue.ToString("0.00", CultureInfo.InvariantCulture) ?? "";
                    resultData.分闸.A相过冲 = sheet.GetRow(29).GetCell(1)?.NumericCellValue.ToString("0.00", CultureInfo.InvariantCulture) ?? "";
                    resultData.分闸.B相过冲 = sheet.GetRow(29).GetCell(2)?.NumericCellValue.ToString("0.00", CultureInfo.InvariantCulture) ?? "";
                    resultData.分闸.C相过冲 = sheet.GetRow(29).GetCell(3)?.NumericCellValue.ToString("0.00", CultureInfo.InvariantCulture) ?? "";
                    resultData.分闸.A相反弹 = sheet.GetRow(30).GetCell(1)?.NumericCellValue.ToString("0.00", CultureInfo.InvariantCulture) ?? "";
                    resultData.分闸.B相反弹 = sheet.GetRow(30).GetCell(2)?.NumericCellValue.ToString("0.00", CultureInfo.InvariantCulture) ?? "";
                    resultData.分闸.C相反弹 = sheet.GetRow(30).GetCell(3)?.NumericCellValue.ToString("0.00", CultureInfo.InvariantCulture) ?? "";
                    resultData.分闸.A相弹跳 = sheet.GetRow(31).GetCell(1)?.NumericCellValue.ToString("0.00", CultureInfo.InvariantCulture) ?? "";
                    resultData.分闸.B相弹跳 = sheet.GetRow(31).GetCell(2)?.NumericCellValue.ToString("0.00", CultureInfo.InvariantCulture) ?? "";
                    resultData.分闸.C相弹跳 = sheet.GetRow(31).GetCell(3)?.NumericCellValue.ToString("0.00", CultureInfo.InvariantCulture) ?? "";
                    resultData.分闸.同期 = sheet.GetRow(32).GetCell(1)?.NumericCellValue.ToString("0.00", CultureInfo.InvariantCulture) ?? "";
                    return resultData;
                }
            }
            catch (Exception ex)
            {
                //LogManagerV2.Instance.Write($"读取文件发生异常: {ex}");
                return null;
            }

        }


        public class KocosData
        {
            public string 出厂编号;
            public string 操作电压;
            public string 断路器型号;
            public string 真空灭弧室厂家;
            public string 真空灭弧室型号;
            public string 真空灭弧室编号A;
            public string 真空灭弧室编号B;
            public string 真空灭弧室编号C;
            public 合闸数据 合闸;
            public 分闸数据 分闸;
        }


        public class 合闸数据
        {
            public string A相电流 { get; set; }
            public string 同期时间 { get; set; }
            public string A相动作时间 { get; set; }
            public string B相动作时间 { get; set; }
            public string C相动作时间 { get; set; }
            public string A相速度 { get; set; }
            public string B相速度 { get; set; }
            public string C相速度 { get; set; }
            public string A相总行程 { get; set; }
            public string B相总行程 { get; set; }
            public string C相总行程 { get; set; }
            public string A相超行程 { get; set; }
            public string B相超行程 { get; set; }
            public string C相超行程 { get; set; }
            public string A相开距 { get; set; }
            public string B相开距 { get; set; }
            public string C相开距 { get; set; }
            public string A相过冲 { get; set; }
            public string B相过冲 { get; set; }
            public string C相过冲 { get; set; }
            public string A相反弹 { get; set; }
            public string B相反弹 { get; set; }
            public string C相反弹 { get; set; }
            public string A相弹跳 { get; set; }
            public string B相弹跳 { get; set; }
            public string C相弹跳 { get; set; }
            public string 同期 { get; set; }


        }
        public class 分闸数据
        {
            public string A相电流 { get; set; }
            public string 同期时间 { get; set; }
            public string A相动作时间 { get; set; }
            public string B相动作时间 { get; set; }
            public string C相动作时间 { get; set; }
            public string A相速度 { get; set; }
            public string B相速度 { get; set; }
            public string C相速度 { get; set; }
            public string A相总行程 { get; set; }
            public string B相总行程 { get; set; }
            public string C相总行程 { get; set; }
            public string A相超行程 { get; set; }
            public string B相超行程 { get; set; }
            public string C相超行程 { get; set; }
            public string A相开距 { get; set; }
            public string B相开距 { get; set; }
            public string C相开距 { get; set; }
            public string A相过冲 { get; set; }
            public string B相过冲 { get; set; }
            public string C相过冲 { get; set; }
            public string A相反弹 { get; set; }
            public string B相反弹 { get; set; }
            public string C相反弹 { get; set; }
            public string A相弹跳 { get; set; }
            public string B相弹跳 { get; set; }
            public string C相弹跳 { get; set; }
            public string 同期 { get; set; }

        }

    }
}
