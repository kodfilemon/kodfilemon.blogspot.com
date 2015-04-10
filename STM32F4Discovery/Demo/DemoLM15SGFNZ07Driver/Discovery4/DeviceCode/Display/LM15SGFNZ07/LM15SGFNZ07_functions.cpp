////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) Microsoft Corporation.  All rights reserved.
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

#include "tinyhal.h"
//#include <Targets\Native\STM32F4\DeviceCode\stm32f4xx.h>

//#define REDLED (3*16 + 14)
//#define LED4 (3*16 + 15)

#define PE9 (4*16 + 9)
#define PE10 (4*16 + 10)
#define PE11 (4*16 + 11)

#define CS_PIN PE11
#define RESET_PIN PE10
#define RS_PIN PE9

SPI_CONFIGURATION spiCfg = {
                            CS_PIN, // Chip select
                            FALSE,                 // Chip Select polarity
                            TRUE,                  // MSK_IDLE
                            TRUE,                  // MSK_SAMPLE_EDGE
                            FALSE,                 // 16-bit mode
                            5000,                  // SPI Clock Rate KHz
                            0,                     // CS setup time us
                            0,                     // CS hold time us
                            1,   // SPI Module
                            {
                                GPIO_PIN_NONE,     // SPI BusyPin
                                FALSE,             // SPI BusyPinActiveState
                            }
                          }; 

void Lm15Sgfnz07_SendCommand(UINT8* data, INT32 count) 
{
  CPU_GPIO_SetPinState(RS_PIN, TRUE);
  CPU_SPI_nWrite8_nRead8(spiCfg, data, count, NULL, 0, 0);  
}

void Lm15Sgfnz07_SendData(UINT8* data, INT32 count) 
{
  CPU_GPIO_SetPinState(RS_PIN, FALSE);
  CPU_SPI_nWrite8_nRead8(spiCfg, data, count, NULL, 0, 0);  
}

void Lm15Sgfnz07_SendData16(UINT16* data, INT32 count) 
{
  CPU_GPIO_SetPinState(RS_PIN, FALSE);
  spiCfg.MD_16bits = TRUE;
  CPU_SPI_nWrite16_nRead16(spiCfg, data, count, NULL, 0, 0);
  spiCfg.MD_16bits = FALSE;
}

void Lm15Sgfnz07_Window(UINT8 x1, UINT8 y1, UINT8 x2, UINT8 y2) 
{
    x1 <<= 1;
    x1 += 6;
    x2 <<= 1;
    x2 += 7;
    
    UINT8 data[10];
    
    data[0] = 0xf0;
    data[1] = 0x00 | (x1 & 0x0f);
    data[2] = 0x10 | (x1 >> 4);
    data[3] = 0x20 | (y1 & 0x0f);
    data[4] = 0x30 | (y1 >> 4);
    data[5] = 0xf5;
    data[6] = 0x00 | (x2 & 0x0f);
    data[7] = 0x10 | (x2 >> 4);
    data[8] = 0x20 | (y2 & 0x0f);
    data[9] = 0x30 | (y2 >> 4);
    
    Lm15Sgfnz07_SendCommand(data, 10);
}

void Lm15Sgfnz07_Contrast(UINT8 contrast)
{
  UINT8 buf[3];
  buf[0] = 0xf4;
  buf[1] = 0xb0 | (contrast >> 4);
  buf[2] = 0xa0 | (contrast & 0x0f);
  
  Lm15Sgfnz07_SendCommand(buf, 3);
}

