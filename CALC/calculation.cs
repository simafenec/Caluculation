using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CALC
{
    class KEISANNKI
    {
        public string Super_Easy_calc(int x,int y, char c)
        {
            //計算機プログラム　超簡易型
            if (c == '+')
            {
                return (x + y).ToString();
            }
            else if (c == '-')
            {
                return (x - y).ToString();
            }
            else if (c == '*')
            {
                return (x * y).ToString();
            }
            else
            {
                return (x / y).ToString();
            }
          
        }
        public string Splitstring(string str, int s, int e) {
            //str[s]からstr[e]までを切り出す。
            return str[s..(e + 1)];
        }
        public int Search_op(string str, char c, int s, int e)
        {
            //入力
            //str : 式 c: 探したい演算子
            //s,e: 探す範囲(str[s] からstr[e]までを探す)
            //出力 あればその位置を、なければ-1を返す
            string part_str = Splitstring(str,s,e);
            return part_str.IndexOf(c);
        }
        //一行で入力可能なプログラム  ()演算子も扱えるようにする
        //ただしこのプログラムだと答えが整数になる計算しかできない　要修正
        public string Calc(string formula)
        {
            char[] op = new char[] { '(', ')', '*', '/', '+', '-' };//計算記号をまとめておく
            string return_value=" ",ans; //最終的な結果をここに入れる
            int start = 0;
            int end = formula.Length - 1;
            // ()から探す
            int exist_op_left_brak = Search_op(formula, op[0], start, end);//左括弧を探す
            int exist_op_right_brak = Search_op(formula, op[1], start, end);//右括弧を探す
            if (exist_op_left_brak != -1 && exist_op_right_brak != -1)//もしどっちもあれば括弧内のみを抜き出してCalcに渡す
            {
                if (exist_op_left_brak == start && exist_op_right_brak == end)//この場合は括弧の意味がないので中身を出してCalcに渡す
                {
                    return Calc(formula[(start+1)..(end-1)]);
                }
                else
                {
                    ans = Calc(Splitstring(formula,exist_op_left_brak + 1, exist_op_right_brak - 1));
                    if (exist_op_left_brak == 0) { return_value = ans +Splitstring(formula,exist_op_right_brak + 1, end); }
                    else if (exist_op_right_brak == end) { return_value = Splitstring(formula,start, exist_op_left_brak - 1) + ans; }
                    else { return_value = Splitstring(formula,start, exist_op_left_brak - 1) + ans + Splitstring(formula,exist_op_right_brak + 1, end); }
                    return Calc(return_value);
                }
            }
            else
            {
                int exist_op_times = Search_op(formula, op[2], start, end); //掛け算記号を探す
                if (exist_op_times != -1) { //掛け算記号が式の中にあったときの処理
                    //前に記号があればその場所を指す　前側には同記号はSearch_op関数の性質上来ない
                    int exist_op_front = Math.Max(Math.Max(Search_op(formula,op[3],start,exist_op_times-1),Search_op(formula, op[4], start, exist_op_times - 1)),Search_op(formula,op[5],start,exist_op_times-1));
                    //後ろに記号があればその場所を探す　後ろには同じ符号も来るので注意！
                    int exist_op_times_back = Search_op(formula, op[2], exist_op_times + 1, end);
                    int exist_op_plus_back = Search_op(formula, op[4], exist_op_times+1,end);
                    int exist_op_minus_back = Search_op(formula, op[5], exist_op_times + 1, end);
                    int exist_op_div_back = Search_op(formula,op[3],exist_op_times+1,end);
                    //後々の処理のために配列に入れる　大分汚いやり方　要改善
                    int[] exist_op_back = new int[] { exist_op_plus_back, exist_op_minus_back, exist_op_div_back ,exist_op_times_back};

                    if (exist_op_plus_back ==-1&& exist_op_minus_back==-1&&exist_op_div_back==-1&&exist_op_times_back==-1) {
                        //掛け算記号の後ろに計算記号がないとき
                        if (exist_op_front == -1)
                        {
                            //前にも記号がないとき　
                            int x = int.Parse(Splitstring(formula, start, exist_op_times - 1));
                            int y = int.Parse(Splitstring(formula, exist_op_times + 1, end));
                            char c = formula[exist_op_times];
                            return_value = Super_Easy_calc(x, y, c);//前後に記号がなければ計算はそれ以上発生しないので返り値に入れてしまう
                            return return_value;
                        }
                        else {
                            //前には記号があるとき
                            int x = int.Parse(Splitstring(formula, exist_op_front + 1, exist_op_times - 1));
                            int y = int.Parse(Splitstring(formula, exist_op_times + 1, end));
                            char c = formula[exist_op_times];
                            ans = Super_Easy_calc(x, y, c);
                            return_value = Splitstring(formula, start, exist_op_front) + ans;
                            return Calc(return_value);
                        }
                    }
                    else{
                        //掛け算記号の後ろに記号があるとき
                        var backlist = exist_op_back.ToList();
                        backlist.RemoveAt(-1);
                        exist_op_back = backlist.ToArray();
                        int min_op_pos = exist_op_back.Min(); //後ろにある記号のうち最も掛け算記号に近い場所を探した　
                        if (exist_op_front == -1)
                        {
                            //前には記号がないとき　
                            int x = int.Parse(Splitstring(formula, start, exist_op_times - 1));
                            int y = int.Parse(Splitstring(formula, exist_op_times + 1, min_op_pos-1));
                            char c = formula[exist_op_times];
                            ans = Super_Easy_calc(x, y, c);
                            return_value = ans + Splitstring(formula, min_op_pos, end);
                            return Calc(return_value);
                        }
                        else
                        {
                            //前にも記号があるとき
                            int x = int.Parse(Splitstring(formula, exist_op_front + 1, exist_op_times - 1));
                            int y = int.Parse(Splitstring(formula, exist_op_times + 1, min_op_pos - 1));
                            char c = formula[exist_op_times];
                            ans = Super_Easy_calc(x, y, c);
                            return_value = Splitstring(formula, start, exist_op_front) + ans+Splitstring(formula,min_op_pos,end);
                            return Calc(return_value);
                        }
                    }

                }
                int exist_op_div = Search_op(formula, op[3], start, end); //割り算記号を探す
                //割り算記号の処理では先に処理した掛け算記号は考慮しなくていい　割り算処理に入ったということは式にもう掛け算がないということ
                if (exist_op_div != -1) {
                    //割り算記号があった時の処理 掛け算よりも最適化が少しできている
                    int exist_op_front = Math.Max(Search_op(formula,op[4],start,exist_op_div-1),Search_op(formula,op[5],start,exist_op_div+1));
                    int exist_plus_back = Search_op(formula, op[4], exist_op_div + 1, end);
                    int exist_minus_back = Search_op(formula, op[5], exist_op_div + 1, end);
                    int exist_div_back = Search_op(formula, op[3], exist_op_div + 1, end);
                    int[] exist_back = new int[] { exist_plus_back, exist_minus_back ,exist_div_back};
                    if (exist_op_front != -1)
                    {
                        //前に記号があった時
                        int x = int.Parse(Splitstring(formula, exist_op_front + 1, exist_op_div - 1));
                        char c = formula[exist_op_div];
                        if (exist_plus_back == -1 && exist_minus_back == -1&&exist_div_back==-1)
                        {
                            //後ろに記号がなかった時

                            int y = int.Parse(Splitstring(formula, exist_op_div + 1, end));

                            ans = Super_Easy_calc(x, y, c);
                            return_value = Splitstring(formula, start, exist_op_div - 1) + ans;
                            return Calc(return_value);
                        }
                        else
                        {
                            //後ろに記号があった時
                            var backlist = exist_back.ToList();
                            backlist.RemoveAt(-1);
                            exist_back = backlist.ToArray();
                            int min_op_pos = exist_back.Min();
                            int y = int.Parse(Splitstring(formula, exist_op_div + 1, min_op_pos - 1));
                            ans = Super_Easy_calc(x, y, c);
                            return_value = Splitstring(formula, start, exist_op_div - 1) + ans + Splitstring(formula, min_op_pos, end);
                            return Calc(return_value);
                        }
                    }
                    else {
                        //前に記号がなかった時
                        int x = int.Parse(Splitstring(formula,start,exist_op_div-1));
                        char c = formula[exist_op_div];
                        if (exist_plus_back == -1 && exist_minus_back == -1&&exist_div_back==-1)
                        {
                            //後ろに記号がなかった時 最後の計算なので値を返す

                            int y = int.Parse(Splitstring(formula, exist_op_div + 1, end));

                            ans = Super_Easy_calc(x, y, c);
                            return_value = Splitstring(formula, start, exist_op_div - 1) + ans;
                            return return_value;
                        }
                        else
                        {
                            //後ろに記号があった時
                            var backlist = exist_back.ToList();
                            backlist.RemoveAt(-1);
                            exist_back = backlist.ToArray();
                            int min_op_pos = exist_back.Min();
                            int y = int.Parse(Splitstring(formula, exist_op_div + 1, min_op_pos - 1));
                            ans = Super_Easy_calc(x, y, c);
                            return_value = ans + Splitstring(formula, min_op_pos, end);
                            return Calc(return_value);
                        }
                    }
                }
                int exist_op_plus = Search_op(formula, op[4], start, end);//足し算記号を探す
                if (exist_op_plus != -1) {
                    //足し算記号があった時 前に存在しうるのは引き算のみ
                    int exist_op_front = Search_op(formula,op[5],start,exist_op_plus-1);
                    int exist_plus_back = Search_op(formula, op[4], exist_op_plus + 1, end);
                    int exist_minus_back = Search_op(formula, op[5], exist_op_plus + 1, end);
                    int[] exist_back = new int[] { exist_plus_back, exist_minus_back };
                    if (exist_op_front == -1)
                    {
                        //前に記号がなかった時
                        int x = int.Parse(Splitstring(formula, start, exist_op_plus - 1));
                        char c = formula[exist_op_plus];
                        if (exist_plus_back == -1 && exist_minus_back == -1)
                        {
                            //後ろにも記号がなかった時
                            int y = int.Parse(Splitstring(formula, exist_op_plus + 1, end));
                            return_value = Super_Easy_calc(x, y, c);
                            return return_value;
                        }
                        else
                        {
                            //後ろに記号があった時
                            var backlist = exist_back.ToList();
                            backlist.RemoveAt(-1);
                            exist_back = backlist.ToArray();
                            int min_op_pos = exist_back.Min();
                            int y = int.Parse(Splitstring(formula, exist_op_plus + 1, min_op_pos - 1));
                            ans = Super_Easy_calc(x, y, c);
                            return_value = ans + Splitstring(formula, min_op_pos, end);
                            return Calc(return_value);
                        }
                    }
                    else {
                        //前に記号があった時
                        int x = int.Parse(Splitstring(formula, exist_op_front + 1, exist_op_plus - 1));
                        char c = formula[exist_op_plus];
                        if (exist_plus_back == -1 && exist_minus_back == -1)
                        {
                            //後ろに記号がなかった時

                            int y = int.Parse(Splitstring(formula, exist_op_plus + 1, end));

                            ans = Super_Easy_calc(x, y, c);
                            return_value = Splitstring(formula, start, exist_op_plus - 1) + ans;
                            return Calc(return_value);
                        }
                        else
                        {
                            //後ろに記号があった時
                            var backlist = exist_back.ToList();
                            backlist.RemoveAt(-1);
                            exist_back = backlist.ToArray();
                            int min_op_pos = exist_back.Min();
                            int y = int.Parse(Splitstring(formula, exist_op_plus + 1, min_op_pos - 1));
                            ans = Super_Easy_calc(x, y, c);
                            return_value = Splitstring(formula, start, exist_op_plus - 1) + ans + Splitstring(formula, min_op_pos, end);
                            return Calc(return_value);
                        }
                    }
                }
                int exist_op_minus = Search_op(formula, op[5], start, end);
                if (exist_op_minus!=-1) {
                    //引き算記号があった時
                    int x = int.Parse(Splitstring(formula, start, exist_op_minus - 1)); //前に記号はもう来ない
                    char c = formula[exist_op_minus];
                    int exist_back = Search_op(formula,op[5],exist_op_minus+1,end);
                    if (exist_back!=-1) {
                        //後ろに記号がある
                        int y = int.Parse(Splitstring(formula,exist_op_minus+1,exist_back-1));
                        ans = Super_Easy_calc(x, y, c);
                    }
                    else { 
                    
                    }
                }

            }
            return_value = formula;
            return return_value;
        }
    }
}
