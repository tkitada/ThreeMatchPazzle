using System;
using ThreeMatchPazzle.Model.ApplicationService;
using ThreeMatchPazzle.Model.Domain;

namespace ThreeMatchPazzle
{
    internal class ViewModel
    {
        public int[,] Field { get; private set; }
        public int SelectedX { get; private set; }
        public int SelectedY { get; private set; }
        public bool IsSelected { get; private set; }
        public int CursorX { get; private set; }
        public int CursorY { get; private set; }
        public bool IsProccessing { get; private set; }

        private readonly ThreeMatchPazzleAppService appService_ = new ThreeMatchPazzleAppService();

        public void Update()
        {
            appService_.Process();
            (Field, SelectedX, SelectedY, IsSelected, CursorX, CursorY, IsProccessing) = appService_.Update();
        }

        public void Input(ConsoleKeyInfo keyInfo)
        {
            switch (keyInfo.Key)
            {
                case ConsoleKey.LeftArrow:
                case ConsoleKey.A:
                    appService_.MoveCursor(Direction.Left);
                    break;

                case ConsoleKey.UpArrow:
                case ConsoleKey.W:
                    appService_.MoveCursor(Direction.Up);
                    break;

                case ConsoleKey.RightArrow:
                case ConsoleKey.D:
                    appService_.MoveCursor(Direction.Right);
                    break;

                case ConsoleKey.DownArrow:
                case ConsoleKey.S:
                    appService_.MoveCursor(Direction.Down);
                    break;

                case ConsoleKey.Enter:
                    appService_.Select();
                    break;

                default:
                    break;
            }
        }
    }
}