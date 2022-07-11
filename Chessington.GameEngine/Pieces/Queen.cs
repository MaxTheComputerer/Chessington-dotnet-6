using System.Collections.Generic;
using System.Linq;

namespace Chessington.GameEngine.Pieces
{
    public class Queen : Piece
    {
        public Queen(Player player, int numberOfMoves = 0)
            : base(player, numberOfMoves) { }

        public override IEnumerable<Square> GetAvailableMoves(Board board)
        {
            var currentSquare = board.FindPiece(this);
            return GetLateralMoves(board, currentSquare).Union(GetDiagonalMoves(board, currentSquare));
        }
    }
}