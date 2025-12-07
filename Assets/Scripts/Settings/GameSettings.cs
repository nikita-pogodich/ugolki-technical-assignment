using System;
using UnityEngine;

namespace Settings
{
    [Serializable]
    public class GameSettings : IGameSettings
    {
        [field: SerializeField]
        public int BoardSize { get; private set; } = 8;
    }
}