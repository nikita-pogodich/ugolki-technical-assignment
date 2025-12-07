using System;
using UnityEngine;

namespace Settings.LocalizationKeys
{
    [Serializable]
    public class MessagePopupLocalizationKeys : IMessagePopupLocalizationKeys
    {
        [field: SerializeField]
        public string NotYourMoveMessage { get; private set; } = "not_your_move_message";

        [field: SerializeField]
        public string SelectPieceMessage { get; private set; } = "select_piece_message";

        [field: SerializeField]
        public string MoveUnreachableMessage { get; private set; } = "move_unreachable_message";
    }
}