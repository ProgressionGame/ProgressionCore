using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Progression.Util.Keys
{
    public class Key
    {
        private readonly Dictionary<string, Key> _children = new Dictionary<string, Key>();
        private string _fullKey;
        private readonly WeakReference<IKeyed> _holder;
        private KeyFlavour _flavour;
        private List<object> _attachments =null;
        public string Name { get; }
        public bool IsRoot => this is RootKey;
        public Key Parent { get; }
        public virtual RootKey Root => Parent.Root;

        public KeyFlavour Flavour {
            get => _flavour;
            set {
                if (_attachments != null) throw new InvalidOperationException("Cannot change flavour while in use");
                _flavour = value;
            }
        }

        public IKeyed Holder {
            get {
                IKeyed rslt = null;
                _holder?.TryGetTarget(out rslt);
                return rslt;
            }
        }


        internal Key(string name)
        {
            Name = name;
        }

        public Key(Key parent, string name) : this(parent, null, name) { }

        public Key(Key parent, IKeyed holder, string name)
        {
            Name = name.ToLower();
            parent.AddChild(this);
            Parent = parent;
            _holder = holder == null ? null : new WeakReference<IKeyed>(holder);
        }


        private void AddChild(Key child)
        {
            lock (_children) {
                if (_children.ContainsKey(child.Name)) {
                    throw new ArgumentException("There is already a key with the name " + AsString() + "." +
                                                child.Name + "! (all keys are lowercase)");
                }
                _children.Add(child.Name, child);
            }
        }

        public Key GetChild(string name)
        {
            name = name.ToLower();
            lock (_children) {
                return _children[name];
            }
        }

        public virtual string AsString()
        {
            if (_fullKey != null) return _fullKey;

            var sb = new StringBuilder(Name.Length);
            Parent.AsString(sb);
            sb.Append(Name);
            _fullKey = sb.ToString();
            return _fullKey;
        }
        
        public T Get<T>(AttachmentKey<T> key)
        {
            return _attachments == null || _attachments.Count < key.Id ? default(T) : (T)_attachments[key.Id];
        }

        public void Set<T>(AttachmentKey<T> key, T value)
        {
            if (_attachments == null) {
                _attachments = new List<object>(_flavour.Count);
            }
            key.Use();
            int id = key.Id;
            while (_attachments.Count <= id) {
                _attachments.Add(null);
            }
            _attachments[id] = value;
        }
        


        protected virtual void AsString(StringBuilder sb)
        {
            Parent.AsString(sb);
            sb.Append(Name);
            sb.Append('.');
        }
    }
}