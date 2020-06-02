using System;
using System.Collections.Generic;
using System.Linq;

namespace SnakeBattle.Api
{
    public class PathFinder
    {
        public static List<Point> GetPath(Board board, Point targetPosinion, Point startPosition, Element[] checkMyself)
        {
            List<Point> PathToTarget = new List<Point>();
            List<Node> CheckedNodes = new List<Node>();
            List<Node> WaitingNodes = new List<Node>();

            if (startPosition == targetPosinion) return PathToTarget;

            Node startNode = new Node(0, startPosition, targetPosinion, null);
            CheckedNodes.Add(startNode);
            WaitingNodes.AddRange(GetNeighbourNodes(startNode));

            while (WaitingNodes.Count > 0)
            {
                Node nodeToCheck = WaitingNodes.Where(n => n.F == WaitingNodes.Min(y => y.F)).FirstOrDefault();

                if (nodeToCheck.Position == targetPosinion)
                    return CalculatePathFromNode(nodeToCheck);

                var walkable = CheckForFreeSpace(board, nodeToCheck, checkMyself) && !WillDieEnemyHead(board, nodeToCheck.Position); //  Element.None можно будет заменить чтобы ходить не толлько по свободным ячейкам

                if (!walkable)
                {
                    WaitingNodes.Remove(nodeToCheck);
                    CheckedNodes.Add(nodeToCheck);
                }
                else if (walkable)
                {
                    WaitingNodes.Remove(nodeToCheck);
                    if (!CheckedNodes.Where(n => n.Position == nodeToCheck.Position).Any())
                    {
                        CheckedNodes.Add(nodeToCheck);
                        WaitingNodes.AddRange(GetNeighbourNodes(nodeToCheck));
                    }
                }
            }

            return PathToTarget;
        }


        private static bool CheckForFreeSpace(Board board, Node nodeToCheck, Element[] snake)
        {
            Element imageNodeToCheck = board.GetElementAt(nodeToCheck.Position);
            var myHead = (Point)board.GetMyHead();
            if (snake != null)
            {
                return board.HasElementAt(nodeToCheck.Position, snake);
                //return imageNodeToCheck == Element.BodyHorizontal || imageNodeToCheck == Element.BodyLeftDown || imageNodeToCheck == Element.BodyLeftUp ||
                //    imageNodeToCheck == Element.BodyRightDown || imageNodeToCheck == Element.BodyRightUp || imageNodeToCheck == Element.BodyVertical;
            }

            if (board.IAmAngry)
            {
                if (IAmNotShort(board, myHead))
                    return !(imageNodeToCheck == Element.Wall || imageNodeToCheck == Element.StartFloor || imageNodeToCheck == Element.Other || //  знак вопроса  
                         imageNodeToCheck == Element.BodyHorizontal || imageNodeToCheck == Element.BodyLeftDown || imageNodeToCheck == Element.BodyLeftUp ||
                         imageNodeToCheck == Element.BodyRightDown || imageNodeToCheck == Element.BodyRightUp || imageNodeToCheck == Element.BodyVertical);
                else
                {
                    return !(imageNodeToCheck == Element.Wall || imageNodeToCheck == Element.StartFloor || imageNodeToCheck == Element.Other || // знак вопроса  
                         imageNodeToCheck == Element.BodyHorizontal || imageNodeToCheck == Element.BodyLeftDown || imageNodeToCheck == Element.BodyLeftUp ||
                         imageNodeToCheck == Element.BodyRightDown || imageNodeToCheck == Element.BodyRightUp || imageNodeToCheck == Element.BodyVertical ||
                         imageNodeToCheck == Element.TailEndDown || imageNodeToCheck == Element.TailEndLeft || // конец моей змейки
                         imageNodeToCheck == Element.TailEndUp || imageNodeToCheck == Element.TailEndRight || // конец моей змейки
                         imageNodeToCheck == Element.TailInactive);
                }
            }

            else
            {
                if (IAmNotShort(board, myHead))
                    return imageNodeToCheck == Element.None ||
                           imageNodeToCheck == Element.TailEndDown || imageNodeToCheck == Element.TailEndLeft || // конец моей змейки
                           imageNodeToCheck == Element.TailEndUp || imageNodeToCheck == Element.TailEndRight || // конец моей змейки
                           imageNodeToCheck == Element.TailInactive ||                                         // конец моей змейки

                           ((imageNodeToCheck == Element.EnemyTailEndDown || imageNodeToCheck == Element.EnemyTailEndLeft ||    // конец вражины
                           imageNodeToCheck == Element.EnemyTailEndUp || imageNodeToCheck == Element.EnemyTailEndRight) && !EnemyWillGrow(board, nodeToCheck)) || // конец вражины

                           imageNodeToCheck == Element.Apple || imageNodeToCheck == Element.FuryPill || imageNodeToCheck == Element.Gold ||
                           (imageNodeToCheck == Element.Stone && board.MyLenght > 6);
                //(board.IAmAngry && imageNodeToCheck == Element.Stone);
                else
                    return imageNodeToCheck == Element.None ||
                           ((imageNodeToCheck == Element.EnemyTailEndDown || imageNodeToCheck == Element.EnemyTailEndLeft ||    // конец вражины
                           imageNodeToCheck == Element.EnemyTailEndUp || imageNodeToCheck == Element.EnemyTailEndRight) && !EnemyWillGrow(board, nodeToCheck)) || // конец вражины
                           imageNodeToCheck == Element.Apple || imageNodeToCheck == Element.FuryPill || imageNodeToCheck == Element.Gold;
                //(board.IAmAngry && imageNodeToCheck == Element.Stone);
            }
        }

