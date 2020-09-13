using System.Timers;
using static System.Console;
using static ThreeMatchPazzle.Model.Domain.Const;

namespace ThreeMatchPazzle
{
    public class View
    {
        private static readonly Timer timer_ = new Timer(1000 / 60.0);

        private static readonly ViewModel vm_ = new ViewModel();

        private static void Main()
        {
            CursorVisible = false;

            timer_.Elapsed += (_, e) =>
            {
                timer_.Stop();
                vm_.Update();
                Draw();
                timer_.Start();
            };
            timer_.Start();

            while (true)
            {
                vm_.Input(ReadKey(true));
            };
        }

        private static void Draw()
        {
            SetCursorPosition(0, 0);
            for (var y = 0; y < Height + 1; y++)
            {
                for (var x = 0; x < Width + 1; x++)
                {
                    if (x == Width)
                    {
                        Write(vm_.IsSelected && y == vm_.SelectedY ? "←" : "　");
                        continue;
                    }
                    if (y == Height)
                    {
                        Write(vm_.IsSelected && x == vm_.SelectedX ? "↑" : "　");
                        continue;
                    }
                    if (x == vm_.CursorX && y == vm_.CursorY && !vm_.IsProccessing)
                    {
                        Write("◎");
                        continue;
                    }
                    switch (vm_.Field[x, y])
                    {
                        case 0:
                            Write("・");
                            break;

                        case 1:
                            Write("○");
                            break;

                        case 2:
                            Write("△");
                            break;

                        case 3:
                            Write("□");
                            break;

                        case 4:
                            Write("●");
                            break;

                        case 5:
                            Write("▲");
                            break;

                        case 6:
                            Write("■");
                            break;

                        case 7:
                            Write("☆");
                            break;

                        default:
                            break;
                    }
                }
                WriteLine("");
            }
        }
    }
}