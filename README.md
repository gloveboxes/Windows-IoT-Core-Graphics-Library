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

##MAX7219

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




###Constructors

Name|Description
-----------------|---------------
MAX7219()|Defaults to 1 Display Panel, No rotation, SPI CE0, SPI Controller Name SPI0
MAX7219(numOfPanels)| Number of chained Display Panels
MAX7219(numOfPanels, rotate)| Number of chained Display Panels. Rotate each display panel none, 90 degrees, 180 degrees
MAX7219(numOfPanels, rotate, chipSelect)| Number of chained Display Panels. Rotate each display panel none, 90 degrees, 180 degrees. SPI Chip Select CE0, CE1
MAX7219(numOfPanels, rotate, chipSelect, SPIControllerName)| Number of chained Display Panels. Rotate each display panel none, 90 degrees, 180 degrees. SPI Chip Select CE0, CE1. SPIControllerName = SPI0 on Raspberry Pi and MinnowBoard Max

###Examples

	MAX7219 driver = new MAX7219()  // create new MAX7219 LED Matrix driver and take all the defaults
	
	MAX7219 driver = new MAX7219(4, MAX7219.Rotate.D90, MAX7219.ChipSelect.CE0);  // 4 panels, rotate 90 degrees, SPI CE0
	

##Class Ht16K33

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

Name|Description
-----------------|---------------
ScrollStringInFromRight("Hello World 2015", 100)| Scroll text in from the right every 100 milliseconds
ScrollStringInFromLeft("Hello World 2015", 100)| Scroll text in from the right every 100 milliseconds
FrameClear()| Clears the LED Matrix frame buffer
FrameDraw()| Used to write the LED Matrix frame buffer to the physical device
DrawSymbol(Grid8x8.Symbols.Heart, Mono.On)| Draw one of the predefined symbols, turn pixel on, this example assumes a mono color matrix
DrawSymbol(Grid8x8.Symbols.Heart, Mono.On, 0) | Draw the symbol on display panel 0
matrix.DrawSymbol(new Grid8x8.Symbols[] { Grid8x8.Symbols.Heart, Grid8x8.Symbols.HourGlass }, new Pixel[] { BiColour.Red, BiColour.Green, BiColour.Yellow }, 100, 1)| pass in a collection of symbols, a collection of colours and time to display each symbol in milliseconds plus the panel number to display the collection of symbols




Method documentation to be continued - for now see the samples...