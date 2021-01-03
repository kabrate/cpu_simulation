using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApp1
{
    public class PC//程序计数寄存器
    {
        public string name;
        public int value = 0;
        public int[] bit = new int[16];
        public PC()
        {
            
            for(int i=0;i<16;i++)
            {
                bit[i] = 0;
            }
        }
            
    }
    class CR//通用寄存器
    {
        public string name;//名字
        public int value = 0;//十进制的值
        public int address;//地址
        public int[] bit = new int[8];//二进制的值
        public CR()
        {
            
            for (int i = 0; i < 8; i++)
            {
                bit[i] = 0;
            }
        }
    }
    class SR//程序状态寄存器
    {
        public string name=" ";
        public int value = 0;
        public int addres = 0;
        public int[] bit = new int[8];
        public SR()
        {
            
            for (int i = 0; i < 8; i++)
            {
                bit[i] = 0;
            }
        }
    }
}
