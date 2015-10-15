# Windows 10 for IoT
# LED Matrix Graphics Library

Graphics Lib to draw, scroll &amp; control text or symbols on multiple 8x8 LED Matrices. Supports HT16K33, Ht16K33BiColor and the MAX7219 LED Matrix Driver chips

documentation in progress


#Hello World Example


	using Glovebox.Graphics.Components;
	using Glovebox.Graphics.Drivers;
	using Windows.ApplicationModel.Background;
	
	// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409
	
	namespace HelloWorld {
		public sealed class StartupTask : IBackgroundTask
		{
			BackgroundTaskDeferral _deferral;   // for a headless Windows 10 for IoT projects you need to hold a deferral to keep the app active in the background
	
			public void Run(IBackgroundTaskInstance taskInstance) {
				_deferral = taskInstance.GetDeferral();  // request the deferral
	
				MAX7219 driver = new MAX7219(4, MAX7219.Rotate.D90, MAX7219.ChipSelect.CE0);  // 4 panels, rotate 90 degrees, SPI CE0
				LED8x8Matrix matrix = new LED8x8Matrix(driver);     // pass the driver to the LED8x8Matrix Graphics Library
	
				while (true) {
					matrix.ScrollStringInFromRight("Hello World 2015", 100);
				}
			}
		}
	}



