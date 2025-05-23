#include <cuttlefish_cam_trigger_app.h>

PWM pwm_outputs[PWM_OUTPUT_COUNT] =
{
    {PORT_BASE},
    {PORT_BASE + 1},
    {PORT_BASE + 2},
    {PORT_BASE + 3},
    {PORT_BASE + 4},
    {PORT_BASE + 5},
    {PORT_BASE + 6},
    {PORT_BASE + 7}
};


app_regs_t app_regs;


RegSpecs app_reg_specs[REG_COUNT]
{
    {(uint8_t*)&app_regs.PWMEnabledMask, sizeof(app_regs.PWMEnabledMask), U8},
    {(uint8_t*)&app_regs.PWMInvertedMask, sizeof(app_regs.PWMInvertedMask), U8},
    {(uint8_t*)&app_regs.RisingEdgeEventMask, sizeof(app_regs.RisingEdgeEventMask), U8},
    {(uint8_t*)&app_regs.RisingEdgeEvent, sizeof(app_regs.RisingEdgeEvent), U8},

    {(uint8_t*)&app_regs.PWMTaskSettings[0].frequency_hz, sizeof(pwm_task_settings_t::frequency_hz), Float},
    {(uint8_t*)&app_regs.PWMTaskSettings[0].duty_cycle, sizeof(pwm_task_settings_t::duty_cycle), Float},

    {(uint8_t*)&app_regs.PWMTaskSettings[1].frequency_hz, sizeof(pwm_task_settings_t::frequency_hz), Float},
    {(uint8_t*)&app_regs.PWMTaskSettings[1].duty_cycle, sizeof(pwm_task_settings_t::duty_cycle), Float},

    {(uint8_t*)&app_regs.PWMTaskSettings[2].frequency_hz, sizeof(pwm_task_settings_t::frequency_hz), Float},
    {(uint8_t*)&app_regs.PWMTaskSettings[2].duty_cycle, sizeof(pwm_task_settings_t::duty_cycle), Float},

    {(uint8_t*)&app_regs.PWMTaskSettings[3].frequency_hz, sizeof(pwm_task_settings_t::frequency_hz), Float},
    {(uint8_t*)&app_regs.PWMTaskSettings[3].duty_cycle, sizeof(pwm_task_settings_t::duty_cycle), Float},

    {(uint8_t*)&app_regs.PWMTaskSettings[4].frequency_hz, sizeof(pwm_task_settings_t::frequency_hz), Float},
    {(uint8_t*)&app_regs.PWMTaskSettings[4].duty_cycle, sizeof(pwm_task_settings_t::duty_cycle), Float},

    {(uint8_t*)&app_regs.PWMTaskSettings[5].frequency_hz, sizeof(pwm_task_settings_t::frequency_hz), Float},
    {(uint8_t*)&app_regs.PWMTaskSettings[5].duty_cycle, sizeof(pwm_task_settings_t::duty_cycle), Float},

    {(uint8_t*)&app_regs.PWMTaskSettings[6].frequency_hz, sizeof(pwm_task_settings_t::frequency_hz), Float},
    {(uint8_t*)&app_regs.PWMTaskSettings[6].duty_cycle, sizeof(pwm_task_settings_t::duty_cycle), Float},

    {(uint8_t*)&app_regs.PWMTaskSettings[7].frequency_hz, sizeof(pwm_task_settings_t::frequency_hz), Float},
    {(uint8_t*)&app_regs.PWMTaskSettings[7].duty_cycle, sizeof(pwm_task_settings_t::duty_cycle), Float},
};


RegFnPair reg_handler_fns[REG_COUNT]
{
    {HarpCore::read_reg_generic, write_pwm_enabled_mask},
    {HarpCore::read_reg_generic, write_pwm_inverted_mask},
    {HarpCore::read_reg_generic, write_pwm_edge_event_mask},
    {HarpCore::read_reg_generic, HarpCore::write_to_read_only_reg_error},

    {HarpCore::read_reg_generic, write_pwm_frequency_hz},
    {HarpCore::read_reg_generic, write_pwm_duty_cycle},

    {HarpCore::read_reg_generic, write_pwm_frequency_hz},
    {HarpCore::read_reg_generic, write_pwm_duty_cycle},

    {HarpCore::read_reg_generic, write_pwm_frequency_hz},
    {HarpCore::read_reg_generic, write_pwm_duty_cycle},

    {HarpCore::read_reg_generic, write_pwm_frequency_hz},
    {HarpCore::read_reg_generic, write_pwm_duty_cycle},

    {HarpCore::read_reg_generic, write_pwm_frequency_hz},
    {HarpCore::read_reg_generic, write_pwm_duty_cycle},

    {HarpCore::read_reg_generic, write_pwm_frequency_hz},
    {HarpCore::read_reg_generic, write_pwm_duty_cycle},

    {HarpCore::read_reg_generic, write_pwm_frequency_hz},
    {HarpCore::read_reg_generic, write_pwm_duty_cycle},

    {HarpCore::read_reg_generic, write_pwm_frequency_hz},
    {HarpCore::read_reg_generic, write_pwm_duty_cycle},
};

