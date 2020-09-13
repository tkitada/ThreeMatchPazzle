using Microsoft.VisualBasic;
using System.Timers;
using ThreeMatchPazzle.Model.Domain;
using static ThreeMatchPazzle.Model.Domain.Const;

namespace ThreeMatchPazzle.Model.ApplicationService
{
    public class ThreeMatchPazzleAppService
    {
        private readonly Field field_ = new Field();
        private readonly Cursor cursor_ = new Cursor(Width, Height);

        private readonly Timer proccessTimer_ = new Timer(Wait);

        public ThreeMatchPazzleAppService()
        {
            proccessTimer_.Elapsed += (_, _) =>
            {
                proccessTimer_.Stop();
                field_.Process();
            };
        }

        public (int[,] field, int selectedX, int selectedY, bool isSelected, int cursorX, int cursorY, bool isProccessing) Update()
        {
            return (field_.Layout, field_.SelectedX, field_.SelectedY, field_.IsSelected, cursor_.X, cursor_.Y, field_.IsProcessing);
        }

        public void DeleteMatchStones()
        {
            field_.DeleteMatchStones();
        }

        public void FillEmpty()
        {
            field_.FillEmpty();
        }

        public void MoveCursor(Direction dir)
        {
            cursor_.Move(dir);
        }

        public void Select()
        {
            field_.Select(cursor_.X, cursor_.Y);
        }

        public void Process()
        {
            if (!field_.IsProcessing) return;
            proccessTimer_.Start();
        }
    }
}