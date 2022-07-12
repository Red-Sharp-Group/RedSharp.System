﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RedSharp.Sys.Helpers;

namespace RedSharp.Sys.Utils
{
    public static class FlexibleSets
    {
        public static FlexibleSet<int> CreateArrayIndexing<TItem>(TItem[] array)
        {
            ArgumentsGuard.ThrowIfNull(array);

            return CreateArrayIndexing(0, array.Length);
        }

        public static FlexibleSet<int> CreateArrayIndexing(int right) => CreateArrayIndexing(0, right);

        public static FlexibleSet<int> CreateArrayIndexing(int left, int right, int step = 1) => Create(left, true, right, false, step);

        public static FlexibleSet<int> CreateReverseArrayIndexing<TItem>(TItem[] array)
        {
            ArgumentsGuard.ThrowIfNull(array);

            return CreateReverseArrayIndexing(array.Length);
        }

        public static FlexibleSet<int> CreateReverseArrayIndexing(int left, int right = 0, int step = 1) => Create(left, false, right, true, step * (-1));

        public static FlexibleSet<int> Create(int left, bool isLeftStrictly, int right, bool isRightStrictly, int step = 1)
        {
            return new FlexibleSet<int>(FlexibleRanges.Create(left, isLeftStrictly, right, isRightStrictly), current => current + step);
        }

        public static FlexibleSet<double> Create(double left, bool isLeftStrictly, double right, bool isRightStrictly, double step = 1.0)
        {
            return new FlexibleSet<double>(FlexibleRanges.Create(left, isLeftStrictly, right, isRightStrictly), current => current + step);
        }
    }
}
