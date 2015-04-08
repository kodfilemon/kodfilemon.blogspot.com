using System.IO.Ports;
using Microsoft.SPOT.Hardware;

namespace Common
{
    public class Stm32F4Discovery
    {
        static Stm32F4Discovery()
        {
            HardwareProvider.Register(new Stm32F4DiscoveryHardwareProvider());
        }

        private sealed class Stm32F4DiscoveryHardwareProvider : HardwareProvider
        {
        }

        public class Pins
        {
            // ReSharper disable InconsistentNaming
            public const Cpu.Pin GPIO_NONE = Cpu.Pin.GPIO_NONE;

            public const Cpu.Pin PA0 = 0*16 + 0; //0
            public const Cpu.Pin PA1 = (Cpu.Pin) (0*16 + 1); //1 ADC0 COM2(rts)
            public const Cpu.Pin PA2 = (Cpu.Pin) (0*16 + 2); //2 ADC1 COM2(tx)
            public const Cpu.Pin PA3 = (Cpu.Pin) (0*16 + 3); //3 ADC2 COM2(rx)
            public const Cpu.Pin PA4 = (Cpu.Pin) (0*16 + 4); //4
            public const Cpu.Pin PA5 = (Cpu.Pin) (0*16 + 5); //5 SPI0(msk)
            public const Cpu.Pin PA6 = (Cpu.Pin) (0*16 + 6); //6 SPI0(miso)
            public const Cpu.Pin PA7 = (Cpu.Pin) (0*16 + 7); //7 SPI0(mosi)
            public const Cpu.Pin PA8 = (Cpu.Pin) (0*16 + 8); //8
            public const Cpu.Pin PA9 = (Cpu.Pin) (0*16 + 9); //9 COM1(tx)
            public const Cpu.Pin PA10 = (Cpu.Pin) (0*16 + 10); //10 COM1(rx)
            public const Cpu.Pin PA11 = (Cpu.Pin) (0*16 + 11); //11 COM1(cts)
            public const Cpu.Pin PA12 = (Cpu.Pin) (0*16 + 12); //12 COM1(rts)
            public const Cpu.Pin PA13 = (Cpu.Pin) (0*16 + 13); //13
            public const Cpu.Pin PA14 = (Cpu.Pin) (0*16 + 14); //14
            public const Cpu.Pin PA15 = (Cpu.Pin) (0*16 + 15); //15

            public const Cpu.Pin PB0 = (Cpu.Pin) (1*16 + 0); //16 ADC3
            public const Cpu.Pin PB1 = (Cpu.Pin) (1*16 + 1); //17 ADC4
            public const Cpu.Pin PB2 = (Cpu.Pin) (1*16 + 2); //18
            public const Cpu.Pin PB3 = (Cpu.Pin) (1*16 + 3); //19
            public const Cpu.Pin PB4 = (Cpu.Pin) (1*16 + 4); //20
            public const Cpu.Pin PB5 = (Cpu.Pin) (1*16 + 5); //21
            public const Cpu.Pin PB6 = (Cpu.Pin) (1*16 + 6); //22 I2C(scl)
            public const Cpu.Pin PB7 = (Cpu.Pin) (1*16 + 7); //23
            public const Cpu.Pin PB8 = (Cpu.Pin) (1*16 + 8); //24
            public const Cpu.Pin PB9 = (Cpu.Pin) (1*16 + 9); //25 I2C(sda)
            public const Cpu.Pin PB10 = (Cpu.Pin) (1*16 + 10); //26
            public const Cpu.Pin PB11 = (Cpu.Pin) (1*16 + 11); //27
            public const Cpu.Pin PB12 = (Cpu.Pin) (1*16 + 12); //28
            public const Cpu.Pin PB13 = (Cpu.Pin) (1*16 + 13); //29 SPI1(msk)
            public const Cpu.Pin PB14 = (Cpu.Pin) (1*16 + 14); //30 SPI1(miso)
            public const Cpu.Pin PB15 = (Cpu.Pin) (1*16 + 15); //31 SPI1(mosi)

