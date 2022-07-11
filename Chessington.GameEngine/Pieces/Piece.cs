using System;
using System.Collections.Generic;
using System.Linq;

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

        private bool CanCaptureAtSquare(Board board, Square square)
        {
            if (Board.IsOutOfBounds(square))
            {
                return false;
            }

            var piece = board.GetPiece(square);
            return Player != piece.Player;
        }
        
        public abstract IEnumerable<Square> GetAvailableMoves(Board board);

        private IEnumerable<Square> GetLinearMovesInDirection(Board board, Square currentSquare, int rowDirection, int colDirection)
        {
            // Moves in the specified direction, adding available moves until an obstruction is found
            var availableMoves = new List<Square>();
            
            for (var i = 1; i < 8; i++)
            {
                var candidateSquare = Square.At(currentSquare.Row + (rowDirection * i), currentSquare.Col + (colDirection * i));
                if (board.IsObstructed(candidateSquare))
                {
                    // Still add this move if we can take the piece
                    if (CanCaptureAtSquare(board, candidateSquare))
                    {
                        availableMoves.Add(candidateSquare);
                    }
                    break;
                }
                availableMoves.Add(candidateSquare);
            }

            return availableMoves;
        }
        
        protected IEnumerable<Square> GetLateralMoves(Board board, Square currentSquare)
        {
            var availableMoves = new List<Square>();

            // Right
            availableMoves.AddRange(GetLinearMovesInDirection(board, currentSquare, 0, 1));
            
            // Left
            availableMoves.AddRange(GetLinearMovesInDirection(board, currentSquare, 0, -1));
            
            // Down
            availableMoves.AddRange(GetLinearMovesInDirection(board, currentSquare, 1, 0));
            
            // Up
            availableMoves.AddRange(GetLinearMovesInDirection(board, currentSquare, -1, 0));

            return availableMoves;
        }

        protected IEnumerable<Square> GetDiagonalMoves(Board board, Square currentSquare)
        {
            var availableMoves = new List<Square>();
            
            // Down and right
            availableMoves.AddRange(GetLinearMovesInDirection(board, currentSquare, 1, 1));

            // Up and left
            availableMoves.AddRange(GetLinearMovesInDirection(board, currentSquare, -1, -1));

            // Down and right
            availableMoves.AddRange(GetLinearMovesInDirection(board, currentSquare, 1, -1));

            // Down and left
            availableMoves.AddRange(GetLinearMovesInDirection(board, currentSquare, -1, 1));

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