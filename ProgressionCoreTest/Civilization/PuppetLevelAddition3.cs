using Progression.Engine.Core.Civilization;

namespace Progression.Test.Engine.Core.Civilization
{
    public class PuppetLevelAddition3 : PuppetLevel
    {
        public PuppetLevelAddition3() { }

        public PuppetLevelAddition3(bool masterUnitControl = false, bool masterBuildControl = false,
            bool masterTechControl = false, bool masterGovControl = false, bool masterDiplomacyControl = false,
            bool masterSharesTech = false, bool puppetSharesTech = false, bool masterSharesMap = false,
            bool puppetSharesMap = false, bool masterSharesLiveMap = false, bool puppetSharesLiveMap = false,
            bool addition3 = false)
            : this(Create(masterUnitControl, masterBuildControl, masterTechControl, masterGovControl,
                masterDiplomacyControl, masterSharesTech, puppetSharesTech, masterSharesMap, puppetSharesMap,
                masterSharesLiveMap, puppetSharesLiveMap), addition3) { }

        public PuppetLevelAddition3(IPuppetLevel parent, bool addition3 = false) : base(parent)
        {
            Addition3 = addition3;
        }
        
        public bool Addition3 { get; set; }
    }
}