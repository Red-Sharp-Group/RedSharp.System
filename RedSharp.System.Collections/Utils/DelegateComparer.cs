using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RedSharp.Sys.Collections.Abstracts;
using RedSharp.Sys.Helpers;

namespace RedSharp.Sys.Collections.Utils
{
    public class DelegateComparer<TItem> : ComparerBase<TItem>
    {
        private Comparison<TItem> _comparer;

        public DelegateComparer(Comparison<TItem> comparer, bool isAscending = true) : base(isAscending)
        {
            ArgumentsGuard.ThrowIfNull(comparer);

            _comparer = comparer;
        }

        protected override int InternalCompare(TItem first, TItem second)
        {
            return _comparer(first, second);
        }
    }
}
