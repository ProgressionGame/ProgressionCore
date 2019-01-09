using Progression.Util.Keys;

namespace Progression.UI.TileUI
{
    public class Action : IKeyNameable
    {
        public Action(Key key, string name, ActionType type)
        {
            Key = key;
            Name = name;
            Type = type;
        }
        public Key Key { get; }
        public string Name { get; }
        public ActionType Type { get; }
    }
}