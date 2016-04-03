using System;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.I2c;

namespace SensorClock
{
    /// <summary>
    /// BME280
    /// Combined humidity and pressure sensor
    /// https://ae-bst.resource.bosch.com/media/products/dokumente/bme280/BST-BME280_DS001-11.pdf
    /// </summary>
    class BME280 : IDisposable
    {
        const byte ADDR = 0x77;

        const byte REGISTER_CTRL_HUM = 0xF2;
        const byte REGISTER_CTRL_MEAS = 0xF4;
        const byte REGISTER_CONFIG = 0xF5;
        const byte REGISTER_ID = 0xD0;
        const byte REGISTER_CALIB00 = 0x88;
        const byte REGISTER_CALIB26 = 0xE1;
        const byte REGISTER_HUM_LSB = 0xF7;

        const byte CTRL_HUM_OSRS_H = 0x01; // Humidity oversampling ×1
        const byte CTRL_MEAS_OSRS_P = 0x14; // Pressure oversampling ×16
        const byte CTRL_MEAS_OSRS_T = 0x40; // Temperature oversampling ×2
        const byte CTRL_MEAS_MODE = 0x03; // Mode normal
        const byte CONFIG_TSB = 0x80; // 500ms
        const byte CONFIG_FILTER = 0x10; // 16

        const byte CHIP_ID = 0x60;

        #region  Compensation parameters
        ushort dig_T1, dig_P1;
        short dig_T2, dig_T3, dig_P2, dig_P3, dig_P4, dig_P5, dig_P6, dig_P7, dig_P8, dig_P9, dig_H2, dig_H4, dig_H5;
        byte dig_H1, dig_H3;
        sbyte dig_H6;
        int t_fine;
        #endregion

        I2cDevice _bme280;

        public double Temperature { get; private set; }
        public double Pressure { get; private set; }
        public double Humidity { get; private set; }

        public async Task Init()
        {
            var deviceSelector = I2cDevice.GetDeviceSelector();
            var controllers = await DeviceInformation.FindAllAsync(deviceSelector);

            _bme280 = await I2cDevice.FromIdAsync(controllers[0].Id, new I2cConnectionSettings(ADDR) { BusSpeed = I2cBusSpeed.FastMode });
            _bme280.Write(new byte[] { REGISTER_CTRL_HUM, CTRL_HUM_OSRS_H });
            _bme280.Write(new byte[] { REGISTER_CTRL_MEAS, CTRL_MEAS_OSRS_P | CTRL_MEAS_OSRS_T | CTRL_MEAS_MODE });
            _bme280.Write(new byte[] { REGISTER_CONFIG, CONFIG_TSB | CONFIG_FILTER });

            ValidateChipId();
            SetTrimmingParamaters();
        }

        void ValidateChipId()
        {
            var id = new byte[1];
            _bme280.WriteRead(new byte[] { REGISTER_ID }, id);
            if (id[0] != CHIP_ID)
                throw new Exception("Incorrect chip_id");
        }

        void SetTrimmingParamaters()
        {
            var buffer = new byte[25];
            _bme280.WriteRead(new byte[] { REGISTER_CALIB00 }, buffer);

            dig_T1 = ReadUsignedShort(buffer);
            dig_T2 = (short)ReadUsignedShort(buffer, 2);
            dig_T3 = (short)ReadUsignedShort(buffer, 4);
            dig_P1 = ReadUsignedShort(buffer, 6);
            dig_P2 = (short)ReadUsignedShort(buffer, 8);
            dig_P3 = (short)ReadUsignedShort(buffer, 10);
            dig_P4 = (short)ReadUsignedShort(buffer, 12);
            dig_P5 = (short)ReadUsignedShort(buffer, 14);
            dig_P6 = (short)ReadUsignedShort(buffer, 16);
            dig_P7 = (short)ReadUsignedShort(buffer, 18);
            dig_P8 = (short)ReadUsignedShort(buffer, 20);
            dig_P9 = (short)ReadUsignedShort(buffer, 22);

            buffer = new byte[8];
            _bme280.WriteRead(new byte[] { REGISTER_CALIB26 }, buffer);

            dig_H1 = buffer[0];
            dig_H2 = (short)ReadUsignedShort(buffer, 1);
            dig_H3 = buffer[3];
            dig_H4 = (short)((buffer[4] << 4) + (buffer[5] & 0x0F));
            dig_H5 = (short)((buffer[5] >> 4) + (buffer[6] << 4));
            dig_H6 = (sbyte)buffer[7];
        }