            public const Cpu.Pin PC0 = (Cpu.Pin) (2*16 + 0); //32
            public const Cpu.Pin PC1 = (Cpu.Pin) (2*16 + 1); //33
            public const Cpu.Pin PC2 = (Cpu.Pin) (2*16 + 2); //34
            public const Cpu.Pin PC3 = (Cpu.Pin) (2*16 + 3); //35
            public const Cpu.Pin PC4 = (Cpu.Pin) (2*16 + 4); //36 ADC5
            public const Cpu.Pin PC5 = (Cpu.Pin) (2*16 + 5); //37 ADC6
            public const Cpu.Pin PC6 = (Cpu.Pin) (2*16 + 6); //38 COM6(tx)
            public const Cpu.Pin PC7 = (Cpu.Pin) (2*16 + 7); //39 COM6(rx)
            public const Cpu.Pin PC8 = (Cpu.Pin) (2*16 + 8); //40
            public const Cpu.Pin PC9 = (Cpu.Pin) (2*16 + 9); //41
            public const Cpu.Pin PC10 = (Cpu.Pin) (2*16 + 10); //42 SPI2(msk) COM4(tx)
            public const Cpu.Pin PC11 = (Cpu.Pin) (2*16 + 11); //43 SPI2(miso) COM4(rx)
            public const Cpu.Pin PC12 = (Cpu.Pin) (2*16 + 12); //44 SPI2(mosi) COM5(tx)
            public const Cpu.Pin PC13 = (Cpu.Pin) (2*16 + 13); //45
            public const Cpu.Pin PC14 = (Cpu.Pin) (2*16 + 14); //46
            public const Cpu.Pin PC15 = (Cpu.Pin) (2*16 + 15); //47

            public const Cpu.Pin PD0 = (Cpu.Pin) (3*16 + 0); //48
            public const Cpu.Pin PD1 = (Cpu.Pin) (3*16 + 1); //49
            public const Cpu.Pin PD2 = (Cpu.Pin) (3*16 + 2); //50 COM5: (rx)
            public const Cpu.Pin PD3 = (Cpu.Pin) (3*16 + 3); //51 COM2(cts)
            public const Cpu.Pin PD4 = (Cpu.Pin) (3*16 + 4); //52
            public const Cpu.Pin PD5 = (Cpu.Pin) (3*16 + 5); //53
            public const Cpu.Pin PD6 = (Cpu.Pin) (3*16 + 6); //54
            public const Cpu.Pin PD7 = (Cpu.Pin) (3*16 + 7); //55
            public const Cpu.Pin PD8 = (Cpu.Pin) (3*16 + 8); //56 COM3(tx)
            public const Cpu.Pin PD9 = (Cpu.Pin) (3*16 + 9); //57 COM3(rx)
            public const Cpu.Pin PD10 = (Cpu.Pin) (3*16 + 10); //58
            public const Cpu.Pin PD11 = (Cpu.Pin) (3*16 + 11); //59 COM3(cts)
            public const Cpu.Pin PD12 = (Cpu.Pin) (3*16 + 12); //60 PWM0 COM3(rts)
            public const Cpu.Pin PD13 = (Cpu.Pin) (3*16 + 13); //61 PWM1
            public const Cpu.Pin PD14 = (Cpu.Pin) (3*16 + 14); //62 PWM2
            public const Cpu.Pin PD15 = (Cpu.Pin) (3*16 + 15); //63 PWM3

