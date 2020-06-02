namespace SnakeBattle.Api
{
    public class SnakeTester
    {
        //public static bool IsAngryEnemy(Board board, Point partEnemy)
        //{
        //    if (board.IsAt(partEnemy, Element.EnemyHeadLeft) || board.IsAt(partEnemy, Element.EnemyHeadRight) || board.IsAt(partEnemy, Element.EnemyHeadUp) || board.IsAt(partEnemy, Element.EnemyHeadDown))
        //        return false;

        //    Element imagePartEnemy = board.GetElementAt(partEnemy);

        //    if (imagePartEnemy == Element.EnemyBodyHorizontal)
        //        return CheckShiftOnTheBody(board, partEnemy, Direction.Left) || CheckShiftOnTheBody(board, partEnemy, Direction.Right);

        //    if (imagePartEnemy == Element.EnemyBodyVertical)
        //        return CheckShiftOnTheBody(board, partEnemy, Direction.Up) || CheckShiftOnTheBody(board, partEnemy, Direction.Down);

        //    if (imagePartEnemy == Element.EnemyBodyRightUp)
        //        return CheckShiftOnTheBody(board, partEnemy, Direction.Up) || CheckShiftOnTheBody(board, partEnemy, Direction.Right);

        //    if (imagePartEnemy == Element.EnemyBodyRightDown)
        //        return CheckShiftOnTheBody(board, partEnemy, Direction.Down) || CheckShiftOnTheBody(board, partEnemy, Direction.Right);

        //    if (imagePartEnemy == Element.EnemyBodyLeftUp)
        //        return CheckShiftOnTheBody(board, partEnemy, Direction.Up) || CheckShiftOnTheBody(board, partEnemy, Direction.Left);

        //    if (imagePartEnemy == Element.EnemyBodyLeftDown)
        //        return CheckShiftOnTheBody(board, partEnemy, Direction.Down) || CheckShiftOnTheBody(board, partEnemy, Direction.Left);

        //    return false; /////
        //}

        //private static bool CheckShiftOnTheBody(Board board, Point partEnemy, Direction directionToCheck)
        //{
        //    if (board.IsAt(partEnemy.ShiftLeft(), Element.EnemyHeadEvil))
        //        return true;

        //    //if (board.IsAt(partEnemy.ShiftLeft(), Element.EnemyTailEndDown) || board.IsAt(partEnemy.ShiftLeft(), Element.EnemyTailEndLeft) || //хвосты
        //    //    board.IsAt(partEnemy.ShiftLeft(), Element.EnemyTailEndRight) || board.IsAt(partEnemy.ShiftLeft(), Element.EnemyTailEndUp) ||
        //    //    board.IsAt(partEnemy.ShiftLeft(), Element.EnemyTailInactive) || board.IsAt(partEnemy.ShiftLeft(), Element.EnemyHeadDead) ||
        //    //    board.IsAt(partEnemy.ShiftLeft(), Element.EnemyHeadDown) || board.IsAt(partEnemy.ShiftLeft(), Element.EnemyHeadFly) ||  //головы чужой змеи
        //    //    board.IsAt(partEnemy.ShiftLeft(), Element.EnemyHeadLeft) || board.IsAt(partEnemy.ShiftLeft(), Element.EnemyHeadRight) ||
        //    //    board.IsAt(partEnemy.ShiftLeft(), Element.EnemyHeadSleep) || board.IsAt(partEnemy.ShiftLeft(), Element.EnemyHeadUp))
        //    //    return false;

        //    if (directionToCheck == Direction.Left)
        //    {
        //        if (board.IsAt(partEnemy.ShiftLeft(), Element.EnemyBodyHorizontal))
        //            return CheckShiftOnTheBody(board, partEnemy.ShiftLeft(), Direction.Left);

        //        if (board.IsAt(partEnemy.ShiftLeft(), Element.EnemyBodyRightDown))
        //            return CheckShiftOnTheBody(board, partEnemy.ShiftLeft(), Direction.Down);

        //        if (board.IsAt(partEnemy.ShiftLeft(), Element.EnemyBodyRightUp))
        //            return CheckShiftOnTheBody(board, partEnemy.ShiftLeft(), Direction.Up);
        //    }

        //    if (directionToCheck == Direction.Right)
        //    {
        //        if (board.IsAt(partEnemy.ShiftRight(), Element.EnemyBodyHorizontal))
        //            return CheckShiftOnTheBody(board, partEnemy.ShiftRight(), Direction.Right);

        //        if (board.IsAt(partEnemy.ShiftRight(), Element.EnemyBodyLeftUp))
        //            return CheckShiftOnTheBody(board, partEnemy.ShiftRight(), Direction.Up);

        //        if (board.IsAt(partEnemy.ShiftRight(), Element.EnemyBodyLeftDown))
        //            return CheckShiftOnTheBody(board, partEnemy.ShiftRight(), Direction.Down);
        //    }

        //    if (directionToCheck == Direction.Up)
        //    {
        //        if (board.IsAt(partEnemy.ShiftTop(), Element.EnemyBodyVertical))
        //            return CheckShiftOnTheBody(board, partEnemy.ShiftTop(), Direction.Up);

        //        if (board.IsAt(partEnemy.ShiftTop(), Element.EnemyBodyRightDown))
        //            return CheckShiftOnTheBody(board, partEnemy.ShiftTop(), Direction.Right);

        //        if (board.IsAt(partEnemy.ShiftTop(), Element.EnemyBodyLeftDown))
        //            return CheckShiftOnTheBody(board, partEnemy.ShiftTop(), Direction.Left);
        //    }

        //    if (directionToCheck == Direction.Down)
        //    {
        //        if (board.IsAt(partEnemy.ShiftBottom(), Element.EnemyBodyVertical))
        //            return CheckShiftOnTheBody(board, partEnemy.ShiftBottom(), Direction.Down);

        //        if (board.IsAt(partEnemy.ShiftBottom(), Element.EnemyBodyRightUp))
        //            return CheckShiftOnTheBody(board, partEnemy.ShiftBottom(), Direction.Right);

        //        if (board.IsAt(partEnemy.ShiftBottom(), Element.EnemyBodyLeftUp))
        //            return CheckShiftOnTheBody(board, partEnemy.ShiftBottom(), Direction.Left);
        //    }

        //    return false;///////
        //}
    }
}
