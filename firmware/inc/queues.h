#ifndef QUEUES_H
#define QUEUES_H
#include <pico/util/queue.h>
#ifdef DEBUG
    #include <stdio.h>
    #include <cstdio> // for printf
#endif

/**
 * \brief which pins to detect rising edge timing events on. Disabled if none
 *  are set.
**/
extern queue_t rising_edge_monitor_queue;

/**
 * \brief queue to send timestamped rising edge events to core0.
**/
extern queue_t rising_edge_event_queue;


struct RisingEdgeEventData
{
    uint32_t output_state;
    uint64_t time_us;
};

#endif // QUEUES_H
