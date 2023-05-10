from adafruit_datetime import datetime
import adafruit_dotstar
import adafruit_ntp
import asyncio
import board
import busio
import clock
import digitalio
import os
import rtc
import socketpool
import wifi


from tzdb import timezone

# try to update the rtc every 5 minutes from ntp
async def update_rtc_from_ntp(ntp: adafruit_ntp.NTP):
    while True:
        try:
            rtc.RTC().datetime = ntp.datetime
        except OSError as error:
            print(error)
        await asyncio.sleep(300)

async def update_clock(clock: clock.Clock):
    display_dot = False
    tz_name = os.getenv("TZ_NAME")
    while True:
        utc_now = datetime.now()
        localtime = utc_now + timezone(tz_name).utcoffset(utc_now)

        if display_dot:
            display_dot = False
        else:
            display_dot = True

        # clock.update(localtime, display_dot)

        print("{}: {} {}".format(tz_name, localtime.ctime(), display_dot))

        await asyncio.sleep_ms(500)


async def main():
     # initialize and turn off all apa102 leds
    adafruit_dotstar.DotStar(board.GP18, board.GP19, 8, brightness=0)

    # turn of the heater for the gas sensors
    heater = digitalio.DigitalInOut(board.GP2)
    heater.direction = digitalio.Direction.OUTPUT
    heater.value = False

    # enable 3v3
    shdn_3v3 = digitalio.DigitalInOut(board.GP3)
    shdn_3v3.direction = digitalio.Direction.OUTPUT
    shdn_3v3.value = True

    wifi.radio.connect(os.getenv("WIFI_SSID"), os.getenv("WIFI_PASSWORD"))
    print("Connected to WiFi")

    pool = socketpool.SocketPool(wifi.radio)
    print("My IP address is", wifi.radio.ipv4_address)

    ntp = adafruit_ntp.NTP(pool, server=os.getenv("NTP_SERVER"), tz_offset=0)
    rtc.RTC().datetime = ntp.datetime

    # i2c = busio.I2C(scl=board.GP5, sda=board.GP4)
    # clock = clock.Clock(i2c)
    clock = None

    rtc_update_task = asyncio.create_task(update_rtc_from_ntp(ntp))
    display_tick_task = asyncio.create_task(update_clock(clock))
    await asyncio.gather(rtc_update_task, display_tick_task)

asyncio.run(main())
