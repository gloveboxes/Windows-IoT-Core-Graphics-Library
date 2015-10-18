using System;

namespace Glovebox.Graphics.Grid {

    /// <summary>
    /// Grid Privatives, builds on Frame Primatives
    /// </summary>
    public class GridBase : FrameBase {
        public readonly int ColumnsPerPanel;
        public readonly int RowsPerPanel;
        public readonly int PixelsPerPanel;
        protected readonly int NumberOfPanels;
        public readonly int ColumnsPerFrame;
        public readonly int RowsPerFrame;



        public GridBase(int columnsPerPanel, int rowsPerPanel, int panelsPerFrame)
            : base(columnsPerPanel * rowsPerPanel * (panelsPerFrame = panelsPerFrame < 1 ? 1 : panelsPerFrame)) {

            if (columnsPerPanel < 0 || rowsPerPanel < 0 ) {
                throw new Exception("invalid columns, rows or panels specified");
            }

            this.ColumnsPerPanel = columnsPerPanel;
            this.RowsPerPanel = rowsPerPanel;
            this.NumberOfPanels = panelsPerFrame;
            PixelsPerPanel = rowsPerPanel * columnsPerPanel;
            ColumnsPerFrame = columnsPerPanel * panelsPerFrame;
            RowsPerFrame = rowsPerPanel; // for now only support horizontal frame layout

            FrameClear();
        }


        public ushort PointPostion(int row, int column) {
            if (row < 0 || column < 0) { return 0; }

            int currentPanel, rowOffset;

            column = (ushort)(column % ColumnsPerFrame);
            row = (ushort)(row % RowsPerPanel);

            currentPanel = column / ColumnsPerPanel;
            rowOffset = (row * ColumnsPerPanel) + (currentPanel * PixelsPerPanel);

            return (ushort)((column % ColumnsPerPanel) + rowOffset);
        }

        public void PointColour(int row, int column, Pixel pixel) {
            if (row < 0 || column < 0) { return; }

            ushort pixelNumber = PointPostion(row, column);
            Frame[pixelNumber] = pixel;
        }

        public override void FrameSet(Pixel pixel, int position) {
            if (position < 0) { return; }

            int currentRow = position / (int)(NumberOfPanels * ColumnsPerPanel);
            int currentColumn = position % (int)(NumberOfPanels * ColumnsPerPanel);
            Frame[PointPostion(currentRow, currentColumn)] = pixel;
        }

        public new void FrameSet(Pixel pixel, int position, int panel) {
            int pos = panel * (int)PixelsPerPanel + position;
            if (pos < 0 || pos >= Length) { return; }
            Frame[pos] = pixel;
        }

        public void ColumnRollRight(int rowIndex) {
            if (rowIndex < 0) { return; }

            rowIndex = (ushort)(rowIndex % RowsPerPanel);

            Pixel temp = Frame[PointPostion(rowIndex, ColumnsPerFrame - 1)];

            for (int col = (int)(ColumnsPerFrame - 1); col > 0; col--) {
                Frame[PointPostion(rowIndex, col)] = Frame[PointPostion(rowIndex, (col - 1))];
            }

            Frame[PointPostion(rowIndex, 0)] = temp;
        }

        public void ColumnRollLeft(int rowIndex) {
            if (rowIndex < 0) { return; }

            rowIndex = rowIndex % RowsPerPanel;

            Pixel temp = Frame[PointPostion(rowIndex, 0)];

            for (int col = 1; col < ColumnsPerFrame; col++) {
                Frame[PointPostion(rowIndex, col - 1)] = Frame[PointPostion(rowIndex, col)];
            }

            Frame[PointPostion(rowIndex, ColumnsPerFrame - 1)] = temp;
        }

        public void FrameRollDown() {
            for (int i = 0; i < ColumnsPerFrame; i++) {
                ColumnRollDown(i);
            }
        }

        public void FrameRollUp() {
            for (int i = 0; i < ColumnsPerFrame; i++) {
                ColumnRollUp(i);
            }
        }

        public void FrameRollRight() {
            for (int row = 0; row < RowsPerPanel; row++) {
                ColumnRollRight(row);
            }
        }

        public void FrameRollLeft() {
            for (int row = 0; row < RowsPerPanel; row++) {
                ColumnRollLeft(row);
            }
        }

        public void ColumnRightShift(int rowIndex) {
            if (rowIndex < 0) { return; }

            rowIndex = (ushort)(rowIndex % RowsPerPanel);

            for (int col = (int)(ColumnsPerFrame - 1); col > 0; col--) {
                Frame[PointPostion(rowIndex, col)] = Frame[PointPostion(rowIndex, col - 1)];
            }

            Frame[PointPostion(rowIndex, 0)] = Colour.Black;
        }


        public void FrameShiftRight() {
            for (int i = 0; i < RowsPerPanel; i++) {
                ColumnRightShift(i);
            }
        }

        public void FrameShiftLeft() {
            for (int i = 0; i < RowsPerPanel; i++) {
                ColumnShiftLeft(i);
            }
        }

