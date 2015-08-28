using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iRacingSdkWrapper.Bitfields
{
    public abstract class BitfieldBase<T>
       where T : struct, IConvertible, IComparable
    {
        protected BitfieldBase(int value)
        {
            _Value = (uint)value;
        }

        private readonly uint _Value;
        public uint Value { get { return _Value; } }

        public bool Contains(T bit)
        {
            var bitValue = (uint) Convert.ChangeType(bit, bit.GetTypeCode());
            return (this.Value & bitValue) == bitValue;
        }

        public override string ToString()
        {
            var values = new List<T>();
            foreach (var value in Enum.GetValues(typeof (T)))
            {
                if (this.Contains((T) value))
                {
                    values.Add((T)value);
                }
            }
            return string.Join(" | ", values.Select(v => v.ToString()));
        }
    }
}
