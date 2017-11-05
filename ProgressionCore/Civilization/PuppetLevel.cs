using System;

namespace Progression.Engine.Core.Civilization
{
    public class PuppetLevel : IPuppetLevel
    {  //just saying i have no idea if that was a good idea. probably going to improve on this
        protected IPuppetLevel Parent;
        protected PuppetLevelData Data;

        protected PuppetLevel() { }

        protected PuppetLevel(IPuppetLevel parent)
        {
            if (parent is PuppetLevel level) {
                Init(level.Data.Head);
            } else {
                var data = (PuppetLevelData) parent;
                Init(data.Head ?? (IPuppetLevel) data);
            }
        }

        public static IPuppetLevel Create(bool masterUnitControl = false, bool masterBuildControl = false,
            bool masterTechControl = false, bool masterGovControl = false, bool masterDiplomacyControl = false,
            bool masterSharesTech = false, bool puppetSharesTech = false, bool masterSharesMap = false,
            bool puppetSharesMap = false, bool masterSharesLiveMap = false, bool puppetSharesLiveMap = false) =>
            new PuppetLevelData(null, masterUnitControl, masterBuildControl, masterTechControl, masterGovControl,
                masterDiplomacyControl, masterSharesTech, puppetSharesTech, masterSharesMap, puppetSharesMap,
                masterSharesLiveMap, puppetSharesLiveMap);

        public T GetSpecialised<T>() where T : PuppetLevel, new()
        {
            return GetSpecialised<T>(true);
        }

        public T GetSpecialised<T>(bool create) where T : PuppetLevel, new()
        {
            if (this is T thisT) return thisT;
            IPuppetLevel tmp = Data.Head;
            while (tmp is PuppetLevel) {
                if (tmp is T result) return result;
                tmp = ((PuppetLevel) tmp).Parent;
            }
            return create ? (T) new T().Init(Data.Head) : null;
        }

        protected PuppetLevel Init(IPuppetLevel parent)
        {
            if (Parent != null) throw new InvalidOperationException("Cannot be initialized twice");
            var tmp = parent;
            while (tmp is PuppetLevel casted) {
                if (tmp.GetType().IsAssignableFrom(GetType()))
                    throw new ArgumentException("All parents must be of different type");
                tmp = casted.Parent;
            }
            Parent = parent;
            Data = (PuppetLevelData) tmp;
            Data.Head = this;
            return this;
        }
        
        public IPuppetLevel Clone()
        {
            return Data.Head == this ? CloneForReal(Data.CloneForReal()) : Data.Head.Clone();
        }


        protected virtual PuppetLevel CloneForReal(PuppetLevelData newData)
        {
            var clone = (PuppetLevel) MemberwiseClone();
            clone.Data = newData;
            if (newData.Head == null) newData.Head = clone;
            if (Parent is PuppetLevel parentClone) {
                parentClone = parentClone.CloneForReal(newData);
                clone.Parent = parentClone;
            } else {
                clone.Parent = newData;
            }
            return clone;
        }

        public class PuppetLevelData : IPuppetLevel
        {
            private bool _masterSharesLiveMap;
            private bool _puppetSharesLiveMap;
            protected internal PuppetLevel Head;

            protected internal PuppetLevelData(PuppetLevel head, bool masterUnitControl, bool masterBuildControl,
                bool masterTechControl, bool masterGovControl, bool masterDiplomacyControl,
                bool masterSharesTech, bool puppetSharesTech, bool masterSharesMap,
                bool puppetSharesMap, bool masterSharesLiveMap, bool puppetSharesLiveMap)
            {
                Head = head;
                MasterUnitControl = masterUnitControl;
                MasterBuildControl = masterBuildControl;
                MasterTechControl = masterTechControl;
                MasterGovControl = masterGovControl;
                MasterDiplomacyControl = masterDiplomacyControl;
                MasterSharesTech = masterSharesTech;
                PuppetSharesTech = puppetSharesTech;
                MasterSharesMap = masterSharesMap;
                PuppetSharesMap = puppetSharesMap;
                MasterSharesLiveMap = masterSharesLiveMap;
                PuppetSharesLiveMap = puppetSharesLiveMap;
            }


            public bool MasterUnitControl { get; set; }
            public bool MasterBuildControl { get; set; }
            public bool MasterTechControl { get; set; }
            public bool MasterGovControl { get; set; }
            public bool MasterDiplomacyControl { get; set; }
            public bool PuppetCanDeclareWar => !MasterUnitControl & !MasterDiplomacyControl;
            public bool MasterSharesTech { get; set; }
            public bool PuppetSharesTech { get; set; }
            public bool MasterSharesMap { get; set; }
            public bool PuppetSharesMap { get; set; }

            public bool MasterSharesLiveMap {
                get => _masterSharesLiveMap & MasterSharesMap;
                set => _masterSharesLiveMap = value;
            }

            public bool PuppetSharesLiveMap {
                get => _puppetSharesLiveMap & PuppetSharesMap;
                set => _puppetSharesLiveMap = value;
            }

            public IPuppetLevel Clone() => Head == null ? CloneForReal() : Head.Clone();

            public T GetSpecialised<T>() where T : PuppetLevel, new() => GetSpecialised<T>(true);

            public T GetSpecialised<T>(bool create) where T : PuppetLevel, new() => 
                Head == null ? 
                    create ? 
                        (T) new T().Init(this) 
                        : null
                    : Head.GetSpecialised<T>(create);

            protected internal PuppetLevelData CloneForReal()
            {
                var clone = (PuppetLevelData) MemberwiseClone();
                clone.Head = null;
                return clone;
            }

            object ICloneable.Clone() => Clone();
        }

        #region Wrapper

        public bool MasterUnitControl {
            get => Data.MasterUnitControl;
            set => Data.MasterUnitControl = value;
        }

        public bool MasterBuildControl {
            get => Data.MasterBuildControl;
            set => Data.MasterBuildControl = value;
        }

        public bool MasterTechControl {
            get => Data.MasterTechControl;
            set => Data.MasterTechControl = value;
        }

        public bool MasterGovControl {
            get => Data.MasterGovControl;
            set => Data.MasterGovControl = value;
        }

        public bool MasterDiplomacyControl {
            get => Data.MasterDiplomacyControl;
            set => Data.MasterDiplomacyControl = value;
        }

        public bool PuppetCanDeclareWar => Data.PuppetCanDeclareWar;

        public bool MasterSharesTech {
            get => Data.MasterSharesTech;
            set => Data.MasterSharesTech = value;
        }

        public bool PuppetSharesTech {
            get => Data.PuppetSharesTech;
            set => Data.PuppetSharesTech = value;
        }

        public bool MasterSharesMap {
            get => Data.MasterSharesMap;
            set => Data.MasterSharesMap = value;
        }

        public bool PuppetSharesMap {
            get => Data.PuppetSharesMap;
            set => Data.PuppetSharesMap = value;
        }

        public bool MasterSharesLiveMap {
            get => Data.MasterSharesLiveMap;
            set => Data.MasterSharesLiveMap = value;
        }

        public bool PuppetSharesLiveMap {
            get => Data.PuppetSharesLiveMap;
            set => Data.PuppetSharesLiveMap = value;
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        #endregion
    }
}