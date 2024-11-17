/* mbed Microcontroller Library
 * Copyright (c) 2019 ARM Limited
 * SPDX-License-Identifier: Apache-2.0
 */

#include "mbed.h"
#include <cstdio>
#include <string>

#define WAIT_TIME_MS 100 
UnbufferedSerial pc(USBTX, USBRX, 115200);
DigitalOut led1(LED1);

float x = 0;
float y = 0;
char outPut[50];

int main()
{
    int length = snprintf(outPut, sizeof(outPut), "init\n");
    pc.write(outPut, length);
    thread_sleep_for(500);

    while (true)
    {
        led1 = !led1;
        x += 0.01;
        y = 1/x;
        int length = snprintf(outPut, sizeof(outPut), "%f,%f\n", x, y);
        pc.write(outPut, length);
        thread_sleep_for(WAIT_TIME_MS);
    }
}
