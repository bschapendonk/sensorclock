ADDR_ALLCALL = const(0x70)
ADDR_HOUR = const(0x71)
ADDR_MINUTE = const(0x01)
ADDR_SECOND = const(0x02)
AUTO_INCREMENT = const(0x80)
DIMM_HOUR_BEGIN = const(22)
DIMM_HOUR_END = const(8)
MODE1_ALLCALL = const(0x01)
MODE1_SLEEP = const(0x10)
MODE1_SUBADDR1 = const(0x08)
PWM_DEFAULT = const(0x44)
PWM_DIMM = const(0x01)
REGISTER_GRPPWM = const(0x12)
REGISTER_LEDOUT0 = const(0x14)
REGISTER_MODE1 = const(0x00)
REGISTER_PWM0 = const(0x02)

DIGITS = [
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0x00,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0x00,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0x00,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0x00,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0xFF,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0x00,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0x00,
        0x00,
        0x00,
        0x00,
        0x00,
        0x00,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0x00,
        0x00,
        0x00,
        0x00,
        0x00,
        0x00,
        0xFF,
        0x00,
        0x00,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0x00,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0x00,
        0x00,
        0x00,
        0x00,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0x00,
        0x00,
        0x00,
        0x00,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0x00,
        0x00,
        0x00,
        0x00,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0x00,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0x00,
        0x00,
        0x00,
        0x00,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0x00,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0x00,
        0x00,
        0x00,
        0x00,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0xFF,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0x00,
        0x00,
        0x00,
        0x00,
        0x00,
        0x00,
        0xFF,
        0x00,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0x00,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0x00,
        0x00,
        0x00,
        0x00,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0x00,
        0x00,
        0x00,
        0x00,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0x00,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0x00,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0x00,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0x00,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0xFF,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0x00,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0x00,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0x00,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0x00,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0x00,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0xFF,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0x00,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0x00,
        0x00,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0x00,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0x00,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0x00,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0xFF,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0x00,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0x00,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0x00,
        0x00,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0x00,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0x00,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0x00,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0xFF,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0x00,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0x00,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0x00,
        0x00,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0x00,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0x00,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0x00,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0xFF,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0x00,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0x00,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0x00,
        0x00,
        0x00,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0x00,
        0x00,
        0x00,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0x00,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0x00,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0x00,
        0x00,
        0x00,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0x00,
        0x00,
        0x00,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0x00,
        0x00,
        0x00,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0x00,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0x00,
        0x00,
        0x00,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0x00,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0x00,
        0x00,
        0x00,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0xFF,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0x00,
        0x00,
        0x00,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0x00,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0x00,
        0x00,
        0x00,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0x00,
        0x00,
        0x00,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0x00,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0x00,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0x00,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0x00,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0xFF,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0x00,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0x00,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0x00,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0x00,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0x00,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0xFF,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0x00,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0x00,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
    ],
    [
        REGISTER_PWM0 | AUTO_INCREMENT,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
        0xFF,
        0xFF,
        0xFF,
        0xFF,
        0x00,
    ],
]