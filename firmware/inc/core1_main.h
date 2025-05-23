#ifndef CORE1_MAIN_H
#define CORE1_MAIN_H
#include <pico/stdlib.h>
#include <hardware/gpio.h>
#include <hardware/timer.h>
#include <config.h>
#include <queues.h>

void time_us_64_unsafe();
void update_rising_edge_pins();
void run();
void core1_main();

#endif // CORE1_MAIN_H
