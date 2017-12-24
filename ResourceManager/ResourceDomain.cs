using System;

namespace Progression.Resource
{
    [Flags]
    public enum ResourceDomain
    {
        //Base type mask 0xFF
        Server = 0x1,
        Client = 0x2,
        CalculationHelper = 0x4,
        Editor = 0x8,
        
        Graphical = Client | Editor,
        Calculational = Server | CalculationHelper,
        WhilePlaying = Client | Calculational,
        
        //Grafics Type mask 0xFF00
        Graphics2DOnly = 0x100,
        Graphics2D = Graphics2DOnly | Graphical,
        Graphics3DOnly = 0x200,
        Graphics3D = Graphics3DOnly | Graphical,
        GraphicsAnsiOnly = 0x300,
        GraphicsAnsi = GraphicsAnsiOnly | Graphical,
        
        //Calc types mask 0xFF0000
        ExecutionOnly = 0x10000,
        Execution = ExecutionOnly | Calculational,
        AIOnly = 0x20000,
        AI = AIOnly  | Calculational,
        
        //special 
        None = 0x0,
        All = Server | Client | CalculationHelper | Editor | Graphics2DOnly | Graphics3DOnly | GraphicsAnsiOnly | ExecutionOnly | AIOnly
    }
}