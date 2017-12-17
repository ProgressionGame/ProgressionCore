using System;
using System.Collections.Generic;
using System.Threading;

namespace Progression.TerminalRenderer.Ansi
{
    public class AnsiInputConverter
    {
        private readonly Queue<char> _inputQueue = new Queue<char>();
        private readonly Queue<char> _processedQueue = new Queue<char>();
        private readonly Queue<AnsiInputEvent> _eventQueue = new Queue<AnsiInputEvent>();
        private readonly Queue<char> _currentSequence = new Queue<char>();
        private readonly SeqDic _seqDic;
        private static readonly TimeSpan WaitTime = new TimeSpan(5000);


        public AnsiInputConverter()
        {
            var root = new SeqDic();
            _seqDic = root;
            var csi = new SeqDic();
            root['\u001B'] = csi;
            root['\u0007'] = csi;
            csi['['] = new CharacterTester('8', new CharacterTester(';',
                new DataInitializer(2,
                    new NumberParser(0, ';',
                        new NumberParser(1, 't',
                            new SizeEventDataConsumer())))));


            var teststr = "\u001B[8;54;124t";
            for (int i = 0; i < teststr.Length; i++) {
                _inputQueue.Enqueue(teststr[i]);
            }
        }


        /// <summary>
        /// reads a character from input stream
        /// </summary>
        /// <param name="blocking">whether method should wait for input</param>
        /// <param name="instant">whether method should allow for very short input lag</param>
        /// <param name="c">Read character</param>
        /// <returns>amount of characters left to read or -1 if no character was read</returns>
        protected virtual int ReadNext(bool blocking, bool instant, out char c)
        {
            if (!blocking) {
                if (instant) {
                    if (!Console.KeyAvailable) {
                        c = default(char);
                        return -1;
                    }
                } else {
                    for (var i = 0; i < 10; i++) {
                        if (Console.KeyAvailable) break;
                        Thread.Sleep(WaitTime);
                    }
                    if (!Console.KeyAvailable) {
                        c = default(char);
                        return -1;
                    }
                }
            }
            var cc = Console.ReadKey(true);
            c = cc.KeyChar;

            var shift = (cc.Modifiers & ConsoleModifiers.Shift) != 0 ? "Shift" : "NoShift";
            var control = (cc.Modifiers & ConsoleModifiers.Control) != 0 ? "Control" : "NoControl";
            var alt = (cc.Modifiers & ConsoleModifiers.Alt) != 0 ? "Alt" : "NoAlt";
            Console.Write($"'{c}' {(int) c}, {cc.Key}, {shift}, {control}, {alt} \n");

            return Console.KeyAvailable ? 1 : 0;
        }

        /// <summary>
        /// reads a character from queue and calls ReadNext() eagerly
        /// </summary>
        /// <param name="blocking">whether method should wait for input</param>
        /// <param name="queueOnly">reads only from queue</param>
        /// <param name="c">Read character</param>
        /// <returns>amount of characters left to read or -1 if no character was read</returns>
        protected int ReadQueue(bool blocking, bool queueOnly, out char c)
        {
            if (queueOnly) {
                if (_inputQueue.Count == 0) {
                    c = default(char);
                    return -1;
                }
                c = _inputQueue.Dequeue();
                return _inputQueue.Count;
            }

            if (_inputQueue.Count == 0) return ReadNext(blocking, false, out c);

            c = _inputQueue.Dequeue();

            //if there are more chars ready to be queued add now
            while (ReadNext(false, true, out var read) > 0) {
                _inputQueue.Enqueue(read);
            }

            return _inputQueue.Count;
        }


        protected void InterpretConsoleKey(ConsoleKeyInfo cc) { }

        protected void Process(bool blocking, bool queueOnly)
        {
            int n;
            while ((n = ReadQueue(blocking, queueOnly, out var c0)) >= 0) {
                var t = _seqDic.GetNext(c0);
                if (t != null) {
                    var ev = NextPart(t, blocking, queueOnly, null); // t.Traverse(this, c0, blocking, queueOnly, null);
                    if (ev == null) {
                        if (_currentSequence.Count == 0) {
                            return; //already dequeued means that a reading failed
                        }
                        CancelSeq();
                    } else {
                        _eventQueue.Enqueue(ev);
                        _currentSequence.Clear();
                    }
                    continue;
                }
                _processedQueue.Enqueue(c0);
                if (n == 0) break;
            }
        }

        protected bool ReadQueueSeq(bool blocking, bool queueOnly, out char c)
        {
            var n = ReadQueue(blocking, queueOnly, out c);
            if (n < 0) {
                //seq was not a match 
                CancelSeq();
                return false;
            }

            _currentSequence.Enqueue(c);
            return true;
        }

        protected void CancelSeq()
        {
            while (_currentSequence.Count > 0) {
                _processedQueue.Enqueue(_currentSequence.Dequeue());
            }
        }


        /// <summary>
        /// reads a character from processed queue and processes more data if needed
        /// </summary>
        /// <param name="blocking">whether method should wait for input</param>
        /// <param name="discard">whether old, unread, still buffered data should be discarded</param>
        /// <returns>read character or default if none was read</returns>
        public char Read(bool blocking, bool discard = false)
        {
            if (discard) {
                //process everything not yet processed
                Process(false, true);
                //discard old data
                _processedQueue.Clear();
            }
            Process(_processedQueue.Count == 0 && blocking, _processedQueue.Count > 0);
            return _processedQueue.Count == 0 ? default(char) : _processedQueue.Dequeue();
        }

        public AnsiInputEvent DequeueEvent(bool blocking = false, bool process = true)
        {
            if (_eventQueue.Count > 0) {
                if (process) Process(false, false);
                return _eventQueue.Dequeue();
            }
            if (process) Process(blocking, false);
            return _eventQueue.Count == 0 ? null : _eventQueue.Dequeue();
        }

        protected internal AnsiInputEvent NextPart(ISequenceTraverserPart part, bool b, bool qo, params object[] data)
        {
            if (part is ISequenceConsumer) {
                return part.Traverse(this, default(char), b, qo, data);
            }
            if (part != null && ReadQueueSeq(b, qo, out var c1)) {
                return part.Traverse(this, c1, b, qo, data);
            }
            return null;
        }
    }
}