using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace LogControl
{
    public partial class LogControl : UserControl
    {
        private readonly BindingList<LogEntryModel> _rows = new BindingList<LogEntryModel>();
        private bool _topLineDeleted;

        public LogControl()
        {
            InitializeComponent();

            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = _rows;

            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        internal void AddInformation(string message)
        {
            var entry = new LogEntryModel
            {
                Time = DateTime.Now,
                Message = message,
                Level =  LogLevel.Information,
            };

            AddLogEntry(entry);
        }

        internal void AddWarning(string message)
        {
            var entry = new LogEntryModel
            {
                Time = DateTime.Now,
                Message = message,
                Level = LogLevel.Warning,
            };

            AddLogEntry(entry);
        }

        internal void AddError(string message)
        {
            var entry = new LogEntryModel
            {
                Time = DateTime.Now,
                Message = message,
                Level = LogLevel.Error,
            };

            AddLogEntry(entry);
        }

        private void AddLogEntry(LogEntryModel entry)
        {
            _rows.Add(entry);
            bool topLineDeleted;
            if (_rows.Count > 20)
            {
                Debug.WriteLine("Удалена строка");
                _rows.RemoveAt(0);
                //if(dataGridView1.FirstDisplayedScrollingRowIndex > 0)
                //    dataGridView1.FirstDisplayedScrollingRowIndex--;

                //if (_topLineDeleted)
                {
                    topLineDeleted = true;
                }
                //else
                //{
                //    _topLineDeleted = true;
                //    topLineDeleted = false;
                //}
            }
            else
            {
                topLineDeleted = false;
            }

            DoAutoScroll(topLineDeleted);
        }

        private void DoAutoScroll(bool topLineDeleted)
        {
            EnsureVisibleRow(dataGridView1, _rows.Count - 1, topLineDeleted);
        }

        private static void EnsureVisibleRow(DataGridView view, int rowToShow, bool topLineDeleted)
        {
            if (rowToShow >= 0 && rowToShow < view.RowCount)
            {
                int countVisible = view.DisplayedRowCount(false);
                int firstVisible = view.FirstDisplayedScrollingRowIndex;

                if (rowToShow < firstVisible)
                {
                    Debug.WriteLine($"FirstDisplayedScrollingRowIndex = {rowToShow}");
                    view.FirstDisplayedScrollingRowIndex = rowToShow;
                }
                else if (rowToShow >= firstVisible + countVisible)
                {
                    int newRowIndex = rowToShow - countVisible + 1;
                    //if(topLineDeleted)
                    //{
                    //    newRowIndex++;
                    //}

                    // Автоскролим только если лифт находится в самом низу
                    // Иначе пользователь просматривает записи в истории и нельзя сбивать позицию скрола.
                    //if (newRowIndex - view.FirstDisplayedScrollingRowIndex <= 1)
                    {
                        if(topLineDeleted)
                        {
                            newRowIndex++;

                            //if (newRowIndex - view.FirstDisplayedScrollingRowIndex <= 2)
                            {
                                Debug.WriteLine($"FirstDisplayedScrollingRowIndex = {newRowIndex}");
                                view.FirstDisplayedScrollingRowIndex = newRowIndex;
                            }
                            //else
                            //{
                            //    Debug.WriteLine($"FirstDisplayedScrollingRowIndex--");
                            //    view.FirstDisplayedScrollingRowIndex--;
                            //}
                        }
                        else
                        {
                            if (newRowIndex - view.FirstDisplayedScrollingRowIndex <= 1)
                            {
                                Debug.WriteLine($"FirstDisplayedScrollingRowIndex = {newRowIndex}");
                                view.FirstDisplayedScrollingRowIndex = newRowIndex;
                            }
                        }
                    }
                    //else if (topLineDeleted && view.FirstDisplayedScrollingRowIndex > 0)
                    //// Пользователь смотрит историю и из-за удаляющихся строк нужно восстанавливать позицию.
                    //{
                    //    view.FirstDisplayedScrollingRowIndex--;
                    //}
                }
            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if(e.Value is LogLevel logLevel)
            {
                switch (logLevel)
                {
                    case LogLevel.Verbose:
                        {
                            e.Value = "VRB";
                            e.FormattingApplied = true;
                        }
                        break;
                    case LogLevel.Debug:
                        {
                            e.Value = "DBG";
                            e.FormattingApplied = true;
                        }
                        break;
                    case LogLevel.Information:
                        {
                            e.Value = "INF";
                            e.FormattingApplied = true;
                        }
                        break;
                    case LogLevel.Warning:
                        {
                            e.Value = "WRN";
                            e.CellStyle.ForeColor = Color.Yellow;
                            e.CellStyle.SelectionForeColor = Color.Yellow;
                            e.FormattingApplied = true;
                        }
                        break;
                    case LogLevel.Error:
                        {
                            e.Value = "ERR";
                            e.CellStyle.ForeColor = Color.White;
                            e.CellStyle.SelectionForeColor = Color.White;
                            e.FormattingApplied = true;
                        }
                        break;
                    case LogLevel.Fatal:
                        {
                            e.Value = "ERR";
                            e.CellStyle.ForeColor = Color.White;
                            e.CellStyle.SelectionForeColor = Color.White;
                            e.FormattingApplied = true;
                        }
                        break;
                }
            }
        }

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // High light and searching apply over selective fields of grid.  
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                if (e.Value is LogLevel logLevel)
                {
                    switch (logLevel)
                    {
                        case LogLevel.Debug:
                            break;
                        case LogLevel.Information:
                            {
                                break;
                            }
                        case LogLevel.Warning:
                            break;
                        case LogLevel.Error:
                            {
                                // Check data for search  
                                //if (HighlightText?.Count > 0 && HighlightColor?.Count > 0 && HighlightColor.Count >= HighlightText.Count)
                                {
                                    string gridCellValue = e.FormattedValue as string;
                                    //int startIndexInCellValue;
                                    // check the index of search text into grid cell.  

                                    bool BackgroundPainted = false;
                                    //for (int i = 0; i < HighlightText.Count; i++)
                                    {
                                        //if ((startIndexInCellValue = gridCellValue.ToLower().IndexOf(HighlightText[i])) >= 0)
                                        {
                                            if (BackgroundPainted == false)
                                            {
                                                e.Handled = true;
                                                e.PaintBackground(e.CellBounds, true);
                                                BackgroundPainted = true;
                                            }
                                            //the highlite rectangle  
                                            var hl_rect = new Rectangle();
                                            hl_rect.Y = e.CellBounds.Y + 4;
                                            //hl_rect.Height = e.CellBounds.Height - 5;
                                            //find the size of the text before the search word in grid cell data.  
                                            //string sBeforeSearchword = gridCellValue.Substring(0, startIndexInCellValue);
                                            //size of the search word in the grid cell data  
                                            //string sSearchWord = gridCellValue.Substring(startIndexInCellValue, HighlightText[i].Length);
                                            Size s1 = TextRenderer.MeasureText(e.Graphics, gridCellValue, e.CellStyle.Font, e.CellBounds.Size);
                                            //Size s2 = TextRenderer.MeasureText(e.Graphics, sSearchWord, e.CellStyle.Font, e.CellBounds.Size);
                                            
                                                hl_rect.X = e.CellBounds.X + e.CellStyle.Padding.Left; /*+ s1.Width - 5*/;
                                                hl_rect.Height = s1.Height + 1;
                                                hl_rect.Width = s1.Width;
                                            
                                            //var hl_brush = new SolidBrush(HighlightColor[i]);
                                            //paint the background behind the search word  
                                            e.Graphics.FillRectangle(Brushes.Red, hl_rect);
                                            //hl_brush.Dispose();
                                        }
                                        //This was the wrong position
                                        //if (BackgroundPainted) { e.PaintContent(e.CellBounds); } 
                                    }
                                    //This is the right position
                                    if (BackgroundPainted) { e.PaintContent(e.CellBounds); }
                                }
                                //SizeF textSize = e.Graphics.MeasureString(e.FormattedValue as string, e.CellStyle.Font);
                                //e.Graphics.FillRectangle(Brushes.Red, e.CellBounds);
                                //e.Handled = true;
                                //e.PaintParts = DataGridViewPaintParts.
                                break;
                            }
                        case LogLevel.Fatal:
                            break;
                    }
                }
            }
        }

        //private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        //{
        //    if (e.Value is LogLevel logLevel)
        //    {
        //        switch (logLevel)
        //        {
        //            case LogLevel.Debug:
        //                break;
        //            case LogLevel.Information:
        //                {
        //                    break;
        //                }
        //            case LogLevel.Warning:
        //                break;
        //            case LogLevel.Error:
        //                {
        //                    Rectangle newRect = new Rectangle(e.CellBounds.X + 1,
        //                        e.CellBounds.Y + 1, e.CellBounds.Width - 4,
        //                        e.CellBounds.Height - 4);

        //                    using (
        //                        Brush gridBrush = new SolidBrush(this.dataGridView1.GridColor),
        //                        backColorBrush = new SolidBrush(e.CellStyle.BackColor))
        //                    {
        //                        using (var gridLinePen = new Pen(gridBrush))
        //                        {
        //                            // Erase the cell.
        //                            e.Graphics.FillRectangle(backColorBrush, e.CellBounds);

        //                            //// Draw the grid lines (only the right and bottom lines;
        //                            //// DataGridView takes care of the others).
        //                            //e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left,
        //                            //    e.CellBounds.Bottom - 1, e.CellBounds.Right - 1,
        //                            //    e.CellBounds.Bottom - 1);
        //                            //e.Graphics.DrawLine(gridLinePen, e.CellBounds.Right - 1,
        //                            //    e.CellBounds.Top, e.CellBounds.Right - 1,
        //                            //    e.CellBounds.Bottom);

        //                            //// Draw the inset highlight box.
        //                            e.Graphics.DrawRectangle(Pens.Blue, newRect);

        //                            // Draw the text content of the cell, ignoring alignment.
        //                            if (e.FormattedValue != null)
        //                            {
        //                                SizeF textSize = e.Graphics.MeasureString(e.FormattedValue as string, e.CellStyle.Font);
        //                                e.Graphics.FillRectangle(Brushes.Red, e.CellBounds.X, e.CellBounds.Y, textSize.Width, textSize.Height);

        //                                e.Graphics.DrawString(e.FormattedValue as string, e.CellStyle.Font,
        //                                    Brushes.White, e.CellBounds.X + 2,
        //                                    e.CellBounds.Y + 2, StringFormat.GenericDefault);
        //                            }
        //                            e.Handled = true;
        //                        }
        //                    }

        //                    //SizeF textSize = e.Graphics.MeasureString(e.FormattedValue as string, e.CellStyle.Font);
        //                    //e.Graphics.FillRectangle(Brushes.Red, e.CellBounds);
        //                    //e.Handled = true;
        //                    //e.PaintParts = DataGridViewPaintParts.
        //                    break;
        //                }
        //            case LogLevel.Fatal:
        //                break;
        //        }
        //    }

        //    //e.Graphics.MeasureString()
        //}
    }

    public sealed class LogEntryModel
    {
        public DateTime Time { get; internal set; }
        public LogLevel Level { get; internal set; }
        public string Message { get; internal set; }
    }

    // Не менять порядок значений.
    public enum LogLevel
    {
        Verbose,
        Debug,
        Information,
        Warning,
        Error,
        Fatal,
    }
}
