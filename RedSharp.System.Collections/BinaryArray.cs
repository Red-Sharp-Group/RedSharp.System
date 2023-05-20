using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using RedSharp.Sys.Collections.Helpers;
using RedSharp.Sys.Helpers;

namespace RedSharp.Sys.Collections
{
    public class BinaryArray : IEnumerable<byte>
    {
        /// <summary>
        /// Light wrapper for the indexed like
        /// </summary>
        private class BinaryArrayEnumerator : IEnumerator<byte>
        {
            private BinaryArray _source;
            private int _currentIndex = -1;

            public BinaryArrayEnumerator(BinaryArray source)
            {
                _source = source;

                Reset();
            }

            public byte Current => _source[_currentIndex];

            object IEnumerator.Current => Current;

            public bool MoveNext()
            {
                _currentIndex++;

                return _currentIndex < _source.Length;
            }

            public void Reset() => _currentIndex = -1;

            public void Dispose() => Reset();
        }

        private static byte[] _maxValues;

        private byte[] _source;

        static BinaryArray()
        {
            _maxValues = new byte[]
            {
                0,
                1,
                3,
                7,
                15,
                31,
                63,
                127,
                255
            };
        }

        /// <summary>
        /// Creates a new exemplar of the collection
        /// </summary>
        /// <remarks>
        /// Bit count can have next values: 1, 2, 4, 8
        /// </remarks>
        public BinaryArray(int bitCount, int length)
        {
            ThrowIfIncorrectBitCount(bitCount);

            ArgumentsGuard.ThrowIfLess(length, 0);

            BitCount = bitCount;
            Length = length;

            var capacity = (int)Math.Ceiling((double)(bitCount * length) / 8);

            _source = new byte[capacity];
        }

        /// <summary>
        /// Returns the value by input index
        /// </summary>
        /// <remarks>
        /// Max value relies on the <see cref="BitCount"/>:
        /// <br/>1 bit: 1
        /// <br/>2 bit: 3
        /// <br/>4 bit: 15
        /// <br/>8 bit: 255
        /// </remarks>
        public byte this[int index]
        {
            get => Get(index);
            set => Set(index, value);
        }

        /// <summary>
        /// Packing schema for the instance
        /// </summary>
        public int BitCount { get; private set; }

        /// <summary>
        /// The defined number of values that can be packed
        /// </summary>
        public int Length { get; private set; }
        
        /// <summary>
        /// Number of bytes that used for the packing
        /// </summary>
        public int Capacity => _source.Length;

        /// <summary>
        /// Returns internal buffer for the outside manipulations
        /// </summary>
        /// <remarks>
        /// Buffer contains packed values
        /// </remarks>
        public Span<byte> ToSpan()
        {
            return new Span<byte>(_source);
        }

        /// <inheritdoc/>
        public IEnumerator<byte> GetEnumerator()
        {
            return new BinaryArrayEnumerator(this);
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new BinaryArrayEnumerator(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ThrowIfIncorrectBitCount(int bitCount)
        {
            switch(bitCount)
            {
                case 1:
                case 2:
                case 4:
                case 8:
                    return;
                default:
                    throw new ArgumentOutOfRangeException(nameof(bitCount));
            }
        }

        /// <summary>
        /// Gets value from the packed byte
        /// </summary>
        private byte Get(int index)
        {
            ArgumentsGuard.ThrowIfLess(index, 0);
            ArgumentsGuard.ThrowIfGreaterOrEqual(index, Length);

            //uses at least twice, so it is better to cache
            var pseudoIndex = BitCount * index;
            var maskShift = pseudoIndex % 8;

            //whole byte value where the needed value is stored
            var combinedValue = _source[pseudoIndex / 8];

            //need to remove unneeded values
            var mask = _maxValues[BitCount] << maskShift;

            //value with incorrect position
            var unshiftedValue = combinedValue & mask;

            var result = unshiftedValue >> maskShift;

            return (byte)result;
        }

        /// <summary>
        /// Erase needed portion of bits and sets the input value
        /// </summary>
        private void Set(int index, byte value)
        {
            ArgumentsGuard.ThrowIfLess(index, 0);
            ArgumentsGuard.ThrowIfGreaterOrEqual(index, Length);

            if (value > _maxValues[BitCount])
                throw new ArgumentOutOfRangeException(nameof(value));

            //uses at least twice, so it is better to cache
            var pseudoIndex = BitCount * index;
            var maskShift = pseudoIndex % 8;
            var trueIndex = pseudoIndex / 8; 

            //need to remove unneeded values
            var mask = ~(_maxValues[BitCount] << maskShift);

            //shift to needed position
            var shiftedValue = value << maskShift;

            _source[trueIndex] = (byte)(_source[trueIndex] & mask | shiftedValue);
        }
    }
}
