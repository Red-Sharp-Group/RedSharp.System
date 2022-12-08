using RedSharp.Sys.Collections.Abstracts;

namespace RedSharp.Sys.Collections.Utils
{
    public class IntComparer : ComparerBase<int>
    {
        public IntComparer(bool isAscending = true) : base(isAscending)
        { }

        protected override int InternalCompare(int first, int second)
        {
            return first - second;
        }
    }
}
