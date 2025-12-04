using Microsoft.Win32;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace AppTools.ExportExport
{
    public class ExcelExportServer<T> : IExcelExport<IEnumerable<T>>
    {
        public event Action<IEnumerable<T>> ExportCompleted;
        private readonly IEnumerable<T> _data;

        public ExcelExportServer(IEnumerable<T> data)
        {
            this._data = data ?? throw new ArgumentNullException(nameof(data));
        }

        public void AutoFitColumns()
        {
            throw new NotImplementedException();
        }

        public void ExportData(IEnumerable<T> data)
        {
            ExportToExcel(data);
        }

        public void SetHeaderStyle(string styleName)
        {
            throw new NotImplementedException();
        }

        public void SetWorksheet(string sheetName)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 导出原始对象数据到Excel文件
        /// </summary>
        /// <param name="data"></param>
        public void ExportToExcel(IEnumerable<T> data)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Excel 文件 (*.xlsx)|*.xlsx|Excel 97-2003 文件 (*.xls)|*.xls|所有文件 (*.*)|*.*";
                saveFileDialog.Title = "Export To Excel File";
                saveFileDialog.FileName = $"ExportedData_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        IWorkbook workbook;
                        if (Path.GetExtension(saveFileDialog.FileName).ToLower() == ".xls")
                        {
                            workbook = new HSSFWorkbook();
                        }
                        else
                        {
                            workbook = new XSSFWorkbook();
                        }

                        // 检查数据是否为空
                        if (data == null || !data.Any())
                        {
                            ExportEmptyData(workbook, saveFileDialog.FileName);
                            return;
                        }

                        // 导出数据
                        ExportCollectionData(workbook, data, saveFileDialog.FileName);

                        MessageBox.Show($"数据导出成功！\n共导出 {data.Count()} 条记录\n文件位置：{saveFileDialog.FileName}",
                            "导出成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        System.Diagnostics.Process.Start(saveFileDialog.FileName);
                        // 触发完成事件
                        ExportCompleted?.Invoke(data);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"导出时发生错误: {ex.Message}", "错误",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void ExportCollectionData(IWorkbook workbook, IEnumerable<T> data, string filePath)
        {
            // 在创建字体之前设置这个环境变量
            Environment.SetEnvironmentVariable("SKIP_SYSTEMFONTSCAN", "1");

            ISheet sheet = workbook.CreateSheet("数据导出");
            int rowIndex = 0;

            // 获取属性信息
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                     .Where(p => p.CanRead)
                                     .ToArray();

            // 创建表头行
            IRow headerRow = sheet.CreateRow(rowIndex++);

            try
            {
                // 设置表头样式
                ICellStyle headerStyle = workbook.CreateCellStyle();
                IFont headerFont = workbook.CreateFont();
                headerFont.IsBold = true;
                headerFont.FontHeightInPoints = 12;
                headerStyle.SetFont(headerFont);
                headerStyle.FillForegroundColor = IndexedColors.LightBlue.Index;
                headerStyle.FillPattern = FillPattern.SolidForeground;

                // 写入表头
                for (int i = 0; i < properties.Length; i++)
                {
                    var cell = headerRow.CreateCell(i);
                    cell.SetCellValue(GetPropertyDisplayName(properties[i]));
                    cell.CellStyle = headerStyle;
                }
            }
            catch (Exception ex)
            {
                // 如果字体创建失败，使用默认样式
                Console.WriteLine($"字体创建失败，使用默认样式: {ex.Message}");

                // 使用默认样式写入表头
                for (int i = 0; i < properties.Length; i++)
                {
                    var cell = headerRow.CreateCell(i);
                    cell.SetCellValue(GetPropertyDisplayName(properties[i]));
                }
            }

            // 写入数据行
            foreach (var item in data)
            {
                IRow dataRow = sheet.CreateRow(rowIndex++);

                for (int i = 0; i < properties.Length; i++)
                {
                    var cell = dataRow.CreateCell(i);
                    var value = GetPropertyValue(properties[i], item);
                    cell.SetCellValue(value);
                }
            }

            //// 自动调整列宽
            //for (int i = 0; i < properties.Length; i++)
            //{
            //    sheet.AutoSizeColumn(i);
            //}

            // 保存文件
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(fileStream);
            }
        }

        private void ExportEmptyData(IWorkbook workbook, string filePath)
        {
            ISheet sheet = workbook.CreateSheet("数据导出");
            IRow row = sheet.CreateRow(0);
            row.CreateCell(0).SetCellValue("没有数据可导出");

            using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(fileStream);
            }

            MessageBox.Show("没有数据可导出", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private string GetPropertyDisplayName(PropertyInfo property)
        {
            // 尝试获取DisplayName特性
            var displayNameAttr = property.GetCustomAttribute<System.ComponentModel.DisplayNameAttribute>();
            if (displayNameAttr != null)
            {
                return displayNameAttr.DisplayName;
            }

            // 尝试获取Display特性
            var displayAttr = property.GetCustomAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>();
            if (displayAttr != null && !string.IsNullOrEmpty(displayAttr.Name))
            {
                return displayAttr.Name;
            }

            // 返回属性名
            return property.Name;
        }

        private string GetPropertyValue(PropertyInfo property, T item)
        {
            try
            {
                object value = property.GetValue(item);

                if (value == null) return "空值";

                // 特殊类型处理
                if (value is DateTime dateTime)
                {
                    return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
                }

                if (value is bool boolValue)
                {
                    return boolValue ? "是" : "否";
                }

                if (value is IEnumerable enumerable && !(value is string))
                {
                    var count = 0;
                    var enumerator = enumerable.GetEnumerator();
                    while (enumerator.MoveNext()) count++;
                    return $"[集合: {count} 项]";
                }

                return value.ToString();
            }
            catch (Exception ex)
            {
                return $"读取错误: {ex.Message}";
            }
        }

        private void SetCellStyle(IWorkbook workbook, ICell cell, Type propertyType)
        {
            ICellStyle style = workbook.CreateCellStyle();

            if (propertyType == typeof(DateTime))
            {
                style.DataFormat = workbook.CreateDataFormat().GetFormat("yyyy-mm-dd hh:mm:ss");
            }
            else if (propertyType == typeof(decimal) || propertyType == typeof(double) || propertyType == typeof(float))
            {
                style.DataFormat = workbook.CreateDataFormat().GetFormat("0.00");
            }
            else if (propertyType == typeof(int) || propertyType == typeof(long))
            {
                style.DataFormat = workbook.CreateDataFormat().GetFormat("0");
            }

            cell.CellStyle = style;
        }

        // 重载方法：导出单个对象
        public void ExportToExcel(T singleData)
        {
            ExportToExcel(new List<T> { singleData });
        }

        // 重载方法：使用构造函数中的数据
        public void ExportToExcel()
        {
            ExportToExcel(_data);
        }

        private string GetTypeDisplayName(Type type)
        {
            if (type == typeof(string)) return "字符串";
            if (type == typeof(int)) return "整数";
            if (type == typeof(decimal)) return "小数";
            if (type == typeof(double)) return "双精度小数";
            if (type == typeof(float)) return "单精度小数";
            if (type == typeof(bool)) return "布尔值";
            if (type == typeof(DateTime)) return "日期时间";
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return $"可空{GetTypeDisplayName(type.GetGenericArguments()[0])}";
            }
            if (type.IsEnum) return "枚举";
            if (typeof(IEnumerable).IsAssignableFrom(type)) return "集合";

            return type.Name;
        }
    }
}