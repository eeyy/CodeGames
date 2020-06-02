using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static SnakeBattle.Api.Element;

namespace SnakeBattle.Api
{
    public class Board
    {
        public Board(string boardString)
        {
            Console.OutputEncoding = Encoding.GetEncoding(866);/////////////

            BoardString = boardString.Replace("\n", "");
            Size = (int)Math.Sqrt(BoardString.Length);
            Square = BoardString.Length;
            IAmAngry = AmIAngry();
            IAmDied = AmIDied();
            MyLenght = GetMyLenght();
        }


        /// <summary>
        /// Строка, представляющая собой поле.
        /// </summary>
        public string BoardString { get; private set; }

        /// <summary>
        /// Размер поля.
        /// </summary>
        public int Size { get; private set; }


        /// <summary>
        /// Площадь поля.
        /// </summary>
        public int Square { get; private set; }

        /// <summary>
        /// Злая ли моя змейка.
        /// </summary>
        public bool IAmAngry { get; private set; }

        /// <summary>
        /// Жива ли моя змейка
        /// </summary>
        public bool IAmDied { get; private set; }

        /// <summary>
        /// Моя длина
        /// </summary>
        public int MyLenght { get; private set; }

        public List<Point> GetWalls()
        {
            return FindAllElements(Wall);
        }

        public List<Point> GetStones()
        {
            return FindAllElements(Stone);
        }

        public bool IsBarrierAt(Point point)
        {
            return GetBarriers().Contains(point);
        }

        public List<Point> GetApples()
        {
            return FindAllElements(Apple);
        }

        public bool AmIEvil()
        {
            return FindAllElements(HeadEvil).Any(e => e == GetMyHead());
        }

        public bool AmIFlying()
        {
            return FindAllElements(HeadFly).Any(e => e == GetMyHead());
        }

        public List<Point> GetFlyingPills()
        {
            return FindAllElements(FlyingPill);
        }

        public List<Point> GetFuryPills()
        {
            return FindAllElements(FuryPill);
        }

        public List<Point> GetGold()
        {
            return FindAllElements(Gold);
        }

        /// <summary>
        /// Получает список координат стартовых точек.
        /// </summary>
        public List<Point> GetStartPoints()
        {
            return FindAllElements(StartFloor);
        }

        private List<Point> GetBarriers()
        {
            return FindAllElements(Wall, StartFloor, EnemyHeadSleep, EnemyTailInactive, TailInactive, Stone);
        }

        /// <summary>
        /// Проверяет имеется ли передаваемый тип элемента в соответствующих координатах.
        /// </summary>
        public bool IsAt(Point point, Element element)
        {
            //    if (point.IsOutOfBoard(Size))
            //    {
            //        return false;
            //    }

            return GetElementAt(point) == element;
        }

        public Element GetElementAt(Point point)
        {
            return (Element)BoardString[GetShiftByPoint(point)];
        }

        public void PrintBoard()
        {
            for (int i = 0; i < Size; i++)
            {
                //Console.OutputEncoding = Encoding.GetEncoding(866);
                Console.OutputEncoding = Encoding.UTF8;
                Console.WriteLine(BoardString.Substring(i * Size, Size));
            }
        }

        public Point? FindElement(Element elementType)
        {
            for (int i = 0; i < Square; i++)
            {
                var point = GetPointByShift(i);
                if (IsAt(point, elementType))
                {
                    return point;
                }
            }
            return null;
        }

        /// <summary>
        /// Находит первый попавшийся из перечисления элемент в строке карты <see cref="BoardString"/>
        /// </summary>
        public Point? FindFirstElement(params Element[] elements)
        {
            for (int i = 0; i < Square; i++)
            {
                var point = GetPointByShift(i);

                foreach (var element in elements)
                {
                    if (IsAt(point, element))
                    {
                        return point;
                    }
                }
            }
            return null;
        }

        public List<Point> FindAllElements(params Element[] elements)
        {
            var result = new List<Point>();

            for (int i = 0; i < Square; i++)
            {
                var point = GetPointByShift(i);

                foreach (var element in elements)
                {
                    if (IsAt(point, element))
                    {
                        result.Add(point);
                    }
                }
            }

            return result;
        }

        public bool HasElementAt(Point point, params Element[] elements)
        {
            return elements.Any(e => IsAt(point, e));
        }

        private int GetShiftByPoint(Point point)
        {
            return point.Y * Size + point.X;
        }

        private Point GetPointByShift(int shift)
        {
            return new Point(shift % Size, shift / Size);
        }

