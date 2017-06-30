# miniproto<br>
<br>
mini proto tool like google protobuf. parse 'xx.proto' files to cpp/c#/java code.<br>
<br>
1. package<br>
it is zhe same as protobuf, you can declare package like this:<br>
package xxx.xxx.xxx;<br>
<br>
<br>
2. import<br>
it is zhe same as protobuf, you can declare import like this:<br>
import "xxx.proto";<br>
the import filename is unsupport with filepath.<br>
you can give zhe path in tool's running params.<br>
<br>
<br>
3. enum<br>
in proto file, you can define enum like this:<br>
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
you can use mathematical expression for enum's entry, and use some defined entries in expression.<br>
you can use operator like (), +, -, *, /, %, <<, >>, |, & in expression.<br>
you can define entry without value. if zhe entry without value is zhe first entry in the enum, it while be assign as 0. zhe others entries without value while be assign as (previous entry + 1).<br>
you can use same values for different entries, and need not declare "allow_alias = true" like protobuf.<br>
you can use any value for first entry, and need not be assign as 0 for it.<br>
<br>
<br>
4. message<br>
in proto file, you can define message like this:<br>
```
message MsgTest
{
	int32 m_int32 = 1;
	string m_string = 2;
	repeated float m_float = 3;
	set<sint64> m_sint64 = 4 [packed = true];
	map<uint32, EnumTest> m_uint32enum = 5 [packed = true];
}
```
you can use optional/required for singular field, or not. zhe code/decode rules are all the same.<br>
you can use repeated/set/map for container field. repeated and set's code/decode rules are all the same. map's element is zhe same as nested message like this:<br>
```
message pair
{
	K key = 1;
	V value = 2;
}
```
you can declare "packed = true" for any container fields.<br>
miniproto code/decode rule is zhe same as protobuf. all of protobuf2.0 and 3.0's rules are supported. you can give version in tool's running params.<br>
<br>
4-1. message code for c++<br>
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
it is used stl container for repeated/set/map, and you can set allocator by yourself. so you can control memory malloc and free enjoy yourself.<br>
it has Release interface for free and Clear interface for clear. so you can Clear repeated field but not free, and Release it for free memory.<br>
it is support right-value operator, and you need build it by c++11.<br>
you can traversed repeated field by index, and traversed set/map by begin/end interface.<br>
you can reserve repeated field by reserve interface for realloc memory.