BOOL LCD_Initialize()
{
    NATIVE_PROFILE_HAL_DRIVERS_DISPLAY();
    //CPU_GPIO_EnableOutputPin(LED4, FALSE);
    
    CPU_GPIO_EnableOutputPin(CS_PIN, TRUE);
    CPU_GPIO_EnableOutputPin(RESET_PIN, TRUE);
    CPU_GPIO_EnableOutputPin(RS_PIN, FALSE);
    
    CPU_GPIO_SetPinState(RESET_PIN, FALSE);
    HAL_Time_Sleep_MicroSeconds_InterruptEnabled(10*1000);
    CPU_GPIO_SetPinState(RESET_PIN, TRUE);
    HAL_Time_Sleep_MicroSeconds_InterruptEnabled(10*1000);
        
    UINT8 initData1[139] = {0xF4, 0x90, 0xB3, 0xA0, 0xD0,
                             0xF0, 0xE2, 0xD4, 0x70, 0x66, 0xB2, 0xBA, 0xA1, 0xA3, 0xAB, 0x94, 0x95,
                             0x95, 0x95, 0xF5, 0x90, 0xF1, 0x00, 0x10, 0x22, 0x30, 0x45, 0x50, 0x68,
                             0x70, 0x8A, 0x90, 0xAC, 0xB0, 0xCE, 0xD0, 0xF2, 0x0F, 0x10, 0x20, 0x30,
                             0x43, 0x50, 0x66, 0x70, 0x89, 0x90, 0xAB, 0xB0, 0xCD, 0xD0, 0xF3, 0x0E,
                             0x10, 0x2F, 0x30, 0x40, 0x50, 0x64, 0x70, 0x87, 0x90, 0xAA, 0xB0, 0xCB,
                             0xD0, 0xF4, 0x0D, 0x10, 0x2E, 0x30, 0x4F, 0x50, 0xF5, 0x91, 0xF1, 0x01,
                             0x11, 0x22, 0x31, 0x43, 0x51, 0x64, 0x71, 0x86, 0x91, 0xA8, 0xB1, 0xCB,
                             0xD1, 0xF2, 0x0F, 0x11, 0x21, 0x31, 0x42, 0x51, 0x63, 0x71, 0x85, 0x91,
                             0xA6, 0xB1, 0xC8, 0xD1, 0xF3, 0x0B, 0x11, 0x2F, 0x31, 0x41, 0x51, 0x62,
                             0x71, 0x83, 0x91, 0xA4, 0xB1, 0xC6, 0xD1, 0xF4, 0x08, 0x11, 0x2B, 0x31,
                             0x4F, 0x51, 0x80, 0x94, 0xF5, 0xA2, 0xF4, 0x60, 0xF0, 0x40, 0x50, 0xC0,
                             0xF4, 0x70};
    
    Lm15Sgfnz07_SendCommand(initData1, 139);
    HAL_Time_Sleep_MicroSeconds_InterruptEnabled(10*1000);
    
    UINT8 initData2[15] = {0xF0, 0x81,
                            0xF4, 0xB3, 0xA0,
                            0xF0, 0x06, 0x10, 0x20, 0x30,
                            0xF5, 0x0F, 0x1C, 0x2F, 0x34};
                                          
    Lm15Sgfnz07_SendCommand(initData2, 15);
    HAL_Time_Sleep_MicroSeconds_InterruptEnabled(10*1000);
    
    LCD_Clear();
    Lm15Sgfnz07_Contrast(42);
    
    return TRUE;
}

BOOL LCD_Uninitialize()
{
    NATIVE_PROFILE_HAL_DRIVERS_DISPLAY();

	LCD_Clear();

    return TRUE;
}

void LCD_Clear()
{
    NATIVE_PROFILE_HAL_DRIVERS_DISPLAY();
    
    int width = LCD_GetWidth();
    int height = LCD_GetHeight(); 
    int size = width * height * 2;
    UINT8* buf = (UINT8*)private_malloc(size);
    memset(buf, 0x00, size);
           
    Lm15Sgfnz07_Window(0, 0, width - 1, height - 1);
    Lm15Sgfnz07_SendData(buf, size);
    private_free(buf);
}

