using System;
using System.Collections.Generic;
using Chessington.GameEngine.Pieces;

namespace Chessington.GameEngine
{
    public class Board
    {
        private readonly Piece[,] _board;
        public Player CurrentPlayer { get; private set; }
        public IList<Piece> CapturedPieces { get; private set; } 

        public Board()
            : this(Player.White) { }

        public Board(Player currentPlayer, Piece[,] boardState = null)
        {
            _board = boardState ?? new Piece[GameSettings.BoardSize, GameSettings.BoardSize]; 
            CurrentPlayer = currentPlayer;
            CapturedPieces = new List<Piece>();
        }

        public void AddPiece(Square square, Piece pawn)
        {
            _board[square.Row, square.Col] = pawn;
        }
    
        public Piece GetPiece(Square square)
        {
            return _board[square.Row, square.Col];
        }

        public static bool IsOutOfBounds(Square square)
        {
            return square.Row is < 0 or >= 8 || square.Col is < 0 or >= 8;
        }

        public bool IsObstructed(Square square)
        {
            return IsOutOfBounds(square) || GetPiece(square) != null;
        }
        
        public Square FindPiece(Piece piece)
        {
            for (var row = 0; row < GameSettings.BoardSize; row++)
                for (var col = 0; col < GameSettings.BoardSize; col++)
                    if (_board[row, col] == piece)
                        return Square.At(row, col);

            throw new ArgumentException("The supplied piece is not on the board.", "piece");
        }

        private void TryEnPassant(Square from, Square to)
        {
            var movingPiece = _board[from.Row, from.Col];
            var victim = _board[from.Row, to.Col];
            if (victim != null 
                && movingPiece is Pawn 
                && victim is Pawn { IsVulnerableToEnPassant: true })
            {
                OnPieceCaptured(_board[from.Row, to.Col]);
                _board[from.Row, to.Col] = null;
            }
        }

        private void TryPawnPromotion(Square from, Square to)
        {
            var movingPiece = _board[from.Row, from.Col];
            if (movingPiece is Pawn && (CurrentPlayer == Player.White && to.Row == 0) ||
                (CurrentPlayer == Player.Black && to.Row == 7))
            {
                _board[to.Row, to.Col] = new Queen(CurrentPlayer, movingPiece.NumberOfMoves);
            }
        }

        public void MovePiece(Square from, Square to)
        {
            var movingPiece = _board[from.Row, from.Col];
            if (movingPiece == null) { return; }

            if (movingPiece.Player != CurrentPlayer)
            {
                throw new ArgumentException("The supplied piece does not belong to the current player.");
            }

            //If the space we're moving to is occupied, we need to mark it as captured.
            if (_board[to.Row, to.Col] != null)
            {
                OnPieceCaptured(_board[to.Row, to.Col]);
            }
            
            //Move the piece
            _board[to.Row, to.Col] = _board[from.Row, from.Col];
            
            // Extras
            TryEnPassant(from, to);
            TryPawnPromotion(from, to);
            
            // Set the 'from' square to be empty.
            _board[from.Row, from.Col] = null;

            CurrentPlayer = movingPiece.Player == Player.White ? Player.Black : Player.White;
            OnCurrentPlayerChanged(CurrentPlayer);
        }
        
        public delegate void PieceCapturedEventHandler(Piece piece);
        
        public event PieceCapturedEventHandler PieceCaptured;

        protected virtual void OnPieceCaptured(Piece piece)
        {
            var handler = PieceCaptured;
            handler?.Invoke(piece);
        }

        public delegate void CurrentPlayerChangedEventHandler(Player player);

        public event CurrentPlayerChangedEventHandler CurrentPlayerChanged;

        protected virtual void OnCurrentPlayerChanged(Player player)
        {
            var handler = CurrentPlayerChanged;
            handler?.Invoke(player);
        }
    }
}
