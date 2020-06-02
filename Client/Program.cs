using System;
using System.Collections.Generic;
using System.Linq;
using SnakeBattle.Api;

namespace Client
{
    class Program
    {
        const string SERVER_ADDRESS = "http://codebattle-pro-2020s1.westeurope.cloudapp.azure.com/codenjoy-contest/board/player/2hta7y54vrh8yvm6d4fq?code=6148786542746731067&gameName=snakebattle";
        /// <summary>
        /// ////////////////////////////////////////////
        /// </summary>
        static int CountTicWithAngry = 0;
        static int CountTicToTheEndToTheRaund = 300;

        /// <summary>
        /// //////////////////////////
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            var client = new SnakeBattleClient(SERVER_ADDRESS);
            client.Run(DoRun);

            Console.ReadKey();
            client.InitiateExit();
        }


        private static SnakeAction DoRun(Board board)
        {
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            ////////////////
            bool act = GetAct();
            if (board.IAmDied)
            {
                CountTicWithAngry = 0;
                CountTicToTheEndToTheRaund = 300;
                return new SnakeAction(act, Direction.Stop);
            }

            CountTicToTheEndToTheRaund--;
            CountTicWithAngry--;

            if (CountTicWithAngry < 0) { CountTicWithAngry = 0; }

            Point? myHead = board.GetMyHead();
            if(myHead == null)
                return new SnakeAction(act, Direction.Stop);

            Element[] elementsToFind = GetElementsToFind(board);  // кого искать
            Point? mostNearElement = board.GetMostNearTarget(elementsToFind, out List<Point> path, CountTicWithAngry); // куда ходить

            if (mostNearElement == null)
                return MoveToEmpty(board, act, myHead);

            if (path == null)
                path = PathFinder.GetPath(board, (Point)mostNearElement, (Point)myHead, null);  // как ходить


            if (mostNearElement != new Point(4, 4) && board.IsNear((Point)myHead, Element.FuryPill))
                CountTicWithAngry += 10;

            Direction direction = GetDirection(path, (Point)myHead);
            //////////////////
            stopwatch.Stop();
            TimeSpan ts = stopwatch.Elapsed;
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter("Log.txt", true))
            {
                sw.WriteLine($"{DateTime.Now}, Seconds:{ts.Seconds}, Milliseconds:{ts.Milliseconds}, direction:{direction}, mostNearElement:{mostNearElement}, {board.IAmAngry}");
            }

            return new SnakeAction(act, direction);
        }

        private static SnakeAction MoveToEmpty(Board board, bool act, Point? myHead)
        {
            Point? mostNearElement = board.GetMostNear(GetFreeSpace(), false, (Point)myHead);
            if(mostNearElement == null)
                return new SnakeAction(act, Direction.Stop);

            List<Point> path = PathFinder.GetPath(board, (Point)mostNearElement, (Point)myHead, null);  // как ходить
            Direction direction = GetDirection(path, (Point)myHead);
            return new SnakeAction(act, direction);
        }



        private static bool GetAct()
        {
            if (CountTicWithAngry > 2)
                return true;
            else
                return false;
        }

        private static Element[] GetFreeSpace()
        {
            return new Element[] { Element.None };
        }

        private static Element[] GetElementsToFind(Board board)
        {
            Element[] elementsToFind;

            if (board.IAmAngry && CountTicWithAngry > 3)
                elementsToFind = new Element[] { Element.FuryPill, Element.Gold, Element.Stone, Element.Apple };
            else
                elementsToFind = new Element[] { Element.FuryPill, Element.Gold, Element.Apple };

            //if (board.MyLenght > 6)
            //    elementsToFind = new Element[] { Element.FuryPill, Element.Gold, Element.Apple, Element.Stone };

            if (CountTicToTheEndToTheRaund <= CountTicWithAngry)
                elementsToFind = elementsToFind.Where(e => e != Element.FuryPill).ToArray();

            return elementsToFind;
        }

        private static Direction GetDirection(List<Point> path, Point start)
        {
            if (path.Count == 0)
                return Direction.Stop;

            if (path.Last().X > start.X)
                return Direction.Right;
            if (path.Last().X < start.X)
                return Direction.Left;
            if (path.Last().Y < start.Y)
                return Direction.Up;
            if (path.Last().Y > start.Y)
                return Direction.Down;

            return Direction.Stop;
        }

        //private static Direction MakeMove(Board board, Direction dirction)
        //{
        //    countOverflow++;
        //    string action = "";

        //    if (countOverflow > 20)
        //    {
        //        countOverflow = 0;
        //        countForGoast = 0;

        //        return Direction.Left;
        //    }

        //    if (dirction == Direction.RightUp)
        //    {
        //        countForGoast++;
        //        if (UpRightMove(board, out action))
        //        {
        //            if (DoBoom(board))
        //                action = "Act" + action;

        //            return action;
        //        }

        //        return MakeMove(Direction.RightDown, board);
        //    }

        //    if (dirction == Direction.RightDown)
        //    {
        //        countForGoast++;
        //        if (DownRightMove(board, out action))
        //        {
        //            if (DoBoom(board))
        //                action = "Act" + action;

        //            return action;
        //        }

        //        return MakeMove(Direction.LeftUp, board);
        //    }

        //    if (dirction == Direction.LeftUp)
        //    {
        //        countForGoast++;
        //        if (LeftUpMove(board, out action))
        //        {
        //            if (DoBoom(board))
        //                action = "Act" + action;

        //            return action;
        //        }

        //        return MakeMove(Direction.LeftDown, board);
        //    }

        //    if (dirction == Direction.LeftDown)
        //    {
        //        countForGoast++;
        //        if (LeftDownMove(board, out action))
        //        {
        //            if (DoBoom(board))
        //                action = "Act" + action;

        //            return action;
        //        }

        //        return MakeMove(Direction.RightUp, board);
        //    }

        //    return "Left";
        //}


        //private static Direction GetDirectionNear(Board board, Element element)
        //{
        //    Point? head = board.GetMyHead();
        //    Point mostNearElement = board.FindMostNear(element);

        //    if (head.Value.X >= mostNearElement.X && head.Value.Y >= mostNearElement.Y)
        //        return Direction.LeftDown;

        //    if (head.Value.X >= mostNearElement.X && head.Value.Y < mostNearElement.Y)
        //        return Direction.LeftUp;

        //    if (head.Value.X < mostNearElement.X && head.Value.Y >= mostNearElement.Y)
        //        return Direction.RightDown;

        //    if (head.Value.X < mostNearElement.X && head.Value.Y < mostNearElement.Y)
        //        return Direction.RightUp;

        //    return Direction.RightUp;
        //}
    }
}

//Element.EnemyBodyHorizontal, Element.EnemyBodyVertical, Element.EnemyBodyLeftDown, // другая змейка (тело)
//Element.EnemyBodyLeftUp, Element.EnemyBodyRightDown, Element.EnemyBodyRightUp,
//Element.EnemyHeadDown, Element.EnemyHeadLeft, Element.EnemyHeadRight, Element.EnemyHeadUp, // другая змейка (голова)