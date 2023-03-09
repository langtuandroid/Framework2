using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Framework
{
    public static class OperationParser
    {

        public static int CalculateResult(string str, Func<string, int> getNumFromStr)
        {
            var rnp = GetRpn(str);
            return GetVal(rnp, (_, section) => getNumFromStr(section.Num));
        }
        
        public static int GetVal(List<OperationSection> sections,Func<int,OperationSection,int> convertNumFunc)
        {
            //提前判断优化
            if (sections.Count == 1)
            {
                //一个必然是数字
                Debug.Assert(sections[0].Type == OpSymbolType.NumberStr);
                //纯数字直接parse
                return int.TryParse(sections[0].Num, out var num) ? num : convertNumFunc(0, sections[0]);
            }
            
            
            var valStack = new Stack<int>();
            for (var i = 0; i < sections.Count; i++)
            {
                var section = sections[i];
                if (section.Type == OpSymbolType.NumberStr)
                {
                    //纯数字直接parse
                    if (int.TryParse(section.Num, out var num))
                    {
                        valStack.Push(num);
                    }
                    //否则按照外部给的来
                    else
                    {
                        valStack.Push(convertNumFunc(i,section));
                    }
                }
                else
                {
                    var right = valStack.Pop();
                    var left = valStack.Pop();
                    var val = 0;
                    switch (section.Op)
                    {
                        case Operator.None:
                            break;
                        case Operator.Add:
                            val = left + right;
                            break;
                        case Operator.Subtract:
                            val = left - right;
                            break;
                        case Operator.Multiply:
                            val = left * right;
                            break;
                        case Operator.Divide:
                            val = left / right;
                            break;
                        case Operator.Pow:
                            val = (int)Math.Pow(left, right);
                            break;
                        case Operator.Mod:
                            val = left % right;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    valStack.Push(val);
                }
            }

            return valStack.Pop();
        }
        
        
        //获取后缀表达式,Reverse Polish notation
        public static List<OperationSection> GetRpn(string str)
        {
            str = str.RemoveWhitespace();
            var sections = new List<OperationSection>();
            if (string.IsNullOrEmpty(str))
            {
                AddNum(0, sections);
                return sections;
            }

            var opStack = new Stack<Operator>();
            var numLength = 0;
            for (int i = 0; i < str.Length; i++)
            {
                if (IsOperatorChar(str[i], out var op))
                {
                    //检查括号
                    //左括号
                    if (op == Operator.OpenParenthesis)
                    {
                        opStack.Push(op);
                        continue;
                    }

                    //第一个字符是符号位(+,-)
                    if (i == 0  )
                    {
                        if (op == Operator.Add || op == Operator.Subtract)
                        {
                            //添加一个0;
                            AddNum(0,sections);
                        }
                        else
                        {
                            throw new Exception("第一符号非法:"+str);
                        }
                    }
                    
                    //之前的字符全都拼到一起作为数字字符串
                    if (numLength > 0)
                    {
                        var numStr = str.Substring(i - numLength, numLength);
                        numLength = 0;
                        AddNum(numStr, sections);
                    }

                    //右括号
                    if (op == Operator.CloseParenthesis)
                    {
                        var topOp = opStack.Pop();
                        while (topOp!=Operator.OpenParenthesis)
                        {
                            AddOperator(topOp,sections);
                            topOp = opStack.Pop();
                        }
                        continue;
                    }
                    
                    //开始检查符号优先级
                    while (opStack.Count != 0)
                    {
                        var topOp = opStack.Peek();
                        if (topOp == Operator.OpenParenthesis) break;
                        if (IsPriorityGreater(op, topOp)) break;
                        AddOperator(opStack.Pop(), sections);
                    }
                    opStack.Push(op);
                }
                else
                {
                    numLength++;
                }
            }
            //处理最后的数字
            if (numLength>0)
            {
                var num = str.Substring(str.Length - numLength, numLength);
                AddNum(num,sections);
            }
            //处理剩下的运算符
            while (opStack.Count!=0)
            {
                AddOperator(opStack.Pop(), sections);
            }
            return sections;
        }

        //判断第一个运算符优先级是否大于第二个
        private static bool IsPriorityGreater(Operator op1, Operator op2)
        {
            return GetOperatorLevel(op1) > GetOperatorLevel(op2);
        }

        private static int GetOperatorLevel(Operator op)
        {
            switch (op)
            {
                case Operator.None:
                    throw new ArgumentException();
                case Operator.Add:
                    return 1;
                case Operator.Subtract:
                    return 1;
                case Operator.Multiply:
                    return 2;
                case Operator.Divide:
                    return 2;
                case Operator.Mod:
                    return 2;
                case Operator.Pow:
                    return 3;
                default:
                    throw new ArgumentOutOfRangeException(nameof(op), op, null);
            }
        }

        private static bool IsOperatorChar(char c, out Operator op)
        {
            op = Operator.None;
            switch (c)
            {
                case '+':
                    op = Operator.Add;
                    break;
                case '-':
                    op = Operator.Subtract;
                    break;
                case '*':
                    op = Operator.Multiply;
                    break;
                case '/':
                    op = Operator.Divide;
                    break;
                case '^':
                    op = Operator.Pow;
                    break;
                case '%':
                    op = Operator.Mod;
                    break;
                case '(':
                    op = Operator.OpenParenthesis;
                    break;
                case ')':
                    op = Operator.CloseParenthesis;
                    break;
            }

            return op != Operator.None;
        }

        private static void AddNum(string str, List<OperationSection> sections)
        {
            sections.Add(new OperationSection
            {
                Type = OpSymbolType.NumberStr,
                Num = str
            });
        }

        private static void AddNum(int val, List<OperationSection> sections)
        {
            AddNum(val.ToString(), sections);
        }
        

        private static void AddOperator(Operator op, List<OperationSection> sections)
        {
            sections.Add(new OperationSection
            {
                Type = OpSymbolType.Operator,
                Op = op
            });
        }
        
        private static string RemoveWhitespace(this string str) {
            return string.Join("", str.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
        }

        public static string PrintRpn(List<OperationSection> sections)
        {
            var sb = new StringBuilder();
            foreach (var section in sections)
            {
                switch (section.Type)
                {
                    case OpSymbolType.NumberStr:
                        sb.Append(section.Num);
                        break;
                    case OpSymbolType.Operator:
                        switch (section.Op)
                        {
                            case Operator.None:
                                break;
                            case Operator.Add:
                                sb.Append("+");
                                break;
                            case Operator.Subtract:
                                sb.Append("-");
                                break;
                            case Operator.Multiply:
                                sb.Append("*");
                                break;
                            case Operator.Divide:
                                sb.Append("/");
                                break;
                            case Operator.Pow:
                                sb.Append("^");
                                break;
                            case Operator.Mod:
                                sb.Append("%");
                                break;
                            //Rpn中不存在
                            case Operator.OpenParenthesis:
                                break;
                            //Rpn中不存在
                            case Operator.CloseParenthesis:
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                sb.Append('\t');
            }

            return sb.ToString();
        }


        public class OperationSection
        {
            public OpSymbolType Type;
            public Operator Op;
            public string Num;
        }

        public enum OpSymbolType
        {
            NumberStr,
            Operator
        }

        public enum Operator
        {
            None,
            Add,
            Subtract,
            Multiply,
            Divide,
            Pow,
            Mod,
            //左括号,解析使用,计算不使用
            OpenParenthesis,
            //右括号,解析使用,计算不使用
            CloseParenthesis,
        }
    }
}