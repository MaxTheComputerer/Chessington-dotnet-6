﻿using System.Collections.Generic;
using Chessington.GameEngine.Pieces;
using FluentAssertions;
using NUnit.Framework;

namespace Chessington.GameEngine.Tests.Pieces
{
    [TestFixture]
    public class KingTests
    {
        [Test]
        public void KingsCanMoveToAdjacentSquares()
        {
            var board = new Board();
            var king = new King(Player.White);
            board.AddPiece(Square.At(4, 4), king);

            var moves = king.GetAvailableMoves(board);

            var expectedMoves = new List<Square>
            {
                Square.At(3, 3),
                Square.At(3, 4),
                Square.At(3, 5),
                Square.At(4, 3),
                Square.At(4, 5),
                Square.At(5, 3),
                Square.At(5, 4),
                Square.At(5, 5)
            };

            moves.Should().BeEquivalentTo(expectedMoves);
        }

        [Test]
        public void Kings_CannotLeaveTheBoard()
        {
            var board = new Board();
            var king = new King(Player.White);
            board.AddPiece(Square.At(0, 0), king);

            var moves = king.GetAvailableMoves(board);

            var expectedMoves = new List<Square>
            {
                Square.At(1, 0),
                Square.At(1, 1),
                Square.At(0, 1)
            };

            moves.Should().BeEquivalentTo(expectedMoves);
        }

        [Test]
        public void Kings_CanTakeOpposingPieces()
        {
            var board = new Board();
            var king = new King(Player.White);
            board.AddPiece(Square.At(4, 4), king);
            var pawn = new Pawn(Player.Black);
            board.AddPiece(Square.At(4, 5), pawn);

            var moves = king.GetAvailableMoves(board);
            moves.Should().Contain(Square.At(4, 5));
        }

        [Test]
        public void Kings_CannotTakeFriendlyPieces()
        {
            var board = new Board();
            var king = new King(Player.White);
            board.AddPiece(Square.At(4, 4), king);
            var pawn = new Pawn(Player.White);
            board.AddPiece(Square.At(4, 5), pawn);

            var moves = king.GetAvailableMoves(board);
            moves.Should().NotContain(Square.At(4, 5));
        }

        [Test]
        public void Kings_CanDetectWhenInCheck()
        {
            var board = new Board();
            var king = new King(Player.Black);
            board.AddPiece(Square.At(2, 2), king);
            
            var rook = new Rook(Player.White);
            board.AddPiece(Square.At(6, 2), rook);

            king.IsInCheckAtPosition(board, board.FindPiece(king)).Should().BeTrue();
        }
        
        [Test]
        public void Kings_CannotMoveIntoCheck()
        {
            var board = new Board();
            var king = new King(Player.Black);
            board.AddPiece(Square.At(2, 3), king);
            
            var rook = new Rook(Player.White);
            board.AddPiece(Square.At(6, 2), rook);

            var moves = king.GetAvailableMoves(board);

            moves.Should().NotContain(Square.At(2, 2))
                .And.NotContain(Square.At(1, 2))
                .And.NotContain(Square.At(3, 2));
        }

        [Test]
        public void Kings_AreForcedToGetOutOfCheck_WhenInCheck()
        {
            var board = new Board();
            var king = new King(Player.Black);
            board.AddPiece(Square.At(2, 2), king);
            
            var rook = new Rook(Player.White);
            board.AddPiece(Square.At(6, 2), rook);

            var moves = king.GetAvailableMoves(board);

            moves.Should().NotContain(Square.At(3, 2)).And.NotContain(Square.At(5, 2));
        }
    }
}