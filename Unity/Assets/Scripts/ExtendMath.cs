using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExtendMath
{
    public class Basic : MonoBehaviourCustom
    {
        static public float sqrt(float num)
        {
            return num * num;
        }
    }

    public class RPN : MonoBehaviourCustom
    {
        static public decimal StrToResult(string str)
        {
            decimal result;
            try
            {
                result = RPNToResult(TermsToRPN(StrToTerms(str+"+0"))); //Str>Terms>RPN>Result
            }
            catch
            {
                throw;
            }
            return result;
        }
        static public Queue<string> StrToTerms(string str)
        {
            Queue<string> result = new Queue<string>();
            string tempStr = "";
            string lastType = "none";
            string type = "none";
            foreach (char charData in str)
            {
                if (charData >= '0' && charData <= '9' || charData == '.')
                {
                    type = "number";
                }
                else if ("()+-*/".Contains(charData.ToString()))
                {
                    type = "operator";
                }
                else
                {
                    type = "exception";
                }

                //add string
                if (lastType != type && tempStr != "")
                {
                    result.Enqueue(tempStr);
                    tempStr = "";
                }
                else if (type == "operator" && tempStr != "")
                {
                    result.Enqueue(tempStr);
                    tempStr = "";
                }
                if (type != "exception")
                {
                    tempStr += charData;
                }
                lastType = type;
            }
            result.Enqueue(tempStr);
            return result;
        }

        static public Queue<string> TermsToRPN(Queue<string> terms)
        {
            Queue<string> result = new Queue<string>();
            Stack<string> opeStack = new Stack<string>(); //operatorStack
            foreach (string strData in terms)
            {
                if ("()+-/*".Contains(strData))
                {
                    string stackPeek = "";
                    TryAction(() => { stackPeek = opeStack.Peek(); });
                    if (!"(".Contains(strData))
                    {
                        if ("(".Contains(stackPeek))
                        {
                            TryAction(() => { opeStack.Pop(); });
                        }
                        else if ("+-".Contains(stackPeek))
                        {
                            if (!"*/".Contains(strData))
                            {
                                result.Enqueue(stackPeek);
                                TryAction(() => { opeStack.Pop(); });
                            }
                        }
                        else
                        {
                            result.Enqueue(stackPeek);
                            TryAction(() => { opeStack.Pop(); });
                        }
                    }
                    if ("(+-/*".Contains(strData)) opeStack.Push(strData);
                }
                else
                {
                    result.Enqueue(strData);
                }
            }
            foreach (string strData in opeStack)
            {
                if (strData != "(") result.Enqueue(strData);
            }
            return result;
        }

        static public string TermsToString(Queue<string> terms)
        {
            string result = "";
            foreach (string strData in terms)
            {
                result += strData + " ";
            }
            return result;
        }

        static public decimal RPNToResult(Queue<string> inTerms)
        {
            decimal result = 0;
            List<Fraction> terms = new List<Fraction>();
            foreach (string str in inTerms)
            {
                Fraction tempFaction = new Fraction();
                tempFaction.data = str;
                if (!"+-*/".Contains(str))
                {
                    tempFaction.numerator = decimal.Parse(str);
                }
                terms.Add(tempFaction);
            }
            int ofset = 0;
            int tempCount = terms.Count;
            for (int i = 0; i < tempCount; i++)
            {
                int oi = i + ofset;
                if ("+-*/".Contains(terms[oi].data))
                {
                    try
                    {
                        decimal target1Num = terms[oi - 2].numerator;
                        decimal target2Num = terms[oi - 1].numerator;
                        decimal target1Den = terms[oi - 2].denominator;
                        decimal target2Den = terms[oi - 1].denominator;
                        decimal tempDataNum = 0;
                        decimal tempDataDen = 0;
                        decimal lcm = 0;
                        if (target1Den >= target2Den)
                        {
                            lcm = Fraction.LCM(target1Den, target2Den);
                        }
                        else
                        {
                            lcm = Fraction.LCM(target2Den, target1Den);
                        }
                        if (terms[oi].data == "+")
                        {
                            tempDataNum = target1Num * lcm / target1Den + target2Num * lcm / target2Den;
                            tempDataDen = lcm;
                        }
                        if (terms[oi].data == "-")
                        {
                            tempDataNum = target1Num * lcm / target1Den - target2Num * lcm / target2Den;
                            tempDataDen = lcm;
                        }
                        if (terms[oi].data == "*")
                        {
                            tempDataNum = target1Num * target2Num;
                            tempDataDen = target1Den * target2Den;
                        }
                        if (terms[oi].data == "/")
                        {
                            tempDataNum = target1Num * target2Den;
                            tempDataDen = target1Den * target2Num;
                        }
                        //marge
                        for (int a = 0; a < 3; a++)
                        {
                            terms.RemoveAt(oi - 2);
                        }
                        Fraction fraction = new Fraction();
                        fraction.numerator = tempDataNum;
                        fraction.denominator = tempDataDen;
                        terms.Insert(oi - 2, fraction);
                        ofset -= 2;
                    }
                    catch
                    {
                    }
                }
            }
            result = terms[0].numerator / terms[0].denominator;
            return result;
        }
    }

    class Fraction
    {
        public decimal numerator;
        public decimal denominator = 1;
        public string data;
        static public decimal GCD(decimal a, decimal b)
        {
            if (b == 0)
                return a;
            return GCD(b, a % b);
        }

        static public decimal LCM(decimal a, decimal b)
        {
            return a * b / GCD(a, b);
        }
    }
}