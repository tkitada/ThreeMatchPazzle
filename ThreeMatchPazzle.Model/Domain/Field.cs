using System;
using static ThreeMatchPazzle.Model.Domain.Const;

namespace ThreeMatchPazzle.Model.Domain
{
    internal class Field
    {
        public int[,] Layout { get; } = new int[Width, Height];
        public int SelectedX { get; private set; }
        public int SelectedY { get; private set; }
        public bool IsSelected { get; private set; } = false;
        public bool IsProcessing { get; private set; } = false;
        public ProcessStateType State { get; private set; } = ProcessStateType.None;

        private readonly Random rand_ = new Random();

        public Field()
        {
            do
            {
                for (var y = 0; y < Width; y++)
                {
                    for (var x = 0; x < Height; x++)
                    {
                        if (Layout[x, y] == 0)
                        {
                            Layout[x, y] = rand_.Next(1, StoneTypeNum);
                        }
                    }
                }
            } while (DeleteMatchStones());
        }

        /// <summary>
        /// フィールドの座標を選択する<br/>
        /// 選択状態時: すでに選択されている座標の石と選択した座標の石を入れ替え、選択状態を解除する<br/>
        /// 非選択状態時: 座標を選択し、選択状態にする
        /// </summary>
        /// <param name="x">選択したいx座標</param>
        /// <param name="y">選択したいy座標</param>
        public void Select(int x, int y)
        {
            if (IsProcessing) return;
            if (IsSelected)
            {
                (Layout[SelectedX, SelectedY], Layout[x, y]) = (Layout[x, y], Layout[SelectedX, SelectedY]);
                IsSelected = false;
                StartProcess();
            }
            else
            {
                SelectedX = x;
                SelectedY = y;
                IsSelected = true;
            }
        }

        /// <summary>
        /// MatchCount以上つながっている石を消去する
        /// </summary>
        /// <returns>石を削除したか否か</returns>
        public bool DeleteMatchStones()
        {
            var IsDeletable = false;
            var countSheet = new int[Width, Height];
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    countSheet[x, y] = CountConnectedStones(x, y);
                }
            }
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    if (countSheet[x, y] >= MatchCount)
                    {
                        IsDeletable = true;
                        Layout[x, y] = 0;
                    }
                }
            }
            return IsDeletable;
        }

        /// <summary>
        /// 石を補填する(1段分石が上から降ってくる)
        /// </summary>
        /// <returns>落下処理が完了したか否か</returns>
        public bool FillEmpty()
        {
            var completed = true;
            for (var y = Height - 1; y >= 0; y--)
            {
                for (var x = 0; x < Width; x++)
                {
                    if (Layout[x, y] != 0) continue;
                    completed = false;
                    if (y == 0)
                    {
                        Layout[x, y] = rand_.Next(1, StoneTypeNum);
                    }
                    else
                    {
                        (Layout[x, y], Layout[x, y - 1]) = (Layout[x, y - 1], Layout[x, y]);
                    }
                }
            }
            return completed;
        }

        public void Process()
        {
            if (!IsProcessing) return;
            switch (State)
            {
                case ProcessStateType.Delete:
                    if (DeleteMatchStones())
                    {
                        State = ProcessStateType.Fall;
                    }
                    else
                    {
                        FinishProcess();
                    }
                    break;

                case ProcessStateType.Fall:
                    State = FillEmpty() ? ProcessStateType.Delete : ProcessStateType.Fall;
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// つながっている石(連続で隣接した同色の石)の数を返す(自分自身を含む)
        /// </summary>
        /// <param name="x">確認したい石のx座標</param>
        /// <param name="y">確認したい石のy座標</param>
        /// <returns>接続石数</returns>
        private int CountConnectedStones(int x, int y)
        {
            var checkSheet = new bool[Width, Height];
            var count = 1;
            CountConnectedStoneRecursive(checkSheet, x, y, Layout[x, y], ref count);
            return count;
        }

        private void CountConnectedStoneRecursive(bool[,] checkSheet, int x, int y, int stone, ref int count)
        {
            checkSheet[x, y] = true;
            Check(checkSheet, x - 1, y, stone, ref count);
            Check(checkSheet, x, y - 1, stone, ref count);
            Check(checkSheet, x + 1, y, stone, ref count);
            Check(checkSheet, x, y + 1, stone, ref count);

            void Check(bool[,] checkSheet, int x, int y, int stone, ref int count)
            {
                if (!(0 <= x && x < Width && 0 <= y && y < Height && !checkSheet[x, y])) return;
                checkSheet[x, y] = true;
                if (Layout[x, y] != stone) return;
                count++;
                CountConnectedStoneRecursive(checkSheet, x, y, stone, ref count);
            }
        }

        private void StartProcess()
        {
            IsProcessing = true;
            State = ProcessStateType.Delete;
        }

        private void FinishProcess()
        {
            IsProcessing = false;
            State = ProcessStateType.None;
        }
    }

    internal enum ProcessStateType
    {
        None,
        Delete,
        Fall,
    }
}