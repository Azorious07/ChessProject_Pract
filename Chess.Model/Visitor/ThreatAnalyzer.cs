namespace Chess.Model.Visitor
{
    using Chess.Model.Data;
    using Chess.Model.Game;
    using Chess.Model.Piece;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ThreatAnalyzer : IChessPieceVisitor<Func<Board, Position, IEnumerable<Position>>>
    {
        public IEnumerable<Position> GetThreats(Board board, PlacedPiece piece)
        {
            return piece.Accept(this)(board, piece.Position);
        }

        public Func<Board, Position, IEnumerable<Position>> Visit(Bishop bishop)
        {
            return (board, position) =>
            {
                return Iterate(board, position, bishop.Color, Direction.Diagonals);
            };
        }

        public Func<Board, Position, IEnumerable<Position>> Visit(King king)
        {
            return (board, position) =>
            {
                var directions = Direction.Orthogonals.Union(Direction.Diagonals);
                return Iterate(board, position, king.Color, directions, 1);
            };
        }

        public Func<Board, Position, IEnumerable<Position>> Visit(Knight knight)
        {
            return (board, position) =>
            {
                var deltas = new int[] { -1, 1, 2, -2 };
                var directions = from row in deltas
                                 from column in deltas
                                 where row != column && row != -column
                                 select new Direction(row, column);
                return Iterate(board, position, knight.Color, directions, 1);
            };
        }

        public Func<Board, Position, IEnumerable<Position>> Visit(Pawn pawn)
        {
            return (board, position) =>
            {
                var rowDelta = pawn.Color == Color.White ? 1 : -1;
                var directions = Direction.Diagonals.Where(p => p.RowDelta == rowDelta);
                return Iterate(board, position, pawn.Color, directions, 1);
            };
        }

        public Func<Board, Position, IEnumerable<Position>> Visit(Queen queen)
        {
            return (board, position) =>
            {
                var directions = Direction.Orthogonals.Union(Direction.Diagonals);
                return Iterate(board, position, queen.Color, directions);
            };
        }

        public Func<Board, Position, IEnumerable<Position>> Visit(Rook rook)
        {
            return (board, position) =>
            {
                return Iterate(board, position, rook.Color, Direction.Orthogonals);
            };
        }

        private static IEnumerable<Position> Iterate(Board board, Position position, Color color, IEnumerable<Direction> directions)
        {
            return Iterate(board, position, color, directions, int.MaxValue);
        }

        private static IEnumerable<Position> Iterate(Board board, Position position, Color color, IEnumerable<Direction> directions, int maxSteps)
        {
            var friends = board.GetPieces(color);
            var enemies = board.GetPieces(color.Toggle());

            bool isFriend(Position p) => friends.Any(f => f.Position.Equals(p));
            bool isEnemy(Position p) => enemies.Any(f => f.Position.Equals(p));

            foreach (var dir in directions)
            {
                var start = position.Offset(dir);
                var positionStream = start.Repeat(p => p.Bind(m => m.Offset(dir)));
                var inBoundsPositions = positionStream.TakeWhile(p => p.HasValue);
                var validPositions = inBoundsPositions.FilterMaybes().Take(maxSteps);

                foreach (var pos in validPositions)
                {
                    if (isFriend(pos))
                    {
                        break;
                    }

                    yield return pos;

                    if (isEnemy(pos))
                    {
                        break;
                    }
                }
            }
        }
    }
}