        /// <summary>
        /// Panel aware scroll left
        /// </summary>
        /// <param name="rowIndex"></param>
        public void ColumnShiftLeft(int rowIndex) {
            if (rowIndex < 0) { return; }

            int currentPanel, source = 0, destination, rowOffset, destinationColumn;

            rowIndex = rowIndex % RowsPerPanel;

            for (int sourceColumn = 1; sourceColumn < ColumnsPerFrame; sourceColumn++) {

                currentPanel = sourceColumn / ColumnsPerPanel;
                rowOffset = (rowIndex * ColumnsPerPanel) + (currentPanel * PixelsPerPanel);
                source = (sourceColumn % ColumnsPerPanel) + rowOffset;

                destinationColumn = sourceColumn - 1;
                currentPanel = (destinationColumn) / ColumnsPerPanel;
                rowOffset = (rowIndex * ColumnsPerPanel) + (currentPanel * PixelsPerPanel);
                destination = (destinationColumn % ColumnsPerPanel) + rowOffset;

                Frame[destination] = Frame[source];
            }

            Frame[source] = Colour.Black;
        }

        public void ColumnRollDown(int columnIndex) {
            if (columnIndex < 0) { return; }

            columnIndex = (ushort)(columnIndex % ColumnsPerFrame);

            Pixel temp = Frame[PointPostion(RowsPerPanel - 1, columnIndex)];

            for (int row = (int)RowsPerPanel - 2; row >= 0; row--) {
                Frame[PointPostion(row + 1, columnIndex)] = Frame[PointPostion(row, columnIndex)];
            }

            Frame[PointPostion(0, columnIndex)] = temp;
        }

        public void ColumnRollUp(int columnIndex) {
            if (columnIndex < 0) { return; }

            columnIndex = (ushort)(columnIndex % ColumnsPerFrame);

            Pixel temp = Frame[PointPostion(0, columnIndex)];

            for (int row = 1; row < RowsPerPanel ; row++) {
                Frame[PointPostion(row - 1, columnIndex)] = Frame[PointPostion(row, columnIndex)];
            }

            Frame[PointPostion(RowsPerPanel - 1, columnIndex)] = temp;
        }

        public void RowDrawLine(int rowIndex, int startColumnIndex, int endColumnIndex) {
            RowDrawLine(rowIndex, startColumnIndex, endColumnIndex, Led.On);
        }

        public void RowDrawLine(int rowIndex, int startColumnIndex, int endColumnIndex, Pixel pixel) {
            if (rowIndex < 0 || startColumnIndex < 0 || endColumnIndex < 0) { return; }

            if (startColumnIndex > endColumnIndex) {
                int temp = startColumnIndex;
                startColumnIndex = endColumnIndex;
                endColumnIndex = temp;
            }

            for (int col = startColumnIndex; col <= endColumnIndex; col++) {
                Frame[PointPostion(rowIndex, col)] = pixel;
            }
        }

        public void RowDrawLine(int rowIndex) {
            RowDrawLine(rowIndex, Led.On);
        }

        public void RowDrawLine(int rowIndex, Pixel pixel) {
            if (rowIndex < 0) { return; }

            for (int panel = 0; panel < NumberOfPanels; panel++) {
                for (int i = (panel * PixelsPerPanel) + rowIndex * ColumnsPerPanel; i < (panel * PixelsPerPanel) + rowIndex * ColumnsPerPanel + (ColumnsPerPanel); i++) {
                    Frame[i] = pixel;
                }
            }
        }

        public void RowDrawLine(int rowIndex, Pixel[] pixel) {
            if (rowIndex < 0) { return; }

            for (int panel = 0; panel < NumberOfPanels; panel++) {
                for (int i = (panel * PixelsPerPanel) + rowIndex * ColumnsPerPanel; i < (panel * PixelsPerPanel) + rowIndex * ColumnsPerPanel + (ColumnsPerPanel); i++) {
                    Frame[i] = pixel[i % pixel.Length];
                }
            }
        }

        public void ColumnDrawLine(int columnIndex) {
            ColumnDrawLine(columnIndex, Led.On);
        }

        public void ColumnDrawLine(int columnIndex, Pixel pixel) {
            if (columnIndex < 0) { return; }

            for (int r = 0; r < RowsPerPanel; r++) {
                Frame[PointPostion(r, columnIndex)] = pixel;
            }
        }

        public void ColumnDrawLine(int columnIndex, Pixel[] pixel) {
            if (columnIndex < 0) { return; }

            for (int r = 0; r < RowsPerPanel; r++) {
                Frame[PointPostion(r, columnIndex)] = pixel[r % pixel.Length];
            }
        }

        public void DrawBox(int startRow, int startColumn, int width, int depth) {
            DrawBox(startRow, startColumn, width, depth, Led.On);
        }


        public void DrawBox(int startRow, int startColumn, int width, int depth, Pixel pixel) {
            if (startRow < 0 || startColumn < 0 || width <= 0 || depth <= 0) { return; }

            RowDrawLine(startRow, startColumn, startRow + width - 1);
            RowDrawLine(startRow + depth - 1, startColumn, startRow + width - 1);
            for (int d = 1; d < depth - 1; d++) {
                Frame[PointPostion(startRow + d, startColumn)] = pixel;
                Frame[PointPostion(startRow + d, startColumn + width - 1)] = pixel;
            }
        }
    }
}
