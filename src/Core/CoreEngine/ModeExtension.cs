namespace Core.CoreEngine
{
    public static class ModeExtension
    {
        public static Mode Decrease(this Mode mode)
        {
            var modeNumber = (int)mode;
            var newNumber = modeNumber - 1;
            return (Mode)newNumber;
        }
    }
}
