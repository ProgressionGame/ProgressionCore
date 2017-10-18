using Progression.Engine.Core.Civilization;

namespace Progression.Test.Engine.Core.Civilization
{
    public class PuppetLevelAddition2 : PuppetLevel
    {
        public PuppetLevelAddition2() { }

        public PuppetLevelAddition2(bool masterUnitControl = false, bool masterBuildControl = false,
            bool masterTechControl = false, bool masterGovControl = false, bool masterDiplomacyControl = false,
            bool masterSharesTech = false, bool puppetSharesTech = false, bool masterSharesMap = false,
            bool puppetSharesMap = false, bool masterSharesLiveMap = false, bool puppetSharesLiveMap = false,
            bool addition2 = false)
            : this(Create(masterUnitControl, masterBuildControl, masterTechControl, masterGovControl,
                masterDiplomacyControl, masterSharesTech, puppetSharesTech, masterSharesMap, puppetSharesMap,
                masterSharesLiveMap, puppetSharesLiveMap), addition2) { }

        public PuppetLevelAddition2(IPuppetLevel parent, bool addition2 = false) : base(parent)
        {
            Addition2 = addition2;
        }
        
        public bool Addition2 { get; set; }
    }
}