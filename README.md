# Windows-10-for-IoT-Graphics-Library-for-LED-Matrices
Graphics Lib to draw, scroll &amp; control text or symbols on multiple 8x8 LED Matrices. Supports HT16K33 &amp; MAX7219 LED Driver chips
tbc

[Raspberry Pi 2 Pinouts]()
[MinnowBoard Max Pinouts](https://ms-iot.github.io/content/en-US/win10/samples/PinMappingsMBM.htm)

# LED Matrix Drivers

##Class MAX7219

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

