# miniproto

mini proto tool like google protobuf. parse 'xx.proto' files to cpp/c#/java code.<br>

## 1. package

it is zhe same as protobuf, you can declare package like this:<br>

```
package xxx.xxx.xxx;
```

## 2. import

it is zhe same as protobuf, you can declare import like this:<br>

```
import "xxx.proto";
```

the import filename is unsupport with filepath.<br>
you can give zhe path in tool's running params.<br>

## 3. enum

in proto file, you can declare enum like this:<br>

```
enum EnumBase
{
	EnumBase_1; // 0
	EnumBase_2 = 100;
	EnumBase_3 = 10 + (9 - 8) * 7 / 6 % 5 << 4 >> 3 | 2 & 1;
}
enum EnumTest
{
	EnumTest_1 = 0x0001;
	EnumTest_2 = 0x0002 + EnumTest_1;
	EnumTest_3 = 0x0003 - EnumBase.EnumBase_1;
	EnumTest_4 = EnumTest_2 | EnumTest_3;
	EnumTest_5; // EnumTest_4 + 1
}
```

you can use mathematical expression as enum's value, and use some declared entries in expression.<br>
you can use operator like (), +, -, *, /, %, <<, >>, |, & in expression.<br>
you can declare entry without value. if zhe entry without value is zhe first entry in the enum, it while be assign as 0. zhe others entries without value while be assign as (previous entry + 1).<br>
you can use same values to assign to different entries, and need not declare "allow_alias = true" like protobuf.<br>
you can use any value as first entry, and need not be assign as 0 for it.<br>

## 4. message

in proto file, you can declare message like this:<br>

```
message MsgTest
{
	int32 m_int32 = 1;
	optional string m_string = 2;
	repeated float m_float = 3;
	set<sint64> m_sint64 = 4 [packed = true];
	map<uint32, EnumTest> m_uint32enum = 5 [packed = true];
}
```

support list: <br>
you can use optional/required to declare singular field, or not. <br>
you can use repeated/set/map to declare container field. <br>
you can use bool/int32/sint32/uint32/int64/sint64/uint64/float/double/enum/string/bytes/message as field data type. <br>
you can use int32/sint32/uint32/int64/sint64/uint64/string as set's element and map's key.<br>
you can declare 'packed = true' for container fields.<br>
<br>
unsupports list: <br>
you can not declare default value for any fields.<br>
you can not declare deprecated field.<br>
you can not declare extensions field.<br>
you can not declare reserved field.<br>
you can not declare option field.<br>
you can not declare nested message or enum like this:

```
message A 
{
	message B 
	{ 
		enum C 
		{}
	}
}
```

miniproto code/decode rules are all zhe same as protobuf. <br>
all of protobuf2.0 and 3.0's rules are supported. <br>
you can give version in tool's running params.<br>
set field's code/decode rule is the same as repeated field. <br>
map's element is zhe same as nested message like this:<br>

```
message pair
{
	K key = 1;
	V value = 2;
}
```

## 5. running params

1.proto_version<br>
you can code/decode proto message like protobuf2.0 by 'proto_version=2', or code/decode it like protobuf3.0 by 'proto_version=3'.<br>
<br>
2.proto_file<br>
you can enumerate proto file like 'proto_file=1.proto proto_file=2.proto proto_file=3.proto' by import order.<br>
<br>
3.cpp_path<br>
you can give target path like 'cpp_path=./code/cpp' if you want autogeneration c++ code.<br>
<br>
4.csharp_path<br>
you can give target path like 'csharp_path=../../code/cs' if you want autogeneration c# code.<br>
<br>
5.java_path<br>
you can give target path like 'java_path=D:/code/java' if you want autogeneration java code.<br>

## 6. message code for c++

Eg. zhe message MsgTest's code:<br>

