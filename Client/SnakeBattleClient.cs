using System;
using SnakeBattle.Api;

namespace Client
{
    public class SnakeBattleClient : SnakeBattleBase
    {
        private Func<Board, SnakeAction> _callback;

        public SnakeBattleClient(string serverAddress) : base(serverAddress)
        {
        }

        protected override string DoMove(Board gameBoard)
        {
            //Just print current state (gameBoard) to console
            Console.Clear();
            //Console.SetCursorPosition(0, 0);
            gameBoard.PrintBoard();

            var action = _callback(gameBoard).ToString();
            Console.WriteLine(action);
            return action;
        }

        public void InitiateExit()
        {
            ShouldExit = true;
        }

        public void Run(Func<Board, SnakeAction> callback)
        {
            Connect();
            this._callback = callback;
        }
    }
}
