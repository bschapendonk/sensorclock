from adafruit_datetime import datetime
import adafruit_dotstar
import adafruit_ntp
import asyncio
import board
import constants
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

wifi.radio.connect(wifi_ssid, wifi_password)
print("Connected to WiFi")
pool = socketpool.SocketPool(wifi.radio)
print("My IP address is", wifi.radio.ipv4_address)        
ntp = adafruit_ntp.NTP(pool, server=ntp_server, tz_offset=0)
rtc.RTC().datetime = ntp.datetime

# spi = busio.SPI(board.SCK, MISO=board.MISO)

async def rtc_update():
    while True:
        rtc.RTC().datetime = ntp.datetime

        await asyncio.sleep(600)

async def display_update():
    while True:
        utc_now = time.time()
        utc_now_dt = datetime.fromtimestamp(utc_now)
        # print("UTC: {}".format(utc_now_dt.ctime()))
        localtime = utc_now_dt + timezone(tz_name).utcoffset(utc_now_dt)
        print("{}: {}".format(tz_name, localtime.ctime()))
        # print(constants._DIGITS[localtime.second], sep = ", ")

        await asyncio.sleep(10)
        # await asyncio.sleep_ms(500)

async def main():
    rtc_update_task = asyncio.create_task(rtc_update())
    display_update_task = asyncio.create_task(display_update())

    await asyncio.gather(rtc_update_task, display_update_task)

asyncio.run(main())
