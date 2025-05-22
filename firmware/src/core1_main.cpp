#include <core1_main.h>
// Setup Rising-Edge detection.
// Maybe we bypass it if PWM outputs are set to trigger faster than 1KHz?
// Within ISR, push rising edge events to core0, where they will be bundled with the data.


// TODO: Setup interrupt here.


// Core1 main.
void core1_main()
{
#if defined(DEBUG)
    printf("Hello from core1.\r\n");
#endif
    while (true){}
}
