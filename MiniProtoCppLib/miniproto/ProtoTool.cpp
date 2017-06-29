#include "ProtoTool.h"

namespace common
{
	namespace miniproto
	{

		uint32 ProtoTool::Zigzag(int32 value)
		{
			uint32 res = (value << 1) ^ (value >> 31);
			return res;
		}
		uint64 ProtoTool::Zigzag(int64 value)
		{
			uint64 res = (value << 1) ^ (value >> 63);
			return res;
		}
		int32 ProtoTool::DeZigzag(uint32 value)
		{
			int32 res = (value >> 1) ^ -((*(int32 *)(&value)) & 1);
			return res;
		}
		int64 ProtoTool::DeZigzag(uint64 value)
		{
			int64 res = (value >> 1) ^ -((*(int64 *)(&value)) & 1);
			return res;
		}

		byte_size ProtoTool::NumberByteSize(uint32 value)
		{
			if (value < (1 << 7))
			{
				return 1;
			}
			else if (value < (1 << 14))
			{
				return 2;
			}
			else if (value < (1 << 21))
			{
				return 3;
			}
			else if (value < (1 << 28))
			{
				return 4;
			}
			else
			{
				return 5;
			}
		}
		byte_size ProtoTool::NumberByteSize(uint64 value)
		{
			if (value < (1ull << 35))
			{
				if (value < (1ull << 7))
				{
					return 1;
				}
				else if (value < (1ull << 14))
				{
					return 2;
				}
				else if (value < (1ull << 21))
				{
					return 3;
				}
				else if (value < (1ull << 28))
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
				if (value < (1ull << 42))
				{
					return 6;
				}
				else if (value < (1ull << 49))
				{
					return 7;
				}
				else if (value < (1ull << 56))
				{
					return 8;
				}
				else if (value < (1ull << 63))
				{
					return 9;
				}
				else
				{
					return 10;
				}
			}
		}
		byte_size ProtoTool::NumberByteSize(float value)
		{
			return 4;
		}
		byte_size ProtoTool::NumberByteSize(double value)
		{
			return 8;
		}
		byte_size ProtoTool::NumberCode(uint32 value, byte *buf)
		{
			byte_size bytes = 0;

			byte temp = 0;
			while (true)
			{
				temp = (byte)(value & 0x7f);
				if ((value >>= 7) != 0)
				{
					buf[bytes++] = temp | 0x80;
				}
				else
				{
					buf[bytes++] = temp;
					break;
				}
			};

			return bytes;
		}
		byte_size ProtoTool::NumberCode(uint64 value, byte *buf)
		{
			byte_size bytes = 0;

			byte temp = 0;
			while (true)
			{
				temp = (byte)(value & 0x7f);
				if ((value >>= 7) != 0)
				{
					buf[bytes++] = temp | 0x80;
				}
				else
				{
					buf[bytes++] = temp;
					break;
				}
			};

			return bytes;
		}
		byte_size ProtoTool::NumberCode(float value, byte *buf)
		{
			for (int i = 0; i < 4; i++)
			{
				buf[i] = (*(uint32 *)(&value) >> (i * 8)) & 0xff;
			}

			return 4;
		}
		byte_size ProtoTool::NumberCode(double value, byte *buf)
		{
			for (int i = 0; i < 8; i++)
			{
				buf[i] = (*(uint64 *)(&value) >> (i * 8)) & 0xff;
			}

			return 8;
		}
		byte_size ProtoTool::NumberDecode(uint32& value, const byte *buf)
		{
			byte_size bytes = 0;

			value = 0;
			while (true)
			{
				byte temp = buf[bytes];
				value = value | ((((uint32)temp) & 0x7f) << (7 * bytes));

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
		byte_size ProtoTool::NumberDecode(uint64& value, const byte *buf)
		{
			byte_size bytes = 0;

			value = 0;
			while (true)
			{
				byte temp = buf[bytes];
				value = value | ((((uint64)temp) & 0x7f) << (7 * bytes));

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
		byte_size ProtoTool::NumberDecode(float& value, const byte *buf)
		{
			*(uint32 *)(&value) = 0;
			for (int i = 0; i < 4; i++)
			{
				byte temp = buf[i];
				*(uint32 *)(&value) = *(uint32 *)(&value) | (((uint32)temp) << (i * 8));
			}

			return 4;
		}
		byte_size ProtoTool::NumberDecode(double& value, const byte *buf)
		{
			*(uint64 *)(&value) = 0;
			for (int i = 0; i < 8; i++)
			{
				byte temp = buf[i];
				*(uint64 *)(&value) = *(uint64 *)(&value) | (((uint64)temp) << (i * 8));
			}

			return 8;
		}
		byte_size ProtoTool::NumberCode(uint32 value, std::ostream& buf)
		{
			byte_size bytes = 0;

			byte temp = 0;
			while (true)
			{
				temp = (byte)(value & 0x7f);
				if ((value >>= 7) != 0)
				{
					temp = temp | 0x80;
					buf.write((const char *)&temp, 1);
					bytes++;
				}
				else
				{
					buf.write((const char *)&temp, 1);
					bytes++;
					break;
				}
			};

			return bytes;
		}
		byte_size ProtoTool::NumberCode(uint64 value, std::ostream& buf)
		{
			byte_size bytes = 0;

			byte temp = 0;
			while (true)
			{
				temp = (byte)(value & 0x7f);
				if ((value >>= 7) != 0)
				{
					temp = temp | 0x80;
					buf.write((const char *)&temp, 1);
					bytes++;
				}
				else
				{
					buf.write((const char *)&temp, 1);
					bytes++;
					break;
				}
			};

			return bytes;
		}
		byte_size ProtoTool::NumberCode(float value, std::ostream& buf)
		{
			byte temp[4] = { 0 };
			for (int i = 0; i < 4; i++)
			{
				temp[i] = (*(uint32 *)(&value) >> (i * 8)) & 0xff;
			}
			buf.write((const char *)&temp, 4);

			return 4;
		}
		byte_size ProtoTool::NumberCode(double value, std::ostream& buf)
		{
			byte temp[8] = { 0 };
			for (int i = 0; i < 8; i++)
			{
				temp[i] = (*(uint64 *)(&value) >> (i * 8)) & 0xff;
			}
			buf.write((const char *)&temp, 8);

			return 8;
		}
		byte_size ProtoTool::NumberDecode(uint32& value, std::istream& buf)
		{
			byte_size bytes = 0;

			value = 0;
			while (true)
			{
				byte temp = 0;
				buf.read((char *)&temp, 1);
				value = value | ((((uint32)temp) & 0x7f) << (7 * bytes));

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
		byte_size ProtoTool::NumberDecode(uint64& value, std::istream& buf)
		{
			byte_size bytes = 0;

			value = 0;
			while (true)
			{
				byte temp = 0;
				buf.read((char *)&temp, 1);
				value = value | ((((uint64)temp) & 0x7f) << (7 * bytes));

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
		byte_size ProtoTool::NumberDecode(float& value, std::istream& buf)
		{
			*(uint32 *)(&value) = 0;
			for (int i = 0; i < 4; i++)
			{
				byte temp = 0;
				buf.read((char *)&temp, 1);
				*(uint32 *)(&value) = *(uint32 *)(&value) | (((uint32)temp) << (i * 8));
			}

			return 4;
		}
		byte_size ProtoTool::NumberDecode(double& value, std::istream& buf)
		{
			*(uint64 *)(&value) = 0;
			for (int i = 0; i < 8; i++)
			{
				byte temp = 0;
				buf.read((char *)&temp, 1);
				*(uint64 *)(&value) = *(uint64 *)(&value) | (((uint64)temp) << (i * 8));
			}

			return 8;
		}

		byte_size ProtoTool::KeyByteSize(unsigned int num, unsigned int type)
		{
			return NumberByteSize(num << 3 | type);
		}
		byte_size ProtoTool::KeyCode(unsigned int num, unsigned int type, byte *buf)
		{
			return NumberCode(num << 3 | type, buf);
		}
		byte_size ProtoTool::KeyDecode(unsigned int& num, unsigned int& type, const byte *buf)
		{
			uint32 key = 0;
			byte_size bytes = NumberDecode(key, buf);
			num = key >> 3;
			type = key & 0x7;
			return bytes;
		}
		byte_size ProtoTool::KeyCode(unsigned int num, unsigned int type, std::ostream& buf)
		{
			return NumberCode(num << 3 | type, buf);
		}
		byte_size ProtoTool::KeyDecode(unsigned int& num, unsigned int& type, std::istream& buf)
		{
			uint32 key = 0;
			byte_size bytes = NumberDecode(key, buf);
			num = key >> 3;
			type = key & 0x7;
			return bytes;
		}

		byte_size ProtoTool::UnknownDecode(unsigned int type, const byte *buf)
		{
			byte_size bytes = 0;
			switch (type)
			{
				case PWT_Varint:
				{
					uint64 value = 0;
					bytes += NumberDecode(value, buf);
				}
				break;
				case PWT_64bit:
				{
					bytes += 8;
				}
				break;
				case PWT_32bit:
				{
					bytes += 4;
				}
				break;
				case PWT_LengthDelimited:
				{
					byte_size length = 0;
					bytes += NumberDecode(length, buf);
					bytes += length;
				}
				break;
				default:
				{
					throw UnknownWireTypeException;
				}
				break;
			}
			return bytes;
		}
		byte_size ProtoTool::UnknownDecode(unsigned int type, std::istream& buf)
		{
			byte_size bytes = 0;
			switch (type)
			{
				case PWT_Varint:
				{
					uint64 value = 0;
					bytes += NumberDecode(value, buf);
				}
				break;
				case PWT_64bit:
				{
					buf.seekg(8, std::ios::cur);
					bytes += 8;
				}
				break;
				case PWT_32bit:
				{
					buf.seekg(4, std::ios::cur);
					bytes += 4;
				}
				break;
				case PWT_LengthDelimited:
				{
					byte_size length = 0;
					bytes += NumberDecode(length, buf);
					buf.seekg(length, std::ios::cur);
					bytes += length;
				}
				break;
				default:
				{
					throw UnknownWireTypeException;
				}
				break;
			}
			return bytes;
		}

		byte_size ProtoTool::BoolByteSize(bool value)
		{
			return 1;
		}
		byte_size ProtoTool::BoolCode(bool value, byte *buf)
		{
			return NumberCode(value ? 1u : 0u, buf);
		}
		byte_size ProtoTool::BoolDecode(bool& value, const byte *buf)
		{
			uint32 temp = 0;
			byte_size bytes = NumberDecode(temp, buf);
			value = (temp != 0) ? true : false;
			return bytes;
		}
		byte_size ProtoTool::BoolCode(bool value, std::ostream& buf)
		{
			return NumberCode(value ? 1u : 0u, buf);
		}
		byte_size ProtoTool::BoolDecode(bool& value, std::istream& buf)
		{
			uint32 temp = 0;
			byte_size bytes = NumberDecode(temp, buf);
			value = (temp != 0) ? true : false;
			return bytes;
		}

		byte_size ProtoTool::Int32ByteSize(int32 value)
		{
			return NumberByteSize(*(uint32 *)(&value));
		}
		byte_size ProtoTool::Int32Code(int32 value, byte *buf)
		{
			return NumberCode(*(uint32 *)(&value), buf);
		}
		byte_size ProtoTool::Int32Decode(int32& value, const byte *buf)
		{
			uint32 temp = 0;
			byte_size bytes = NumberDecode(temp, buf);
			value = static_cast<int32>(temp);
			return bytes;
		}
		byte_size ProtoTool::Int32Code(int32 value, std::ostream& buf)
		{
			return NumberCode(*(uint32 *)(&value), buf);
		}
		byte_size ProtoTool::Int32Decode(int32& value, std::istream& buf)
		{
			uint32 temp = 0;
			byte_size bytes = NumberDecode(temp, buf);
			value = static_cast<int32>(temp);
			return bytes;
		}

		byte_size ProtoTool::SInt32ByteSize(int32 value)
		{
			return NumberByteSize(Zigzag(value));
		}
		byte_size ProtoTool::SInt32Code(int32 value, byte *buf)
		{
			return NumberCode(Zigzag(value), buf);
		}
		byte_size ProtoTool::SInt32Decode(int32& value, const byte *buf)
		{
			uint32 temp = 0;
			byte_size bytes = NumberDecode(temp, buf);
			value = DeZigzag(temp);
			return bytes;
		}
		byte_size ProtoTool::SInt32Code(int32 value, std::ostream& buf)
		{
			return NumberCode(Zigzag(value), buf);
		}
		byte_size ProtoTool::SInt32Decode(int32& value, std::istream& buf)
		{
			uint32 temp = 0;
			byte_size bytes = NumberDecode(temp, buf);
			value = DeZigzag(temp);
			return bytes;
		}

		byte_size ProtoTool::UInt32ByteSize(uint32 value)
		{
			return NumberByteSize(value);
		}
		byte_size ProtoTool::UInt32Code(uint32 value, byte *buf)
		{
			return NumberCode(value, buf);
		}
		byte_size ProtoTool::UInt32Decode(uint32& value, const byte *buf)
		{
			return NumberDecode(value, buf);
		}
		byte_size ProtoTool::UInt32Code(uint32 value, std::ostream& buf)
		{
			return NumberCode(value, buf);
		}
		byte_size ProtoTool::UInt32Decode(uint32& value, std::istream& buf)
		{
			return NumberDecode(value, buf);
		}

		byte_size ProtoTool::Int64ByteSize(int64 value)
		{
			return NumberByteSize(*(uint64 *)(&value));
		}
		byte_size ProtoTool::Int64Code(int64 value, byte *buf)
		{
			return NumberCode(*(uint64 *)(&value), buf);
		}
		byte_size ProtoTool::Int64Decode(int64& value, const byte *buf)
		{
			uint64 temp = 0;
			byte_size bytes = NumberDecode(temp, buf);
			value = static_cast<int64>(temp);
			return bytes;
		}
		byte_size ProtoTool::Int64Code(int64 value, std::ostream& buf)
		{
			return NumberCode(*(uint64 *)(&value), buf);
		}
		byte_size ProtoTool::Int64Decode(int64& value, std::istream& buf)
		{
			uint64 temp = 0;
			byte_size bytes = NumberDecode(temp, buf);
			value = static_cast<int64>(temp);
			return bytes;
		}

		byte_size ProtoTool::SInt64ByteSize(int64 value)
		{
			return NumberByteSize(Zigzag(value));
		}
		byte_size ProtoTool::SInt64Code(int64 value, byte *buf)
		{
			return NumberCode(Zigzag(value), buf);
		}
		byte_size ProtoTool::SInt64Decode(int64& value, const byte *buf)
		{
			uint64 temp = 0;
			byte_size bytes = NumberDecode(temp, buf);
			value = DeZigzag(temp);
			return bytes;
		}
		byte_size ProtoTool::SInt64Code(int64 value, std::ostream& buf)
		{
			return NumberCode(Zigzag(value), buf);
		}
		byte_size ProtoTool::SInt64Decode(int64& value, std::istream& buf)
		{
			uint64 temp = 0;
			byte_size bytes = NumberDecode(temp, buf);
			value = DeZigzag(temp);
			return bytes;
		}

		byte_size ProtoTool::UInt64ByteSize(uint64 value)
		{
			return NumberByteSize(value);
		}
		byte_size ProtoTool::UInt64Code(uint64 value, byte *buf)
		{
			return NumberCode(value, buf);
		}
		byte_size ProtoTool::UInt64Decode(uint64& value, const byte *buf)
		{
			return NumberDecode(value, buf);
		}
		byte_size ProtoTool::UInt64Code(uint64 value, std::ostream& buf)
		{
			return NumberCode(value, buf);
		}
		byte_size ProtoTool::UInt64Decode(uint64& value, std::istream& buf)
		{
			return NumberDecode(value, buf);
		}

		byte_size ProtoTool::FloatByteSize(float value)
		{
			return 4;
		}
		byte_size ProtoTool::FloatCode(float value, byte *buf)
		{
			return NumberCode(value, buf);
		}
		byte_size ProtoTool::FloatDecode(float& value, const byte *buf)
		{
			return NumberDecode(value, buf);
		}
		byte_size ProtoTool::FloatCode(float value, std::ostream& buf)
		{
			return NumberCode(value, buf);
		}
		byte_size ProtoTool::FloatDecode(float& value, std::istream& buf)
		{
			return NumberDecode(value, buf);
		}

		byte_size ProtoTool::DoubleByteSize(double value)
		{
			return 8;
		}
		byte_size ProtoTool::DoubleCode(double value, byte *buf)
		{
			return NumberCode(value, buf);
		}
		byte_size ProtoTool::DoubleDecode(double& value, const byte *buf)
		{
			return NumberDecode(value, buf);
		}
		byte_size ProtoTool::DoubleCode(double value, std::ostream& buf)
		{
			return NumberCode(value, buf);
		}
		byte_size ProtoTool::DoubleDecode(double& value, std::istream& buf)
		{
			return NumberDecode(value, buf);
		}

	}
}