        public void Sample()
        {
            var buffer = new byte[8];
            _bme280.WriteRead(new byte[] { REGISTER_HUM_LSB }, buffer);

            Temperature = BME280_compensate_T_double(ReadSignedInteger(buffer, 3));
            Pressure = BME280_compensate_P_double(ReadSignedInteger(buffer, 0));
            Humidity = BME280_compensate_H_double(ReadUsignedShort(buffer, 6));
        }

        ushort ReadUsignedShort(byte[] buffer, int offset = 0)
        {
            if (offset + 1 >= buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));

            return (ushort)((buffer[offset + 1] << 8) + buffer[offset]);
        }

        int ReadSignedInteger(byte[] buffer, int offset)
        {
            if (offset + 2 >= buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));

            return (buffer[offset] << 12) + (buffer[offset + 1] << 4) + (buffer[offset + 2] >> 4);
        }

        #region Compensation formulas straight from the datasheet
        // Returns temperature in DegC, resolution is 0.01 DegC. Output value of “5123” equals 51.23 DegC.
        // t_fine carries fine temperature as global value
        double BME280_compensate_T_double(int adc_T)
        {
            var var1 = (adc_T / 16384.0 - dig_T1 / 1024.0) * dig_T2;
            var var2 = ((adc_T / 131072.0 - dig_T1 / 8192.0) * (adc_T / 131072.0 - dig_T1 / 8192.0)) * dig_T3;
            t_fine = (int)(var1 + var2);
            return (var1 + var2) / 5120.0;
        }

        // Returns pressure in Pa as double. Output value of “96386.2” equals 96386.2 Pa = 963.862 hPa
        double BME280_compensate_P_double(int adc_P)
        {
            var var1 = (t_fine / 2.0) - 64000.0;
            var var2 = var1 * var1 * dig_P6 / 32768.0;
            var2 = var2 + var1 * dig_P5 * 2.0;
            var2 = (var2 / 4.0) + (dig_P4 * 65536.0);
            var1 = (dig_P3 * var1 * var1 / 524288.0 + dig_P2 * var1) / 524288.0;
            var1 = (1.0 + var1 / 32768.0) * dig_P1;
            if (var1 == 0.0)
            {
                return 0; // avoid exception caused by division by zero
            }
            var p = 1048576.0 - adc_P;
            p = (p - (var2 / 4096.0)) * 6250.0 / var1;
            var1 = dig_P9 * p * p / 2147483648.0;
            var2 = p * dig_P8 / 32768.0;
            p = p + (var1 + var2 + dig_P7) / 16.0;
            return p;
        }

        // Returns humidity in %rH as as double. Output value of “46.332” represents 46.332 %rH
        double BME280_compensate_H_double(int adc_H)
        {
            var var_H = ((t_fine) - 76800.0);
            var_H = (adc_H - ((dig_H4) * 64.0 + dig_H5 / 16384.0 * var_H)) * ((dig_H2) / 65536.0 * (1.0 + dig_H6 / 67108864.0 * var_H * (1.0 + dig_H3 / 67108864.0 * var_H)));
            var_H = var_H * (1.0 - dig_H1 * var_H / 524288.0);
            if (var_H > 100.0)
                var_H = 100.0;
            else if (var_H < 0.0)
                var_H = 0.0;
            return var_H;
        }
        #endregion

        public void Dispose()
        {
            // todo put device in low power mode
            if (_bme280 != null)
                _bme280.Dispose();
        }
    }
}
