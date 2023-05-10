import adafruit_dotstar
import adafruit_ntp
import asyncio
import board
import busio
import clock
import digitalio
import json
import microcontroller
import mdns
import os
import rtc
import socketpool
import wifi

from adafruit_datetime import datetime
from adafruit_httpserver.mime_type import MIMEType
from adafruit_httpserver.request import HTTPRequest
from adafruit_httpserver.response import HTTPResponse
from adafruit_httpserver.server import HTTPServer
from tzdb import timezone


# try to update the rtc every 5 minutes from ntp
async def update_rtc():
    while True:
        try:
            rtc.RTC().datetime = ntp.datetime
        except OSError as error:
            print(error)
            continue

        await asyncio.sleep(300)


def get_localtime(utc_now: datetime):
    return utc_now + timezone(tz_name).utcoffset(utc_now)


async def update_clock():
    while True:
        localtime = get_localtime(datetime.now())
        # clock.tick(localtime)
        print("{}: {}".format(tz_name, localtime.ctime()))

        await asyncio.sleep_ms(500)


async def serve_forever():
    server.start(str(wifi.radio.ipv4_address))
    while True:
        try:
            server.poll()
        except OSError as error:
            print(error)
            continue

        await asyncio.sleep_ms(10)


async def main():
    update_rtc_task = asyncio.create_task(update_rtc())
    update_clock_task = asyncio.create_task(update_clock())
    # serve_forever_task = asyncio.create_task(serve_forever())
    await asyncio.gather(update_rtc_task, update_clock_task)


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

mdns_server = mdns.Server(wifi.radio)
mdns_server.hostname = "custom-mdns-hostname"
mdns_server.advertise_service(service_type="_http", protocol="_tcp", port=80)

pool = socketpool.SocketPool(wifi.radio)
print("My IP address is", wifi.radio.ipv4_address)

ntp = adafruit_ntp.NTP(pool, server=os.getenv("NTP_SERVER"), tz_offset=0)
rtc.RTC().datetime = ntp.datetime

tz_name = os.getenv("TZ_NAME")

# i2c = busio.I2C(scl=board.GP5, sda=board.GP4)
# clock = clock.Clock(i2c)
clock: clock.Clock = None

server = HTTPServer(pool, "/static")


@server.route("/info")
def cpu_information_handler(request: HTTPRequest):
    data = {
        "temperature": microcontroller.cpu.temperature,
        "frequency": microcontroller.cpu.frequency,
        "voltage": microcontroller.cpu.voltage,
    }

    with HTTPResponse(request, content_type=MIMEType.TYPE_JSON) as response:
        response.send(json.dumps(data))


asyncio.run(main())
