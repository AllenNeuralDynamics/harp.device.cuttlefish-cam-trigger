#include <core1_main.h>
// Time rising edge Events and queue them for dispatch by core0.
uint32_t last_gpio_state_;
uint32_t rising_edges_to_monitor_;

void update_rising_edge_pins()
{
    if (queue_is_empty(&rising_edge_monitor_queue))
        return;
    queue_try_remove(&rising_edge_monitor_queue, &rising_edges_to_monitor_);
#if(DEBUG)
    printf("Received rising edge(s) to monitor: %d\r\n", rising_edges_to_monitor_);
#endif
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
        uint32_t changed_gpios = last_gpio_state_ ^ gpio_state;
        // Filter for (a) pins we care about and (b) pins that are HIGH.
        uint32_t rising_edge_gpios = changed_gpios & gpio_state & rising_edges_to_monitor_;
        if (rising_edge_gpios)
        {
            push_event(rising_edge_gpios, time_us_64_unsafe());
#if(DEBUG)
            printf("Rising edge event dispatched!\r\n");
#endif
        }
        // Update for next iteration.
        last_gpio_state_ = gpio_state;
    }
}


// Core1 main.
void core1_main()
{
#if(DEBUG)
    printf("Hello from core1.\r\n");
#endif
    last_gpio_state_ = 0;
    rising_edges_to_monitor_ = 0;
    run(); // blocks forever.
}