void write_pwm_enabled_mask(msg_t& msg)
{
    HarpCore::copy_msg_payload_to_register(msg);
    uint8_t pwm_enabled_bits = app_regs.PWMEnabledMask;  // make a copy.
    // Enable/disable each pwm output.
    for (auto& pwm_output: pwm_outputs)
    {
        // Update the hardware pwm state.
        // TODO: make sure we've actually written some settings (freq, duty cycle) to this pwm output.
        if (pwm_enabled_bits & 0x01)
            pwm_output.enable_output();
        else
            pwm_output.disable_output();
        pwm_enabled_bits >>= 1;
    }
    if (!HarpCore::is_muted())
        HarpCore::send_harp_reply(WRITE, msg.header.address);
}

void write_pwm_inverted_mask(msg_t& msg)
{
    HarpCore::copy_msg_payload_to_register(msg);
    uint8_t pwm_inverted_bits = app_regs.PWMInvertedMask;  // make a copy.
    for (size_t pwm_pin = PORT_BASE; pwm_pin < PORT_BASE + PWM_OUTPUT_COUNT; ++pwm_pin)
    {
        if (pwm_inverted_bits & 0x01)
            gpio_set_outover(pwm_pin, GPIO_OVERRIDE_INVERT);
        else
            gpio_set_outover(pwm_pin, GPIO_OVERRIDE_NORMAL);
        pwm_inverted_bits >>= 1;
    }
    if (!HarpCore::is_muted())
        HarpCore::send_harp_reply(WRITE, msg.header.address);
}

void write_pwm_edge_event_mask(msg_t& msg)
{
    HarpCore::copy_msg_payload_to_register(msg);
    uint32_t rising_edges_to_monitor = uint32_t(app_regs.RisingEdgeEventMask)
                                       << PORT_BASE;
    queue_try_add(&rising_edge_monitor_queue, &rising_edges_to_monitor);
    if (!HarpCore::is_muted())
        HarpCore::send_harp_reply(WRITE, msg.header.address);
}

void write_pwm_frequency_hz(msg_t& msg)
{
    HarpCore::copy_msg_payload_to_register(msg);
    uint32_t pwm_index = reg_to_pwm_index(msg.header.address);
    float& frequency_hz = app_regs.PWMTaskSettings[pwm_index].frequency_hz;
    pwm_outputs[pwm_index].set_frequency(frequency_hz);
    if (!HarpCore::is_muted())
        HarpCore::send_harp_reply(WRITE, msg.header.address);
}

void write_pwm_duty_cycle(msg_t& msg)
{
    HarpCore::copy_msg_payload_to_register(msg);

    if (!HarpCore::is_muted())
        HarpCore::send_harp_reply(WRITE, msg.header.address);
}


void update_app()
{
    // Receive msgs from core1 with state/timings.
    if (!queue_is_empty(&rising_edge_event_queue))
    {
        // Retrieve the rising edge event data from the queue.
        RisingEdgeEventData event_data;
        queue_remove_blocking(&rising_edge_event_queue, &event_data);
        // Offset to account for the GPIO to IO mapping.
        app_regs.RisingEdgeEvent = uint8_t(event_data.output_state >> PORT_BASE);
        //  Send them back over Harp Protocol with a Harp clock domain timestamp.
        HarpCore::send_harp_reply(EVENT, AppRegNum::RisingEdgeEvent,
                                  HarpCore::system_to_harp_us_64(event_data.time_us));
    }
    // Disable output waveforms if we've disconnected com ports (safety feature).
    if (HarpCore::get_op_mode() != ACTIVE)
    {
        for (auto& pwm_output: pwm_outputs)
            pwm_output.disable_output();
    }
}

void reset_app()
{
    // Clear all settings configurations to all zero.
    // FIXME: do this.
    // FIXME: disable event dispatch.
    // FIXME: clear queues.
    // Configure bus switches for software control of the BNC connectors.
    // Init bus switch pins.
    gpio_init_mask((0x000000FF << PORT_DIR_BASE));
    // Set bus switch to all-outputs and drive an output setting for main IO pins.
    gpio_set_dir_masked(0x000000FF << PORT_DIR_BASE, 0xFFFFFFFF);
    gpio_put_masked(0x000000FF << PORT_DIR_BASE, 0xFFFFFFFF);
}
