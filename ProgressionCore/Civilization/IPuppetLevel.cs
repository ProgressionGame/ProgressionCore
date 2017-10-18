using System;

namespace Progression.Engine.Core.Civilization
{
    public interface IPuppetLevel : ICloneable
    {
        bool MasterUnitControl { get; set; }
        bool MasterBuildControl { get; set; }
        bool MasterTechControl { get; set; }
        bool MasterGovControl { get; set; }
        bool MasterDiplomacyControl { get; set; }
        bool PuppetCanDeclareWar { get; }
        bool MasterSharesTech { get; set; }
        bool PuppetSharesTech { get; set; }
        bool MasterSharesMap { get; set; }
        bool PuppetSharesMap { get; set; }
        bool MasterSharesLiveMap { get; set; }
        bool PuppetSharesLiveMap { get; set; }
        new IPuppetLevel Clone();

        T GetSpecialised<T>() where T : PuppetLevel, new();

        T GetSpecialised<T>(bool create) where T : PuppetLevel, new();
    }
}