namespace Glovebox.Graphics {


    /// <summary>
    /// Frame primatives - generic across Rings, Stips and Grids
    /// </summary>
    public class FrameBase {

        #region Pixel Colour Definitions

        /// <summary>
        /// NeoPixels run medium bright and cool on this palette
        /// </summary>
        public static Pixel[] PaletteWarmLowPower = new Pixel[] {
            Colour.WarmRed,
            Colour.WarmOrange,
            Colour.WarmYellow,
            Colour.WarmGreen,
            Colour.WarmBlue,
            Colour.WarmPurple, 
            //Pixel.ColourLowPower.WarmIndigo
        };

        /// <summary>
        /// NeoPixels run dim and cool on this palette
        /// </summary>
        public static Pixel[] PaletteCoolLowPower = new Pixel[] {
            Colour.CoolRed,
            Colour.CoolOrange,
            Colour.CoolYellow,
            Colour.CoolGreen,
            Colour.CoolBlue,
            Colour.CoolPurple,
        };

        /// <summary>
        /// NeoPixels run bright but cool on this palette
        /// </summary>
        public static  Pixel[] PaletteHotLowPower = new Pixel[] {
            Colour.HotRed,
            Colour.HotOrange,
            Colour.HotYellow,
            Colour.HotGreen,
            Colour.HotBlue,
            Colour.HotPurple,
        };

        protected static Pixel[] PaletteFullColour = new Pixel[]
        {
            Colour.White,
            Colour.Black,
            Colour.Red,
            Colour.Orange,
            Colour.Yellow,
            Colour.Green,
            Colour.Purple,
            Colour.Blue,
        };
        #endregion


        private readonly int pixelCount;

        public int Length {
            get { return pixelCount; }
        }


        public Pixel[] Frame { get; set; }

        private Pixel[] blinkFrame;
        public FrameBase(int _pixelCount) {
            pixelCount = _pixelCount;
            Frame = new Pixel[pixelCount];

            //lazy initialise in the black/blank in the blink method
            blinkFrame = new Pixel[pixelCount];

            // init frame to all black - specifically not null
            FrameClear();
        }

        #region Primitive Frame Manipulation Methods
        public void FrameClear() {
            FrameSet(Colour.Black);
        }

        /// <summary>
        /// Fill entire frame with one colour
        /// </summary>
        /// <param name="pixel"></param>
        public void FrameSet(Pixel pixel) {
            for (int i = 0; i < Frame.Length; i++) {
                Frame[i] = pixel;
            }
        }

        /// <summary>
        /// Fill entire frame with one colour
        /// </summary>
        /// <param name="pixel"></param>
        public virtual void FrameSet(Pixel pixel, int position) {
            if (position < 0) { return; }

            Frame[position % Length] = pixel;
        }

        /// <summary>
        /// set specific frame pixels a colour - useful for letters on grids, patterns etc
        /// </summary>
        /// <param name="colour"></param>
        /// <param name="pixelPos"></param>
        public void FrameSet(Pixel colour, int[] pixelPos) {
            for (int i = 0; i < pixelPos.Length; i++) {
                if (pixelPos[i] < 0 || pixelPos[i] >= Frame.Length) { continue; }
                Frame[pixelPos[i]] = colour;
            }
        }

        /// <summary>
        /// set specific frame pixels from a rolling palette of colours
        /// </summary>
        /// <param name="pixelPos"></param>
        /// <param name="palette"></param>
        public void FrameSet(Pixel[] palette, int[] pixelPos) {
            for (int i = 0; i < pixelPos.Length; i++) {
                if (pixelPos[i] < 0 || pixelPos[i] >= Frame.Length) { continue; }
                Frame[pixelPos[i]] = palette[i % palette.Length];
            }
        }


        /// <summary>
        /// fill frame pixels from a specified position and repeat 
        /// </summary>
        /// <param name="pixel"></param>
        /// <param name="startPos"></param>
        /// <param name="repeat"></param>
        public void FrameSet(Pixel pixel, int startPos, int repeat = 1) {
            if (startPos < 0 | repeat < 0) { return; }

            for (int i = startPos, r = 0; r < repeat; i++, r++) {
                Frame[i % Frame.Length] = pixel;
            }
        }

        /// <summary>
        /// fill frame pixels from a specified position and repeat from a palette of colours
        /// </summary>
        /// <param name="pixel"></param>
        /// <param name="startPos"></param>
        /// <param name="repeat"></param>
        public void FrameSet(Pixel[] pixel, int startPos, int repeat = 1) {
            if (startPos < 0 | repeat < 0) { return; }

            for (int i = startPos, r = 0; r < repeat; i++, r++) {
                Frame[i % Frame.Length] = pixel[i % pixel.Length];
            }
        }

        /// <summary>
        /// fill frame from a rolling pallet
        /// </summary>
        /// <param name="palette"></param>
        public void FrameSet(Pixel[] palette) {
            for (int i = 0; i < Frame.Length; i++) {
                Frame[i] = palette[i % palette.Length];
            }
        }

        /// <summary>
        /// fill frame with blocks of colour from a palette
        /// </summary>
        /// <param name="palette"></param>
        public void FrameSetBlocks(Pixel[] palette) {
            if (palette == null || palette.Length == 0) {
                FrameClear();
            }
            else if (palette.Length >= pixelCount) {
                FrameSet(palette);
            }
            else {
                var leftovers = pixelCount % palette.Length;
                int leftoversUsed = 0;
                int thisPixel = 0;
                uint baseBlockSize = (uint)(pixelCount / palette.Length);
                for (int i = 0; i < palette.Length; i++) {
                    for (int j = 0; j < baseBlockSize; j++) {
                        Frame[thisPixel] = palette[i];
                        thisPixel++;
                    }
                    if (leftoversUsed < leftovers) {
                        Frame[thisPixel] = palette[i];
                        thisPixel++;
                        leftoversUsed++;
                    }
                }
            }
        }


