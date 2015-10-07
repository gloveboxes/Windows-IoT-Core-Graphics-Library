using System;

namespace Glovebox.Graphics.Grid {

    /// <summary>
    /// NeoPixel Grid Privatives, builds on Frame Primatives
    /// </summary>
    public class GridBase : FrameBase {
        public readonly int Columns;
        public readonly int Rows;
        public readonly int Panels;
        public readonly int PixelsPerPanel;
        public readonly int PixelsPerRow;



        public GridBase(int columns, int rows, int panels)
            : base(columns * rows * (panels = panels < 1 ? (ushort)1 : panels)) {

            if (columns < 0 || rows < 0 || panels < 1) {
                throw new Exception("invalid columns, rows or panels specified");
            }

            this.Columns = columns;
            this.Rows = rows;
            this.Panels = panels;
            PixelsPerPanel = rows * columns;
            PixelsPerRow = columns * panels;

            FrameClear();

        }


        public ushort PointPostion(int row, int column) {
            if (row < 0 || column < 0) { return 0; }

            int currentPanel, rowOffset;

            column = (ushort)(column % PixelsPerRow);
            row = (ushort)(row % Rows);

            currentPanel = column / Columns;
            rowOffset = (row * Columns) + (currentPanel * PixelsPerPanel);

            return (ushort)((column % Columns) + rowOffset);
        }

        public void PointColour(int row, int column, Pixel pixel) {
            if (row < 0 || column < 0) { return; }

            ushort pixelNumber = PointPostion(row, column);
            Frame[pixelNumber] = pixel;
        }

        public override void FrameSet(Pixel pixel, int position) {
            if (position < 0) { return; }

            int currentRow = position / (int)(Panels * Columns);
            int currentColumn = position % (int)(Panels * Columns);
            Frame[PointPostion(currentRow, currentColumn)] = pixel;
        }

        public void FrameSet(Pixel pixel, int position, int panel) {
            int pos = panel * (int)PixelsPerPanel + position;
            if (pos < 0 || pos >= Length) { return; }
            Frame[pos] = pixel;
        }

        public void ColumnRollRight(int rowIndex) {
            if (rowIndex < 0) { return; }

            rowIndex = (ushort)(rowIndex % Rows);

            Pixel temp = Frame[PointPostion(rowIndex, (ushort)(PixelsPerRow - 1))];

            for (int col = (int)(PixelsPerRow - 1); col > 0; col--) {
                Frame[PointPostion(rowIndex, col)] = Frame[PointPostion(rowIndex, (col - 1))];
            }

            Frame[PointPostion(rowIndex, 0)] = temp;
        }

        public void ColumnRollLeft(int rowIndex) {
            if (rowIndex < 0) { return; }

            rowIndex = rowIndex % Rows;

            Pixel temp = Frame[PointPostion(rowIndex, 0)];

            for (int col = 1; col < PixelsPerRow; col++) {
                Frame[PointPostion(rowIndex, col - 1)] = Frame[PointPostion(rowIndex, col)];
            }

            Frame[PointPostion(rowIndex, (ushort)(PixelsPerRow - 1))] = temp;
        }

        public void FrameRowDown() {
            for (int i = 0; i < PixelsPerRow; i++) {
                ColumnRollDown(i);
            }
        }

        public void FrameRowUp() {
            for (int i = 0; i < PixelsPerRow; i++) {
                ColumnRollUp(i);
            }
        }

        public void FrameRollRight() {
            for (int row = 0; row < Rows; row++) {
                ColumnRollRight(row);
            }
        }

        public void FrameRollLeft() {
            for (int row = 0; row < Rows; row++) {
                ColumnRollLeft(row);
            }
        }

        public void ShiftColumnRight(int rowIndex) {
            if (rowIndex < 0) { return; }

            rowIndex = (ushort)(rowIndex % Rows);

            for (int col = (int)(PixelsPerRow - 1); col > 0; col--) {
                Frame[PointPostion(rowIndex, col)] = Frame[PointPostion(rowIndex, col - 1)];
            }

            Frame[PointPostion(rowIndex, 0)] = Pixel.Colour.Black;
        }


