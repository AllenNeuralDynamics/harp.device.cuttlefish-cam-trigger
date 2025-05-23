#include <pico/stdlib.h>
#include <cstring>
#include <config.h>
#include <harp_c_app.h>
#include <harp_synchronizer.h>
#include <cuttlefish_app.h>
#include <schedule_ctrl_queues.h>
#include <pico/multicore.h>
#include <hardware/structs/bus_ctrl.h>
#include <core1_main.h>

queue_t rising_edge_event_queue;

// Create device name array.
const uint16_t who_am_i = CUTTLEFISH_CAM_TRIGGER_HARP_DEVICE_ID;
const uint8_t hw_version_major = 0;
const uint8_t hw_version_minor = 0;
const uint8_t assembly_version = 0;
const uint8_t harp_version_major = 0;
const uint8_t harp_version_minor = 0;
const uint8_t fw_version_major = 0;
const uint8_t fw_version_minor = 0;
const uint16_t serial_number = 0;

// Create Core.
HarpCApp& app = HarpCApp::init(who_am_i, hw_version_major, hw_version_minor,
                               assembly_version,
                               harp_version_major, harp_version_minor,
                               fw_version_major, fw_version_minor,
                               serial_number, "cuttlefish-cam-trigger",
                               (uint8_t*)GIT_HASH,
                               &app_regs, app_reg_specs,
                               reg_handler_fns, reg_count, update_app_state,
                               reset_app);

// Core0 main.
int main()
{
    // Init Synchronizer.
    HarpSynchronizer::init(SYNC_UART, HARP_SYNC_RX_PIN);
    app.set_synchronizer(&HarpSynchronizer::instance());
    // Configure core1 to have high priority on the bus.
    bus_ctrl_hw->priority = 0x00000010;
    // Initialize queues for multicore communication.
    queue_init(&rising_edge_monitor_queue, sizeof(uint32_t), MAX_QUEUE_SIZE);
    queue_init(&rising_edge_monitor_reply_queue, sizeof(uint32_t), MAX_QUEUE_SIZE);
    queue_init(&rising_edge_event_queue, sizeof(RisingEdgeEventData), MAX_QUEUE_SIZE);

#if defined(DEBUG) || defined(PROFILE_CPU)
#warning "Initializing printf from UART will slow down core1 main loop."
    stdio_uart_init_full(DEBUG_UART, 921600, DEBUG_UART_TX_PIN, -1);
#endif

    multicore_reset_core1();
    (void)multicore_fifo_pop_blocking(); // Wait until core1 is ready.
    multicore_launch_core1(core1_main);
    reset_app(); // Setup GPIO states. Get scheduler ready.
    // Loop forever.
    while(true)
        app.run();
}
