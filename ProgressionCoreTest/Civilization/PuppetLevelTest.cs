using System.Diagnostics.CodeAnalysis;
using Progression.Engine.Core.Civilization;
using Xunit;

namespace Progression.Test.Engine.Core.Civilization
{
    [SuppressMessage("ReSharper", "SuggestVarOrType_SimpleTypes")]
    public class PuppetLevelTest
    {


        [Fact]
        public void TestExtensionInstanceConsistency()
        {
            var puppetLevel1 = PuppetLevel.Create();
            var puppetSpecial1 = puppetLevel1.GetSpecialised<PuppetLevelAddition1>();
            var puppetSpecial2 = new PuppetLevelAddition2(puppetLevel1);
            var puppetSpecial3 = new PuppetLevelAddition3(puppetSpecial2);
            var puppetSpecial11 = puppetSpecial3.GetSpecialised<PuppetLevelAddition1>();
            var puppetSpecial21 = puppetLevel1.GetSpecialised<PuppetLevelAddition2>();
            var puppetSpecial31 = puppetSpecial1.GetSpecialised<PuppetLevelAddition3>();
            Assert.True(puppetSpecial1 == puppetSpecial11);
            Assert.True(puppetSpecial2 == puppetSpecial21);
            Assert.True(puppetSpecial3 == puppetSpecial31);
        }


        [Fact]
        public void TestExtensionValueConservation()
        {
            var puppetLevel1 = PuppetLevel.Create(masterBuildControl: true);
            var puppetSpecial1 = puppetLevel1.GetSpecialised<PuppetLevelAddition1>();
            Assert.True(puppetLevel1.MasterBuildControl);
            Assert.Equal(puppetLevel1.MasterBuildControl, puppetSpecial1.MasterBuildControl);
            
        }



        [Fact]
        public void TestExtensionCloning()
        {
            var puppetLevel1 = PuppetLevel.Create(masterBuildControl: true);
            var puppetSpecial1 = new PuppetLevelAddition1(puppetLevel1, true); //give value for Addition1
            var puppetSpecial2 = puppetLevel1.GetSpecialised<PuppetLevelAddition2>();
            
            
            var puppetLevel1C = puppetLevel1.Clone();
            PuppetLevelAddition1 puppetSpecial1C = puppetLevel1C.GetSpecialised<PuppetLevelAddition1>();
            PuppetLevelAddition2 puppetSpecial2C = puppetLevel1C.GetSpecialised<PuppetLevelAddition2>();
            //Value Conservation
            Assert.True(puppetLevel1C.MasterBuildControl);
            Assert.True(puppetSpecial1C.MasterBuildControl);
            Assert.True(puppetSpecial2C.MasterBuildControl);
            //Instance independence
            Assert.NotEqual(puppetLevel1, puppetLevel1C);
            Assert.NotEqual(puppetSpecial1, puppetSpecial1C);
            Assert.NotEqual(puppetSpecial2, puppetSpecial2C);
            //Value independence
            puppetLevel1.MasterBuildControl = false;
            puppetSpecial1.Addition1 = false;
            puppetSpecial2C.Addition2 = true;
            Assert.NotEqual(puppetLevel1.MasterBuildControl, puppetLevel1C.MasterBuildControl);
            Assert.NotEqual(puppetSpecial1.MasterBuildControl, puppetSpecial1C.MasterBuildControl);
            Assert.NotEqual(puppetSpecial2.MasterBuildControl, puppetSpecial2C.MasterBuildControl);
            Assert.NotEqual(puppetSpecial1.Addition1, puppetSpecial1C.Addition1);
            Assert.NotEqual(puppetSpecial2.Addition2, puppetSpecial2C.Addition2);
        }
        
        
        
    }
}