package common.miniproto;

public class ProtoDefine {

	// 解码失败时，throw UnknownWriteTypeException
	public static final String UnknownWriteTypeException = "UnknownWriteTypeException";

	// Map中出现不支持的键/值类型时，throw IncompatibleTypeException
	public static final String IncompatibleTypeException = "IncompatibleTypeException";

	// proto字段类型枚举
	public enum ProtoFieldType {
		// bool
		PFT_Bool(0),
		// int32
		PFT_Int32(1),
		// int64
		PFT_Int64(2),
		// sint32
		PFT_SInt32(3),
		// sint64
		PFT_SInt64(4),
		// uint32
		PFT_UInt32(5),
		// uint64
		PFT_UInt64(6),
		// enum
		PFT_Enum(7),
		// float
		PFT_Float(8),
		// double
		PFT_Double(9),
		// string
		PFT_String(10),
		// message
		PFT_Message(11);

		public static final int PFT_Bool_VALUE = 0;
		public static final int PFT_Int32_VALUE = 1;
		public static final int PFT_Int64_VALUE = 2;
		public static final int PFT_SInt32_VALUE = 3;
		public static final int PFT_SInt64_VALUE = 4;
		public static final int PFT_UInt32_VALUE = 5;
		public static final int PFT_UInt64_VALUE = 6;
		public static final int PFT_Enum_VALUE = 7;
		public static final int PFT_Float_VALUE = 8;
		public static final int PFT_Double_VALUE = 9;
		public static final int PFT_String_VALUE = 10;
		public static final int PFT_Message_VALUE = 11;

		private final int value;

		private ProtoFieldType(int value) {
			this.value = value;
		}

		public int getValue() {
			return value;
		}

		public static ProtoFieldType valueOf(int value) {
			switch (value) {
				case 0 :
					return PFT_Bool;
				case 1 :
					return PFT_Int32;
				case 2 :
					return PFT_Int64;
				case 3 :
					return PFT_SInt32;
				case 4 :
					return PFT_SInt64;
				case 5 :
					return PFT_UInt32;
				case 6 :
					return PFT_UInt64;
				case 7 :
					return PFT_Enum;
				case 8 :
					return PFT_Float;
				case 9 :
					return PFT_Double;
				case 10 :
					return PFT_String;
				case 11 :
					return PFT_Message;
				default :
					return null;
			}
		}
	}

	// proto字段类型枚举
	public enum ProtoWireType {
		// int32, int64, uint32, uint64, sint32, sint64, bool, enum
		PWT_Varint(0),
		// double
		PWT_64bit(1),
		// string, bytes, messages, proto3 repeated, miniproto repeated
		PWT_LengthDelimited(2),
		// unused
		PWT_StartGroup(3),
		// unused
		PWT_EndGroup(4),
		// float
		PWT_32bit(5),
		// invalid type
		PWT_Unknown(6);

		public static final int PWT_Varint_VALUE = 0;
		public static final int PWT_64bit_VALUE = 1;
		public static final int PWT_LengthDelimited_VALUE = 2;
		public static final int PWT_StartGroup_VALUE = 3;
		public static final int PWT_EndGroup_VALUE = 4;
		public static final int PWT_32bit_VALUE = 5;
		public static final int PWT_Unknown_VALUE = 6;

		private final int value;

		private ProtoWireType(int value) {
			this.value = value;
		}

		public int getValue() {
			return value;
		}

		public static ProtoWireType valueOf(int value) {
			switch (value) {
				case 0 :
					return PWT_Varint;
				case 1 :
					return PWT_64bit;
				case 2 :
					return PWT_LengthDelimited;
				case 3 :
					return PWT_StartGroup;
				case 4 :
					return PWT_EndGroup;
				case 5 :
					return PWT_32bit;
				case 6 :
					return PWT_Unknown;
				default :
					return null;
			}
		}
	}
}
