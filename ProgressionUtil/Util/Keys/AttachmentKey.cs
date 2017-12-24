using System;
using System.Collections.Generic;

namespace Progression.Util.Keys
{
    public class AttachmentKey<T> : AttachmentKey { }

    public abstract class AttachmentKey : IFreezable
    {
        private const string OnlyAllowedBeforeRegistration = "Only allowed before registration";
        private readonly List<KeyFlavour> _flavours = new List<KeyFlavour>();
        private int _id=-1;


        public int Id => _id;

        public bool Applicable(KeyFlavour flavour)
        {
            if (IsFrozen) throw new InvalidOperationException(OnlyAllowedBeforeRegistration);
            return _flavours.Contains(flavour);
        }

        public void AddFlavour(KeyFlavour flavour)
        {
            if (IsFrozen) throw new InvalidOperationException(OnlyAllowedBeforeRegistration);
            if (Applicable(flavour)) throw new ArgumentException("Cannot add flavour twice");
            _flavours.Add(flavour);
        }

        public void AddFlavour(IKeyFlavourable flavourable) => AddFlavour(flavourable.KeyFlavour);

        public int CountFlovours => _flavours.Count;

        public void Register()
        {
            if (IsFrozen) throw new InvalidOperationException(OnlyAllowedBeforeRegistration);
            bool CheckId(int id)
            {
                foreach (var flavour in _flavours) {
                    if (!flavour.IsFree(id, this)) return true; //continue searching
                }
                return false;
            }

            var i = 0;
            for (; CheckId(i); i++) { }
            _id = i;
            var rereg = new List<AttachmentKey>();
            foreach (var flavour in _flavours) {
                AttachmentKey att;
                if ((att = flavour.Register(i, this))!= null) rereg.Add(att);
            }
            
            //reregister replaced keys
            foreach (var attachmentKey in rereg) {
                attachmentKey.Register();
            }
            Freeze();
        }
        
        public bool IsFrozen { get; private set; }
        public bool IsUsed { get; private set; }

        public void Freeze()
        {
            IsFrozen = true;
        }
        
        protected internal void Unregister()
        {
            IsFrozen = false;
            foreach (var flavour in _flavours) {
                flavour.Unregister(_id, this);
            }
        }

        public void Use()
        {
            IsUsed = true;
        }
    }
}