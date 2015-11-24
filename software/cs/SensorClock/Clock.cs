using System;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.I2c;

namespace SensorClock
{
    class Clock : IDisposable
    {
        const byte PWM_DEFAULT = 0x32;

        const byte ADDR_ALLCALL = 0x70;
        const byte ADDR_HOUR = 0x71;
        const byte ADDR_MINUTE = 0x01;
        const byte ADDR_SECOND = 0x02;

        const byte AUTO_INCREMENT = 0x80;
        const byte REGISTER_MODE1 = 0x00;
        const byte REGISTER_PWM0 = 0x02;
        const byte REGISTER_GRPPWM = 0x12;
        const byte REGISTER_LEDOUT0 = 0x14;

        const byte MODE1_SLEEP = 0x10;
        const byte MODE1_SUBADDR1 = 0x08;
        const byte MODE1_ALLCALL = 0x08;

        I2cDevice _allcall;
        I2cDevice _hour;
        I2cDevice _minute;
        I2cDevice _second;

        const I2cBusSpeed BUSSPEED = I2cBusSpeed.FastMode;


        public Clock()
        {

        }

        public async Task Init()
        {
            await Reset();

            var deviceSelector = I2cDevice.GetDeviceSelector();
            var controllers = await DeviceInformation.FindAllAsync(deviceSelector);

            _allcall = await I2cDevice.FromIdAsync(controllers[0].Id, new I2cConnectionSettings(ADDR_ALLCALL) { BusSpeed = BUSSPEED });
            _hour = await I2cDevice.FromIdAsync(controllers[0].Id, new I2cConnectionSettings(ADDR_HOUR) { BusSpeed = BUSSPEED });
            _minute = await I2cDevice.FromIdAsync(controllers[0].Id, new I2cConnectionSettings(ADDR_MINUTE) { BusSpeed = BUSSPEED });
            _second = await I2cDevice.FromIdAsync(controllers[0].Id, new I2cConnectionSettings(ADDR_SECOND) { BusSpeed = BUSSPEED });

            _allcall.Write(new byte[] { REGISTER_MODE1, MODE1_SUBADDR1 | MODE1_ALLCALL });
            Task.Delay(10).GetAwaiter().GetResult();

            _allcall.Write(new byte[] { REGISTER_GRPPWM, PWM_DEFAULT });
            _allcall.Write(new byte[] { REGISTER_LEDOUT0 | AUTO_INCREMENT, 0xFF, 0xFF, 0xFF, 0xFF });

            _minute.Write(new byte[] { REGISTER_MODE1, MODE1_ALLCALL });
            _second.Write(new byte[] { REGISTER_MODE1, MODE1_ALLCALL });
        }

        public async Task Reset() {
            var deviceSelector = I2cDevice.GetDeviceSelector();
            var controllers = await DeviceInformation.FindAllAsync(deviceSelector);
            using (var pca9622 = await I2cDevice.FromIdAsync(controllers[0].Id, new I2cConnectionSettings(0x06) { BusSpeed = I2cBusSpeed.StandardMode }))
            {
                pca9622.Write(new byte[] { 0xA5 });
                pca9622.Write(new byte[] { 0x5A });
            }
            Task.Delay(10).GetAwaiter().GetResult();
        }

        public void Dispose()
        {
            if (_hour != null)
                _hour.Dispose();

            if (_minute != null)
                _minute.Dispose();

            if (_second != null)
                _second.Dispose();

            _allcall.Write(new byte[] { REGISTER_LEDOUT0 | AUTO_INCREMENT, 0x00, 0x00, 0x00, 0x00 });
            _allcall.Write(new byte[] { REGISTER_PWM0 | AUTO_INCREMENT, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
            _allcall.Write(new byte[] { REGISTER_MODE1, MODE1_SLEEP | MODE1_ALLCALL });

            if (_allcall != null)
                _allcall.Dispose();
        }
    }
}
