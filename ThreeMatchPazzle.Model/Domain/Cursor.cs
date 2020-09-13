namespace ThreeMatchPazzle.Model.Domain
{
    internal class Cursor
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        private readonly int maxWidth_;
        private readonly int maxHeight_;

        public Cursor(int maxWidth, int maxHeight)
        {
            maxWidth_ = maxWidth;
            maxHeight_ = maxHeight;
        }

        public void Move(Direction dir)
        {
            switch (dir)
            {
                case Direction.Left:
                    if (X > 0) X--;
                    break;

                case Direction.Up:
                    if (Y > 0) Y--;
                    break;

                case Direction.Right:
                    if (X <= maxWidth_) X++;
                    break;

                case Direction.Down:
                    if (Y <= maxHeight_) Y++;
                    break;

                default:
                    break;
            }
        }
    }

    public enum Direction
    {
        Left,
        Up,
        Right,
        Down,
    }
}