        public Point? GetMyHead()
        {
            return FindFirstElement(HeadDead, HeadDown, HeadUp, HeadLeft, HeadRight, HeadEvil, HeadSleep);
        }

        public Point? GetMyTail()
        {
            return FindFirstElement(TailEndDown, TailEndLeft, TailEndRight, TailEndUp, TailInactive);
        }

        public Point? GetMostNearEnemysTail(Point startPosition)
        {
            return GetMostNear(GetEnemysTails(), true, startPosition);
        }

        public Point? GetMostNearEnemysHead(Point startPosition)
        {
            return GetMostNear(GetAllEnemysHeads(), true, startPosition);
        }
        //public Point GetMostNearTarget(Element[] elementsToFind, out List<Point> pathToPartEnemy)
        //{
        //    //List<Point> findedElements = new List<Point>();
        //    ////
        //    //Point myHead = (Point)GetMyHead();
        //    ////
        //    //foreach (Element element in elementsToFind)
        //    //{
        //    //    findedElements.AddRange(FindAllElements(element));
        //    //}

        //    //if (findedElements.Count == 0)
        //    //    return myHead.ShiftRight();

        //    //Dictionary<Point, int> distancesAndElements = new Dictionary<Point, int>();
        //    //foreach (var item in findedElements)
        //    //{
        //    //    int distance = Math.Abs(myHead.X - item.X) + Math.Abs(myHead.Y - item.Y); //GetMyHead может быть null
        //    //    if (!IsDeadlock(item))
        //    //        distancesAndElements.Add(item, distance);
        //    //}

        //    pathToPartEnemy = null;

        //    Point elementPoint = FindTheBestElement(elementsToFind, out pathToPartEnemy);


        //    elementPoint = distancesAndElements.First(x => x.Value == distancesAndElements.Values.Min()).Key;
        //    return elementPoint;
        //}

        public Point? GetMostNearTarget(Element[] elementsToFind, out List<Point> pathToPartEnemy, int countTicWithAngry)
        {
            pathToPartEnemy = null;
            if (countTicWithAngry > 3 && CanCatchEnemy(out pathToPartEnemy)) // я могу поймать змейку чужую
                return new Point(4, 4); //любое значение, потому что не имеет значения

            var target = GetMostNear(elementsToFind, false, (Point)GetMyHead());
            return target;
        }


        private bool CanCatchEnemy(out List<Point> path)
        {
            path = null;

            Point myHead = (Point)GetMyHead();
            var partEnemy = GetMostNear(GetNotAngryEnemy(), true, myHead);
            if (partEnemy == null)
                return false;

            int distanceToPartEnemy = GetDistance(myHead, (Point)partEnemy);

            if (distanceToPartEnemy > 8)
                return false;

            List<Point> pathToPartEnemy = PathFinder.GetPath(this, (Point)partEnemy, myHead, null); ///ивозможно стоит удалить
            if (pathToPartEnemy.Count <= 8)
            {
                path = pathToPartEnemy;
                return true;
            }

            return false;
        }

        private static int GetDistance(Point a, Point b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }

        //private bool IsAngryEnemy(Point partEnemy)
        //{
        //    if (IsAt(partEnemy, EnemyHeadLeft) || IsAt(partEnemy, EnemyHeadRight) || IsAt(partEnemy, EnemyHeadUp) || IsAt(partEnemy, EnemyHeadDown))
        //        return false;

        //    Element imagePartEnemy = GetElementAt(partEnemy);
        //    Element neighbourElement;
        //    while (neighbourElement != )
        //    if (imagePartEnemy == Element.BodyHorizontal)
        //        GetNotAngryEnemyf
        //}

        public static Element[] GetNotAngryEnemy()
        {
            return new Element[] {EnemyHeadDown, EnemyHeadLeft, EnemyHeadRight, EnemyHeadUp, EnemyBodyHorizontal,
                                  EnemyBodyVertical, EnemyBodyLeftDown, EnemyBodyLeftUp, EnemyBodyRightDown, EnemyBodyRightUp};
        }

        public Element[] GetMySnake()
        {
            return new Element[] { BodyHorizontal, BodyLeftDown, BodyLeftUp, BodyRightDown, BodyRightUp, BodyVertical };
        }

        public Element[] GetAllEnemysHeads()
        {
            return new Element[] { EnemyHeadDown, EnemyHeadLeft, EnemyHeadRight, EnemyHeadUp, EnemyHeadEvil };
        }

