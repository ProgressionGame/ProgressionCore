using System;
using System.Collections.Generic;
using System.Text;

namespace Progression.Util.Keys
{
    public class Key
    {
        private readonly Dictionary<string, Key> _children = new Dictionary<string, Key>();
        private string _fullKey;
        public string Name { get; }
        public bool IsRoot => this is RootKey;
        public Key Parent { get; }
        public virtual RootKey Root => Parent.Root;
        

        internal Key(string name)
        {
            Name = name;
        }

        public Key(Key parent, string name)
        {
            Name = name.ToLower();
            parent.AddChild(this);
            Parent = parent;
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

        public virtual string AsString()
        {
            if (_fullKey != null) return _fullKey;

            var sb = new StringBuilder(Name.Length);
            Parent.AsString(sb);
            sb.Append(Name);
            _fullKey = sb.ToString();
            return _fullKey;
        }


        protected virtual void AsString(StringBuilder sb)
        {
            Parent.AsString(sb);
            sb.Append(Name);
            sb.Append('.');
        }
    }
}