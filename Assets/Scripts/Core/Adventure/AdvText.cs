using System.Collections.Generic;
using UnityEngine;

namespace Core.Adventure
{
    [CreateAssetMenu(menuName = "Adventure/AdvText", fileName = "AdvText")]
    public class AdvText : ScriptableObject
    {
        public string ViewKey = "default";
        [TextArea] public List<string> Lines = new();
    }
}