        private static bool EnemyWillGrow(Board board, Node nodeToCheck)
        {
            var mostNearHead = board.GetMostNearEnemysHead(nodeToCheck.Position);
            if (mostNearHead == null)
                return false;
            else
                return board.HasElementAt(nodeToCheck.Position, board.GetEnemysTails()) && board.IsNear((Point)mostNearHead, Element.Apple);
        }

        private static bool IAmNotShort(Board board, Point myHead) //проверка на головастика
        {
            return board.IsNear(myHead, Element.BodyHorizontal) || board.IsNear(myHead, Element.BodyLeftDown) || board.IsNear(myHead, Element.BodyLeftUp) || // Если я не головастик
                                board.IsNear(myHead, Element.BodyRightDown) || board.IsNear(myHead, Element.BodyRightUp) || board.IsNear(myHead, Element.BodyVertical);
        }

        private static bool WillDieEnemyHead(Board board, Point newPosition) ////////////////////////////////////!!!!!!!!!!
        {
            if (board.IAmAngry)
            {
                return false;
                //if (board.IsNear(newPosition, imageMyHead) && board.IsNear(newPosition, Element.EnemyHeadEvil))
                //    return true;
                //if (board.IsNear(newPosition, imageMyHead) &&
                //   (board.IsNear(newPosition, Element.EnemyHeadDown) ||
                //    board.IsNear(newPosition, Element.EnemyHeadLeft) ||
                //    board.IsNear(newPosition, Element.EnemyHeadRight) ||
                //    board.IsNear(newPosition, Element.EnemyHeadUp)))
                //    return false;
            }
            else
            {
                Point myHead = (Point)board.GetMyHead();
                Element imageMyHead = board.GetElementAt(myHead);
                Element imageNewPosition = board.GetElementAt(newPosition);
                var mostNearHead = board.GetMostNear(board.GetAllEnemysHeads(), true, newPosition);
                if (mostNearHead == null)
                    return false;

                if (board.IsNear(newPosition, imageMyHead) && board.IsNearEnemyHead(newPosition) &&
                    !IsNotEnemysTail(imageNewPosition) && !board.IsNear((Point)mostNearHead, Element.Apple))
                    return false;

                if (board.IsNear(newPosition, imageMyHead) && board.IsNearEnemyHead(newPosition) && IsNotEnemysTail(imageNewPosition))
                    return true;

                if (board.IsNearEnemyHead(newPosition))
                    return true;
                //if (board.IsNear(newPosition, imageMyHead) &&
                //   (board.IsNear(newPosition, Element.EnemyHeadDown) ||
                //    board.IsNear(newPosition, Element.EnemyHeadLeft) ||
                //    board.IsNear(newPosition, Element.EnemyHeadRight) ||
                //    board.IsNear(newPosition, Element.EnemyHeadUp)))
                //    return true;
            }

            return false;
        }

        private static bool IsNotEnemysTail(Element imageNewPosition)
        {
            return imageNewPosition != Element.EnemyTailEndDown || imageNewPosition != Element.EnemyTailEndLeft || // что бы я мог на хвост чужой змеи наступать
                                imageNewPosition != Element.EnemyTailEndRight || imageNewPosition != Element.EnemyTailEndUp || imageNewPosition != Element.EnemyTailInactive;
        }

        private static List<Point> CalculatePathFromNode(Node node)
        {
            var path = new List<Point>();
            Node currentNode = node;
            while (currentNode.PreviousNode != null)
            {
                path.Add(new Point(currentNode.Position.X, currentNode.Position.Y));
                currentNode = currentNode.PreviousNode;
            }

            return path;
        }

        private static List<Node> GetNeighbourNodes(Node node)
        {
            List<Node> neighbours = new List<Node>();
            neighbours.Add(new Node(node.G + 1, new Point(node.Position.X - 1, node.Position.Y), node.TargetPosition, node));
            neighbours.Add(new Node(node.G + 1, new Point(node.Position.X + 1, node.Position.Y), node.TargetPosition, node));
            neighbours.Add(new Node(node.G + 1, new Point(node.Position.X, node.Position.Y - 1), node.TargetPosition, node));
            neighbours.Add(new Node(node.G + 1, new Point(node.Position.X, node.Position.Y + 1), node.TargetPosition, node));

            return neighbours;
        }
    }
}
