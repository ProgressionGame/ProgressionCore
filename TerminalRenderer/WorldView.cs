using System.Collections.Generic;
using System.Diagnostics;
using Progression.Engine.Core.World;

namespace Progression.TerminalRenderer
{
    public class WorldView : View
    {
        private readonly HashSet<Coordinate> _inboundUpdates = new HashSet<Coordinate>();
        public readonly List<ICRenderer> Renderers = new List<ICRenderer>();
        private bool _all = true;
        private int _x;
        private int _y;

        public TileWorld World { get; }

        public int X {
            get => _x;
            set {
                _x = value;
                _all = true;
            }
        }

        public int Y {
            get => _y;
            set {
                _y = value;
                _all = true;
            }
        }


        public override void Render()
        {
            //have local copy//avoid recalc
            var heightPx = HeightPx;
            var widthPx = WidthPx;


            if (_all) {
                var lastColour = default(Colour);
                for (int i = 0; i < heightPx; i++) {
                    Console.SetCurrentPos(TopPx + i + 1, LeftPx + 1);
                    for (int j = 0; j < widthPx; j++) {
                        bool found = false;
                        char c = ' ';
                        Colour colour = default(Colour);
                        var tile = World[i / 3 + X, j / 3 + Y];
                        var relX = i % 3;
                        var relY = j % 3;
                        for (int r = 0; r < Renderers.Count; r++) {
                            var renderer = Renderers[r];
                            if (renderer.Active(0) && renderer.Print(tile, relX, relY, out c, out colour)) {
                                found = true;
                                break;
                            }
                        }
                        if (found) {
                            if (lastColour != colour) {
                                lastColour = colour;
                                Console.SetForegroundColour(colour);
                            }
                            Console.Write(c);
                        }
                    }
                }
                Console.ClearScreen(ClearScreenMode.ToEnd);
                Console.HideCursor();
            } else {
                //todo
            }
            //Console.Write(heightPx + " " + Console.Height);
            Console.Flush();
            _inboundUpdates.Clear();
        }

        public WorldView(IConsole console, TileWorld world, int heightPx = -1, int widthPx = -1, int topPx = 0,
            int leftPx = 0) : base(console, heightPx, widthPx, topPx, leftPx)
        {
            World = world;
            X = World.Height / 2;
            Y = World.Width / 2;
        }
    }
}