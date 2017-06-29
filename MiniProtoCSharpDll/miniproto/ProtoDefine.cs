using System;

namespace common
{
	namespace miniproto
	{

		// proto字段类型
		public enum ProtoFieldType
		{
			// bool
			PFT_Bool = 0,
			// int32
			PFT_Int32,
			// sint32
			PFT_SInt32,
			// uint32
			PFT_UInt32,
			// int64
			PFT_Int64,
			// sint64
			PFT_SInt64,
			// uint64
			PFT_UInt64,
			// enum
			PFT_Enum,
			// float
			PFT_Float,
			// double
			PFT_Double,
			// string
			PFT_String,
			// message
			PFT_Message,
		};

		// proto字段类型
		public enum ProtoWireType
		{
			// int32, int64, uint32, uint64, sint32, sint64, bool, enum
			PWT_Varint = 0,
			// double
			PWT_64bit,
			// string, bytes, messages, proto3 repeated, miniproto repeated
			PWT_LengthDelimited,
			// unused
			PWT_StartGroup,
			// unused
			PWT_EndGroup,
			// float
			PWT_32bit,
			// invalid type
			PWT_Unknown,
		};

		// proto字段类型重定义，主要用于编解码函数的重载
		public abstract class ProtoType
		{
			public abstract ProtoWireType GetWireType();
		}
		public class ProtoBool : ProtoType
		{
			public override ProtoWireType GetWireType()
			{
				return ProtoWireType.PWT_Varint;
			}
		}
		public class ProtoInt32 : ProtoType
		{
			public override ProtoWireType GetWireType()
			{
				return ProtoWireType.PWT_Varint;
			}
		}
		public class ProtoSInt32 : ProtoType
		{
			public override ProtoWireType GetWireType()
			{
				return ProtoWireType.PWT_Varint;
			}
		}
		public class ProtoUInt32 : ProtoType
		{
			public override ProtoWireType GetWireType()
			{
				return ProtoWireType.PWT_Varint;
			}
		}
		public class ProtoInt64 : ProtoType
		{
			public override ProtoWireType GetWireType()
			{
				return ProtoWireType.PWT_Varint;
			}
		}
		public class ProtoSInt64 : ProtoType
		{
			public override ProtoWireType GetWireType()
			{
				return ProtoWireType.PWT_Varint;
			}
		}
		public class ProtoUInt64 : ProtoType
		{
			public override ProtoWireType GetWireType()
			{
				return ProtoWireType.PWT_Varint;
			}
		}
		public class ProtoEnum : ProtoType
		{
			public override ProtoWireType GetWireType()
			{
				return ProtoWireType.PWT_Varint;
			}
		}
		public class ProtoFloat : ProtoType
		{
			public override ProtoWireType GetWireType()
			{
				return ProtoWireType.PWT_32bit;
			}
		}
		public class ProtoDouble : ProtoType
		{
			public override ProtoWireType GetWireType()
			{
				return ProtoWireType.PWT_64bit;
			}
		}
		public class ProtoString : ProtoType
		{
			public override ProtoWireType GetWireType()
			{
				return ProtoWireType.PWT_LengthDelimited;
			}
		}
		public class ProtoMessage : ProtoType
		{
			public override ProtoWireType GetWireType()
			{
				return ProtoWireType.PWT_LengthDelimited;
			}
		}

		public static class ProtoDefine
		{
			// 解码失败时，throw UnknownWireTypeException
			public static readonly string UnknownWireTypeException = "UnknownWireTypeException";

			// proto字段类型各自定义各自的常量对象，用于函数重载
			public static readonly ProtoBool CommonProtoBool = new ProtoBool();
			public static readonly ProtoInt32 CommonProtoInt32 = new ProtoInt32();
			public static readonly ProtoSInt32 CommonProtoSInt32 = new ProtoSInt32();
			public static readonly ProtoUInt32 CommonProtoUInt32 = new ProtoUInt32();
			public static readonly ProtoInt64 CommonProtoInt64 = new ProtoInt64();
			public static readonly ProtoSInt64 CommonProtoSInt64 = new ProtoSInt64();
			public static readonly ProtoUInt64 CommonProtoUInt64 = new ProtoUInt64();
			public static readonly ProtoEnum CommonProtoEnum = new ProtoEnum();
			public static readonly ProtoFloat CommonProtoFloat = new ProtoFloat();
			public static readonly ProtoDouble CommonProtoDouble = new ProtoDouble();
			public static readonly ProtoString CommonProtoString = new ProtoString();
			public static readonly ProtoMessage CommonProtoMessage = new ProtoMessage();
		}
	}
}