```cpp
template <template<typename> class A = std::allocator> 
class MsgTest : public common::miniproto::ProtoBase 
{
public:
	MsgTest(); 
	MsgTest(const MsgTest<A>& other); 
	MsgTest(MsgTest<A>&& other); 
	virtual ~MsgTest(); 
	MsgTest<A>& operator = (const MsgTest<A>& other); 
	MsgTest<A>& operator = (MsgTest<A>&& other);

public:
	virtual common::miniproto::byte_size ByteSize() const; 
	virtual common::miniproto::byte_size Code(common::miniproto::byte* buf, common::miniproto::byte_size size) const; 
	virtual common::miniproto::byte_size Decode(const common::miniproto::byte* buf, common::miniproto::byte_size size); 
	virtual common::miniproto::byte_size Code(std::ostream& buf, common::miniproto::byte_size size) const; 
	virtual common::miniproto::byte_size Decode(std::istream& buf, common::miniproto::byte_size size);
	void Clear(); 
	void Release(); 

public:
	void set_m_int32(const common::miniproto::int32& value); 
	void set_m_int32(common::miniproto::int32&& value);
	const common::miniproto::int32& m_int32() const; 
	void clear_m_int32(); 
	void release_m_int32(); 
	bool has_m_int32() const; 
private:
	void set_has_m_int32(); 
	void clear_has_m_int32(); 
private:
	common::miniproto::int32 m_int32_; 

public:
	void set_m_string(const common::miniproto::String<A>& value); 
	void set_m_string(common::miniproto::String<A>&& value);
	void set_m_string(const char* value);
	void set_m_string(const char* value, size_t size);
	common::miniproto::String<A>& m_string(); 
	const common::miniproto::String<A>& m_string() const; 
	void clear_m_string(); 
	void release_m_string(); 
	bool has_m_string() const;
private:
	void set_has_m_string();
	void clear_has_m_string();
private:
	common::miniproto::String<A> m_string_; 

public:
	void reserve_m_float(size_t size); 
	void set_m_float(common::miniproto::uint32 index, const float& value); 
	void set_m_float(common::miniproto::uint32 index, float&& value);
	void add_m_float(const float& value); 
	void add_m_float(float&& value);
	size_t m_float_size() const; 
	const float& m_float(common::miniproto::uint32 index) const; 
	void clear_m_float(); 
	void release_m_float(); 
	bool has_m_float() const; 
private:
	void set_has_m_float();
	void clear_has_m_float();
private:
	common::miniproto::Array<float, A> m_float_; 

public:
	void add_m_sint64(const common::miniproto::int64& value); 
	void add_m_sint64(common::miniproto::int64&& value);
	void remove_m_sint64(const common::miniproto::int64& value); 
	size_t m_sint64_size() const; 
	size_t m_sint64_count(const common::miniproto::int64& value) const; 
	common::miniproto::SetConstIt<common::miniproto::int64, A> m_sint64_begin() const; 
	common::miniproto::SetConstIt<common::miniproto::int64, A> m_sint64_end() const;
	void clear_m_sint64(); 
	void release_m_sint64(); 
	bool has_m_sint64() const; 
private:
	void set_has_m_sint64();
	void clear_has_m_sint64();
private:
	common::miniproto::Set<common::miniproto::int64, A> m_sint64_; 

public:
	void add_m_uint32enum(const common::miniproto::uint32& key, const common::proto1::EnumTest& value); 
	void add_m_uint32enum(common::miniproto::uint32&& key, const common::proto1::EnumTest& value);
	void add_m_uint32enum(const common::miniproto::uint32& key, common::proto1::EnumTest&& value);
	void add_m_uint32enum(common::miniproto::uint32&& key, common::proto1::EnumTest&& value);
	void remove_m_uint32enum(const common::miniproto::uint32& key); 
	size_t m_uint32enum_size() const; 
	const common::proto1::EnumTest* find_m_uint32enum(const common::miniproto::uint32& key) const; 
	common::miniproto::MapConstIt<common::miniproto::uint32, common::proto1::EnumTest, A> m_uint32enum_begin() const; 
	common::miniproto::MapConstIt<common::miniproto::uint32, common::proto1::EnumTest, A> m_uint32enum_end() const;
	void clear_m_uint32enum(); 
	void release_m_uint32enum(); 
	bool has_m_uint32enum() const; 
private:
	void set_has_m_uint32enum();
	void clear_has_m_uint32enum();
private:
	common::miniproto::Map<common::miniproto::uint32, common::proto1::EnumTest, A> m_uint32enum_; 

private:
	common::miniproto::ProtoBitMap<5> m_bits; 
};
```

it is used stl container to realize repeated/set/map field, and you can set allocator by yourself. so you can control memory malloc and free enjoy yourself.<br>
it has Release interface for free and Clear interface for clear. so you can Clear repeated field but not free, and Release it for free memory.<br>
it is support right-value operator, and you need build it by c++11.<br>
you can traversed repeated field by index, and traversed set/map by begin/end interface.<br>
you can reserve repeated field by reserve interface for realloc memory.<br>

## 7.solusion tree

