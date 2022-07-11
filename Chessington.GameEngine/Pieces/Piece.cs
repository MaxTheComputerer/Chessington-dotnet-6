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
        
        private static IEnumerable<Square> GetDiagonalMovesInOneDirection(Board board, Square currentSquare, int rowDirection, int colDirection,
            Func<int, bool> rowBoundCondition, Func<int, bool> colBoundCondition)
        {
            // Moves in the specified diagonal direction, adding available moves until an obstruction is found
            // Bound condition functions specify the upper/lower bound of the board to check for, depending on direction
            
            var availableMoves = new List<Square>();
            
            for (var i = 1; i < 8; i++)
            {
                var candidateSquare = Square.At(currentSquare.Row + (rowDirection * i), currentSquare.Col + (colDirection * i));
                if (rowBoundCondition(candidateSquare.Row) || colBoundCondition(candidateSquare.Col) || board.IsOccupied(candidateSquare))
                {
                    break;
                }
                availableMoves.Add(candidateSquare);
            }

            return availableMoves;
        }

        protected static IEnumerable<Square> GetDiagonalMoves(Board board, Square currentSquare)
        {
            var availableMoves = new List<Square>();

            bool UpperBoundCondition(int coord) => coord >= 8;
            bool LowerBoundCondition(int coord) => coord < 0;

            // Down and right
            availableMoves.AddRange(GetDiagonalMovesInOneDirection(board, currentSquare, 1, 1, UpperBoundCondition, UpperBoundCondition));

            // Up and left
            availableMoves.AddRange(GetDiagonalMovesInOneDirection(board, currentSquare, -1, -1, LowerBoundCondition, LowerBoundCondition));

            // Down and right
            availableMoves.AddRange(GetDiagonalMovesInOneDirection(board, currentSquare, 1, -1, UpperBoundCondition, LowerBoundCondition));

            // Down and left
            availableMoves.AddRange(GetDiagonalMovesInOneDirection(board, currentSquare, -1, 1, LowerBoundCondition, UpperBoundCondition));

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