#!/usr/bin/env python3
from pyharp.device import Device, DeviceMode
from pyharp.messages import WriteFloatHarpMessage, WriteU8HarpMessage
from pyharp.messages import MessageType
from pyharp.messages import CommonRegisters as Regs
from app_registers import AppRegs
from time import sleep
from time import perf_counter as now
import serial.tools.list_ports

#import logging
#logger = logging.getLogger()
#logger.setLevel(logging.DEBUG)
#logger.addHandler(logging.StreamHandler())
#logger.handlers[-1].setFormatter(
#    logging.Formatter(fmt='%(asctime)s:%(name)s:%(levelname)s: %(message)s'))

# Open serial connection with the first camera trigger device
com_port = None
ports = serial.tools.list_ports.comports()
for port, desc, hwid in sorted(ports):
    if desc.startswith("cuttlefish-cam-trigger"):
        print("{}: {} [{}]".format(port, desc, hwid))
        com_port = port
        break
device = Device(com_port)

def send(msg_type, register, data):
    reply = device.send(msg_type(register, data).frame)
    if reply.message_type == MessageType.WRITE_ERROR:
        raise RuntimeError(f"Sending: {msg_type}({register}, {data}) replied with a WRITE_ERROR")

send(WriteU8HarpMessage, AppRegs.RisingEdgeEventMask, 0xFF)
send(WriteFloatHarpMessage, AppRegs.PWM0FrequencyHz, 20)
send(WriteFloatHarpMessage, AppRegs.PWM0DutyCycle, 0.5)
send(WriteFloatHarpMessage, AppRegs.PWM1FrequencyHz, 20)
send(WriteFloatHarpMessage, AppRegs.PWM1DutyCycle, 0.5)

send(WriteU8HarpMessage, AppRegs.PWMEnabledMask, 0x03)

# Read back events.
start_time = now()
while now() - start_time < 3:
    for msg in device.get_events():
        print(msg)
        print()
send(WriteU8HarpMessage, AppRegs.PWMEnabledMask, 0)


