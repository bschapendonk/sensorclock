import adafruit_dotstar
import adafruit_ntp
import board
import constants
import ipaddress
import os
import rtc
import socketpool
import time
import wifi

print()
print("Connecting to WiFi")

wifi.radio.connect(os.getenv('WIFI_SSID'), os.getenv('WIFI_PASSWORD'))

print("Connected to WiFi")

pool = socketpool.SocketPool(wifi.radio)

#  prints MAC address to REPL
print("My MAC addr:", [hex(i) for i in wifi.radio.mac_address])

#  prints IP address to REPL
print("My IP address is", wifi.radio.ipv4_address)

#  pings Google
ipv4 = ipaddress.ip_address("8.8.4.4")
print("Ping google.com: %f ms" % (wifi.radio.ping(ipv4)*1000))

ntp = adafruit_ntp.NTP(pool, tz_offset=0)

rtc.RTC().datetime = ntp.datetime


print(constants._DIGITS[0], sep = ", ")

while True:
    print(time.localtime())
    print(constants._DIGITS[time.localtime().tm_sec], sep = ", ")
    time.sleep(1)