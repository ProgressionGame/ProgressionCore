using System;

namespace Progression.TerminalRenderer.Ansi
{
    public class NumberParser : ISequenceTraverserPart
    {
        private ISequenceTraverserPart _next;
        private int _id;
        private char _end;

        public NumberParser(int id, char end, ISequenceTraverserPart next)
        {
            _next = next;
            _end = end;
            _id = id;
        }

        public AnsiInputEvent Traverse(AnsiInputConverter con, char c, bool b, bool qo, params object[] data)
        {
            int ToInt(char ch)
            {
                switch (ch) {
                    case '1':
                        return 1;
                    case '2':
                        return 2;
                    case '3':
                        return 3;
                    case '4':
                        return 4;
                    case '5':
                        return 5;
                    case '6':
                        return 6;
                    case '7':
                        return 7;
                    case '8':
                        return 8;
                    case '9':
                        return 9;
                    case '0':
                        return 0;
                    default:
                        throw new ArgumentException();
                }
            }

            switch (c) {
                case '-':
                    if (data[_id] != null) return null;
                    data[_id] = true;
                    break;
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                case '0':
                    int digit = ToInt(c);
                    int number;
                    if (data[_id] == null) {
                        number = digit;
                    } else if (data[_id] is bool) {
                        number = -digit;
                    } else {
                        number = (int) data[_id] *10;
                        number += number >= 0 ? digit : -digit;
                    }
                    data[_id] = number;
                    break;
                default:
                    if (c != _end || !(data[_id] is int)) {
                        return null;
                    }
                    return con.NextPart(_next, b, qo, data);
            }
            return con.NextPart(this, b, qo, data);
        }

    }
}