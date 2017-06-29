using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace common
{
	namespace miniproto
	{
		// proto字段编码工具类
		public static class ProtoTool
		{
			// Zigzag编/解码
			public static uint Zigzag(int value)
			{
				uint res = (uint)((value << 1) ^ (value >> 31));
				return res;
			}
			public static ulong Zigzag(long value)
			{
				ulong res = (ulong)((value << 1) ^ (value >> 63));
				return res;
			}
			public static int DeZigzag(uint value)
			{
				int res = (int)(value >> 1) ^ -(int)(value & 1);
				return res;
			}
			public static long DeZigzag(ulong value)
			{
				long res = (long)(value >> 1) ^ -(long)(value & 1);
				return res;
			}

			// Number编/解码
			public static ulong NumberByteSize(uint value)
			{
				if (value < (1u << 7))
				{
					return 1;
				}
				else if (value < (1u << 14))
				{
					return 2;
				}
				else if (value < (1u << 21))
				{
					return 3;
				}
				else if (value < (1u << 28))
				{
					return 4;
				}
				else
				{
					return 5;
				}
			}
			public static ulong NumberByteSize(ulong value)
			{
				if (value < (1ul << 35))
				{
					if (value < (1ul << 7))
					{
						return 1;
					}
					else if (value < (1ul << 14))
					{
						return 2;
					}
					else if (value < (1ul << 21))
					{
						return 3;
					}
					else if (value < (1ul << 28))
					{
						return 4;
					}
					else
					{
						return 5;
					}
				}
				else
				{
					if (value < (1ul << 42))
					{
						return 6;
					}
					else if (value < (1ul << 49))
					{
						return 7;
					}
					else if (value < (1ul << 56))
					{
						return 8;
					}
					else if (value < (1ul << 63))
					{
						return 9;
					}
					else
					{
						return 10;
					}
				}
			}
			public static ulong NumberByteSize(float value)
			{
				return 4;
			}
			public static ulong NumberByteSize(double value)
			{
				return 8;
			}
			public static ulong NumberCode(uint value, Stream buf)
			{
				ulong bytes = 0;

				byte temp = 0;
				while (true)
				{
					temp = (byte)(value & 0x7f);
					if ((value >>= 7) != 0)
					{
						buf.WriteByte((byte)(temp | 0x80));
						bytes++;
					}
					else
					{
						buf.WriteByte(temp);
						bytes++;
						break;
					}
				};

				return bytes;
			}
			public static ulong NumberCode(ulong value, Stream buf)
			{
				ulong bytes = 0;

				byte temp = 0;
				while (true)
				{
					temp = (byte)(value & 0x7f);
					if ((value >>= 7) != 0)
					{
						buf.WriteByte((byte)(temp | 0x80));
						bytes++;
					}
					else
					{
						buf.WriteByte(temp);
						bytes++;
						break;
					}
				};

				return bytes;
			}
			public static ulong NumberCode(float value, Stream buf)
			{
				byte[] floatBytes = BitConverter.GetBytes(value);
				buf.Write(floatBytes, 0, floatBytes.Length);

				return 4;
			}
			public static ulong NumberCode(double value, Stream buf)
			{
				byte[] doubleBytes = BitConverter.GetBytes(value);
				buf.Write(doubleBytes, 0, doubleBytes.Length);

				return 8;
			}
			public static ulong NumberDecode(ref uint value, Stream buf)
			{
				ulong bytes = 0;

				value = 0;
				while (true)
				{
					int readed = buf.ReadByte();
					if (-1 == readed)
					{
						break;
					}

					uint temp = (uint)readed;
					value = value | ((temp & 0x7f) << (int)(7 * bytes));

					if ((temp & 0x80) != 0)
					{
						bytes++;
					}
					else
					{
						bytes++;
						break;
					}
				}

				return bytes;
			}
			public static ulong NumberDecode(ref ulong value, Stream buf)
			{
				ulong bytes = 0;

				value = 0;
				while (true)
				{
					int readed = buf.ReadByte();
					if (-1 == readed)
					{
						break;
					}

					ulong temp = (ulong)readed;
					value = value | ((temp & 0x7f) << (int)(7 * bytes));

					if ((temp & 0x80) != 0)
					{
						bytes++;
					}
					else
					{
						bytes++;
						break;
					}
				}

				return bytes;
			}
			public static ulong NumberDecode(ref float value, Stream buf)
			{
				byte[] floatBytes = new byte[4];
				buf.Read(floatBytes, 0, floatBytes.Length);
				value = BitConverter.ToSingle(floatBytes, 0);

				return 4;
			}
			public static ulong NumberDecode(ref double value, Stream buf)
			{
				byte[] doubleBytes = new byte[8];
				buf.Read(doubleBytes, 0, doubleBytes.Length);
				value = BitConverter.ToDouble(doubleBytes, 0);

				return 8;
			}

			// proto key编/解码
			public static ulong KeyByteSize(uint num, uint type)
			{
				ulong bytes = 0;

				bytes += NumberByteSize(num << 3 | type);

				return bytes;
			}
			public static ulong KeyCode(uint num, uint type, Stream buf)
			{
				ulong bytes = 0;

				bytes += NumberCode(num << 3 | type, buf);

				return bytes;
			}
			public static ulong KeyDecode(ref uint num, ref uint type, Stream buf)
			{
				ulong bytes = 0;

				uint key = 0;
				bytes += NumberDecode(ref key, buf);
				num = key >> 3;
				type = key & 0x7;

				return bytes;
			}

			// 未知字段解码
			public static ulong UnknownDecode(uint type, Stream buf)
			{
				ulong bytes = 0;

				switch (type)
				{
					case (uint)ProtoWireType.PWT_Varint:
					{
						ulong value = 0;
						bytes += NumberDecode(ref value, buf);
					}
					break;
					case (uint)ProtoWireType.PWT_64bit:
					{
						bytes = 8;
					}
					break;
					case (uint)ProtoWireType.PWT_32bit:
					{
						bytes = 4;
					}
					break;
					case (uint)ProtoWireType.PWT_LengthDelimited:
					{
						ulong length = 0;
						bytes += NumberDecode(ref length, buf);
						bytes += length;
					}
					break;
					default:
					{
						throw new Exception(ProtoDefine.UnknownWireTypeException);
					}
				}

				return bytes;
			}

			// 单个字段编/解码
			// 数组按proto2规则编/解码 {bytesize + element}
			// 数组按proto3规则编/解码 bytesize + {element}
			public static ulong BoolByteSize(bool value)
			{
				return 1;
			}
			public static ulong BoolCode(bool value, Stream buf)
			{
				ulong bytes = 0;

				bytes += NumberCode(value ? 1u : 0u, buf);

				return bytes;
			}
			public static ulong BoolDecode(ref bool value, Stream buf)
			{
				ulong bytes = 0;

				uint temp = 0;
				bytes += NumberDecode(ref temp, buf);
				value = (temp != 0) ? true : false;

				return bytes;
			}

			public static ulong Int32ByteSize(int value)
			{
				ulong bytes = 0;

				bytes += NumberByteSize((uint)value);

				return bytes;
			}
			public static ulong Int32Code(int value, Stream buf)
			{
				ulong bytes = 0;

				bytes += NumberCode((uint)value, buf);

				return bytes;
			}
			public static ulong Int32Decode(ref int value, Stream buf)
			{
				ulong bytes = 0;

				uint temp = 0;
				bytes += NumberDecode(ref temp, buf);
				value = (int)temp;

				return bytes;
			}

			public static ulong Int64ByteSize(long value)
			{
				ulong bytes = 0;

				bytes += NumberByteSize((ulong)value);

				return bytes;
			}
			public static ulong Int64Code(long value, Stream buf)
			{
				ulong bytes = 0;

				bytes += NumberCode((ulong)value, buf);

				return bytes;
			}
			public static ulong Int64Decode(ref long value, Stream buf)
			{
				ulong bytes = 0;

				ulong temp = 0;
				bytes += NumberDecode(ref temp, buf);
				value = (long)temp;

				return bytes;
			}

			public static ulong SInt32ByteSize(int value)
			{
				ulong bytes = 0;

				bytes += NumberByteSize(Zigzag(value));

				return bytes;
			}
			public static ulong SInt32Code(int value, Stream buf)
			{
				ulong bytes = 0;

				bytes += NumberCode(Zigzag(value), buf);

				return bytes;
			}
			public static ulong SInt32Decode(ref int value, Stream buf)
			{
				ulong bytes = 0;

				uint temp = 0;
				bytes += NumberDecode(ref temp, buf);
				value = DeZigzag(temp);

				return bytes;
			}

			public static ulong SInt64ByteSize(long value)
			{
				ulong bytes = 0;

				bytes += NumberByteSize(Zigzag(value));

				return bytes;
			}
			public static ulong SInt64Code(long value, Stream buf)
			{
				ulong bytes = 0;

				bytes += NumberCode(Zigzag(value), buf);

				return bytes;
			}
			public static ulong SInt64Decode(ref long value, Stream buf)
			{
				ulong bytes = 0;

				ulong temp = 0;
				bytes += NumberDecode(ref temp, buf);
				value = DeZigzag(temp);

				return bytes;
			}

			public static ulong UInt32ByteSize(uint value)
			{
				ulong bytes = 0;

				bytes += NumberByteSize(value);

				return bytes;
			}
			public static ulong UInt32Code(uint value, Stream buf)
			{
				ulong bytes = 0;

				bytes += NumberCode(value, buf);

				return bytes;
			}
			public static ulong UInt32Decode(ref uint value, Stream buf)
			{
				ulong bytes = 0;

				bytes += NumberDecode(ref value, buf);

				return bytes;
			}

			public static ulong UInt64ByteSize(ulong value)
			{
				ulong bytes = 0;

				bytes += NumberByteSize(value);

				return bytes;
			}
			public static ulong UInt64Code(ulong value, Stream buf)
			{
				ulong bytes = 0;

				bytes += NumberCode(value, buf);

				return bytes;
			}
			public static ulong UInt64Decode(ref ulong value, Stream buf)
			{
				ulong bytes = 0;

				bytes += NumberDecode(ref value, buf);

				return bytes;
			}

			public static ulong EnumByteSize<E>(E value)
			{
				ulong bytes = 0;

				bytes += NumberByteSize(Convert.ToUInt32(value));

				return bytes;
			}
			public static ulong EnumCode<E>(E value, Stream buf)
			{
				ulong bytes = 0;

				bytes += NumberCode(Convert.ToUInt32(value), buf);

				return bytes;
			}
			public static ulong EnumDecode<E>(ref E value, Stream buf)
			{
				ulong bytes = 0;

				uint temp = 0;
				bytes += NumberDecode(ref temp, buf);
				value = (E)Enum.ToObject(typeof(E), temp);

				return bytes;
			}

			public static ulong FloatByteSize(float value)
			{
				return 4;
			}
			public static ulong FloatCode(float value, Stream buf)
			{
				ulong bytes = 0;

				bytes += NumberCode(value, buf);

				return bytes;
			}
			public static ulong FloatDecode(ref float value, Stream buf)
			{
				ulong bytes = 0;

				bytes += NumberDecode(ref value, buf);

				return bytes;
			}

			public static ulong DoubleByteSize(double value)
			{
				return 8;
			}
			public static ulong DoubleCode(double value, Stream buf)
			{
				ulong bytes = 0;

				bytes += NumberCode(value, buf);

				return bytes;
			}
			public static ulong DoubleDecode(ref double value, Stream buf)
			{
				ulong bytes = 0;

				bytes += NumberDecode(ref value, buf);

				return bytes;
			}

			public static ulong StringByteSize(string value)
			{
				ulong bytes = 0;

				byte[] stringBytes = System.Text.Encoding.UTF8.GetBytes(value);
				bytes += NumberByteSize((uint)stringBytes.Length);
				bytes += (ulong)stringBytes.Length;

				return bytes;
			}
			public static ulong StringCode(string value, Stream buf)
			{
				ulong bytes = 0;

				byte[] stringBytes = System.Text.Encoding.UTF8.GetBytes(value);
				bytes += NumberCode((uint)stringBytes.Length, buf);
				buf.Write(stringBytes, 0, stringBytes.Length);
				bytes += (ulong)stringBytes.Length;

				return bytes;
			}
			public static ulong StringDecode(ref string value, Stream buf)
			{
				ulong bytes = 0;

				ulong length = 0;
				bytes += NumberDecode(ref length, buf);
				byte[] stringBytes = new byte[length];
				buf.Read(stringBytes, 0, stringBytes.Length);
				value = System.Text.Encoding.UTF8.GetString(stringBytes, 0, stringBytes.Length);
				bytes += (ulong)length;

				return bytes;
			}

			public static ulong MessageByteSize<M>(M value) where M : ProtoBase
			{
				ulong bytes = 0;

				ulong length = value.ByteSize();
				bytes += NumberByteSize(length);
				bytes += length;

				return bytes;
			}
			public static ulong MessageCode<M>(M value, Stream buf) where M : ProtoBase
			{
				ulong bytes = 0;

				ulong length = value.ByteSize();
				bytes += NumberCode(length, buf);
				bytes += value.Code(buf, length);

				return bytes;
			}
			public static ulong MessageDecode<M>(ref M value, Stream buf) where M : ProtoBase
			{
				ulong bytes = 0;

				ulong length = 0;
				bytes += NumberDecode(ref length, buf);
				bytes += value.Decode(buf, length);

				return bytes;
			}

			// 针对不同基本数据类型的编/解码函数的委托
			delegate TResult FunByteSize<T, TResult>(T value);
			delegate TResult FunCode<T, TResult>(T value, Stream buf);
			delegate TResult FunDecode<T, TResult>(ref T value, Stream buf);

			private static class Cache<T, P>
			{
				public static FunByteSize<T, ulong> ByteSize;
				public static FunCode<T, ulong> Code;
				public static FunDecode<T, ulong> Decode;
			}

			static ProtoTool()
			{
				Cache<bool, ProtoBool>.ByteSize = (bool value) => ProtoTool.BoolByteSize(value);
				Cache<int, ProtoInt32>.ByteSize = (int value) => ProtoTool.Int32ByteSize(value);
				Cache<int, ProtoSInt32>.ByteSize = (int value) => ProtoTool.SInt32ByteSize(value);
				Cache<uint, ProtoUInt32>.ByteSize = (uint value) => ProtoTool.UInt32ByteSize(value);
				Cache<long, ProtoInt64>.ByteSize = (long value) => ProtoTool.Int64ByteSize(value);
				Cache<long, ProtoSInt64>.ByteSize = (long value) => ProtoTool.SInt64ByteSize(value);
				Cache<ulong, ProtoUInt64>.ByteSize = (ulong value) => ProtoTool.UInt64ByteSize(value);
				Cache<float, ProtoFloat>.ByteSize = (float value) => ProtoTool.FloatByteSize(value);
				Cache<double, ProtoDouble>.ByteSize = (double value) => ProtoTool.DoubleByteSize(value);
				Cache<string, ProtoString>.ByteSize = (string value) => ProtoTool.StringByteSize(value);

				Cache<bool, ProtoBool>.Code = (bool value, Stream buf) => ProtoTool.BoolCode(value, buf);
				Cache<int, ProtoInt32>.Code = (int value, Stream buf) => ProtoTool.Int32Code(value, buf);
				Cache<int, ProtoSInt32>.Code = (int value, Stream buf) => ProtoTool.SInt32Code(value, buf);
				Cache<uint, ProtoUInt32>.Code = (uint value, Stream buf) => ProtoTool.UInt32Code(value, buf);
				Cache<long, ProtoInt64>.Code = (long value, Stream buf) => ProtoTool.Int64Code(value, buf);
				Cache<long, ProtoSInt64>.Code = (long value, Stream buf) => ProtoTool.SInt64Code(value, buf);
				Cache<ulong, ProtoUInt64>.Code = (ulong value, Stream buf) => ProtoTool.UInt64Code(value, buf);
				Cache<float, ProtoFloat>.Code = (float value, Stream buf) => ProtoTool.FloatCode(value, buf);
				Cache<double, ProtoDouble>.Code = (double value, Stream buf) => ProtoTool.DoubleCode(value, buf);
				Cache<string, ProtoString>.Code = (string value, Stream buf) => ProtoTool.StringCode(value, buf);

				Cache<bool, ProtoBool>.Decode = (ref bool value, Stream buf) => ProtoTool.BoolDecode(ref value, buf);
				Cache<int, ProtoInt32>.Decode = (ref int value, Stream buf) => ProtoTool.Int32Decode(ref value, buf);
				Cache<int, ProtoSInt32>.Decode = (ref int value, Stream buf) => ProtoTool.SInt32Decode(ref value, buf);
				Cache<uint, ProtoUInt32>.Decode = (ref uint value, Stream buf) => ProtoTool.UInt32Decode(ref value, buf);
				Cache<long, ProtoInt64>.Decode = (ref long value, Stream buf) => ProtoTool.Int64Decode(ref value, buf);
				Cache<long, ProtoSInt64>.Decode = (ref long value, Stream buf) => ProtoTool.SInt64Decode(ref value, buf);
				Cache<ulong, ProtoUInt64>.Decode = (ref ulong value, Stream buf) => ProtoTool.UInt64Decode(ref value, buf);
				Cache<float, ProtoFloat>.Decode = (ref float value, Stream buf) => ProtoTool.FloatDecode(ref value, buf);
				Cache<double, ProtoDouble>.Decode = (ref double value, Stream buf) => ProtoTool.DoubleDecode(ref value, buf);
				Cache<string, ProtoString>.Decode = (ref string value, Stream buf) => ProtoTool.StringDecode(ref value, buf);
			}

			// Array，Set容器中，每一个元素的编/解码
			private static ulong EntryByteSize<T, P>(T value, P type)
			{
				if (Cache<T, P>.ByteSize == null)
				{
					if (value is Enum && type is ProtoEnum)
					{
						Cache<T, P>.ByteSize = (T parm) => ProtoTool.EnumByteSize(parm);
					}
					else
					{
						throw new NotImplementedException();
					}
				}
				return Cache<T, P>.ByteSize(value);
			}
			private static ulong EntryCode<T, P>(T value, Stream buf, P type)
			{
				if (Cache<T, P>.Code == null)
				{
					if (value is Enum && type is ProtoEnum)
					{
						Cache<T, P>.Code = (T parm1, Stream parm2) => ProtoTool.EnumCode(parm1, parm2);
					}
					else
					{
						throw new NotImplementedException();
					}
				}
				return Cache<T, P>.Code(value, buf);
			}
			private static ulong EntryDecode<T, P>(ref T value, Stream buf, P type)
			{
				if (Cache<T, P>.Decode == null)
				{
					if (value is Enum && type is ProtoEnum)
					{
						Cache<T, P>.Decode = (ref T parm1, Stream parm2) => ProtoTool.EnumDecode(ref parm1, parm2);
					}
					else
					{
						throw new NotImplementedException();
					}
				}
				return Cache<T, P>.Decode(ref value, buf);
			}

			// Array容器的编/解码
			private static ulong ArrayByteSizeWithoutLength<T, P>(IList<T> values, P type)
			{
				ulong bytes = 0;

				foreach (var value in values)
				{
					bytes += EntryByteSize(value, type);
				}

				return bytes;
			}
			public static ulong ArrayByteSize<T, P>(IList<T> values, P type)
			{
				ulong bytes = 0;

				bytes += ArrayByteSizeWithoutLength(values, type);

				bytes += NumberByteSize(bytes);

				return bytes;
			}
			public static ulong ArrayCode<T, P>(IList<T> values, Stream buf, P type)
			{
				ulong bytes = 0;

				bytes += NumberCode(ArrayByteSizeWithoutLength(values, type), buf);

				foreach (var value in values)
				{
					bytes += EntryCode(value, buf, type);
				}

				return bytes;
			}
			public static ulong ArrayDecode<T, P>(IList<T> values, Stream buf, P type)
			{
				ulong bytes = 0;

				ulong bufSize = 0;
				bytes += NumberDecode(ref bufSize, buf);
				bufSize += bytes;

				while (bytes < bufSize)
				{
					T value = default(T);
					bytes += EntryDecode(ref value, buf, type);
					values.Add(value);
				}

				return bytes;
			}

			// Array容器中元素为的ProtoBase时的编/解码（由于需要调用ProtoBase的接口，所以T必须做约束）
			private static ulong ArrayByteSizeWithoutLength<T>(IList<T> values) where T : ProtoBase
			{
				ulong bytes = 0;

				foreach (var value in values)
				{
					bytes += MessageByteSize(value);
				}

				return bytes;
			}
			public static ulong ArrayByteSize<T>(IList<T> values) where T : ProtoBase
			{
				ulong bytes = 0;

				bytes += ArrayByteSizeWithoutLength(values);

				bytes += NumberByteSize(bytes);

				return bytes;
			}
			public static ulong ArrayCode<T>(IList<T> values, Stream buf) where T : ProtoBase
			{
				ulong bytes = 0;

				bytes += NumberCode(ArrayByteSizeWithoutLength(values), buf);

				foreach (var value in values)
				{
					bytes += MessageCode(value, buf);
				}

				return bytes;
			}
			public static ulong ArrayDecode<T>(IList<T> values, Stream buf) where T : ProtoBase, new()
			{
				ulong bytes = 0;

				ulong bufSize = 0;
				bytes += NumberDecode(ref bufSize, buf);
				bufSize += bytes;

				while (bytes < bufSize)
				{
					T value = new T();
					bytes += MessageDecode(ref value, buf);
					values.Add(value);
				}

				return bytes;
			}

			// Set容器的编/解码
			private static ulong SetByteSizeWithoutLength<T, P>(ISet<T> values, P type)
			{
				ulong bytes = 0;

				foreach (var value in values)
				{
					bytes += EntryByteSize(value, type);
				}

				return bytes;
			}
			public static ulong SetByteSize<T, P>(ISet<T> values, P type)
			{
				ulong bytes = 0;

				bytes += SetByteSizeWithoutLength(values, type);

				bytes += NumberByteSize(bytes);

				return bytes;
			}
			public static ulong SetCode<T, P>(ISet<T> values, Stream buf, P type)
			{
				ulong bytes = 0;

				bytes += NumberCode(SetByteSizeWithoutLength(values, type), buf);

				foreach (var value in values)
				{
					bytes += EntryCode(value, buf, type);
				}

				return bytes;
			}
			public static ulong SetDecode<T, P>(ISet<T> values, Stream buf, P type)
			{
				ulong bytes = 0;

				ulong bufSize = 0;
				bytes += NumberDecode(ref bufSize, buf);
				bufSize += bytes;

				while (bytes < bufSize)
				{
					T value = default(T);
					bytes += EntryDecode(ref value, buf, type);
					values.Add(value);
				}

				return bytes;
			}

			// Map容器中，每一个元素的编/解码
			private static ulong EntryByteSizeWithoutLength<K, V, KP, VP>(K key, V value, KP keyType, VP valueType)
				where KP : ProtoType
				where VP : ProtoType
			{
				if (Cache<K, KP>.ByteSize == null)
				{
					throw new NotImplementedException();
				}
				if (Cache<V, VP>.ByteSize == null)
				{
					if (value is Enum && valueType is ProtoEnum)
					{
						Cache<V, VP>.ByteSize = (V parm) => ProtoTool.EnumByteSize(parm);
					}
					else
					{
						throw new NotImplementedException();
					}
				}

				ulong bytes = 0;
				bytes += KeyByteSize(1, (uint)keyType.GetWireType());
				bytes += Cache<K, KP>.ByteSize(key);
				bytes += KeyByteSize(2, (uint)valueType.GetWireType());
				bytes += Cache<V, VP>.ByteSize(value);
				return bytes;
			}
			public static ulong EntryByteSize<K, V, KP, VP>(K key, V value, KP keyType, VP valueType)
				where KP : ProtoType
				where VP : ProtoType
			{
				ulong bytes = 0;
				ulong length = EntryByteSizeWithoutLength(key, value, keyType, valueType);
				bytes += NumberByteSize(length);
				bytes += length;
				return bytes;
			}
			public static ulong EntryCode<K, V, KP, VP>(K key, V value, Stream buf, KP keyType, VP valueType)
				where KP : ProtoType
				where VP : ProtoType
			{
				if (Cache<K, KP>.Code == null)
				{
					throw new NotImplementedException();
				}
				if (Cache<V, VP>.Code == null)
				{
					if (value is Enum && valueType is ProtoEnum)
					{
						Cache<V, VP>.Code = (V parm1, Stream parm2) => ProtoTool.EnumCode(parm1, parm2);
					}
					else
					{
						throw new NotImplementedException();
					}
				}

				ulong bytes = 0;
				bytes += NumberCode(EntryByteSizeWithoutLength(key, value, keyType, valueType), buf);
				bytes += KeyCode(1, (uint)keyType.GetWireType(), buf);
				bytes += Cache<K, KP>.Code(key, buf);
				bytes += KeyCode(2, (uint)valueType.GetWireType(), buf);
				bytes += Cache<V, VP>.Code(value, buf);
				return bytes;
			}
			public static ulong EntryDecode<K, V, KP, VP>(ref K key, ref V value, Stream buf, KP keyType, VP valueType)
				where KP : ProtoType
				where VP : ProtoType
			{
				if (Cache<K, KP>.Decode == null)
				{
					throw new NotImplementedException();
				}
				if (Cache<V, VP>.Decode == null)
				{
					if (value is Enum && valueType is ProtoEnum)
					{
						Cache<V, VP>.Decode = (ref V parm1, Stream parm2) => ProtoTool.EnumDecode(ref parm1, parm2);
					}
					else
					{
						throw new NotImplementedException();
					}
				}

				ulong bytes = 0;
				ulong length = 0;
				bytes += NumberDecode(ref length, buf);
				uint tag = 0;
				uint type = 0;
				bytes += KeyDecode(ref tag, ref type, buf);
				bytes += Cache<K, KP>.Decode(ref key, buf);
				bytes += KeyDecode(ref tag, ref type, buf);
				bytes += Cache<V, VP>.Decode(ref value, buf);
				return bytes;
			}

			// Map容器中元素为的ProtoBase时，每一个元素的编/解码（由于需要调用ProtoBase的接口，所以V必须做约束）
			private static ulong EntryByteSizeWithoutLength<K, V, KP>(K key, V value, KP keyType)
				where V : ProtoBase
				where KP : ProtoType
			{
				if (Cache<K, KP>.ByteSize == null)
				{
					throw new NotImplementedException();
				}

				ulong bytes = 0;
				bytes += KeyByteSize(1, (uint)keyType.GetWireType());
				bytes += Cache<K, KP>.ByteSize(key);
				bytes += KeyByteSize(2, (uint)ProtoDefine.CommonProtoMessage.GetWireType());
				bytes += MessageByteSize(value);
				return bytes;
			}
			public static ulong EntryByteSize<K, V, KP>(K key, V value, KP keyType)
				where V : ProtoBase
				where KP : ProtoType
			{
				ulong bytes = 0;
				ulong length = EntryByteSizeWithoutLength(key, value, keyType);
				bytes += NumberByteSize(length);
				bytes += length;
				return bytes;
			}
			public static ulong EntryCode<K, V, KP>(K key, V value, Stream buf, KP keyType)
				where V : ProtoBase
				where KP : ProtoType
			{
				if (Cache<K, KP>.Code == null)
				{
					throw new NotImplementedException();
				}

				ulong bytes = 0;
				bytes += NumberCode(EntryByteSizeWithoutLength(key, value, keyType), buf);
				bytes += KeyCode(1, (uint)keyType.GetWireType(), buf);
				bytes += Cache<K, KP>.Code(key, buf);
				bytes += KeyCode(2, (uint)ProtoDefine.CommonProtoMessage.GetWireType(), buf);
				bytes += MessageCode(value, buf);
				return bytes;
			}
			public static ulong EntryDecode<K, V, KP>(ref K key, ref V value, Stream buf, KP keyType)
				where V : ProtoBase
				where KP : ProtoType
			{
				if (Cache<K, KP>.Decode == null)
				{
					throw new NotImplementedException();
				}

				ulong bytes = 0;
				ulong length = 0;
				bytes += NumberDecode(ref length, buf);
				uint tag = 0;
				uint type = 0;
				bytes += KeyDecode(ref tag, ref type, buf);
				bytes += Cache<K, KP>.Decode(ref key, buf);
				bytes += KeyDecode(ref tag, ref type, buf);
				bytes += MessageDecode(ref value, buf);
				return bytes;
			}

			// Map容器的编/解码
			private static ulong DictionaryByteSizeWithoutLength<K, V, KP, VP>(IDictionary<K, V> values, KP keyType, VP valueType)
				where KP : ProtoType
				where VP : ProtoType
			{
				ulong bytes = 0;

				foreach (var value in values)
				{
					bytes += EntryByteSize(value.Key, value.Value, keyType, valueType);
				}

				return bytes;
			}
			public static ulong DictionaryByteSize<K, V, KP, VP>(IDictionary<K, V> values, KP keyType, VP valueType)
				where KP : ProtoType
				where VP : ProtoType
			{
				ulong bytes = 0;

				bytes += DictionaryByteSizeWithoutLength(values, keyType, valueType);

				bytes += NumberByteSize(bytes);

				return bytes;
			}
			public static ulong DictionaryCode<K, V, KP, VP>(IDictionary<K, V> values, Stream buf, KP keyType, VP valueType)
				where KP : ProtoType
				where VP : ProtoType
			{
				ulong bytes = 0;

				bytes += NumberCode(DictionaryByteSizeWithoutLength(values, keyType, valueType), buf);

				foreach (var value in values)
				{
					bytes += EntryCode(value.Key, value.Value, buf, keyType, valueType);
				}

				return bytes;
			}
			public static ulong DictionaryDecode<K, V, KP, VP>(IDictionary<K, V> values, Stream buf, KP keyType, VP valueType)
				where KP : ProtoType
				where VP : ProtoType
			{
				ulong bytes = 0;

				ulong bufSize = 0;
				bytes += NumberDecode(ref bufSize, buf);
				bufSize += bytes;

				while (bytes < bufSize)
				{
					K key = default(K);
					V value = default(V);
					bytes += EntryDecode(ref key, ref value, buf, keyType, valueType);
					values.Add(key, value);
				}

				return bytes;
			}

			// Map容器中元素为的ProtoBase时的编/解码
			private static ulong DictionaryByteSizeWithoutLength<K, V, KP>(IDictionary<K, V> values, KP keyType)
				where V : ProtoBase
				where KP : ProtoType
			{
				ulong bytes = 0;

				foreach (var value in values)
				{
					bytes += EntryByteSize(value.Key, value.Value, keyType);
				}

				return bytes;
			}
			public static ulong DictionaryByteSize<K, V, KP>(IDictionary<K, V> values, KP keyType)
				where V : ProtoBase
				where KP : ProtoType
			{
				ulong bytes = 0;

				bytes += DictionaryByteSizeWithoutLength(values, keyType);

				bytes += NumberByteSize(bytes);

				return bytes;
			}
			public static ulong DictionaryCode<K, V, KP>(IDictionary<K, V> values, Stream buf, KP keyType)
				where V : ProtoBase
				where KP : ProtoType
			{
				ulong bytes = 0;

				bytes += NumberCode(DictionaryByteSizeWithoutLength(values, keyType), buf);

				foreach (var value in values)
				{
					bytes += EntryCode(value.Key, value.Value, buf, keyType);
				}

				return bytes;
			}
			public static ulong DictionaryDecode<K, V, KP>(IDictionary<K, V> values, Stream buf, KP keyType)
				where V : ProtoBase, new()
				where KP : ProtoType
			{
				ulong bytes = 0;

				ulong bufSize = 0;
				bytes += NumberDecode(ref bufSize, buf);
				bufSize += bytes;

				while (bytes < bufSize)
				{
					K key = default(K);
					V value = new V();
					bytes += EntryDecode(ref key, ref value, buf, keyType);
					values.Add(key, value);
				}

				return bytes;
			}
		}
	}
}