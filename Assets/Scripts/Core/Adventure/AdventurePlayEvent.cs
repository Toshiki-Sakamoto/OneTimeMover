using System.Collections.Generic;

namespace Core.Adventure
{
    public class AdventurePlayEvent
    {
        public string ViewKey;
        public IReadOnlyList<string> Lines;
        public System.Action OnFinished;
    }
}
