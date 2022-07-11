using System.Collections.Generic;

namespace Chessington.GameEngine.Pieces
{
    public abstract class Piece
    {
        protected Piece(Player player)
        {
            Player = player;
            NumberOfMoves = 0;
        }

        public Player Player { get; private set; }
        protected int NumberOfMoves { get; private set; }
        
        public abstract IEnumerable<Square> GetAvailableMoves(Board board);

        protected static IEnumerable<Square> GetLateralMoves(Square currentSquare)
        {
            var availableMoves = new List<Square>();

            for (var i = 0; i < 8; i++)
            {
                availableMoves.Add(Square.At(currentSquare.Row, i));
                availableMoves.Add(Square.At(i, currentSquare.Col));
            }

            //Get rid of our starting location.
            availableMoves.RemoveAll(s => s == Square.At(currentSquare.Row, currentSquare.Col));

            return availableMoves;
        }

        protected static IEnumerable<Square> GetDiagonalMoves(Square currentSquare)
        {
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

        public void MoveTo(Board board, Square newSquare)
        {
            var currentSquare = board.FindPiece(this);
            board.MovePiece(currentSquare, newSquare);
            NumberOfMoves++;
        }
    }
}