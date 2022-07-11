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

            if (Player == Player.White)
            {
                availableMoves.Add(Square.At(currentSquare.Row - 1, currentSquare.Col));
            }
            else
            {
                availableMoves.Add(Square.At(currentSquare.Row + 1, currentSquare.Col));
            }
            return availableMoves;
        }
    }
}