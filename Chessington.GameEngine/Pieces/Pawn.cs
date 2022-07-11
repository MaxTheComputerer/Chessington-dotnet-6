using System.Collections.Generic;
using System.Linq;

namespace Chessington.GameEngine.Pieces
{
    public class Pawn : Piece
    {
        public Pawn(Player player) 
            : base(player) { }

        public override IEnumerable<Square> GetAvailableMoves(Board board)
        {
            var currentSquare = board.FindPiece(this);
            var availableMoves = new List<Square>();

            // Can always move one square up/down depending on player colour
            var direction = (Player == Player.White) ? -1 : 1;
            availableMoves.Add(Square.At(currentSquare.Row + direction, currentSquare.Col));

            // Can move two spaces up/down if it hasn't moved yet, and if path is clear
            if (NumberOfMoves == 0 && !board.IsObstructed(availableMoves[0]))
            {
                availableMoves.Add(Square.At(currentSquare.Row + (2 * direction), currentSquare.Col));
            }
            
            availableMoves.RemoveAll(board.IsObstructed);

            // Allow pawns to capture on diagonals
            var leftDiagonal = Square.At(currentSquare.Row + direction, currentSquare.Col - 1);
            var rightDiagonal = Square.At(currentSquare.Row + direction, currentSquare.Col + 1);

            if (board.IsObstructed(leftDiagonal) && CanCaptureAtSquare(board, leftDiagonal))
            {
                availableMoves.Add(leftDiagonal);
            }
            if (board.IsObstructed(rightDiagonal) && CanCaptureAtSquare(board, rightDiagonal))
            {
                availableMoves.Add(rightDiagonal);
            }
            
            availableMoves.RemoveAll(square => board.IsObstructed(square) && !CanCaptureAtSquare(board, square));
            return availableMoves;
        }
    }
}