using System.Text;

namespace Progression.Engine.Core.Keys
{
    public class RootKey : Key
    {
        public RootKey(string name) : base(name) { }

        public override string AsString()
        {
            return Name;
        }

        protected override void AsString(StringBuilder sb)
        {
            sb.Append(Name);
            sb.Append('.');
        }

        public override RootKey Root => this;
    }
}