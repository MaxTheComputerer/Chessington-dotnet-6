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
        public int NumberOfMoves { get; set; }
        
        public abstract IEnumerable<Square> GetAvailableMoves(Board board);

        public void MoveTo(Board board, Square newSquare)
        {
            var currentSquare = board.FindPiece(this);
            board.MovePiece(currentSquare, newSquare);
            NumberOfMoves++;
        }
    }
}