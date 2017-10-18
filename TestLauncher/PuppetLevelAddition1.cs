using Progression.Engine.Core.Civilization;

namespace TestLauncher
{
    public class PuppetLevelAddition1 : PuppetLevel
    {
        public PuppetLevelAddition1() { }

        public PuppetLevelAddition1(bool masterUnitControl = false, bool masterBuildControl = false,
            bool masterTechControl = false, bool masterGovControl = false, bool masterDiplomacyControl = false,
            bool masterSharesTech = false, bool puppetSharesTech = false, bool masterSharesMap = false,
            bool puppetSharesMap = false, bool masterSharesLiveMap = false, bool puppetSharesLiveMap = false,
            bool addition1 = false)
            : this(Create(masterUnitControl, masterBuildControl, masterTechControl, masterGovControl,
                masterDiplomacyControl, masterSharesTech, puppetSharesTech, masterSharesMap, puppetSharesMap,
                masterSharesLiveMap, puppetSharesLiveMap), addition1) { }

        public PuppetLevelAddition1(IPuppetLevel parent, bool addition1 = false) : base(parent)
        {
            Addition1 = addition1;
        }
        
        public bool Addition1 { get; set; }
        
    }
}