            public const Cpu.Pin PE0 = (Cpu.Pin) (4*16 + 0); //64
            public const Cpu.Pin PE1 = (Cpu.Pin) (4*16 + 1); //65
            public const Cpu.Pin PE2 = (Cpu.Pin) (4*16 + 2); //66
            public const Cpu.Pin PE3 = (Cpu.Pin) (4*16 + 3); //67
            public const Cpu.Pin PE4 = (Cpu.Pin) (4*16 + 4); //68
            public const Cpu.Pin PE5 = (Cpu.Pin) (4*16 + 5); //69
            public const Cpu.Pin PE6 = (Cpu.Pin) (4*16 + 6); //70
            public const Cpu.Pin PE7 = (Cpu.Pin) (4*16 + 7); //71
            public const Cpu.Pin PE8 = (Cpu.Pin) (4*16 + 8); //72
            public const Cpu.Pin PE9 = (Cpu.Pin) (4*16 + 9); //73 PWM4
            public const Cpu.Pin PE10 = (Cpu.Pin) (4*16 + 10); //74 
            public const Cpu.Pin PE11 = (Cpu.Pin) (4*16 + 11); //75 PWM5
            public const Cpu.Pin PE12 = (Cpu.Pin) (4*16 + 12); //76
            public const Cpu.Pin PE13 = (Cpu.Pin) (4*16 + 13); //77 PWM6
            public const Cpu.Pin PE14 = (Cpu.Pin) (4*16 + 14); //78 PWM7
            public const Cpu.Pin PE15 = (Cpu.Pin) (4*16 + 15); //79
            // ReSharper restore InconsistentNaming
        }

        public class ButtonPins
        {
            public const Cpu.Pin User = Pins.PA0;
        }

        public class LedPins
        {
            public const Cpu.Pin Green = Pins.PD12; //60
            public const Cpu.Pin Orange = Pins.PD13; //61
            public const Cpu.Pin Red = Pins.PD14; //62
            public const Cpu.Pin Blue = Pins.PD15; //63
        }

        public class FreePins
        {
            // ReSharper disable InconsistentNaming
            public const Cpu.Pin PA1 = Pins.PA1;
            public const Cpu.Pin PA2 = Pins.PA2;
            public const Cpu.Pin PA3 = Pins.PA3;
            public const Cpu.Pin PA8 = Pins.PA8;
            public const Cpu.Pin PA15 = Pins.PA15;

            public const Cpu.Pin PB0 = Pins.PB0;
            public const Cpu.Pin PB1 = Pins.PB1;
            public const Cpu.Pin PB2 = Pins.PB2;
            public const Cpu.Pin PB4 = Pins.PB4;
            public const Cpu.Pin PB5 = Pins.PB5;
            public const Cpu.Pin PB7 = Pins.PB7;
            public const Cpu.Pin PB8 = Pins.PB8;
            public const Cpu.Pin PB11 = Pins.PB11;
            public const Cpu.Pin PB12 = Pins.PB12;
            public const Cpu.Pin PB13 = Pins.PB13;
            public const Cpu.Pin PB14 = Pins.PB14;
            public const Cpu.Pin PB15 = Pins.PB15;

            public const Cpu.Pin PC1 = Pins.PC1;
            public const Cpu.Pin PC2 = Pins.PC2;
            public const Cpu.Pin PC4 = Pins.PC4;
            public const Cpu.Pin PC5 = Pins.PC5;
            public const Cpu.Pin PC6 = Pins.PC6;
            public const Cpu.Pin PC8 = Pins.PC8;
            public const Cpu.Pin PC9 = Pins.PC9;
            public const Cpu.Pin PC11 = Pins.PC11;
            public const Cpu.Pin PC13 = Pins.PC13;
            public const Cpu.Pin PC14 = Pins.PC14;
            public const Cpu.Pin PC15 = Pins.PC15;

            public const Cpu.Pin PD1 = Pins.PD1;
            public const Cpu.Pin PD2 = Pins.PD2;
            public const Cpu.Pin PD3 = Pins.PD3;
            public const Cpu.Pin PD6 = Pins.PD6;
            public const Cpu.Pin PD7 = Pins.PD7;
            public const Cpu.Pin PD8 = Pins.PD8;
            public const Cpu.Pin PD9 = Pins.PD9;
            public const Cpu.Pin PD10 = Pins.PD10;
            public const Cpu.Pin PD11 = Pins.PD11;

