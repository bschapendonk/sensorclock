from adafruit_datetime import datetime
import adafruit_dotstar
import adafruit_ntp
import asyncio
import board
import busio
import PCA9622
import digitalio
import ipaddress
import os
import rtc
import socketpool
import time
import wifi

from tzdb import timezone

wifi_ssid = os.getenv('WIFI_SSID')
wifi_password = os.getenv('WIFI_PASSWORD')
ntp_server = os.getenv('NTP_SERVER')
tz_name = os.getenv('TZ_NAME')

current_second = bytearray(17)

wifi.radio.connect(wifi_ssid, wifi_password)
print("Connected to WiFi")
pool = socketpool.SocketPool(wifi.radio)
print("My IP address is", wifi.radio.ipv4_address)        
ntp = adafruit_ntp.NTP(pool, server=ntp_server, tz_offset=0)
rtc.RTC().datetime = ntp.datetime

# i2c = busio.I2C(scl=board.GP5, sda=board.GP4)

def init():
    num_pixels = 8
    pixels = adafruit_dotstar.DotStar(board.GP18, board.GP19, num_pixels, 0)

    heater = digitalio.DigitalInOut(board.GP2)
    heater.direction = digitalio.Direction.OUTPUT
    heater.value = False

    # shdn_3v3 = digitalio.DigitalInOut(board.GP3)
    # shdn_3v3.direction = digitalio.Direction.OUTPUT
    # shdn_3v3.value = True

def display_init():
    i2c = busio.I2C(scl=board.GP5, sda=board.GP4)
    i2c.writeto(PCA9622.ADDR_ALLCALL, bytes([PCA9622.REGISTER_MODE1, PCA9622.MODE1_SUBADDR1 | PCA9622.MODE1_ALLCALL ]))
    time.sleep_ms(10)
    i2c.writeto(PCA9622.ADDR_ALLCALL, bytes([PCA9622.REGISTER_GRPPWM, PCA9622.PWM_DEFAULT]))
    i2c.writeto(PCA9622.ADDR_ALLCALL, bytes([PCA9622.REGISTER_PWM0 | PCA9622.AUTO_INCREMENT, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00]))
    i2c.writeto(PCA9622.ADDR_ALLCALL, bytes([PCA9622.REGISTER_LEDOUT0 | PCA9622.AUTO_INCREMENT, 0xFF, 0xFF, 0xFF, 0xFF]))
    i2c.writeto(PCA9622.ADDR_MINUTE, bytes([PCA9622.REGISTER_MODE1, PCA9622.MODE1_ALLCALL]))
    i2c.writeto(PCA9622.ADDR_HOUR, bytes([PCA9622.REGISTER_MODE1, PCA9622.MODE1_ALLCALL]))

async def rtc_update():
    while True:
        rtc.RTC().datetime = ntp.datetime
        await asyncio.sleep(300)

async def display_update():
    while True:
        utc_now = time.time()
        utc_now_dt = datetime.fromtimestamp(utc_now)
        # print("UTC: {}".format(utc_now_dt.ctime()))
        localtime = utc_now_dt + timezone(tz_name).utcoffset(utc_now_dt)
        print("{}: {}".format(tz_name, localtime.ctime()))

        # if(localtime.propertymicrosecond > 500000)
        #     current_second[11] = 0xFF

        # i2c.writeto(PCA9622.ADDR_SECOND, bytes(PCA9622.DIGITS[localtime.second]))
        # i2c.writeto(PCA9622.ADDR_MINUTE, bytes(PCA9622.DIGITS[localtime.minute]))
        # i2c.writeto(PCA9622.ADDR_HOUR, bytes(PCA9622.DIGITS[localtime.hour]))

        # print(constants._DIGITS[localtime.second], sep = ", ")

        await asyncio.sleep(5)
        # await asyncio.sleep_ms(500)

async def main():
    rtc_update_task = asyncio.create_task(rtc_update())
    display_update_task = asyncio.create_task(display_update())

    await asyncio.gather(rtc_update_task, display_update_task)

asyncio.run(main())
