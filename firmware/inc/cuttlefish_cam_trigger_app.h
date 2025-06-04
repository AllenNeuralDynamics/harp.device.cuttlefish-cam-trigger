#ifndef CUTTLEFISH_CAM_TRIGGER_APP_H
#define CUTTLEFISH_CAM_TRIGGER_APP_H
#include <pico/stdlib.h>
#include <config.h>
#include <harp_message.h>
#include <harp_core.h>
#include <harp_c_app.h>
#include <harp_synchronizer.h>
#include <queues.h>
#include <pico/multicore.h>
#include <cmath>
#include <pwm.h>  // RP2040 PWM library
#ifdef DEBUG
    #include <stdio.h>
    #include <cstdio> // for printf
#endif

// Setup for Harp App

extern PWM pwm_outputs[PWM_OUTPUT_COUNT];

inline constexpr size_t PWM_TASK_ELEMENT_COUNT = 2;

#pragma pack(push, 1)
struct pwm_task_settings_t
{
    float frequency_hz;
    float duty_cycle;   // 0 - 1.0
};
#pragma pack(pop)


inline constexpr uint8_t PWM_REG_OFFSET = 6;

#pragma pack(push, 1)
struct app_regs_t
{
    uint8_t PWMEnabledMask;
    uint8_t PWMSetMask;
    uint8_t PWMClearMask;
    uint8_t PWMInvertedMask;
    uint8_t RisingEdgeEventMask;
    uint8_t RisingEdgeEvent;
    pwm_task_settings_t PWMTaskSettings[PWM_OUTPUT_COUNT];
    // More app "registers" here.
};
#pragma pack(pop)

inline constexpr uint8_t REG_COUNT = PWM_REG_OFFSET
                                     + PWM_TASK_ELEMENT_COUNT * PWM_OUTPUT_COUNT;
extern RegSpecs app_reg_specs[REG_COUNT];
extern RegFnPair reg_handler_fns[REG_COUNT];
extern HarpCApp& app;

enum AppRegNum: uint32_t
{
    PWMEnabledMask = 32,
    PWMSetMask = 33,
    PWMClearMask = 34,
    PWMInvertedMask = 35,
    RisingEdgeEventMask = 36,
    RisingEdgeEvent = 37,

    PWM0FrequencyHz = 38,
    PWM0DutyCycle = 39,
    PWM1FrequencyHz = 40,
    PWM1DutyCycle = 41,
    PWM2FrequencyHz = 42,
    PWM2DutyCycle = 43,
    PWM3FrequencyHz = 44,
    PWM3DutyCycle = 45,
    PWM4FrequencyHz = 46,
    PWM4DutyCycle = 47,
    PWM5FrequencyHz = 48,
    PWM5DutyCycle = 49,
    PWM6FrequencyHz = 50,
    PWM6DutyCycle = 51,
    PWM7FrequencyHz = 52,
    PWM7DutyCycle = 53
};

extern app_regs_t app_regs;

inline uint32_t reg_to_pwm_index(uint32_t reg)
{
    // Warning: no error-checking for registers outside the pwm range!
    size_t pwm_settings_starting_reg = APP_REG_START_ADDRESS
                                       + PWM_REG_OFFSET;
    return ceil(reg - pwm_settings_starting_reg)/PWM_TASK_ELEMENT_COUNT;
}


void write_pwm_enabled_mask(msg_t& msg);
void write_pwm_set_mask(msg_t& msg);
void write_pwm_clear_mask(msg_t& msg);
void write_pwm_inverted_mask(msg_t& msg);
void write_pwm_edge_event_mask(msg_t& msg);

void write_pwm_frequency_hz(msg_t& msg);
void write_pwm_duty_cycle(msg_t& msg);


/**
 * \brief update the app state. Called in a loop.
 */
void update_app();

/**
 * \brief reset the app.
 */
void reset_app();

#endif // CUTTLEFISH_CAM_TRIGGER_APP_H
