using LightLibrary;

namespace LightLibrary.Grid {

    /// <summary>
    /// NeoPixel Grid Privatives, builds on Frame Primatives
    /// </summary>
    public class GridBase : FrameBase {
        public ushort Columns { get; private set; }
        public ushort Rows { get; private set; }
        public ushort Panels { get; private set; }


        public GridBase(ushort columns, ushort rows, ushort panels)
            : base(columns * rows * (panels = panels < 1 ? (ushort)1 : panels)) {
            this.Columns = columns;
            this.Rows = rows;
            this.Panels = panels;

            FrameClear();

        }


        public ushort PointPostion(ushort row, ushort column) {
            int totalColumns = Columns * Panels;
            int currentPanel, rowOffset;
            int panelSize = Columns * Rows;

            column = (ushort)(column % totalColumns);
            row = (ushort)(row % Rows);

            currentPanel = column / Columns;
            rowOffset = (row * Columns) + (currentPanel * panelSize);

            return (ushort)((column % Columns) + rowOffset);
        }

        public void PointColour(ushort row, ushort column, Pixel pixel) {
            ushort pixelNumber = PointPostion(row, column);
            Frame[pixelNumber] = pixel;
        }

        public void ColumnRollRight(ushort rowIndex) {
            rowIndex = (ushort)(rowIndex % Rows);
            ushort totalColumns = (ushort)(Columns * Panels);

            Pixel temp = Frame[PointPostion(rowIndex, (ushort)(totalColumns - 1))];

            for (ushort col = (ushort)(totalColumns - 1); col > 0; col--) {
                Frame[PointPostion(rowIndex, col)] = Frame[PointPostion(rowIndex, (ushort)(col - 1))];
            }

            Frame[PointPostion(rowIndex, 0)] = temp;
        }

        public void ColumnRollLeft(ushort rowIndex) {
            rowIndex = (ushort)(rowIndex % Rows);
            ushort totalColumns = (ushort)(Columns * Panels);

            Pixel temp = Frame[PointPostion(rowIndex, 0)];      

            for (ushort col = 1; col < (ushort)(totalColumns); col++) {
                Frame[PointPostion(rowIndex, (ushort)(col - 1))] = Frame[PointPostion(rowIndex, col)]; 
            }

            Frame[PointPostion(rowIndex, (ushort)(totalColumns - 1))] = temp;
        }

        public void FrameRollRight() {
            for (ushort row = 0; row < Rows; row++) {
                ColumnRollRight(row);
            }
        }

        public void ShiftColumnRight(ushort rowIndex) {
            rowIndex = (ushort)(rowIndex % Rows);
            ushort totalColumns = (ushort)(Columns * Panels);

            for (ushort col = (ushort)(totalColumns - 1); col > 0; col--) {
                Frame[PointPostion(rowIndex, col)] = Frame[PointPostion(rowIndex, (ushort)(col - 1))];
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
        public void ShiftColumnLeft(ushort rowIndex) {
            int totalColumns = Columns * Panels;
            int currentPanel, source = 0, destination, rowOffset, destinationColumn;
            int panelSize = Columns * Rows;

            rowIndex = (ushort)(rowIndex % Rows);

            for (int sourceColumn = 1; sourceColumn < totalColumns; sourceColumn++) {

                currentPanel = sourceColumn / Columns;
                rowOffset = (rowIndex * Columns) + (currentPanel * panelSize);
                source = (sourceColumn % Columns) + rowOffset;

                destinationColumn = sourceColumn - 1;
                currentPanel = (destinationColumn) / Columns;
                rowOffset = (rowIndex * Columns) + (currentPanel * panelSize);
                destination = (destinationColumn % Columns) + rowOffset;

                Frame[destination] = Frame[source];
            }

            Frame[source] = Pixel.Colour.Black;
        }

        public void ColumnRollDown(ushort columnIndex) {
            int current;
            columnIndex = (ushort)(columnIndex % Columns);
            Pixel temp = Frame[Columns * (Rows - 1) + columnIndex];

            for (int row = Rows - 2; row >= 0; row--) {
                current = row * Columns + columnIndex;

                Frame[current + Rows] = Frame[current];
            }
            Frame[columnIndex] = temp;
        }

        public void RowDrawLine(ushort rowIndex, Pixel pixel) {
            for (int i = rowIndex * Columns; i < rowIndex * Columns + Columns; i++) {
                Frame[i] = pixel;
            }
        }

        public void RowDrawLine(ushort rowIndex, Pixel[] pixel) {
            for (int i = 0; i < rowIndex * Columns + Columns; i++) {
                Frame[i] = pixel[i % pixel.Length];
            }
        }

        public void ColumnDrawLine(ushort columnIndex, Pixel pixel) {
            for (int r = 0; r < Rows; r++) {
                Frame[columnIndex + (r * Columns)] = pixel;
            }
        }

        public void ColumnDrawLine(ushort columnIndex, Pixel[] pixel) {
            for (int r = 0; r < Rows; r++) {
                Frame[columnIndex + (r * Columns)] = pixel[r % pixel.Length];
            }
        }

        public void DrawBox(ushort startRow, ushort startColumn, ushort width, Pixel pixel) {
            if (width + startRow > Rows || width + startColumn > Columns) { return; }

            FrameSet(pixel, (ushort)(startRow * Columns + startColumn), width);

            int startPos = startRow * Columns + ((width - 1) * Columns + startColumn);

            FrameSet(pixel, (ushort)startPos, width);

            // draw sides of boxes
            for (ushort r = (ushort)(startRow + 1); r < startRow + width - 1; r++) {
                PointColour(r, startColumn, pixel);
                PointColour(r, (ushort)(width - 1 + startColumn), pixel);
            }
        }
    }
}
