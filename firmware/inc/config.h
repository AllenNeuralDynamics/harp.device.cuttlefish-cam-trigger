#ifndef CONFIG_H
#define CONFIG_H


#define DEBUG_UART (uart0)
#define DEBUG_UART_TX_PIN (0) // for printf-style debugging.

#define SYNC_UART (uart1)
#define HARP_SYNC_RX_PIN (5)
#define LED0 (24)
#define LED1 (25)

#define PORT_BASE (8)
#define PORT_DIR_BASE (16)

#define CUTTLEFISH_CAM_TRIGGER_HARP_DEVICE_ID (1408)

inline constexpr uint8_t MAX_QUEUE_SIZE = 32;

#endif // CONFIG_H
