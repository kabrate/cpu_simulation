using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public int num = 0;//进行到到几步
        string[] sArray;//指令数组
        int time;//机器周期
        List<CR> crlist = new List<CR>();//通用寄存器
        PC pc = new PC();//PC寄存器
        SR sr = new SR();//SR寄存器
        string text = "";//保存结果
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {


            for (int i = 0; i < 8; i++)
            {
                crlist.Add(new CR());
            }
            for (int i = 0; i < 8; i++)
            {
                crlist[i].name = "R" + (i).ToString();//得到寄存器的名字
                crlist[i].address = i;//得到寄存器的地址
            }
            for (int i = 0; i < 8; i++)
            {
                fresh("R" + i, "00000000");
            }
            fresh("PC", "00000000");
            fresh("IR", "00000000");
            fresh("DR", "00000000");
            fresh("SR", "00000000");
            fresh("TM", "0");
            H.Text = "0";
            S.Text = "0";
            V.Text = "0";
            N.Text = "0";
            Z.Text = "0";
            C.Text = "0";
            //string str = "Ldi R9,AA" +
            //    "/r/n" + "Ldi R10,55";//两次分隔

            //// string[] sArray = str.Split(new char[2] { ' ', ',' });
            //string[] sArray = str.Split(new string[] { "/r/n" }, StringSplitOptions.None);
            ////string temp1 = Convert.ToString(66,2);//将10进制值转化为二进制字符串
        }
        public void display (string i)
            {
            if (crlist[register(i)].value > 15) H.Text = "1";//H位
            else H.Text = "0";
            if (crlist[register(i)].value < -128 || crlist[register(i)].value > 127) V.Text = "1";//V位
            else V.Text = "0";
            if (crlist[register(i)].value > 255) C.Text = "1";//C位
            else C.Text = "0";
            if (crlist[register(i)].value < 0) N.Text = "1";//N位
            else N.Text = "0";
            if (crlist[register(i)].value == 0) Z.Text = "1";//Z位
            else Z.Text = "0";
            if (N.Text == V.Text) S.Text = "0";
            else { S.Text = "1"; }
            S_R.Text = "00" + H.Text + S.Text + V.Text + N.Text + Z.Text + C.Text;
        }
        private void Button1_Click(object sender, EventArgs e)//分割成为单个的指令
        {
            StreamReader sr;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "指令(*.txt;*.data)|*.txt;*.data";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                sr = File.OpenText(ofd.FileName);
                string str = sr.ReadToEnd();
                sArray = str.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for(int i=0;i<sArray.Length;i++)
                {
                    textBox1.Text = textBox1.Text+sArray[i]+"\r\n";
                }
                
                sr.Close();
            }
        }
        private void Button2_Click(object sender, EventArgs e)//运行指令
        {
            string[] str = instrcting.Text.Split(new char[2] { ' ', ',' });
            if (str.Length == 1) instruct_processing(str[0]);
            else if (str.Length == 2) instruct_processing(str[0], str[1]);
            else if (str.Length == 3) instruct_processing(str[0], str[1], str[2]);
        }
        public int register(string r)//通过字符串得到寄存器号
        {
            if (r == "R0") return 0;
            else if (r == "R1") return 1;
            else if (r == "R2") return 2;
            else if (r == "R3") return 3;
            else if (r == "R4") return 4;
            else if (r == "R5") return 5;
            else if (r == "R6") return 6;
            else if (r == "R7") return 7;
            else return 8;
        }
        private void fresh(string name, string str)//刷新寄存器显示
        {
            switch (name)
            {
                case "R0": { R0.Text = str; } break;
                case "R1": { R1.Text = str; } break;
                case "R2": { R2.Text = str; } break;
                case "R3": { R3.Text = str; } break;
                case "R4": { R4.Text = str; } break;
                case "R5": { R5.Text = str; } break;
                case "R6": { R6.Text = str; } break;
                case "R7": { R7.Text = str; } break;
                case "PC": { P_C.Text = str; } break;
                case "IR": { I_R.Text = str; } break;
                case "DR": { D_R.Text = str; } break;
                case "SR": { S_R.Text = str; } break;
                case "TM": { TM.Text = str; } break;
            }
        }
        
        public string And(string str1, string str2)//二进制与运算
        {
            char[] str = new char[8];
            for (int i = 0; i < 8; i++)
            {
                if (str1[i] == '1' && str2[i] == '1') str[i] = '1';
                else str[i] = '0';
            }
            return new string(str);
        }
        public string supply_4(string str)
        {
            while (str.Length < 4)
            {
                str = "0" + str;
            }
            return str;
        }
        public string supply_8(string str)
        {
            while (str.Length < 8)
            {
                str = "0" + str;
            }
            return str;
        }

        public void instruct_processing(string instruct, string r1, string r2)//两个参数的运算符指令
        {
            if (instruct == "ADD")
            {


                if (register(r2) == 8)//第二参数不是寄存器
                {
                    crlist[register(r1)].value += Convert.ToInt32(r2, 16);//将第二个参数加到第一个参数中
                    fresh("DR", supply_4(Convert.ToString(register(r1), 2)) + "0000");
                }
                else//第二个参数是寄存器
                {

                    crlist[register(r1)].value += crlist[register(r2)].value;
                    fresh("DR", supply_4(Convert.ToString(register(r1), 2)) + supply_4(Convert.ToString(register(r2), 2)));
                }
                time++;//时间周期加1
                string temp = Convert.ToString(crlist[register(r1)].value, 2);//将10进制值转化为二进制字符串
                while (temp.Length < 8)//补零
                {
                    temp = "0" + temp;
                }
                fresh(crlist[register(r1)].name, temp);
                fresh("TM", time.ToString());
                display(r1);
                //fresh("SR", "00111111");
                fresh("IR", "0000");



            }
            else if (instruct == "SUB")
            {

                if (register(r2) == 8)//第二参数不是寄存器
                {
                    crlist[register(r1)].value -= Convert.ToInt32(r2, 16);//第一个参数减第二个参数
                    fresh("DR", supply_4(Convert.ToString(register(r1), 2)) + "0000");
                }
                else//第二个参数是寄存器
                {
                    crlist[register(r2)].name = "R" + register(r2).ToString();//得到寄存器的名字
                    crlist[register(r2)].address = register(r2);//得到寄存器的地址
                    crlist[register(r1)].value -= crlist[register(r2)].value;
                    fresh("DR", supply_4(Convert.ToString(register(r1), 2)) + supply_4(Convert.ToString(register(r2), 2)));
                }
                time++;//时间周期加1
                string temp = Convert.ToString(crlist[register(r1)].value, 2);//将10进制值转化为二进制字符串
                while (temp.Length < 8)//补零
                {
                    temp = "0" + temp;
                }
                fresh(crlist[register(r1)].name, temp);
                fresh("TM", time.ToString());
                //fresh("SR", "00111111");
                display(r1);
                fresh("IR", "0001");
            }
            else if (instruct == "AND")//相与
            {


                if (register(r2) == 8)//第二参数不是寄存器
                {
                    string str1 = Convert.ToString(crlist[register(r1)].value, 2);//第一个寄存器的值转化为二进制
                    string str2 = Convert.ToString(Convert.ToInt32(r2, 16), 2);//第二个参数转化为二进制
                    string str = And(supply_8(str1),supply_8(str2));
                    crlist[register(r1)].value = Convert.ToInt32(str, 2);//相与后赋值         
                    while (str.Length < 8)//补零
                    {
                        str = "0" + str;
                    }
                    fresh(crlist[register(r1)].name, str);
                    fresh("DR", supply_4(Convert.ToString(register(r1), 2)) + "0000");
                }
                else//第二个参数是寄存器
                {
                    crlist[register(r2)].name = "R" + register(r2).ToString();//得到寄存器的名字
                    crlist[register(r2)].address = register(r2);//得到寄存器的地址
                    string str1 = supply_8(Convert.ToString(crlist[register(r1)].value, 2));//第一个寄存器的值转化为二进制
                    string str2 = supply_8(Convert.ToString(crlist[register(r2)].value, 2));//第二个参数转化为二进制
                    string str = And(str1, str2);
                    crlist[register(r1)].value = Convert.ToInt32(str, 2);//相与后赋值         
                    while (str.Length < 8)//补零
                    {
                        str = "0" + str;
                    }
                    fresh(crlist[register(r1)].name, str);
                    fresh("DR", supply_4(Convert.ToString(register(r1), 2)) + supply_4(Convert.ToString(register(r2), 2)));

                }
                time = time + 3;//时间周期加1

                fresh("TM", time.ToString());
                //fresh("SR", "00000011");
                //display(r1);
                if (crlist[register(r1)].value > 255) C.Text = "1";//C位
                else C.Text = "0";
                if (crlist[register(r1)].value == 0) Z.Text = "1";//Z位
                else Z.Text = "0";

                S_R.Text = "000000"+Z.Text+C.Text;
                fresh("IR", "0010");
            }
            else if (instruct == "MOVE")//赋值
            {


                if (register(r2) == 8)//第二参数不是寄存器
                {
                    crlist[register(r1)].value = Convert.ToInt32(r2, 16);//第一个参数等于第二个参数
                    fresh("DR", supply_4(Convert.ToString(register(r1), 2)) + "0000");
                }
                else//第二个参数是寄存器
                {

                    crlist[register(r1)].value = crlist[register(r2)].value;
                    fresh("DR", supply_4(Convert.ToString(register(r1), 2)) + supply_4(Convert.ToString(register(r2), 2)));
                }
                time++;//时间周期加1
                string temp = Convert.ToString(crlist[register(r1)].value, 2);//将10进制值转化为二进制字符串
                while (temp.Length < 8)//补零
                {
                    temp = "0" + temp;
                }
                fresh(crlist[register(r1)].name, temp);
                fresh("TM", time.ToString());
                fresh("IR", "0011");
            }
            else if (instruct == "LDI")//载入立即数
            {


                crlist[register(r1)].value = Convert.ToInt32(r2, 16);//第一个参数等于第二个参数
                time++;//时间周期加1
                string temp = Convert.ToString(crlist[register(r1)].value, 2);//将10进制值转化为二进制字符串
                while (temp.Length < 8)//补零
                {
                    temp = "0" + temp;
                }
                fresh(crlist[register(r1)].name, temp);
                fresh("IR", "0100");
                fresh("TM",time.ToString()) ;
                supply_4(Convert.ToString(register(r1), 2));
            }
            else if (instruct == "LD")//装载指令
            {


                crlist[register(r1)].value = crlist[Convert.ToInt32(r2, 16)].value;//通过地址得到寄存器的值进行赋值
                time++;//时间周期加1
                string temp = Convert.ToString(crlist[register(r1)].value, 2);//将10进制值转化为二进制字符串
                while (temp.Length < 8)//补零
                {
                    temp = "0" + temp;
                }
                fresh(crlist[register(r1)].name, temp);
                fresh("TM", time.ToString());
                fresh("IR", "0101");
                supply_4(Convert.ToString(register(r1), 2));
            }
        }
        public void instruct_processing(string instruct, string r1)//一个参数的运算符指令
        {
            if (instruct == "INC")//单操作数加一
            {


                crlist[register(r1)].value += 1;//第一个参数等于第二个参数
                time++;//时间周期加1
                string temp = Convert.ToString(crlist[register(r1)].value, 2);//将10进制值转化为二进制字符串
                while (temp.Length < 8)//补零
                {
                    temp = "0" + temp;
                }
                fresh(crlist[register(r1)].name, temp);
                fresh("TM", time.ToString());
                //fresh("SR", "00111111");
                display(r1);
                fresh("IR", "0110");
                supply_4(Convert.ToString(register(r1), 2));

            }
            else if (instruct == "DEC")//单操作数减一
            {


                crlist[register(r1)].value -= 1;//第一个参数等于第二个参数
                time++;//时间周期加1
                string temp = Convert.ToString(crlist[register(r1)].value, 2);//将10进制值转化为二进制字符串
                while (temp.Length < 8)//补零
                {
                    temp = "0" + temp;
                }
                fresh(crlist[register(r1)].name, temp);
                fresh("TM", time.ToString());
                //fresh("SR", "00111111");
                display(r1);
                fresh("IR", "0111");
                supply_4(Convert.ToString(register(r1), 2));
            }
            else if (instruct == "NEC")//求补码
            {


                time++;//时间周期加1
                string temp = Convert.ToString(crlist[register(r1)].value, 2);//将10进制值转化为二进制字符串
                temp = calculate.CalculateComplement(temp);//求补码
                while (temp.Length < 8)//补零
                {
                    temp = "0" + temp;
                }
                fresh(crlist[register(r1)].name, temp);
                fresh("TM", time.ToString());
                //crlist[register(r1)].value = Convert.ToInt32(temp,10) ;
                //fresh("SR", "00111111");
                display(r1);
                fresh("IR", "1000");
                supply_4(Convert.ToString(register(r1), 2));
            }
            else if (instruct == "JMP")//无条件跳转指令
            {


                pc.value = crlist[register(r1)].value;//PC的值等于寄存器的值
                time++;//时间周期加1
                string temp = Convert.ToString(pc.value, 2);//将10进制值转化为二进制字符串
                while (temp.Length < 8)//补零
                {
                    temp = "0" + temp;
                }
                fresh("PC", temp);
                fresh("TM", time.ToString());
                fresh("IR", "1001");
                supply_4(Convert.ToString(register(r1), 2));
            }
            else if (instruct == "JC")//有条件跳转指令
            {


                if (S_R.Text[2] == '1') //SR寄存器H位为1
                {
                    pc.value = crlist[register(r1)].value;//PC的值等于寄存器的值
                    time++;//时间周期加1
                    string temp = Convert.ToString(pc.value, 2);//将10进制值转化为二进制字符串
                    while (temp.Length < 8)//补零
                    {
                        temp = "0" + temp;
                    }
                    fresh("PC", temp);
                    fresh("TM", time.ToString());
                    fresh("IR", "1010");
                    supply_4(Convert.ToString(register(r1), 2));
                }

            }

        }
        public void instruct_processing(string instruct)//无参数的运算符指令
        {
            if (instruct == "NOP")
            {
                time++;
                fresh("TM", time.ToString());
                fresh("IR", "1011");
                fresh("DR", "00000000");
            }
        }

        private void Button3_Click(object sender, EventArgs e)//全部指令
        {
            for (int i = 0; i < sArray.Length; i++)
            {
                instrcting.Text = sArray[i];
                Button2_Click(sender, e);
            }
        }

        private void Button4_Click(object sender, EventArgs e)//单步执行
        {
            if (num < sArray.Length)
            {
                instrcting.Text = sArray[num];
                Button2_Click(sender, e);
                num++;
            }      
            else { MessageBox.Show("已经执行完所有指令"); }
        }

        private void Button5_Click(object sender, EventArgs e)//保存结果
        {
            text = text + "R0:" + R0.Text + "\r\n";
            text = text + "R1:" + R1.Text + "\r\n";
            text = text + "R2:" + R2.Text + "\r\n";
            text = text + "R3:" + R3.Text + "\r\n";
            text = text + "R4:" + R4.Text + "\r\n";
            text = text + "R5:" + R5.Text + "\r\n";
            text = text + "R6:" + R6.Text + "\r\n";
            text = text + "R7:" + R7.Text + "\r\n";
            text = text + "PC:" + P_C.Text + "\r\n";
            text = text + "SR:" + S_R.Text + "\r\n";
            text = text + "IR:" + I_R.Text + "\r\n";
            text = text + "DR:" + D_R.Text + "\r\n";
            text = text + "TIME:" + TM.Text + "\r\n";
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "(*.txt)|*.txt|(*.*)|*.*";
            saveFileDialog.FileName = "Result" + ".txt";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                StreamWriter streamWriter = new StreamWriter(saveFileDialog.FileName, false);
                streamWriter.Write(text);
                streamWriter.Close();
            }
        }
    }
}