            public const Cpu.Pin PE3 = Pins.PE3;
            public const Cpu.Pin PE4 = Pins.PE4;
            public const Cpu.Pin PE5 = Pins.PE5;
            public const Cpu.Pin PE6 = Pins.PE6;
            public const Cpu.Pin PE7 = Pins.PE7;
            public const Cpu.Pin PE8 = Pins.PE8;
            public const Cpu.Pin PE9 = Pins.PE9;
            public const Cpu.Pin PE10 = Pins.PE10;
            public const Cpu.Pin PE11 = Pins.PE11;
            public const Cpu.Pin PE12 = Pins.PE12;
            public const Cpu.Pin PE13 = Pins.PE13;
            public const Cpu.Pin PE14 = Pins.PE14;
            public const Cpu.Pin PE15 = Pins.PE15;
            // ReSharper restore InconsistentNaming
        }

        public static class SerialPorts
        {
            // ReSharper disable InconsistentNaming
            public const string COM1 = Serial.COM1;
            public const string COM2 = Serial.COM2;
            public const string COM3 = Serial.COM3;
            public const string COM4 = "COM4";
            public const string COM5 = "COM5";
            public const string COM6 = "COM6";
            // ReSharper restore InconsistentNaming
        }

        public static class BaudRates
        {
            public const BaudRate Baud9600 = BaudRate.Baudrate9600;
            public const BaudRate Baud19200 = BaudRate.Baudrate19200;
            public const BaudRate Baud38400 = BaudRate.Baudrate38400;
            public const BaudRate Baud57600 = BaudRate.Baudrate57600;
            public const BaudRate Baud115200 = BaudRate.Baudrate115200;
            public const BaudRate Baud230400 = BaudRate.Baudrate230400;
        }

        public static class SpiDevices
        {
            // ReSharper disable InconsistentNaming
            public const SPI.SPI_module SPI1 = SPI.SPI_module.SPI1;
            public const SPI.SPI_module SPI2 = SPI.SPI_module.SPI2;
            public const SPI.SPI_module SPI3 = SPI.SPI_module.SPI3;
            // ReSharper restore InconsistentNaming
        }

        public static string GetPinName(Cpu.Pin pin)
        {
            if (pin == Cpu.Pin.GPIO_NONE)
                return "GPIO_NONE";

            var pinNumber = (int) pin;

            int port = pinNumber / 16;
            int num = pinNumber - 16*port;
            string result = "P" + (char)('A' + port) + num;
            return result;
        }
    }
}

//I2C: (SCL, SDA)=(PB6, PB9)

//PWMChannel0: pin=PD12
//PWMChannel1: pin=PD13
//PWMChannel2: pin=PD14
//PWMChannel3: pin=PD15
//PWMChannel4: pin=PE9
//PWMChannel5: pin=PE11
//PWMChannel6: pin=PE13
//PWMChannel7: pin=PE14

//AnalogOutputChannel0: pin=PA4 precisions=12
//AnalogOutputChannel1: pin=PA5 precisions=12

//AnalogChannel0: pin=PA1 precisions=12
//AnalogChannel1: pin=PA2 precisions=12
//AnalogChannel2: pin=PA3 precisions=12
//AnalogChannel3: pin=PB0 precisions=12
//AnalogChannel4: pin=PB1 precisions=12
//AnalogChannel5: pin=PC4 precisions=12
//AnalogChannel6: pin=PC5 precisions=12

//SPI_module1: (msk, miso, mosi)=(PA5, PA6, PA7)
//SPI_module2: (msk, miso, mosi)=(PB13, PB14, PB15)
//SPI_module3: (msk, miso, mosi)=(PC10, PC11, PC12)

//COM1: (rx, tx, cts, rts)=(PA10, PA9, PA11, PA12) baud=1281...5250000
//COM2: (rx, tx, cts, rts)=(PA3, PA2, PD3, PA1) baud=640...2625000
//COM3: (rx, tx, cts, rts)=(PD9, PD8, PD11, PD12) baud=640...2625000
//COM4: (rx, tx, cts, rts)=(PC11, PC10, GPIO_NONE, GPIO_NONE) baud=640...2625000
//COM5: (rx, tx, cts, rts)=(PD2, PC12, GPIO_NONE, GPIO_NONE) baud=640...2625000
//COM6: (rx, tx, cts, rts)=(PC7, PC6, GPIO_NONE, GPIO_NONE) baud=1281...5250000

//USB0: running
//USB1: stopped
