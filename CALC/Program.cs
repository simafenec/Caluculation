﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CALC
{
    class KEISANNKI
    {
        //計算機プログラム　超簡易型
        public void Super_Easy_calc()
        {

            Console.WriteLine("x 演算子 yを一行ずつ記入");
            Console.WriteLine("使用可能演算子: + - / *");
            int x = 0, y = 0;
            char c = ' ';
            try
            {
                x = int.Parse(Console.ReadLine());
                c = char.Parse(Console.ReadLine());
                y = int.Parse(Console.ReadLine());

            }
            catch (System.FormatException)
            {
                Console.WriteLine("不正な入力です");

            }

            if (c == '+')
            {
                Console.WriteLine(x + y);
            }
            else if (c == '-')
            {
                Console.WriteLine(x - y);
            }
            else if (c == '*')
            {
                Console.WriteLine(x * y);
            }
            else if (c == '/')
            {
                Console.WriteLine(x / y);
            }
            else
            {
                Console.WriteLine("不適当な演算子の入力");
            }
        }
        public int search_op(string str, char c, int s, int e)
        {
            //入力
            //str : 式 c: 探したい演算子
            //s,e: 探す範囲(str[s] からstr[e]までを探す)
            //出力 あればその位置を、なければ-1を返す
            string part_str = str.Substring(s, e);
            return part_str.IndexOf(c);
        }
        //一行で入力可能なプログラム  ()演算子も扱えるようにする
        public void Calc()
        {
            string formula;
            formula = Console.ReadLine();
            Console.WriteLine(formula);
        }
    }
}