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

        protected static IEnumerable<Square> GetLateralMoves(Board board, Square currentSquare)
        {
            var availableMoves = new List<Square>();

            // Right
            for (var col = currentSquare.Col + 1; col < 8; col++)
            {
                var candidateSquare = Square.At(currentSquare.Row, col);
                if (board.IsOccupied(candidateSquare))
                {
                    break;
                }
                availableMoves.Add(candidateSquare);
            }
            
            // Left
            for (var col = currentSquare.Col - 1; col >= 0; col--)
            {
                var candidateSquare = Square.At(currentSquare.Row, col);
                if (board.IsOccupied(candidateSquare))
                {
                    break;
                }
                availableMoves.Add(candidateSquare);
            }
            
            // Down
            for (var row = currentSquare.Row + 1; row < 8; row++)
            {
                var candidateSquare = Square.At(row, currentSquare.Col);
                if (board.IsOccupied(candidateSquare))
                {
                    break;
                }
                availableMoves.Add(candidateSquare);
            }
            
            // Up
            for (var row = currentSquare.Row - 1; row >= 0; row--)
            {
                var candidateSquare = Square.At(row, currentSquare.Col);
                if (board.IsOccupied(candidateSquare))
                {
                    break;
                }
                availableMoves.Add(candidateSquare);
            }

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