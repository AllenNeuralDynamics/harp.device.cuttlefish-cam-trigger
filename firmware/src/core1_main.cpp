#include <core1_main.h>
// Time rising edge Events and queue them for dispatch by core0.
uint32_t last_gpio_state_;
uint32_t rising_edges_to_monitor_;

void update_rising_edge_pins()
{
    if (queue_is_empty(&rising_edge_monitor_queue))
        return;
    queue_try_remove(&rising_edge_monitor_queue, &rising_edges_to_monitor_);
    uint32_t reply = 1;
}


void run()
{
    while (true)
    {
        update_rising_edge_pins();
        if (!rising_edges_to_monitor_)
            continue;
        // Check for new rising edges.
        uint32_t gpio_state = gpio_get_all();
        uint8_t changed_gpios = last_gpio_state_ ^ gpio_state;
        // Filter for (a) pins we care about and (b) pins that are HIGH.
        uint8_t rising_edge_gpios = changed_gpios & rising_edges_to_monitor_;
        if (rising_edge_gpios)
            push_event(rising_edge_gpios, time_us_64_unsafe());
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
    rising_edges_to_monitor_ = 0;
    run(); // blocks forever.
}
