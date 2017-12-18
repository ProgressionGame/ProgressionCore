using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Progression.Util.Keys;
using Progression.Engine.Core.World;
using Progression.Engine.Core.World.Features.Base;
using Progression.Engine.Core.World.Features.Yield;
using static Progression.Engine.Core.World.Features.Yield.YieldModifierType;
using static Progression.Engine.Core.World.Features.Yield.TileYieldModifierPriority;
using System.Threading;
using Progression.Engine.Core.Civilization;
using Progression.Util.BinPacking;
using Progression.Engine.Core.World.Features.Terrain;
using Progression.Resources.Manager;
using Progression.Util;
using static Progression.Resources.Manager.ResourceTypeEnum;

// ReSharper disable LocalizableElement

namespace TestLauncher
{
    [SuppressMessage("ReSharper", "HeapView.BoxingAllocation")]
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            MethodCall();
            
            Console.WriteLine($"Starting version {Utils.ReleaseType}");

            Console.WriteLine($"Category of {CategoryUndefined} is {CategoryUndefined.Category()}");

            var resMan = new ResourceDecoderManager();
            resMan.Init();
            var sw = new Stopwatch();
            sw.Start();
            resMan.LoadExtensions();
            sw.Stop();
            Console.WriteLine($"Loading ressource plugins took {sw.ElapsedMilliseconds}ms");
            //TestWorld();
        }


        private static void TestBinPackingRealistic()
        {
            const int size = 32;
            const int amount = 200;


            var packets = new List<Packet>();
            var rnd = new Random();
            for (int i = 0; i < 40 * amount; i++) {
                packets.Add(new Packet(null, rnd.Next(1, 6)));
            }
            for (int i = 0; i < 15 * amount; i++) {
                packets.Add(new Packet(null, rnd.Next(6, 20)));
            }
            for (int i = 0; i < 3 * amount; i++) {
                packets.Add(new Packet(null, rnd.Next(16, 33)));
            }

            var sws = new Stopwatch();
            sws.Start();
            var binsStupid = BinPackingSolvers.SolveStupid(new List<Packet>(packets), size);
            sws.Stop();

            var swBf = new Stopwatch();
            swBf.Start();
            var binsBf = BinPackingSolvers.SolveBestFitMaybe(new List<Packet>(packets), size);
            swBf.Stop();

//            Console.WriteLine("binsStupid:");
//            printBin(binsStupid, size);
//            Console.WriteLine("binsBF:");
//            printBin(binsBF, size);
            Console.WriteLine(
                $"binsStupid:{binsStupid.Count} {CalcFreeSpaceBins(binsStupid, size)} {sws.ElapsedMilliseconds}ms+{sws.ElapsedTicks}");
            Console.WriteLine(
                $"binsBF:{binsBf.Count} {CalcFreeSpaceBins(binsBf, size)} {swBf.ElapsedMilliseconds}ms+{swBf.ElapsedTicks}");


            Console.ReadKey();
        }

        private static void TestBinPacking()
        {
            List<Packet> packets = new List<Packet>();
            const int min = 3;
            const int max = 8;
            const int size = 10;
            const int amount = 1;


            for (int j = 0; j < amount; j++) {
                for (int i = min; i <= max; i++) {
                    packets.Add(new Packet(null, i));
                }
            }
            var binsStupid = BinPackingSolvers.SolveStupid(new List<Packet>(packets), size);
            var binsBf = BinPackingSolvers.SolveBestFitMaybe(new List<Packet>(packets), size);
            Console.WriteLine($"binsStupid:{binsStupid.Count} {CalcFreeSpaceBins(binsStupid, size)}");
            PrintBin(binsStupid, size);
            Console.WriteLine($"binsBF:{binsBf.Count} {CalcFreeSpaceBins(binsBf, size)}");
            PrintBin(binsBf, size);
            Console.WriteLine("now random numbers");
            packets = new List<Packet>();
            var rnd = new Random();
            for (int i = 0; i < (max - min + 1) * amount; i++) {
                packets.Add(new Packet(null, rnd.Next(min, max)));
            }
            binsStupid = BinPackingSolvers.SolveStupid(new List<Packet>(packets), size);
            binsBf = BinPackingSolvers.SolveBestFitMaybe(new List<Packet>(packets), size);
            Console.WriteLine($"binsStupid:{binsStupid.Count} {CalcFreeSpaceBins(binsStupid, size)}");
            PrintBin(binsStupid, size);
            Console.WriteLine($"binsBF:{binsBf.Count} {CalcFreeSpaceBins(binsBf, size)}");
            PrintBin(binsBf, size);


            Console.ReadKey();
        }

        private static int CalcFreeSpaceBins(List<Bin> bins, int size)
        {
            int result = 0;
            foreach (var bin in bins) {
                result += size - bin.Used;
                if (size < bin.Used) {
                    Console.WriteLine("bin too full");
                }
            }
            return result;
        }

        private static void PrintBin(List<Bin> bins, int size)
        {
            for (int i = 0; i < bins.Count; i++) {
                var bin = bins[i];
                Console.Write($"Bin {i} {bin.Used}/{size}:");
                foreach (var packet in bin) {
                    Console.Write($" {packet.Size}");
                }
                Console.WriteLine();
            }
        }

        private static void TestWorld()
        {
            //game data
            var root = new RootKey("root");

            YieldManager ym = new YieldManagerImpl(new Key(root, "yield"));
            var food = new YieldType("food", ym);
            var production = new YieldType("production", ym);
            var commerce = new YieldType("commerce", ym);

            var resTb = new YieldModifyingSSFR<TerrainBiome>(WorldType.World, new Key(root, "tb"), false, Terrain);
            var grassland = new TerrainBiome("grassland", resTb, ym, Addition, new double[] {2, 0, 0});
            var plains = new TerrainBiome("plains", resTb, ym, Addition, new double[] {1, 1, 0});
            var desert = new TerrainBiome("desert", resTb, ym, Addition, new double[] {0, 1, 0});
            var tundra = new TerrainBiome("tundra", resTb, ym, Addition, new double[] {1, 0, 0});
            var ice = new TerrainBiome("ice", resTb, ym, Addition, new double[] {0, 0, 0});
            var resTv = new YieldModifyingSMFR<TerrainVegetation>(WorldType.World, new Key(root, "tv"), Terrain);
            var jungle = new TerrainVegetation("jungle", resTv, ym, Addition, new double[] {0, -1, -1});
            var forest = new TerrainVegetation("forest", resTv, ym, Addition, new double[] {0, 1, 0});
            var pineForest = new TerrainVegetation("pine forest", resTv, ym, Addition, new double[] {0, 1, 0});
            var resTl = new YieldModifyingSSFR<TerrainLandform>(WorldType.World, new Key(root, "tl"), false, Terrain);
            var flatland = new TerrainLandform("flatland", resTl, ym, Addition, new double[] {0, 0, 0});
            var hills = new TerrainLandform("hills", resTl, ym, Addition, new double[] {-1, 1, 0});
            var mountains = new TerrainLandform("mountains", resTl, ym, Addition, new double[] {-2, 1, 0});
            var highMountains = new TerrainLandform("high mountains", resTl, ym, Addition, new double[] {-2, 0, -1});

            //civs
            var civMan = new CivilizationManager(new Key(root, "civs"), 4);
            var rome = new Civilization("Rome", civMan);
            var egypt = new Civilization("Egypt", civMan);


            //init
            var fw = new FeatureWorld(2, ym);
            fw.Register(resTb);
            fw.Register(resTv);
            fw.Register(resTl);
            fw.Register(civMan.Resolver);
            fw.Lock();

            //world creation
            var world = new TileWorld(fw, 5, 5);

            Console.WriteLine(world.HasFeature(3, 3, grassland));
            Console.WriteLine(world.HasFeature(3, 3, flatland));
            Console.WriteLine(world.HasFeature(3, 3, jungle));
            Console.WriteLine(world.HasFeature(3, 3, mountains));
            Console.WriteLine(flatland.Id);
            Console.WriteLine(hills.Id);
            Console.WriteLine(mountains.Id);
            Console.WriteLine();
            Console.WriteLine(
                $"{world.CalcYield(3, 3, food)}, {world.CalcYield(3, 3, production)}, {world.CalcYield(3, 3, commerce)}");
            world.AddFeature(3, 3, forest);
            Console.WriteLine(
                $"{world.CalcYield(3, 3, food)}, {world.CalcYield(3, 3, production)}, {world.CalcYield(3, 3, commerce)}");
            world.AddFeature(3, 3, hills);
            Console.WriteLine(
                $"{world.CalcYield(3, 3, food)}, {world.CalcYield(3, 3, production)}, {world.CalcYield(3, 3, commerce)}");
            Console.WriteLine(world.HasFeature(3, 3, grassland));
            Console.WriteLine(world.HasFeature(3, 3, forest));
            Console.WriteLine(world.HasFeature(3, 3, hills));

            Console.WriteLine($"Owner: 2,3={civMan.GetOwner(world[2, 3])?.Name ?? "NoMensLand"}");
            Console.WriteLine(
                $"Rome Vision={rome.GetVision(world[2, 3])}, Egypt Vision={egypt.GetVision(world[2, 3])}, ");
            rome.Own(world[2, 3]);
            Console.WriteLine($"Owner: 2,3={civMan.GetOwner(world[2, 3])?.Name ?? "NoMensLand"}");
            Console.WriteLine(
                $"Rome Vision={rome.GetVision(world[2, 3])}, Egypt Vision={egypt.GetVision(world[2, 3])}, ");
            egypt.Own(world[2, 3]);
            Console.WriteLine($"Owner: 2,3={civMan.GetOwner(world[2, 3])?.Name ?? "NoMensLand"}");
            Console.WriteLine(
                $"Rome Vision={rome.GetVision(world[2, 3])}, Egypt Vision={egypt.GetVision(world[2, 3])}, ");
            Console.WriteLine($"Owner: 3,2={civMan.GetOwner(world[3, 2])?.Name ?? "NoMensLand"}");


            Console.ReadKey();

            var sw = new Stopwatch();

            //big world alloc
            sw.Start();
            const int worldSize = 5000;
            world = new TileWorld(fw, worldSize, worldSize);
            var world2 = new TileWorld(fw, worldSize, worldSize);
            sw.Stop();
            GC.Collect();
            Console.WriteLine($"ms:{sw.ElapsedMilliseconds}");

            //prepair multithreading test
            var wInterface = new WorldInterfaceImpl(world2);
            var monitor = new object();
            bool[] flag = {
                true
            };

            var thread2 = new Thread(() => {
                lock (monitor) Monitor.Wait(monitor);
                Console.WriteLine("Thread 2 stopped waiting");
                var sw2 = new Stopwatch();
                sw2.Start();
                wInterface.Execute();
                sw2.Stop();
                Console.WriteLine($"Thread 2 took {sw2.Elapsed.TotalMilliseconds}ms");
                lock (monitor) Monitor.Pulse(monitor);
                lock (monitor) Monitor.Wait(monitor);
                int workLoops = 0;
                int allLoops = 0;
                int j = 0;
                long lastTiming = 0;
                //Thread.Sleep(50);
                while (flag[0]) {
                    j++;
                    sw.Restart();
                    if (wInterface.Execute()) {
                        sw.Stop();
                        lastTiming += sw.ElapsedTicks;
                        workLoops++;
                        allLoops += j;
                        j = 0;
                    }
                }

                Console.WriteLine(
                    $"Thread 2 made {workLoops}/{allLoops} loops. Took: {new TimeSpan(lastTiming).TotalMilliseconds}ms");
            });
            thread2.Start();

            //random updates
            world.RegisterUpdate(wInterface.ScheduleUpdate);
            IFeature[] allFeatures = {
                desert, flatland, forest, grassland, highMountains, hills, ice, jungle, mountains, pineForest, plains,
                tundra
            };
            var rnd = new Random();
            Console.WriteLine("random updates");

            sw.Start();
            for (int i = 0; i < 1_000_000; i++) {
                world[rnd.Next(worldSize), rnd.Next(worldSize), allFeatures[rnd.Next(allFeatures.Length)]] = true;
            }
            sw.Stop();
            lock (monitor) Monitor.Pulse(monitor);
            Console.WriteLine($"random updates done. took {sw.ElapsedMilliseconds}ms");
            lock (monitor) Monitor.Wait(monitor);
            Console.ReadKey();


            Console.WriteLine("random updates parallel");
            lock (monitor) Monitor.Pulse(monitor);
            for (int i = 0; i < 1_000_000; i++) {
                world[rnd.Next(worldSize), rnd.Next(worldSize), allFeatures[rnd.Next(allFeatures.Length)]] = true;
            }
            flag[0] = false;
            Console.WriteLine("random updates parallel done");


            Console.ReadKey();
            Console.WriteLine(world.HasFeature(3, 3, mountains));
            Console.WriteLine(world.HasFeature(3, 3, grassland));

            Console.ReadKey();
        }


        private static void TestPuppetLevel()
        {
            var puppetLevel1 = PuppetLevel.Create(masterBuildControl: true);
            var puppetSpecial1 = new PuppetLevelAddition1(puppetLevel1);
            var puppetSpecial2 = puppetLevel1.GetSpecialised<PuppetLevelAddition2>();
            Console.WriteLine(
                $"{puppetLevel1.MasterBuildControl} {puppetSpecial1.MasterBuildControl} {puppetSpecial2.MasterBuildControl}");
            var puppetSpecial21 = puppetSpecial1.GetSpecialised<PuppetLevelAddition2>();
            Console.WriteLine(puppetSpecial2 == puppetSpecial21);
            Console.WriteLine();
            puppetSpecial2.Addition2 = true;

            var puppetLevel1C = puppetLevel1.Clone();
            var puppetSpecial1C = puppetLevel1C.GetSpecialised<PuppetLevelAddition1>();
            var puppetSpecial2C = puppetLevel1C.GetSpecialised<PuppetLevelAddition2>();
            Console.WriteLine(
                $"{puppetLevel1.MasterBuildControl} {puppetSpecial1.MasterBuildControl} {puppetSpecial2.MasterBuildControl}");
            Console.WriteLine(
                $"{puppetLevel1 == puppetLevel1C} {puppetSpecial1 == puppetSpecial1C} {puppetSpecial2 == puppetSpecial2C}");
            Console.WriteLine();
            puppetSpecial1C.Addition1 = true;
            puppetSpecial2.Addition2 = false;

            Console.WriteLine($"{puppetSpecial1.Addition1} vs {puppetSpecial1C.Addition1}");
            Console.WriteLine($"{puppetSpecial2.Addition2} vs {puppetSpecial2C.Addition2}");
        }


        //bnechmark 
        abstract class ClassA
        {
            public abstract int Func2(int a);
            public abstract int Func3(int a);
        }

        class ClassB : ClassA
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            public int Func1(int a)
            {
                return a + 2;
            }

            public override int Func2(int a)
            {
                return a + 2;
            }

            public sealed override int Func3(int a)
            {
                return a + 2;
            }
        }

        public static void MethodCall()
        {
            const int loops = 1000000000;
            int x = 0;
            ClassB b = new ClassB();

            Console.WriteLine("Method Call Overhead:");

            Stopwatch watch = new Stopwatch();
            watch.Start();
            for (int i = 0; i < loops; i++) {
                x = x + 2;
            }
            watch.Stop();
            Report("No function", loops, watch.ElapsedMilliseconds);
            x -= 2 * loops;

            watch.Restart();
            for (int i = 0; i < loops; i++) {
                x = b.Func1(x);
            }
            watch.Stop();
            Report("Non-virtual", loops, watch.ElapsedMilliseconds);
            x -= 2 * loops;

            watch.Restart();
            for (int i = 0; i < loops; i++) {
                x = b.Func2(x);
            }
            watch.Stop();
            Report("override", loops, watch.ElapsedMilliseconds);
            x -= 2 * loops;

            watch.Restart();
            for (int i = 0; i < loops; i++) {
                x = b.Func3(x);
            }
            watch.Stop();
            Report("sealed override", loops, watch.ElapsedMilliseconds);
            x -= 2 * loops;

            ClassA a = b;
            watch.Restart();
            for (int i = 0; i < loops; i++) {
                x = a.Func2(x);
            }
            watch.Stop();
            Report("virtual via override", loops, watch.ElapsedMilliseconds);
            x -= 2 * loops;

            watch.Restart();
            for (int i = 0; i < loops; i++) {
                x = a.Func3(x);
            }
            watch.Stop();
            Report("Sealed via override", loops, watch.ElapsedMilliseconds);
            x -= 2 * loops;

            Console.WriteLine(x); // so the compiler doesn't optimize it away
        }

        static void Report(string message, int iterations, long milliseconds)
        {
            Console.WriteLine(string.Format("{0,-26:} {1,10:N1} MOps/s, {2,7:N3} s", message,
                (double) iterations / 1000.0 / milliseconds, milliseconds / 1000.0));
        }
    }
}