        public void ShiftFrameRight() {
            for (int i = 0; i < Rows; i++) {
                ShiftColumnRight(i);
            }
        }

        public void ShiftFrameLeft() {
            for (int i = 0; i < Rows; i++) {
                ShiftColumnLeft(i);
            }
        }

        /// <summary>
        /// Panel aware scroll left
        /// </summary>
        /// <param name="rowIndex"></param>
        public void ShiftColumnLeft(int rowIndex) {
            if (rowIndex < 0) { return; }

            int currentPanel, source = 0, destination, rowOffset, destinationColumn;

            rowIndex = rowIndex % Rows;

            for (int sourceColumn = 1; sourceColumn < PixelsPerRow; sourceColumn++) {

                currentPanel = sourceColumn / Columns;
                rowOffset = (rowIndex * Columns) + (currentPanel * PixelsPerPanel);
                source = (sourceColumn % Columns) + rowOffset;

                destinationColumn = sourceColumn - 1;
                currentPanel = (destinationColumn) / Columns;
                rowOffset = (rowIndex * Columns) + (currentPanel * PixelsPerPanel);
                destination = (destinationColumn % Columns) + rowOffset;

                Frame[destination] = Frame[source];
            }

            Frame[source] = Pixel.Colour.Black;
        }

        public void ColumnRollDown(int columnIndex) {
            if (columnIndex < 0) { return; }

            columnIndex = (ushort)(columnIndex % PixelsPerRow);

            Pixel temp = Frame[PointPostion(Rows - 1, columnIndex)];

            for (int row = (int)Rows - 2; row >= 0; row--) {
                Frame[PointPostion(row + 1, columnIndex)] = Frame[PointPostion(row, columnIndex)];
            }

            Frame[PointPostion(0, columnIndex)] = temp;
        }

        public void ColumnRollUp(int columnIndex) {
            if (columnIndex < 0) { return; }

            columnIndex = (ushort)(columnIndex % PixelsPerRow);

            Pixel temp = Frame[PointPostion(0, columnIndex)];

            for (int row = 1; row < Rows ; row++) {
                Frame[PointPostion(row - 1, columnIndex)] = Frame[PointPostion(row, columnIndex)];
            }

            Frame[PointPostion(Rows - 1, columnIndex)] = temp;
        }

        public void RowDrawLine(int rowIndex, int startColumnIndex, int endColumnIndex) {
            RowDrawLine(rowIndex, startColumnIndex, endColumnIndex, Pixel.Mono.On);
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
            RowDrawLine(rowIndex, Pixel.Mono.On);
        }

        public void RowDrawLine(int rowIndex, Pixel pixel) {
            if (rowIndex < 0) { return; }

            for (int panel = 0; panel < Panels; panel++) {
                for (int i = (panel * PixelsPerPanel) + rowIndex * Columns; i < (panel * PixelsPerPanel) + rowIndex * Columns + (Columns); i++) {
                    Frame[i] = pixel;
                }
            }
        }

        public void RowDrawLine(int rowIndex, Pixel[] pixel) {
            if (rowIndex < 0) { return; }

            for (int panel = 0; panel < Panels; panel++) {
                for (int i = (panel * PixelsPerPanel) + rowIndex * Columns; i < (panel * PixelsPerPanel) + rowIndex * Columns + (Columns); i++) {
                    Frame[i] = pixel[i % pixel.Length];
                }
            }
        }

        public void ColumnDrawLine(int columnIndex) {
            ColumnDrawLine(columnIndex, Pixel.Mono.On);
        }

        public void ColumnDrawLine(int columnIndex, Pixel pixel) {
            if (columnIndex < 0) { return; }

            for (int r = 0; r < Rows; r++) {
                Frame[PointPostion(r, columnIndex)] = pixel;
            }
        }

        public void ColumnDrawLine(int columnIndex, Pixel[] pixel) {
            if (columnIndex < 0) { return; }

            for (int r = 0; r < Rows; r++) {
                Frame[PointPostion(r, columnIndex)] = pixel[r % pixel.Length];
            }
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