```
MiniProto ─┬─ MiniProto ───────────┬─ logic ─────┬─ ProtoParse.h/ProtoParse.cpp<br>
           │                       │             └─ ProtoCodeTool.h/ProtoCodeTool.cpp<br>
           │                       ├─ tool ──────┬─ StringTool.h/StringTool.cpp<br>
           │                       │             └─ FileData.h/FileData.cpp<br>
           │                       ├─ MiniProto.cpp<br>
           │                       └─ MiniProto.vcxproj<br>
           │ <br>
           ├─ MiniProtoCppLib ─────┬─ miniproto ─┬─ ProtoDefine.h<br>
           │                       │             ├─ ProtoBase.h/ProtoBase.cpp<br>
           │                       │             ├─ ProtoTool.h/ProtoTool.cpp<br>
           │                       │             └─ ProtoBitMap.h<br>
           │                       └─ MiniProtoCppLib.vcxproj<br>
           │ <br>
           ├─ MiniProtoCppTest ────┬─ message ───┬─ Proto1.h<br>
           │                       │             └─ Proto2.h<br>
           │                       ├─ MiniProtoCppTest.cpp<br>
           │                       └─ MiniProtoCppTest.vcxproj<br>
           │ <br>
           ├─ MiniProtoCSharpDll ──┬─ miniproto ─┬─ ProtoDefine.cs<br>
           │                       │             ├─ ProtoBase.cs<br>
           │                       │             ├─ ProtoTool.cs<br>
           │                       │             └─ ProtoBitMap.cs<br>
           │                       ├─ Properties ── AssemblyInfo.cs<br>
           │                       └─ MiniProtoCSharpDll.csproj<br>
           │ <br>
           ├─ MiniProtoCSharpTest ─┬─ message ───┬─ Proto1.cs<br>
           │                       │             └─ Proto2.cs<br>
           │                       ├─ Program.cpp<br>
           │                       ├─ App.config<br>
           │                       ├─ Properties ── AssemblyInfo.cs<br>
           │                       └─ MiniProtoCSharpTest.csproj<br>
           │ <br>
           ├─ MiniProtoJavaJar ────┬─ src ──────── common ─── miniproto ─┬─ ProtoDefine.java<br>
           │                       │                                     ├─ ProtoBase.java<br>
           │                       │                                     ├─ ProtoTool.java<br>
           │                       │                                     └─ ProtoBitMap.java<br>
           │                       ├─ .settings<br>
           │                       ├─ .classpath<br>
           │                       └─ .project<br>
           │ <br>
           ├─ MiniProtoJavaTest ───┬─ src ──────┬─ common ─┬─ proto1 ────── Proto1.java<br>
           │                       │            │          └─ proto2 ────── Proto2.java<br>
           │                       │            └─ MiniProtoJavaTest.java<br>
           │                       ├─ .settings<br>
           │                       ├─ .classpath<br>
           │                       └─ .project<br>
           │ <br>
           ├─ Release ─────────────┬─ Proto1.proto<br>
           │                       ├─ Proto2.proto<br>
           │                       ├─ libminiproto.lib<br>
           │                       ├─ dllminiproto.dll<br>
           │                       ├─ miniproto.jar<br>
           │                       ├─ MiniProto.exe<br>
           │                       └─ MiniProto.bat<br>
           │ <br>
           ├─ Clean.bat<br>
	   ├─ readme.txt<br>
	   └─ Tools.sln<br>
```

## 8.build

MiniProto is a c++ project creat by vs2013. <br>
it is depend on boost.spirit lib to parse proto file. so you need configure you boost path for this project. <br>

MiniProtoCppLib is a c++ project creat by vs2013. <br>
you can build it without any third-party libraries to make 'libminiproto.lib' file. <br>
MiniProtoCppTest is an Eg. project creat by vs2013. <br>
it is show you how to use zhe lib and cpp message codes in your project. <br>

MiniProtoCSharpDll is a c# project creat by vs2013. <br>
you can build it to make 'dllminiproto.dll' file. <br>
MiniProtoCSharpTest is an Eg. project creat by vs2013. <br>
it is show you how to use zhe dll and c# message codes in your project. <br>

MiniProtoJavaJar is a java project creat by myeclipse. <br>
you can export from it to get a 'miniproto.jar' file. <br>
MiniProtoJavaTest is an Eg. project creat by myeclipse. <br>
it is show you how to use zhe jar and java message codes in your project. <br>

