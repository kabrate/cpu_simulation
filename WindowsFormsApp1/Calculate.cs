using System;
using System.Linq;
using System.Text;

namespace WindowsFormsApp1
{
    class calculate
    {
        static public string CalculateComplement(string dataF)//补码
        {
            const char POSITIVE_SIGN = '0';
            const char FIRST_CHAR_B = '0';
            if (dataF[0] == POSITIVE_SIGN)
            {
                return dataF;
            }

            StringBuilder result = new StringBuilder();

            bool carry = dataF.Last() == '1';
            result.Append(carry ? FIRST_CHAR_B : '1');

            for (int i = dataF.Length - 2; i >= 0; i--)
            {
                if (carry)
                {
                    carry = dataF[i] == '1';
                    result.Insert(0, carry ? FIRST_CHAR_B : '1');

                    continue;
                }

                result.Insert(0, dataF[i]);
            }

            return result.ToString();
        }
        static public string Add(string A, string W)//二进制加法
        {
            int m = 0; string str;
            int[] a = new int[A.Length];
            int[] b = new int[W.Length];
            foreach (char ch in A)
            {

                a[m++] = int.Parse(ch.ToString());
            }
            m = 0;
            foreach (char ch in W)
            {
                b[m++] = int.Parse(ch.ToString());
            }



            int reLength = Math.Max(a.Length, b.Length);
            int[] re = new int[reLength + 1];//返回计算结果的数组
            int carry = 0;//进位数
            int temp;
            int i, j, k;
            for (i = a.Length - 1, j = b.Length - 1, k = reLength; i >= 0 && j >= 0; i--, j--, k--)
            {
                temp = a[i] + b[j] + carry;
                if (temp >= 2)
                {
                    carry = 1;
                    re[k] = temp - 2;
                }
                else
                {
                    re[k] = temp;
                    carry = 0;
                }
            }
            /*
             * 分为三种情况
             * 1）两个数位数相同，都被加完，re的最高位加carry就可以；
             * 2）a被加完，b还没有加完；
             * 3）与2）情况相反。
             */
            if (i >= 0)
            {
                while (i >= 0)
                {
                    temp = a[i] + carry;
                    re[k] = (temp == 2) ? 0 : temp;
                    carry = (temp == 2) ? 1 : 0;
                    i--;
                    k--;
                }
            }
            else if (j >= 0)
            {
                while (j >= 0)
                {
                    temp = b[j] + carry;
                    re[k] = (temp == 2) ? 0 : temp;
                    carry = (temp == 2) ? 1 : 0;
                    j--;
                    k--;
                }
            }
            re[0] = re[0] + carry;
            str = String.Join("", re);

            return str;
        }
        static public string AND(string A, string W)
        {
            int t;
            string reslut = "";
            int len = A.Length;
            for (t = 0; t < 8 - len; t++)//转化位8位二进制
            {
                A = "0" + A;
            }
            len = W.Length;
            for (t = 0; t < 8 - len; t++)//转化位8位二进制
            {
                W = "0" + W;
            }

            for (int i = 0; i < 8; i++)
            {
                if (A[i] == '0' || W[i] == '0') reslut = reslut + "0";
                else reslut = reslut + "1";
            }


            return reslut;
        }//二进制与运算
        static public string OR(string A, string W)
        {
            int t;
            string reslut = "";
            int len = A.Length;
            for (t = 0; t < 8 - len; t++)//转化位8位二进制
            {
                A = "0" + A;
            }
            len = W.Length;
            for (t = 0; t < 8 - len; t++)//转化位8位二进制
            {
                W = "0" + W;
            }

            for (int i = 0; i < 8; i++)
            {
                if (A[i] == '1' || W[i] == '1') reslut = reslut + "1";
                else reslut = reslut + "0";
            }


            return reslut;
        }//二进制或运算
        static public string NOT(string A)//二进制取反运算
        {
            int t;
            string reslut = "";
            int len = A.Length;
            for (t = 0; t < 8 - len; t++)//转化位8位二进制
            {
                A = "0" + A;
            }

            for (int i = 0; i < 8; i++)
            {
                if (A[i] == '1') reslut += '0';
                else reslut = reslut + "1";
            }
            return reslut;
        }
        private string CalculateTrueForm(int originalValue)//原码
        {
            const char FIRST_CHAR_B = '0';
            const char SECONT_CHAR_B = '1';
            StringBuilder buffer = new StringBuilder();

            int quotient = 0;
            int remainder = 0;

            int tmp = Math.Abs(originalValue);

            do
            {
                quotient = tmp / 2;
                remainder = tmp % 2;

                buffer.Insert(0, Convert.ToString(remainder));

                tmp = quotient;
            } while (tmp != 0);

            string result = buffer.ToString().TrimStart(FIRST_CHAR_B).PadLeft(7, FIRST_CHAR_B);

            return (Convert.ToString(originalValue < 0 ? SECONT_CHAR_B : FIRST_CHAR_B)) + result;
        }
        public string CalculateRadixMinusOneComplement(string dataY)//反码
        {
            const char POSITIVE_SIGN = '0';
            const char FIRST_CHAR_B = '0';
            const char SECONT_CHAR_B = '1';
            if (dataY[0] == POSITIVE_SIGN)
            {
                return dataY;
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(dataY[0]);

            for (int i = 1; i < dataY.Length; i++)
            {
                sb.Append(dataY[i] == FIRST_CHAR_B ? SECONT_CHAR_B : FIRST_CHAR_B);
            }

            return sb.ToString();
        }

    }
}

