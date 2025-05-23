#include <core1_main.h>
// Time rising edge Events and queue them for dispatch by core0.

inline uint64_t time_us_64_unsafe()
{
    uint64_t time = timer_hw->timelr; // Locks time until we read TIMEHR.
    return (uint64_t(timer_hw->timehr) << 32) | time;
}


inline void push_event(uint32_t output_state, uint64_t time_us)
{
    RisingEdgeEventData event_data = {output_state, time_us};
    queue_try_add(&rising_edge_event_queue, &event_data);
}


void update_rising_edge_pins()
{
    if queue_is_empty(&rising_edge_monitor_queue)
        return;
    queue_try_remove(&rising_edge_monitor_queue, &rising_edges_to_monitor_);
    uint32_t reply = 1;
}


void run()
{
    while (true)
    {
        update_rising_edge_pins():
        if (!rising_edges_to_monitor_)
            continue;
        // Check for new rising edges.
        uint32_t gpio_state = gpio_get_all();
        uint8_t changed_gpios = last_gpio_state ^ gpio_state;
        // Filter for (a) pins we care about and (b) pins that are HIGH.
        uint8_t rising_edge_gpios = changed_gpios & rising_edges_to_monitor_;
        if (rising_edge_gpios)
            push_msg(rising_edge_gpios, time_us_64_unsafe());
        // Update for next iteration.
        last_gpio_state_ = gpio_state;
    }
}


// Core1 main.
void core1_main()
{
#if defined(DEBUG)
    printf("Hello from core1.\r\n");
#endif
    last_gpio_state_ = 0;
    run(); // blocks forever.
}
