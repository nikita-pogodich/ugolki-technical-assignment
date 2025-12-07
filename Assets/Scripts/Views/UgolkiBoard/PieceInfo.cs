using UnityEngine;

namespace Views.UgolkiBoard
{
    public class PieceInfo
    {
        public readonly string PieceResourceKey;
        public readonly GameObject Piece;

        public PieceInfo(string pieceResourceKey, GameObject piece)
        {
            PieceResourceKey = pieceResourceKey;
            Piece = piece;
        }
    }
}