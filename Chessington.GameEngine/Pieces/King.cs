using System.Collections.Generic;
using System.Linq;

namespace Chessington.GameEngine.Pieces
{
    public class King : Piece
    {
        public King(Player player)
            : base(player) { }

        public override IEnumerable<Square> GetAvailableMoves(Board board)
        {
            var currentSquare = board.FindPiece(this);
            var availableMoves = new List<Square>
            {
                Square.At(currentSquare.Row + 0, currentSquare.Col + 1),
                Square.At(currentSquare.Row + 0, currentSquare.Col - 1),
                Square.At(currentSquare.Row + 1, currentSquare.Col + 0),
                Square.At(currentSquare.Row - 1, currentSquare.Col + 0),
                Square.At(currentSquare.Row + 1, currentSquare.Col + 1),
                Square.At(currentSquare.Row + 1, currentSquare.Col - 1),
                Square.At(currentSquare.Row - 1, currentSquare.Col + 1),
                Square.At(currentSquare.Row - 1, currentSquare.Col - 1)
            };

            availableMoves.RemoveAll(square => board.IsObstructed(square) && !CanCaptureAtSquare(board, square));
            return availableMoves;
        }
    }
}