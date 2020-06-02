using System;

namespace SnakeBattle.Api
{
    class Node
    {
        public Point Position;
        public Point TargetPosition;
        public Node PreviousNode;
        public int F;
        public int G;
        public int H;

        public Node(int g, Point nodePosition, Point targetPosition, Node previousNode)
        {
            Position = nodePosition;
            TargetPosition = targetPosition;
            PreviousNode = previousNode;
            G = g;
            H = (int)Math.Abs(targetPosition.X - Position.X) + (int)Math.Abs(targetPosition.Y - Position.Y);
            F = G + H;
        }
    }
}
