import adafruit_ds3231
import adafruit_dotstar
import adafruit_ntp
import asyncio
import board
import busio
import clock
import digitalio
import mdns
import os
import socketpool
import time
import wifi

from adafruit_datetime import datetime
from tzdb import timezone


# try to update the rtc every 5 minutes from ntp
async def update_rtc():
    while True:
        try:
            utc_now = datetime.fromtimestamp(time.mktime(ntp.datetime))
            rtc.datetime = timezone(tz_name).fromutc(utc_now).timetuple()
        except OSError as error:
            print(error)
            continue

        await asyncio.sleep(300)


async def update_clock():
    while True:
        clock.tick(rtc.datetime)
        await asyncio.sleep(0.02)


async def main():
    update_rtc_task = asyncio.create_task(update_rtc())
    update_clock_task = asyncio.create_task(update_clock())
    await asyncio.gather(update_rtc_task, update_clock_task)


# initialize and turn off all apa102 leds
pixels = adafruit_dotstar.DotStar(board.GP18, board.GP19, 8, brightness=0)
for pixel in range(len(pixels)):
    pixels[pixel] = (0, 0, 0)

# turn of the heater for the gas sensors
heater = digitalio.DigitalInOut(board.GP2)
heater.direction = digitalio.Direction.OUTPUT
heater.value = False

# enable 3v3
shdn_3v3 = digitalio.DigitalInOut(board.GP3)
shdn_3v3.direction = digitalio.Direction.OUTPUT
shdn_3v3.value = False
time.sleep(0.1)
shdn_3v3.value = True

wifi.radio.connect(os.getenv("WIFI_SSID"), os.getenv("WIFI_PASSWORD"))
print("Connected to WiFi")

mdns_server = mdns.Server(wifi.radio)
mdns_server.hostname = "custom-mdns-hostname"
mdns_server.advertise_service(service_type="_http", protocol="_tcp", port=80)

pool = socketpool.SocketPool(wifi.radio)
print("My IP address is", wifi.radio.ipv4_address)

ntp = adafruit_ntp.NTP(pool, server=os.getenv("NTP_SERVER"), tz_offset=0)

i2c = busio.I2C(scl=board.GP5, sda=board.GP4, frequency=400_000)
rtc = adafruit_ds3231.DS3231(i2c)
tz_name = os.getenv("TZ_NAME")

clock = clock.Clock(i2c)

asyncio.run(main())
