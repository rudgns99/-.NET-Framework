using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace GWF
{
    public class ExcelHelper
    {
        public static DataTable ExcelToDataTable(string filePath)
        {
            DataTable dt = new DataTable();

            HSSFWorkbook hssfworkbook;
            using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                hssfworkbook = new HSSFWorkbook(file);
            }
            ISheet sheet = hssfworkbook.GetSheetAt(0);
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();

            IRow headerRow = sheet.GetRow(0);
            int cellCount = headerRow.LastCellNum;

            for (int j = 0; j < cellCount; j++)
            {
                ICell cell = headerRow.GetCell(j);
                dt.Columns.Add(cell.ToString());
            }

            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                DataRow dataRow = dt.NewRow();
                if (row == null)
                {
                    break;
                }
                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    if (row.GetCell(j) != null)
                        dataRow[j] = row.GetCell(j).ToString();
                }

                dt.Rows.Add(dataRow);
            }
            return dt;
        }

        public static MemoryStream DataToExcel(DataSet ds, Dictionary<string, ExcelStyle> dicStyle, bool ExistShow, byte[] imageData)
        {
            //파일은 이렇게 만드세요
            //MemoryStream ms = ExcelHelper.DataToExcel(dt);
            //FileStream fs = new FileStream("e:\\2.xls", FileMode.Create);
            //ms.WriteTo(fs);
            //fs.Close();
            //ms.Close();

            MemoryStream ms = new MemoryStream();
            using (ds)
            {
                IWorkbook workbook = new HSSFWorkbook();//Create an excel Workbook   
                if (DataHelper.HasRow(ds))
                {
                    ISheet sheet;
                    IRow headerRow;
                    IRow dataRow;
                    #region DataSheet
                    foreach (DataTable dt in ds.Tables)
                    {
                        sheet = workbook.CreateSheet();//Create a work table in the table
                        headerRow = sheet.CreateRow(0); //To add a row in the table
                        int iCellCnt = 0;
                        foreach (DataColumn column in dt.Columns)
                        {
                            if (ExistShow && dicStyle != null && !dicStyle.ContainsKey(column.Caption))
                                continue;

                            headerRow.CreateCell(iCellCnt).SetCellValue(column.Caption);

                            if (dicStyle != null && dicStyle.ContainsKey(column.Caption))
                            {
                                dicStyle[column.Caption].setHeaderStyle(workbook, headerRow.GetCell(iCellCnt));
                                sheet.SetColumnWidth(iCellCnt, calculateColWidth(dicStyle[column.Caption].Width));
                            }

                            iCellCnt++;
                        }
                        int rowIndex = 1;
                        foreach (DataRow row in dt.Rows)
                        {
                            dataRow = sheet.CreateRow(rowIndex);
                            iCellCnt = 0;
                            foreach (DataColumn column in dt.Columns)
                            {
                                if (ExistShow && dicStyle != null && !dicStyle.ContainsKey(column.Caption))
                                    continue;

                                dataRow.CreateCell(iCellCnt).SetCellValue(row[column].ToString());

                                if (dicStyle != null && dicStyle.ContainsKey(column.Caption))
                                {
                                    dicStyle[column.Caption].setDataStyle(workbook, dataRow.GetCell(iCellCnt));
                                    sheet.SetColumnWidth(iCellCnt, calculateColWidth(dicStyle[column.Caption].Width));
                                }

                                iCellCnt++;
                            }
                            rowIndex++;
                        }
                    } 
                    #endregion

                    if (imageData != null)
                    {
                        sheet = workbook.CreateSheet();//Create a work table in the table
                        HSSFPatriarch patriarch = (HSSFPatriarch)sheet.CreateDrawingPatriarch();
                        int intx1 = 0;
                        int inty1 = 0;
                        int intx2 = 14;
                        int inty2 = 19;
                        HSSFClientAnchor anchor = new HSSFClientAnchor(20, 0, 40, 20, intx1, inty1, intx2, inty2);
                        anchor.AnchorType = AnchorType.MoveDontResize;                        
                        int index = workbook.AddPicture(imageData, PictureType.PNG);
                        patriarch.CreatePicture(anchor, index);
                    }
                }

                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;
            }
            return ms;
        }

        /// <summary>
        /// 엑셀 다운로드
        /// </summary>
        /// <param name="Page">다운로드 받는 페이지 객체</param>
        /// <param name="ds">데이터셋</param>
        /// <param name="UriEncoded_FileNameWithExt">확장자를 포함한 파일명</param>
        /// <param name="dicStyle">스타일</param>
        /// <param name="ExistShow">스타일에 선언된것만 보여주려면 True</param>
        public static void ExcelDownload(System.Web.UI.Page Page, DataSet ds, string UriEncoded_FileNameWithExt, Dictionary<string, ExcelStyle> dicStyle = null, bool ExistShow = false, byte[] imageData = null)
        {
            using (MemoryStream ms = DataToExcel(ds, dicStyle, ExistShow, imageData))
            {
                Page.Response.Clear();
                Page.Response.ContentType = "application/force-download";
                Page.Response.AddHeader("content-disposition", string.Format("attachment;filename={0}", UriEncoded_FileNameWithExt));
                Page.Response.AddHeader("Content-Transfer-Encoding", "binary");
                Page.Response.AddHeader("Pragma", "no-cache");
                Page.Response.AddHeader("Expires", "0");
                Page.Response.BinaryWrite(ms.ToArray());
                Page.Response.End();
            }
        }

        public static int calculateColWidth(int width)
        {
            if (width > 254)
                return 65280; // Maximum allowed column width. 
            if (width > 1)
            {
                int floor = (int)(Math.Floor(((double)width) / 5));
                int factor = (30 * floor);
                int value = 450 + factor + ((width - 1) * 250);
                return value;
            }
            else
                return 450; // default to column size 1 if zero, one or negative number is passed. 
                        
        } 
    }

    public class ExcelStyle : abExcelStyle
    {
        public ExcelStyle(ExcelCellStyle HeaderStyle, ExcelCellStyle DataStyle, int Width = 10)
        {
            this.Width = Width;
            this.HeaderStyle = HeaderStyle;
            this.DataStyle = DataStyle;
        }
    }

    public abstract class abExcelStyle
    {
        public int Width;
        protected ExcelCellStyle HeaderStyle;
        protected ExcelCellStyle DataStyle;

        virtual public void setHeaderStyle(IWorkbook workbook, ICell cell)
        {
            setStyle(workbook, cell, ref this.HeaderStyle);
        }

        virtual public void setDataStyle(IWorkbook workbook, ICell cell)
        {
            setStyle(workbook, cell, ref this.DataStyle);
        }

        private void setStyle(IWorkbook workbook, ICell cell,ref ExcelCellStyle cellStyle)
        {  
            if (cellStyle != null)
            {
                if (cellStyle.Style == null)
                {
                    ICellStyle style = workbook.CreateCellStyle();
                    IFont font = workbook.CreateFont();

                    if (cellStyle.ExcelFont != null)
                    {
                        font.FontName = cellStyle.ExcelFont.FontName;
                        font.FontHeightInPoints = cellStyle.ExcelFont.FontHeightInPoints;
                        font.Color = cellStyle.ExcelFont.ColorIndex;
                        font.IsBold = cellStyle.ExcelFont.IsBold;
                        style.SetFont(font);
                    }

                    if (cellStyle.ExcelProperty != null)
                    {
                        style.Alignment = cellStyle.ExcelProperty.HorizontalAlignment;
                        style.VerticalAlignment = cellStyle.ExcelProperty.VerticalAlignment;
                        style.FillForegroundColor = cellStyle.ExcelProperty.ForegroundColor;
                        style.FillPattern = FillPattern.SolidForeground;

                        if(cellStyle.Type == ExcelType.Header)
                            style.BorderTop = cellStyle.ExcelProperty.BorderStyleTop;

                        style.BorderLeft = cellStyle.ExcelProperty.BorderStyleLeft;
                        style.BorderRight = cellStyle.ExcelProperty.BorderStyleRight;
                        style.BorderBottom = cellStyle.ExcelProperty.BorderStyleBottom;
                    }

                    cellStyle.Style = style;
                    cell.CellStyle = style;
                }
                else
                {
                    cell.CellStyle = cellStyle.Style;
                }
            }
        }
    }

    public class ExcelCellStyle
    {
        public ICellStyle Style;
        public ExcelType Type;
        public ExcelFont ExcelFont { get; set; }
        public ExcelProperty ExcelProperty { get; set; }
        
        public ExcelCellStyle()
        {
        }

        public ExcelCellStyle(ExcelType type)
        {
            this.Type = type;
        }

        public void setStyle(IWorkbook workbook, ICell cell)
        {
            if (Style == null)
            {
                ICellStyle style = workbook.CreateCellStyle();
                IFont font = workbook.CreateFont();

                if (ExcelFont != null)
                {
                    font.FontName = ExcelFont.FontName;
                    font.FontHeightInPoints = ExcelFont.FontHeightInPoints;
                    font.Color = ExcelFont.ColorIndex;
                    font.IsBold = ExcelFont.IsBold;
                    style.SetFont(font);
                }

                if (ExcelProperty != null)
                {
                    style.Alignment = ExcelProperty.HorizontalAlignment;
                    style.VerticalAlignment = ExcelProperty.VerticalAlignment;

                    if (ExcelProperty.ForegroundColor != HSSFColor.COLOR_NORMAL)
                    {
                        style.FillForegroundColor = ExcelProperty.ForegroundColor;
                        style.FillPattern = FillPattern.SolidForeground;
                    }

                    if (Type == ExcelType.Header)
                        style.BorderTop = ExcelProperty.BorderStyleTop;

                    style.BorderLeft = ExcelProperty.BorderStyleLeft;
                    style.BorderRight = ExcelProperty.BorderStyleRight;
                    style.BorderBottom = ExcelProperty.BorderStyleBottom;
                }

                Style = style;
                cell.CellStyle = Style;
            }
            else
            {
                cell.CellStyle = Style;
            }
        }
    }
    
    public enum ExcelType
    {
        Header,
        Data
    }

    public class ExcelFont
    {
        public string FontName = "Arial";
        public short FontHeightInPoints = 10;
        public short ColorIndex = HSSFColor.Black.Index;
        public bool IsBold = false;

        public ExcelFont()
        {}

        public ExcelFont(string FontName, short ColorIndex, short FontHeightInPoints = 10, bool IsBold = false)
        {
            this.FontName = FontName;
            this.FontHeightInPoints = FontHeightInPoints;
            this.ColorIndex = ColorIndex;
            this.IsBold = IsBold;
        }

        public ExcelFont(short ColorIndex, short FontHeightInPoints = 10, bool IsBold = false)
        {
            this.FontHeightInPoints = FontHeightInPoints;
            this.ColorIndex = ColorIndex;
            this.IsBold = IsBold;
        }

    }

    public class ExcelProperty
    {
        public HorizontalAlignment HorizontalAlignment = NPOI.SS.UserModel.HorizontalAlignment.Left;
        public VerticalAlignment VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Top;
        public BorderStyle BorderStyleTop = BorderStyle.None;
        public BorderStyle BorderStyleLeft = BorderStyle.None;
        public BorderStyle BorderStyleRight = BorderStyle.None;
        public BorderStyle BorderStyleBottom = BorderStyle.None;
        public short ForegroundColor = HSSFColor.COLOR_NORMAL;
        
        public ExcelProperty()
        {}

        public ExcelProperty(short ColorIndex, HorizontalAlignment h = NPOI.SS.UserModel.HorizontalAlignment.Left, VerticalAlignment v = NPOI.SS.UserModel.VerticalAlignment.Top)
        {
            this.HorizontalAlignment = h;
            this.VerticalAlignment = v;
            this.ForegroundColor = ColorIndex;
        }
    }


}
