using System;
using UnityEngine;

namespace Settings
{
    [Serializable]
    public class UgolkiRulesSettings : IUgolkiRulesSettings
    {
        [field: SerializeField]
        public string Rule1 { get; private set; } = "can_jump_diagonally";

        [field: SerializeField]
        public string Rule2 { get; private set; } = "can_jump_orthogonally";

        [field: SerializeField]
        public string Rule3 { get; private set; } = "cannot_jump";

        public string DefaultRule => Rule1;
    }
}