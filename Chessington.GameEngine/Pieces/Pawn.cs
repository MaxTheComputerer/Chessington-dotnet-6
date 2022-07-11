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
            return availableMoves;
        }
    }
}