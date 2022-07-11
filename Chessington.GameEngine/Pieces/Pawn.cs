using System;
using System.Collections.Generic;

namespace Chessington.GameEngine.Pieces
{
    public class Pawn : Piece
    {
        public bool IsVulnerableToEnPassant { get; set; }
        
        public Pawn(Player player) 
            : base(player) { }

        private bool CanPerformEnPassant(Board board, Square currentSquare, int direction)
        {
            if (Player == Player.White && currentSquare.Row == 3 || Player == Player.Black && currentSquare.Row == 4)
            {
                var candidateSquare = Square.At(currentSquare.Row, currentSquare.Col + direction);

                if (board.IsObstructed(candidateSquare) && CanCaptureAtSquare(board, candidateSquare))
                {
                    var victim = board.GetPiece(candidateSquare);
                    if (victim is Pawn { IsVulnerableToEnPassant: true })
                    {
                        return true;
                    }
                }
            }

            return false;
        }

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

            // En passant rule
            if (CanPerformEnPassant(board, currentSquare, 1))
            {
                availableMoves.Add(rightDiagonal);
            }
            if (CanPerformEnPassant(board, currentSquare, -1))
            {
                availableMoves.Add(leftDiagonal);
            }
            
            return availableMoves;
        }

        public override void MoveTo(Board board, Square newSquare)
        {
            var currentSquare = board.FindPiece(this);
            IsVulnerableToEnPassant = Math.Abs(newSquare.Row - currentSquare.Row) == 2;
            base.MoveTo(board, newSquare);
        }
    }
}