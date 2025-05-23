#ifndef CORE1_MAIN_H
#define CORE1_MAIN_H
#include <pico/stdlib.h>
#include <hardware/gpio.h>
#include <hardware/timer.h>
#include <config.h>
#include <queues.h>

// Do not call this in mixed contexts (inside/outide of ISRs)!
inline uint64_t time_us_64_unsafe()
{
    uint64_t time = timer_hw->timelr;
    return (uint64_t(timer_hw->timehr) << 32) | time;
}

inline void push_event(uint32_t output_state, uint64_t time_us)
{
    RisingEdgeEventData event_data = {output_state, time_us};
    queue_try_add(&rising_edge_event_queue, &event_data);
}

void update_rising_edge_pins();
void run();
void core1_main();

#endif // CORE1_MAIN_H