        /// <summary>
        /// Swap specified pixels with wrap
        /// </summary>
        /// <param name="pixel1"></param>
        /// <param name="pixel2"></param>
        public void FramePixelSwap(int pixel1, int pixel2) {
            if (pixel1 < 0 | pixel2 < 0) { return; }

            Pixel temp = Frame[pixel2 % pixelCount];
            Frame[pixel2 % pixelCount] = Frame[pixel1 % pixelCount];
            Frame[pixel1 % pixelCount] = temp;
        }

        public void FramePixelForward(int pixelIndex, int stepSize = 1) {
            if (pixelIndex < 0 | stepSize < 0) { return; }

            if (pixelIndex >= Frame.Length) { return; }

            int length = Frame.Length;
            int newIndex = (pixelIndex + stepSize) % length;

            Pixel p = Frame[newIndex];
            Frame[newIndex] = Frame[pixelIndex];
            Frame[pixelIndex] = p;
        }


        /// <summary>
        /// Shift wrap forward a block of pixels by specified amount
        /// </summary>
        /// <param name="blockSize"></param>
        public void FrameShiftForward(int blockSize = 1) {
            if (blockSize < 0) { return; }

            blockSize = blockSize % Length;

            int i;
            Pixel[] temp = new Pixel[blockSize];

            for (i = 0; i < blockSize; i++) {
                temp[i] = Frame[Frame.Length - blockSize + i];
            }

            for (i = Frame.Length - 1; i >= blockSize; i--) {
                Frame[i] = Frame[i - blockSize];
            }

            for (i = 0; i < blockSize; i++) {
                Frame[i] = temp[i];
            }
        }

        /// <summary>
        /// Shift wrap forward a block of pixels by specified amount
        /// </summary>
        /// <param name="blockSize"></param>
        public void FrameShiftBack(int blockSize = 1) {
            if (blockSize < 0) { return; }

            blockSize = blockSize % Length;

            int i;
            Pixel[] temp = new Pixel[blockSize];

            for (i = 0; i < blockSize; i++) {
                temp[i] = Frame[i];
            }

            for (i = blockSize; i < Frame.Length; i++) {
                Frame[i - blockSize] = Frame[i];
            }

            for (i = 0; i < blockSize; i++) {
                int p = Frame.Length - blockSize + i;
                Frame[p] = temp[i];
            }
        }


        /// <summary>
        /// cycle the pixels moving them up by increment pixels
        /// </summary>
        /// <param name="increment">number of positions to shift. Negative numbers backwards. If this is more than the number of LEDs, the result wraps</param>
        public void FrameShift(int increment = 1) {
            if (increment > 0) { FrameShiftForward(increment); }
            else if (increment < 0) { FrameShiftBack(System.Math.Abs(increment)); }
        }

        /// <summary>
        /// Forces an update with the current contents of currentDisplay
        /// </summary>
        public void FrameDraw() {
            FrameDraw(Frame);
        }

        protected virtual void FrameDraw(Pixel[] frame) {
        }

        #endregion

        #region Higher Level Display Methods

        /// <summary>
        /// move a singel pixel around (or along) the ring (or strip) - always starts at position 0
        /// </summary>
        /// <param name="pixelColour">Colour of the pixel to show</param>
        /// <param name="cycles">Number of whole cycles to rotate</param>
        /// <param name="stepDelay">Delay between steps (ms)</param>
        public void SpinColour(Pixel pixelColour, int cycles = 1, int stepDelay = 250) {
            SpinColourOnBackground(pixelColour, Colour.Black, cycles, stepDelay);
        }

        public void SpinColourOnBackground(Pixel pixelColour, Pixel backgroundColour, int cycles = 1, int stepDelay = 250) {
            if (cycles < 0 || stepDelay < 0) { return; }

            FrameSet(backgroundColour);
            FrameSet(pixelColour, new int[] { 0 });

            FrameDraw();

            for (int i = 0; i < cycles; i++) {
                for (int j = 0; j < pixelCount; j++) {
                    FrameShift();
                    FrameDraw();
                    Util.Delay(stepDelay);
                }
            }
        }

        protected void Blink(int blinkDelay, int repeat) {
            if (blinkDelay < 0 || repeat < 0) { return; }

            if (blinkFrame[0] == null) {
                for (int i = 0; i < blinkFrame.Length; i++) {
                    blinkFrame[i] = Colour.Black;
                }
            }

            for (int i = 0; i < repeat; i++) {
                Util.Delay(blinkDelay);
                FrameDraw(blinkFrame);
                Util.Delay(blinkDelay);
                FrameDraw();
            }
        }
        #endregion


        /// <summary>
        /// pass a PixelColour enum and get the corresponding Pixel of that colour
        /// assumes that the colourList and the PixelColour enum are in sync
        /// </summary>
        /// <param name="pixelColour">PixelColour of the pixel required</param>
        /// <returns></returns>
        protected Pixel getPixel(PixelColour pixelColour) {
            return PaletteFullColour[(int)pixelColour];
        }

        protected Pixel[] GetColourListFromColourSet(PixelColour[] colourSet) {
            var colourList = new Pixel[colourSet.Length];
            for (int i = 0; i < colourSet.Length; i++) {
                colourList[i] = getPixel(colourSet[i]);
            }
            return colourList;
        }
    }
}