Be sure to review the examples in the [Example Project](https://github.com/gloveboxes/Windows-10-for-IoT-Graphics-Library-for-LED-Matrices/tree/master/Examples) for more extensice samples


[Raspberry Pi 2 Pin Mappings](https://ms-iot.github.io/content/en-US/win10/samples/PinMappingsRPi2.htm)


[MinnowBoard Max Pin Mapings](https://ms-iot.github.io/content/en-US/win10/samples/PinMappingsMBM.htm)


# LED Matrix Drivers

##MAX7219 SPI LED Driver

MAX7219 based LED matrices are great, they are simple to chain together and they connect to the Raspberry Pi via either SPI channel.  Oh, and best of all they are cheap.  You can buy 4 chained 8x8 LED matrices for about $US6 off eBay.  Now that's a lot of LEDs for not a lot of money!

This LED Matrix can only display one colour, so there is a Mono colour palette in the Pixel class. Mono.On and Mono.Off to turn a pixel on or off. But actually you can use any Pixel colour you like other than Black to turn a pixel on.


![max7219 matrix](https://sbczha.bn1303.df.livefilestore.com/y3mBPvZ3ePYiIjkMI3MOS40jAN1bpx_bvQPimDDn0Cd3TCpw6VTxXyD_egqaVPJlAlMqcijna5eqv1_cOnATc79jntj9vbB5iy0xBE-v2usufAVJEePnxDvfu_-PFqYSYgLjXfU-LFNWKdCjw7nFppCMKi2T-EP1_ds1AKy6SCb3GY/MAX7219.jpg)

###Wiring


GPIO pin-outs
-------------
The breakout board has two headers to allow daisy-chaining.

| Name | Remarks     | RPi Pin | RPi Function      |
|:-----|:------------|--------:|-------------------|
| VCC  | +5V Power   | 2       | 5V0               |
| GND  | Ground      | 6       | GND               |
| DIN  | Data In     | 19      | GPIO 10 (MOSI)    |
| CS   | Chip Select | 24      | GPIO 8 (SPI CE0)  |
| CLK  | Clock       | 23      | GPIO 11 (SPI CLK) |


###Examples

	MAX7219 driver = new MAX7219()  // create new MAX7219 LED Matrix driver and take all the defaults
	
	MAX7219 driver = new MAX7219(4, MAX7219.Rotate.D90, MAX7219.ChipSelect.CE0);  // 4 panels, rotate 90 degrees, SPI CE0

###Constructors

Name|Description
-----------------|---------------
MAX7219()|Defaults to 1 Display Panel, No rotation, SPI CE0, SPI Controller Name SPI0
MAX7219(numOfPanels)| Number of chained Display Panels
MAX7219(numOfPanels, rotate)| Number of chained Display Panels. Rotate each display panel none, 90 degrees, 180 degrees
MAX7219(numOfPanels, rotate, chipSelect)| Number of chained Display Panels. Rotate each display panel none, 90 degrees, 180 degrees. SPI Chip Select CE0, CE1
MAX7219(numOfPanels, rotate, chipSelect, SPIControllerName)| Number of chained Display Panels. Rotate each display panel none, 90 degrees, 180 degrees. SPI Chip Select CE0, CE1. SPIControllerName = SPI0 on Raspberry Pi and MinnowBoard Max


	

##Class Ht16K33 I2C LED Driver

This Adafruit 8x8 LED Matrix is a great way to add status to your project.  They are available in multiple colours, from Red, to Green, yellow, white and more. You can create a wide display panel using multiple of these LED Matrices and treat them as one panel.

This LED Matrix can only display one colour, so there is a Mono colour palette in the Pixel class. Mono.On and Mono.Off to turn a pixel on or off. But actually you can use any Pixel colour you like other than Black to turn a pixel on.

![max7219 matrix](https://7ubhjw.bn1303.df.livefilestore.com/y3mL_dA7IxZA5dt2T_pBUgnZX48asZQ0qf3iBQwt8oTodsphnRHw9WzDF4SB38kEoffUzGIcaAOWQEcyWjMs0Ak9cfPsGQsnPVjZgr-iMSor4VYvjNlLHfRpCmysy92tIooOMlfFJYrX-_J1j1aefoIAuZPb0qAGd8zhx3f1W9-cQQ/adafruit-mini-8x8-led-matrix-w_i2c-backpack--_EXP-R15-264_1.jpg)

###Wiring


GPIO pin-outs
-------------
The breakout board has two headers to allow daisy-chaining.

| Name | Remarks     | RPi Pin | RPi Function      |
|:-----|:------------|--------:|-------------------|
| VCC  | +5V Power   | 2       | 5V0               |
| GND  | Ground      | 6       | GND               |
| SDA  | Serial Data Line     | 3      | I2C1 SDA   |
| SCL  | Serial Clock Line | 5      | I2C1 SCL  |

###Examples

	Ht16K33 driver = new Ht16K33(new byte[] { 0x70, 0x72 }, Ht16K33.Rotate.None);  // pass in two I2C Addresses for the panels to be treated as one display panel 
	
	Ht16K33 driver = new Ht16K33() // create new driver and take defaults - I2C Address 0x70
	

###Constructors

Name|Description
-----------------|---------------
Ht16K33()| Defaults: I2C Address = 0x70, Rotate = none, Display on, Brightness 2 (0-15), I2C Controller Name I2C1
Ht16K33(new byte[] { 0x70 })|Collection of I2C Addresses. Specify if not using the default 0x70 address or you are chaining multiple I2C LED Matrices together in to one long display panel
Ht16K33(new byte[] { 0x70 }, Ht16K33.Rotate.None)| Rotate none, 90 degress, 180 degress)
Ht16K33(new byte[] { 0x70 }, Ht16K33.Rotate.None, LedDriver.Display.On)| Display on or off
Ht16K33(new byte[] { 0x70 }, Ht16K33.Rotate.None, LedDriver.Display.On, 2)| Brightness 0-15
Ht16K33(new byte[] { 0x70 }, Ht16K33.Rotate.None, LedDriver.Display.On, 2, LedDriver.BlinkRate.Off)| Blink off, slow, medium, fast
Ht16K33(new byte[] { 0x70 }, Ht16K33.Rotate.None, LedDriver.Display.On, 2, LedDriver.BlinkRate.Off, "I2C1")| I2C Controller name: I2C1 for Raspberry Pi, I2C5 for MinnowBoard Max


##Class Ht16K33BiColor

The [Adafruit Bicolor LED Square Pixel Matrix with I2C Backpack](https://www.adafruit.com/product/902) are a great way to add a little more colour to your projects. With two LEDS, a Red and a Green you can actually combine and create three colours. Red, Green and Yellow, nice.

You can create a wide display panel using multiple of these LED Matrices and treat them as one panel.

The Pixel class includes a three colour palette, BiColour.Red, BiColour.Green and BiColor.Yellow to control this baby.

![max7219 matrix](https://u2yg7a.bn1303.df.livefilestore.com/y3mV3LodjhGwAHTY0FXLAPj4_gcR3fGclexYgUvzBSnMknj95Ew9-rOGfg5RVmGPLhr0LUrys38LEB7fFIV_B6SuwGbyS_9yNv3ROkVDB1byNjbyCFO4vSLox6mFl9DJ-3fT8YvC4L2jIahZexVoGOeoOBkzFdOb5a0AbcHsz84QlI/adafruit-bicolor-led-square-pixel-matrix-with-i2c-backpack.jpg)

###Wiring


GPIO pin-outs
-------------
The breakout board has two headers to allow daisy-chaining.

| Name | Remarks     | RPi Pin | RPi Function      |
|:-----|:------------|--------:|-------------------|
| VCC  | +5V Power   | 2       | 5V0               |
| GND  | Ground      | 6       | GND               |
| SDA  | Serial Data Line     | 3      | I2C1 SDA   |
| SCL  | Serial Clock Line | 5      | I2C1 SCL  |


###Examples

	Ht16K33 driver = new Ht16K33(new byte[] { 0x70, 0x72 }, Ht16K33.Rotate.None);  // pass in two I2C Addresses for the panels to be treated as one display panel 
	
	Ht16K33 driver = new Ht16K33() // create new driver and take defaults - I2C Address 0x70

###Constructors

Name|Description
-----------------|---------------
Ht16K33BiColor()| Defaults: I2C Address = 0x70, Rotate = none, Display on, Brightness 2 (0-15), I2C Controller Name I2C1
Ht16K33BiColor(new byte[] { 0x70 })|Collection of I2C Addresses. Specify if not using the default 0x70 address or you are chaining multiple I2C LED Matrices together in to one long display panel
Ht16K33BiColor(new byte[] { 0x70 }, Ht16K33.Rotate.None)| Rotate none, 90 degress, 180 degress)
Ht16K33BiColor(new byte[] { 0x70 }, Ht16K33.Rotate.None, LedDriver.Display.On)| Display on or off
Ht16K33BiColor(new byte[] { 0x70 }, Ht16K33.Rotate.None, LedDriver.Display.On, 2)| Brightness 0-15
Ht16K33BiColor(new byte[] { 0x70 }, Ht16K33.Rotate.None, LedDriver.Display.On, 2, LedDriver.BlinkRate.Off)| Blink off, slow, medium, fast
Ht16K33BiColor(new byte[] { 0x70 }, Ht16K33.Rotate.None, LedDriver.Display.On, 2, LedDriver.BlinkRate.Off, "I2C1")| I2C Controller name: I2C1 for Raspberry Pi, I2C5 for MinnowBoard Max



#LED Matrix Manager

##Class LED8x8Matrix

###Constructors

Name|Description
-----------------|---------------
LED8x8Matrix(driver)| Pass in a MAX7219 or Ht16K33 or Ht16K33BiColor LED Matrix driver. 

###Examples

	LED8x8Matrix matrix = new LED8x8Matrix(new MAX7219());  // take the defaults for the MAX7219 LED Matrix driver
	
	
	MAX7219 driver = new MAX7219(4, MAX7219.Rotate.D90, MAX7219.ChipSelect.CE0);  // 4 panels, rotate 90 degrees, SPI CE0
	LED8x8Matrix matrix = new LED8x8Matrix(driver);     // pass the driver to the LED8x8Matrix Graphics Library


###Methods


### Column Operators

* ColumnRollDown
* ColumnRollLeft
* ColumnRollRight
* ColumnRollUp

### Draw Operators

* DrawBitmap
* DrawBox
* DrawLetter
* DrawString
* DrawSymbol

###Frame Privatives
* FrameClear
* FrameDraw

###Frame Operations

* FrameRollLeft
* FrameRollRight
* FrameRowDown
* FrameRowUp

### Frame Set Operators

* FrameSet
* FrameSetBlocks


###Shift Operators

* FrameShift
* FrameShiftBack
* FrameShiftForward

## Pixel Point Operators   

* FramePixelForward
* FramePixelSwap
* PointColour
* PointPostion

### Row drawing Operators
* ColumnDrawLine
* RowDrawLine


###Scroll Operators

* ScrollBitmapInFromLeft
* ScrollBitmapInFromRight
* ScrollCharacterFromLeft
* ScrollCharacterFromRight
* ScrollStringInFromLeft
* ScrollStringInFromRight
* ScrollSymbolInFromLeft
* ScrollSymbolInFromRight

### LED Control

* Blink(100, 10);
* SetBlinkRate
* SetBrightness
* SetDisplayState

### Shift Operators


* ShiftColumnLeft
* ShiftColumnRight
* ShiftFrameLeft
* ShiftFrameRight


### Spin - Circular LED Strings
* SpinColour
* SpinColourOnBackground

## Properties

Property| Description
--------|------------
ColumnsPerPanel | Number of columns per panel
RowsPerPanel | Number of rows per panel
PixelsPerPanel | Number of pixels per panel.  ColumnsPerPanel x RowsPerPanel
PanelsPerDisplay | Multiple panels make up a display unit
ColumnsPerDisplay | Total columns across a display unit
Panels | (think redundant) = PanelsPerDisplay
Length | Total number of Pixels in the Display.  PixelsPerPanel x PanelsPerDisplay





