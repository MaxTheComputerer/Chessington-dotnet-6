﻿using System.Collections.Generic;

namespace Chessington.GameEngine.Pieces
{
    public class Knight : Piece
    {
        public Knight(Player player)
            : base(player) { }

        public override IEnumerable<Square> GetAvailableMoves(Board board)
        {
            var currentSquare = board.FindPiece(this);
            var availableMoves = new List<Square>
            {
                Square.At(currentSquare.Row + 2, currentSquare.Col + 1),
                Square.At(currentSquare.Row + 2, currentSquare.Col - 1),
                Square.At(currentSquare.Row - 2, currentSquare.Col + 1),
                Square.At(currentSquare.Row - 2, currentSquare.Col - 1),
                Square.At(currentSquare.Row + 1, currentSquare.Col + 2),
                Square.At(currentSquare.Row - 1, currentSquare.Col + 2),
                Square.At(currentSquare.Row + 1, currentSquare.Col - 2),
                Square.At(currentSquare.Row - 1, currentSquare.Col - 2)
            };

            availableMoves.RemoveAll(square => board.IsObstructed(square) && !CanCaptureAtSquare(board, square));
            return availableMoves;
        }
    }
}