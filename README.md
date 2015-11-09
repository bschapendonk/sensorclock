# sensorclock
**!!This is a work in progress, use at your own risk!!**
![Alt text](/sensorclock_bottom_components.png?raw=true)

I'm a software engineer by trade and I was looking for a nice project to use [Windows 10 IoT Core](https://dev.windows.com/en-us/iot) running on a Raspberry Pi 2 Model B.
So I decided on an clock inspired by Steve Gardner's [6 Digit LED Clock](http://sdgelectronics.co.uk/ledclock-projects/) project.
Initially I only planned on the clock, but I ended up addeding a bunch of sensors that might be used for some "IoT" in the feature.

Everything is controlled thru I2C, except the RGB leds which use SPI, some GPIO's for the rotary encoder, interrupts from the sensors and control lines to turn off the +3V3 (to reset all I2C devices). Basically any MCU that has I2C, SPI and some GPIO's can be used.

* [Schematic](/hardware/pdf/sensorclock.pdf)
* [Board Top](/hardware/pdf/sensorclock_top.pdf)
* [Board Bottom](/hardware/pdf/sensorclock_bottom.pdf)
* [Gerbers](/hardware/gerber)

## features
* Windows 10 IoT Core controlled
* Six 7-segment 70mm (2.3") high displays
* Eight APA102C serial controlled RGB led's for ambient lighting or mood indication
* Temperature, humidity, barometric pressure, ambient light and air quality sensors
* Mechanical rotary encoder with switch for user input

## components
* Raspberry Pi 2 Model B
* DS3231 [http://datasheets.maximintegrated.com/en/ds/DS3231.pdf]
* PEC16 [http://www.bourns.com/data/global/pdfs/pec16.pdf]
* APA102C [http://www.adafruit.com/datasheets/APA102.pdf]

### clock
* PCA9622 [http://www.nxp.com/documents/data_sheet/PCA9622.pdf]
* SA23-11SRWA [http://www.kingbrightusa.com/images/catalog/spec/SA23-11SRWA.pdf]

### sensors
* SparkFun Atmospheric Sensor Breakout - BME280 [https://www.sparkfun.com/products/13676]
* SparkFun Luminosity Sensor Breakout - TSL2561 [https://www.sparkfun.com/products/12055]
* MQ-135 air quality sensor, ADC and loadswitch
 * MCP3425 [http://ww1.microchip.com/downloads/en/DeviceDoc/22072b.pdf]
 * AP2280 [http://www.diodes.com/_files/datasheets/AP2280.pdf]

## renders

![Alt text](/sensorclock_bottom_components.png?raw=true)
![Alt text](/sensorclock_top_components.png?raw=true)
![Alt text](/sensorclock_bottom.png?raw=true)
![Alt text](/sensorclock_top.png?raw=true)
