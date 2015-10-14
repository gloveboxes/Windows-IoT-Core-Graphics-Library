# Windows-10-for-IoT-Graphics-Library-for-LED-Matrices
Graphics Lib to draw, scroll &amp; control text or symbols on multiple 8x8 LED Matrices. Supports HT16K33 &amp; MAX7219 LED Driver chips
tbc

[Raspberry Pi 2 Pinouts]()
[MinnowBoard Max Pinouts](https://ms-iot.github.io/content/en-US/win10/samples/PinMappingsMBM.htm)

# Drivers

##MAX7219

###Constructors

Name|Description
-----------------|---------------
MAX7219()|Defaults to 1 Display Panel, No rotation, SPI CE0, SPI Controller Name SPI0
MAX7219(numOfPanels)| Number of chained Display Panels
MAX7219(numOfPanels, rotate)| Number of chained Display Panels. Rotate each display panel none, 90 degrees, 180 degrees
MAX7219(numOfPanels, rotate, chipSelect)| Number of chained Display Panels. Rotate each display panel none, 90 degrees, 180 degrees. SPI Chip Select CE0, CE1
MAX7219(numOfPanels, rotate, chipSelect, SPIControllerName)| Number of chained Display Panels. Rotate each display panel none, 90 degrees, 180 degrees. SPI Chip Select CE0, CE1. SPIControllerName = SPI0 on Raspberry Pi and MinnowBoard Max


##Ht16K33

###Constructors

Name|Description
-----------------|---------------
Ht16K33()| Defaults: I2C Address = 0&70, Rotate = none, Display on, Brightness 2 (0-15), I2C Controller Name I2C1



I2C5
