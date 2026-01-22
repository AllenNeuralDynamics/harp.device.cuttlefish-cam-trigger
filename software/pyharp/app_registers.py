"""App registers for cuttlefish-cam-controller"""
from enum import IntEnum


class AppRegs(IntEnum):
    PWMEnabledMask = 32
    PWMSetMask = 33
    PWMClearMask = 34
    PWMInvertedMask = 35
    RisingEdgeEventMask = 36
    RisingEdgeEvent = 37

    PWM0FrequencyHz = 38
    PWM0DutyCycle = 39
    PWM1FrequencyHz = 40
    PWM1DutyCycle = 41
    PWM2FrequencyHz = 42
    PWM2DutyCycle = 43
    PWM3FrequencyHz = 44
    PWM3DutyCycle = 45
    PWM4FrequencyHz = 46
    PWM4DutyCycle = 47
    PWM5FrequencyHz = 48
    PWM5DutyCycle = 49
    PWM6FrequencyHz = 50
    PWM6DutyCycle = 51
    PWM7FrequencyHz = 52
    PWM7DutyCycle = 53
