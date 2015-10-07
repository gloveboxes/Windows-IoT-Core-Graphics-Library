namespace Glovebox.Graphics.Grid {

    /// <summary>
    /// NeoPixel Grid Privatives, builds on Frame Primatives
    /// </summary>
    public class GridBase : FrameBase {
        public readonly uint Columns;
        public readonly uint Rows;
        public readonly uint Panels;
        public readonly uint PixelsPerPanel;
        public readonly uint PixelsPerRow;



        public GridBase(uint columns, uint rows, uint panels)
            : base(columns * rows * (panels = panels < 1 ? (ushort)1 : panels)) {
            this.Columns = columns;
            this.Rows = rows;
            this.Panels = panels;
            PixelsPerPanel = rows * columns;
            PixelsPerRow = columns * panels;

            FrameClear();

        }


        public ushort PointPostion(uint row, uint column) {
            uint currentPanel, rowOffset;

            column = (ushort)(column % PixelsPerRow);
            row = (ushort)(row % Rows);

            currentPanel = column / Columns;
            rowOffset = (row * Columns) + (currentPanel * PixelsPerPanel);

            return (ushort)((column % Columns) + rowOffset);
        }

        public void PointColour(ushort row, ushort column, Pixel pixel) {
            ushort pixelNumber = PointPostion(row, column);
            Frame[pixelNumber] = pixel;
        }

        public override void FrameSet(Pixel pixel, int position) {
            if (position < 0) { return; }

            int currentRow = position / (int)(Panels * Columns);
            int currentColumn = position % (int)(Panels * Columns);
            Frame[PointPostion((uint)currentRow, (uint)currentColumn)] = pixel;
        }

        public void FrameSet(Pixel pixel, int position, int panel) {
            int pos = panel * (int)PixelsPerPanel + position;
            if (pos < 0 || pos >= Length) { return; }
            Frame[pos] = pixel;
        }



        public void ColumnRollRight(ushort rowIndex) {
            rowIndex = (ushort)(rowIndex % Rows);

            Pixel temp = Frame[PointPostion(rowIndex, (ushort)(PixelsPerRow - 1))];

            for (int col = (int)(PixelsPerRow - 1); col > 0; col--) {
                Frame[PointPostion(rowIndex, (uint)col)] = Frame[PointPostion(rowIndex, (uint)(col - 1))];
            }

            Frame[PointPostion(rowIndex, 0)] = temp;
        }

        public void ColumnRollLeft(ushort rowIndex) {
            rowIndex = (ushort)(rowIndex % Rows);

            Pixel temp = Frame[PointPostion(rowIndex, 0)];

            for (ushort col = 1; col < (ushort)(PixelsPerRow); col++) {
                Frame[PointPostion(rowIndex, (ushort)(col - 1))] = Frame[PointPostion(rowIndex, col)];
            }

            Frame[PointPostion(rowIndex, (ushort)(PixelsPerRow - 1))] = temp;
        }

        public void FrameRowDown() {
            for (uint i = 0; i < PixelsPerRow; i++) {
                ColumnRollDown(i);
            }
        }

        public void FrameRowUp() {
            for (uint i = 0; i < PixelsPerRow; i++) {
                ColumnRollUp(i);
            }
        }

        public void FrameRollRight() {
            for (ushort row = 0; row < Rows; row++) {
                ColumnRollRight(row);
            }
        }

        public void FrameRollLeft() {
            for (ushort row = 0; row < Rows; row++) {
                ColumnRollLeft(row);
            }
        }

        public void ShiftColumnRight(ushort rowIndex) {
            rowIndex = (ushort)(rowIndex % Rows);

            for (int col = (int)(PixelsPerRow - 1); col > 0; col--) {
                Frame[PointPostion(rowIndex, (uint)col)] = Frame[PointPostion(rowIndex, (uint)(col - 1))];
            }

            Frame[PointPostion(rowIndex, 0)] = Pixel.Colour.Black;
        }


        public void ShiftFrameRight() {
            for (ushort i = 0; i < Rows; i++) {
                ShiftColumnRight(i);
            }
        }

        public void ShiftFrameLeft() {
            for (ushort i = 0; i < Rows; i++) {
                ShiftColumnLeft(i);
            }
        }

        /// <summary>
        /// Panel aware scroll left
        /// </summary>
        /// <param name="rowIndex"></param>
        public void ShiftColumnLeft(uint rowIndex) {
            uint currentPanel, source = 0, destination, rowOffset, destinationColumn;

            rowIndex = rowIndex % Rows;

            for (uint sourceColumn = 1; sourceColumn < PixelsPerRow; sourceColumn++) {

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

        public void ColumnRollDown(uint columnIndex) {
            columnIndex = (ushort)(columnIndex % PixelsPerRow);

            Pixel temp = Frame[PointPostion(Rows - 1, columnIndex)];

            for (int row = (int)Rows - 2; row >= 0; row--) {
                Frame[PointPostion((uint)row + 1, columnIndex)] = Frame[PointPostion((uint)row, columnIndex)];
            }

            Frame[PointPostion(0, columnIndex)] = temp;
        }

        public void ColumnRollUp(uint columnIndex) {
            columnIndex = (ushort)(columnIndex % PixelsPerRow);

            Pixel temp = Frame[PointPostion(0, columnIndex)];

            for (int row = 1; row < Rows ; row++) {
                Frame[PointPostion((uint)row - 1, columnIndex)] = Frame[PointPostion((uint)row, columnIndex)];
            }

            Frame[PointPostion(Rows - 1, columnIndex)] = temp;
        }

        public void RowDrawLine(uint rowIndex, uint startColumnIndex, uint endColumnIndex) {
            RowDrawLine(rowIndex, startColumnIndex, endColumnIndex, Pixel.Mono.On);
        }

        public void RowDrawLine(uint rowIndex, uint startColumnIndex, uint endColumnIndex, Pixel pixel) {
            if (startColumnIndex > endColumnIndex) {
                uint temp = startColumnIndex;
                startColumnIndex = endColumnIndex;
                endColumnIndex = temp;
            }

            for (uint col = startColumnIndex; col <= endColumnIndex; col++) {
                Frame[PointPostion(rowIndex, col)] = pixel;
            }
        }

        public void RowDrawLine(ushort rowIndex) {
            RowDrawLine(rowIndex, Pixel.Mono.On);
        }

        public void RowDrawLine(uint rowIndex, Pixel pixel) {
            for (uint panel = 0; panel < Panels; panel++) {
                for (uint i = (panel * PixelsPerPanel) + rowIndex * Columns; i < (panel * PixelsPerPanel) + rowIndex * Columns + (Columns); i++) {
                    Frame[i] = pixel;
                }
            }
        }

        public void RowDrawLine(uint rowIndex, Pixel[] pixel) {
            for (uint panel = 0; panel < Panels; panel++) {
                for (uint i = (panel * PixelsPerPanel) + rowIndex * Columns; i < (panel * PixelsPerPanel) + rowIndex * Columns + (Columns); i++) {
                    Frame[i] = pixel[i % pixel.Length];
                }
            }
        }

        public void ColumnDrawLine(ushort columnIndex) {
            ColumnDrawLine(columnIndex, Pixel.Mono.On);
        }

        public void ColumnDrawLine(ushort columnIndex, Pixel pixel) {
            for (int r = 0; r < Rows; r++) {
                Frame[PointPostion((uint)r, columnIndex)] = pixel;
            }
        }

        public void ColumnDrawLine(ushort columnIndex, Pixel[] pixel) {
            for (int r = 0; r < Rows; r++) {
                Frame[PointPostion((uint)r, columnIndex)] = pixel[r % pixel.Length];
            }
        }

        public void DrawBox(int startRow, int startColumn, int width, int depth, Pixel pixel) {
            if (width <= 0 || depth <= 0) { return; }
            RowDrawLine((uint)startRow, (uint)startColumn, (uint)(startRow + width - 1));
            RowDrawLine((uint)(startRow + depth - 1), (uint)startColumn, (uint)(startRow + width - 1));
            for (int d = 1; d < depth - 1; d++) {
                Frame[PointPostion((uint)(startRow + d), (uint)startColumn)] = pixel;
                Frame[PointPostion((uint)(startRow + d), (uint)(startColumn + width - 1))] = pixel;
            }
        }



    }
}
