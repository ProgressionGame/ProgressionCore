using System;

namespace Progression.CCL
{
    public interface IAnsiConsole
    {
        int Write(String str);
        void EnableAnsi();
    }
}