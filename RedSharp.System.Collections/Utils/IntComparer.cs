using RedSharp.Sys.Collections.Abstracts;

namespace RedSharp.Sys.Collections.Utils
{
    public class IntComparer : ComparerBase<int>
    {
        public override int Compare(int first, int second)
        {
            if (first == second)
                return Equal;
            else if (first > second)
                return Greater;
            else
                return Less;
        }
    }
}