        public Element[] GetEnemysTails()
        {
            return new Element[] { EnemyTailEndDown, EnemyTailEndLeft, EnemyTailEndRight, EnemyTailEndUp, EnemyTailInactive };
        }


        public Point? GetMostNear(Element[] elementsToFind, bool searchEnemy, Point startPosition)
        {
            List<Point> findedElements = new List<Point>();

            foreach (Element element in elementsToFind)
            {
                findedElements.AddRange(FindAllElements(element));
            }

            if (findedElements.Count == 0)
                return null;

            Dictionary<Point, int> distancesAndElements = new Dictionary<Point, int>();
            foreach (var item in findedElements)
            {
                int distance = GetDistance(startPosition, item);
                if (searchEnemy || !IsDeadlock(item)) // враг может в клубке быть или ещё чего
                    distancesAndElements.Add(item, distance);
            }
            if (distancesAndElements.Count == 0)
                return null;

            return distancesAndElements.First(x => x.Value == distancesAndElements.Values.Min()).Key;
        }

        /// <summary>
        /// Проверка на тупик.
        /// </summary>
        private bool IsDeadlock(Point point)
        {
            Point myHead = (Point)GetMyHead();
            Element imageMyHead = GetElementAt(myHead);
            Point? mostNearEnemysHead = GetMostNearEnemysHead(myHead);
            Point? mostNearEnemysTail = GetMostNearEnemysTail(myHead);

            if (mostNearEnemysHead == null || mostNearEnemysTail == null)
                return true;

            //var EnemysLenght = PathFinder.GetPath(this, mostNearEnemysHead, mostNearEnemysTail, GetNotAngryEnemy()).Count;
            var EnemysLenght = GetDistance((Point)mostNearEnemysHead, (Point)mostNearEnemysTail);
            if (IsNearEnemyHead(point) && GetDistance(myHead, point) > 1 && (MyLenght - EnemysLenght) < 2)
                return true;

            if (CountNear(point, Element.Apple) > 0 && CountNear(point, Element.None) == 0) ////// ошибка если два яблока в дырке 
                return false;


            if (CountNear(point, imageMyHead) > 0 && CountNear(point, Element.None) == 0) // рядом с моей головой и нет свободных клеток
                return true;
            if (CountNear(point, imageMyHead) > 0 && CountNear(point, Element.None) > 0) // рядом с моей головой и есть свободне клетки
                return false;


            if (CountNear(point, imageMyHead) == 0 && CountNear(point, Element.None) < 2) // было почему-то 3 а не 2
                return true;

            return false;
        }

        public bool IsNearEnemyHead(Point point)
        {
            return IsNear(point, EnemyHeadDown) || IsNear(point, EnemyHeadEvil) || IsNear(point, EnemyHeadLeft) ||
                   IsNear(point, EnemyHeadRight) || IsNear(point, EnemyHeadUp);
        }

        /// <summary>
        /// Посчитать количество находящихся рядом с определённой точкой элементов определённого типа.
        /// Проверяются только клетки непосредственно сверху, снизу, справа и слева, то есть находящиеся в соприкосновении по границам.
        /// </summary>
        /// <param name="point">Точка, рядом с которой необходимо посчитать количество элементов.</param>
        /// <param name="element">Тип искомого элемента.</param>
        /// <returns></returns>
        public int CountNear(Point point, Element element)
        {
            if (point.IsOutOfBoard(Size))
                return 0;

            int count = 0;
            if (IsAt(point.ShiftLeft(), element)) count++;
            if (IsAt(point.ShiftRight(), element)) count++;
            if (IsAt(point.ShiftTop(), element)) count++;
            if (IsAt(point.ShiftBottom(), element)) count++;

            return count;
        }

        private bool AmIAngry()
        {
            Point? myHead = GetMyHead();
            if (myHead == null)
                return false;
            if (GetElementAt((Point)myHead) == HeadEvil)
                return true;

            return false;
        }

        private bool AmIDied()
        {
            Point? myHead = GetMyHead();
            if (myHead == null)
                return false;
            if (GetElementAt((Point)myHead) == HeadDead)
                return true;

            return false;
        }

        private int GetMyLenght()
        {
            var myTail = GetMyTail();
            var myHead = GetMyHead();
            if (myTail != null && myHead != null)
                return PathFinder.GetPath(this, (Point)GetMyTail(), (Point)GetMyHead(), GetMySnake()).Count;
            else
                return 2;
        }

        public bool IsNear(Point position, Element element)
        {
            return CountNear(position, element) > 0;
        }
    }
}
