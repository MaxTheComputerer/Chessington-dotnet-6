using System.Collections.Generic;
using System.Linq;

namespace Chessington.GameEngine.Pieces
{
    public class King : Piece
    {
        public King(Player player)
            : base(player) { }

        public bool IsInCheckAtPosition(Board board, Square position)
        {
            var enemyPieces = board.GetAllPieces(board.GetEnemyOf(Player));
            enemyPieces.RemoveAll(p => p is King);
            return enemyPieces.Any(piece => piece.CanAttack(board, position));
        }

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
            availableMoves.RemoveAll(square => IsInCheckAtPosition(board, square));
            return availableMoves;
        }
    }
}