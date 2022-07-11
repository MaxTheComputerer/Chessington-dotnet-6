using System.Collections.Generic;
using System.Linq;

namespace Chessington.GameEngine.Pieces
{
    public class Bishop : Piece
    {
        public Bishop(Player player)
            : base(player) { }

        public override IEnumerable<Square> GetAvailableMoves(Board board)
        {
            var currentSquare = board.FindPiece(this);
            var availableMoves = new List<Square>();

            for (var i = 1; i < 8; i++)
            {
                // Down and right
                if (currentSquare.Row + i < 8 && currentSquare.Col + i < 8)
                {
                    availableMoves.Add(Square.At(currentSquare.Row + i, currentSquare.Col + i));
                }
                
                // Up and left
                if (currentSquare.Row - i >= 0 && currentSquare.Col - i >= 0)
                {
                    availableMoves.Add(Square.At(currentSquare.Row - i, currentSquare.Col - i));
                }

                // Down and right
                if (currentSquare.Row + i < 8 && currentSquare.Col - i >= 0)
                {
                    availableMoves.Add(Square.At(currentSquare.Row + i, currentSquare.Col - i));
                }
                
                // Down and left
                if (currentSquare.Row - i >= 0 && currentSquare.Col + i < 8)
                {
                    availableMoves.Add(Square.At(currentSquare.Row - i, currentSquare.Col + i));
                }
            }

            return availableMoves;
        }
    }
}