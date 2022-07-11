using System.Collections.Generic;
using System.Linq;

namespace Chessington.GameEngine.Pieces
{
    public class Knight : Piece
    {
        public Knight(Player player)
            : base(player) { }

        public override IEnumerable<Square> GetAvailableMoves(Board board)
        {
            var currentSquare = board.FindPiece(this);
            var availableMoves = new List<Square>();
            
            availableMoves.Add(Square.At(currentSquare.Row + 2, currentSquare.Col + 1));
            availableMoves.Add(Square.At(currentSquare.Row + 2, currentSquare.Col - 1));
            
            availableMoves.Add(Square.At(currentSquare.Row - 2, currentSquare.Col + 1));
            availableMoves.Add(Square.At(currentSquare.Row - 2, currentSquare.Col - 1));
            
            availableMoves.Add(Square.At(currentSquare.Row + 1, currentSquare.Col + 2));
            availableMoves.Add(Square.At(currentSquare.Row - 1, currentSquare.Col + 2));
            
            availableMoves.Add(Square.At(currentSquare.Row + 1, currentSquare.Col - 2));
            availableMoves.Add(Square.At(currentSquare.Row - 1, currentSquare.Col - 2));

            return availableMoves;
        }
    }
}