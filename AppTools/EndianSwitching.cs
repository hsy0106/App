using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTools
{
    /// <summary>
    /// 大小端转换
    /// </summary>
    public class EndianSwitching
    {

        public static ushort BitSwitchEndian(ushort value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }

            ushort resotreValue = BitConverter.ToUInt16(bytes, 0);

            return resotreValue;
        }

        public static ushort ConverEndian(ushort value)
        {
            byte heightByte = (byte)(value >> 8);//右移八位

            byte lowByte = (byte)(value & 0x00FF);//

            //合并高位和低位
            ushort mergedValue = (ushort)(heightByte << 8 | lowByte);

            return mergedValue;
        }
    }
}