void LCD_BitBltEx( int x, int y, int width, int height, UINT32 data[] )
{
    NATIVE_PROFILE_HAL_DRIVERS_DISPLAY();
        
    const int size = width * height;
    UINT16* buf = (UINT16*)private_malloc(size*2);
        
    int widthInWords = Graphics_GetWidthInWords(width);
    for(int py = 0; py < height; py++)
    {
      for(int px = 0; px < width; px++)
      {
#if !defined(BIG_ENDIAN)
        UINT32 shift = (px % 2) * 16;
        UINT32 mask = 0x0000FFFF << shift;
#else
        UINT32 shift = ((px+1) % 2) * 16;
        UINT32 mask = 0x0000FFFF << shift;
#endif
      
        UINT16 pixel = (data[py*widthInWords + px/2] & mask) >> shift;
        //from RGB 5:6:5
        //to RGB 0:4:4:4 
        pixel = ((pixel >> 4) & 0x0F00) | ((pixel >> 3) & 0x00F0) | ((pixel >> 1) & 0x000F);
        buf[py*width + px] = pixel;
      }
    }
    
    Lm15Sgfnz07_Window(x, y, x + width - 1, y + height - 1);
    Lm15Sgfnz07_SendData16(buf, size);
    private_free(buf);
}

void LCD_BitBlt( int width, int height, int widthInWords, UINT32 data[], BOOL fUseDelta )
{
    NATIVE_PROFILE_HAL_DRIVERS_DISPLAY();
    
    LCD_BitBltEx( 0, 0, width, height, data );
}

void LCD_WriteChar( unsigned char c, int row, int col )
{
    NATIVE_PROFILE_HAL_DRIVERS_DISPLAY();
    
    //const UINT8* font = Font_GetGlyph(c);
    //UINT8 fontWidth = Font_Width();
    //UINT8 fontHeight = Font_Height();
    
    //int size = fontWidth * fontHeight;
    //UINT16* buf = (UINT16*)private_malloc(size * 2);
    //memset(buf, 0xFF, size * 2);
    
    //for(int y = 0, i = 0; y < fontHeight; y++)
    //{
    //  UINT8 mask = 0x80;
    //  for (int x = 0; x < fontWidth; x++)
    //  {
    //      if ((font[y] & mask) > 0)
    //          buf[i++] = 0x0FFF;
    //      else
    //          buf[i++] = 0x0000;
    //          
    //      mask >>= 1;
    //  }
    //}
    
    //UINT8 px = row * fontWidth;
    //UINT8 py = col * fontHeight;
    
    //Lm15Sgfnz07_Window(px, py, px + fontWidth - 1, py + fontHeight - 1);
    //Lm15Sgfnz07_SendData16(buf, size);
    //private_free(buf);
}

void LCD_WriteFormattedChar( unsigned char c )
{
    NATIVE_PROFILE_HAL_DRIVERS_DISPLAY();
    
    if(c < 32)
    {		
        switch(c)
        {		
          case '\f':                    /* formfeed, clear screen and home cursor */
            break;

        case '\n':                      /* newline */
            break;

        case '\r':                      /* carriage return */
            break;

        case '\t':                      /* horizontal tab */
            break;

        case '\v':                      /* vertical tab */
            break;

        default:
            break;
        }
    }
    else
    {		
        //wszystkie znaki wyswietlami w lewym górnym rogu
        //LCD_WriteChar(c, 0, 0);
        //HAL_Time_Sleep_MicroSeconds_InterruptEnabled(100*1000);
    }
}

INT32 LCD_GetWidth()
{
    NATIVE_PROFILE_HAL_DRIVERS_DISPLAY();
    return 101;
}

INT32 LCD_GetHeight()
{
    NATIVE_PROFILE_HAL_DRIVERS_DISPLAY();
    return 80;
}

INT32 LCD_GetBitsPerPixel()
{
    NATIVE_PROFILE_HAL_DRIVERS_DISPLAY();
    return 16;
}

UINT32 LCD_GetPixelClockDivider()
{
    NATIVE_PROFILE_HAL_DRIVERS_DISPLAY();
    return 0;
}
INT32 LCD_GetOrientation()
{
    NATIVE_PROFILE_HAL_DRIVERS_DISPLAY();
    return 0;
}
void LCD_PowerSave( BOOL On )
{
    NATIVE_PROFILE_HAL_DRIVERS_DISPLAY();
}

UINT32* LCD_GetFrameBuffer()
{
    NATIVE_PROFILE_HAL_DRIVERS_DISPLAY();
    return NULL;
}

UINT32 LCD_ConvertColor(UINT32 color)
{
    NATIVE_PROFILE_HAL_DRIVERS_DISPLAY();
    return color;
}
