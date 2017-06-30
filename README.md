# miniproto</br>
</br>
mini proto tool like google protobuf. parse 'xx.proto' files to cpp/c#/java code.</br>
</br>
1. package</br>
it is zhe same as protobuf, you can declare package like this:</br>
package xxx.xxx.xxx;</br>
</br>
</br>
2. import</br>
it is zhe same as protobuf, you can declare import like this:</br>
import "xxx.proto";</br>
the import filename is unsupport with filepath.</br>
you can give zhe path in tool's running params.</br>
</br>
</br>
3. enum</br>
in proto file, you can define enum like this:</br>
enum EnumBase</br>
{</br>
	EnumBase_1; // 0</br>
	EnumBase_2 = 100;</br>
	EnumBase_3 = 10 + (9 - 8) * 7 / 6 % 5 << 4 >> 3 | 2 & 1;</br>
}</br>
enum EnumTest</br>
{</br>
	EnumTest_1 = 0x0001;</br>
	EnumTest_2 = 0x0002 + EnumTest_1;</br>
	EnumTest_3 = 0x0003 - EnumBase.EnumBase_1;</br>
	EnumTest_4 = EnumTest_2 | EnumTest_3;</br>
	EnumTest_5; // EnumTest_4 + 1</br>
}</br>
you can use mathematical expression for enum's entry, and use some defined entries in expression.</br>
you can use operator like (), +, -, *, /, %, <<, >>, |, & in expression.</br>
you can define entry without value. if zhe entry without value is zhe first entry in the enum, it while be assign as 0. zhe others entries without value while be assign as (previous entry + 1).</br>
you can use same values for different entries, and need not declare "allow_alias = true" like protobuf.</br>
you can use any value for first entry, and need not be assign as 0 for it.</br>
</br>
</br>
4. message</br>
in proto file, you can define message like this:</br>
message MsgTest</br>
{</br>
	int32 m_int32 = 1;</br>
	string m_string = 2;</br>
	repeated float m_float = 3;</br>
	set<sint64> m_sint64 = 4 [packed = true];</br>
	map<uint32, EnumTest> m_uint32enum = 5 [packed = true];</br>
}</br>
you can use optional/required for singular field, or not. zhe code/decode rules are all the same.</br>
you can use repeated/set/map for container field. repeated and set's code/decode rules are all the same. map's element is zhe same as nested message like this:</br>
message pair</br>
{</br>
    K key = 1;</br>
    V value = 2;</br>
}</br>
you can declare "packed = true" for any container fields.</br>
miniproto code/decode rule is zhe same as protobuf. all of protobuf2.0 and 3.0's rules are supported. you can give version in tool's running params.</br>
</br>
4-1. message code for c++</br>
Eg. zhe message MsgTest's code:</br>
</br>
template <template<typename> class A = std::allocator> </br>
class MsgTest : public common::miniproto::ProtoBase </br>
{</br>
public:</br>
	MsgTest(); </br>
	MsgTest(const MsgTest<A>& other); </br>
	MsgTest(MsgTest<A>&& other); </br>
	virtual ~MsgTest(); </br>
	MsgTest<A>& operator = (const MsgTest<A>& other); </br>
	MsgTest<A>& operator = (MsgTest<A>&& other);</br>
</br>
public:</br>
	virtual common::miniproto::byte_size ByteSize() const; </br>
	virtual common::miniproto::byte_size Code(common::miniproto::byte* buf, common::miniproto::byte_size size) const; </br>
	virtual common::miniproto::byte_size Decode(const common::miniproto::byte* buf, common::miniproto::byte_size size); </br>
	virtual common::miniproto::byte_size Code(std::ostream& buf, common::miniproto::byte_size size) const; </br>
	virtual common::miniproto::byte_size Decode(std::istream& buf, common::miniproto::byte_size size);</br>
	void Clear(); </br>
	void Release(); </br>
</br>
public:</br>
	void set_m_int32(const common::miniproto::int32& value); </br>
	void set_m_int32(common::miniproto::int32&& value);</br>
	const common::miniproto::int32& m_int32() const; </br>
	void clear_m_int32(); </br>
	void release_m_int32(); </br>
	bool has_m_int32() const; </br>
private:</br>
	void set_has_m_int32(); </br>
	void clear_has_m_int32(); </br>
private:</br>
	common::miniproto::int32 m_int32_; </br>
</br>
public:</br>
	void set_m_string(const common::miniproto::String<A>& value); </br>
	void set_m_string(common::miniproto::String<A>&& value);</br>
	void set_m_string(const char* value);</br>
	void set_m_string(const char* value, size_t size);</br>
	common::miniproto::String<A>& m_string(); </br>
	const common::miniproto::String<A>& m_string() const; </br>
	void clear_m_string(); </br>
	void release_m_string(); </br>
	bool has_m_string() const;</br>
private:</br>
	void set_has_m_string();</br>
	void clear_has_m_string();</br>
private:</br>
	common::miniproto::String<A> m_string_; </br>
</br>
public:</br>
	void reserve_m_float(size_t size); </br>
	void set_m_float(common::miniproto::uint32 index, const float& value); </br>
	void set_m_float(common::miniproto::uint32 index, float&& value);</br>
	void add_m_float(const float& value); </br>
	void add_m_float(float&& value);</br>
	size_t m_float_size() const; </br>
	const float& m_float(common::miniproto::uint32 index) const; </br>
	void clear_m_float(); </br>
	void release_m_float(); </br>
	bool has_m_float() const; </br>
private:</br>
	void set_has_m_float();</br>
	void clear_has_m_float();</br>
private:</br>
	common::miniproto::Array<float, A> m_float_; </br>
</br>
public:</br>
	void add_m_sint64(const common::miniproto::int64& value); </br>
	void add_m_sint64(common::miniproto::int64&& value);</br>
	void remove_m_sint64(const common::miniproto::int64& value); </br>
	size_t m_sint64_size() const; </br>
	size_t m_sint64_count(const common::miniproto::int64& value) const; </br>
	common::miniproto::SetConstIt<common::miniproto::int64, A> m_sint64_begin() const; </br>
	common::miniproto::SetConstIt<common::miniproto::int64, A> m_sint64_end() const;</br>
	void clear_m_sint64(); </br>
	void release_m_sint64(); </br>
	bool has_m_sint64() const; </br>
private:</br>
	void set_has_m_sint64();</br>
	void clear_has_m_sint64();</br>
private:</br>
	common::miniproto::Set<common::miniproto::int64, A> m_sint64_; </br>
</br>
public:</br>
	void add_m_uint32enum(const common::miniproto::uint32& key, const common::proto1::EnumTest& value); </br>
	void add_m_uint32enum(common::miniproto::uint32&& key, const common::proto1::EnumTest& value);</br>
	void add_m_uint32enum(const common::miniproto::uint32& key, common::proto1::EnumTest&& value);</br>
	void add_m_uint32enum(common::miniproto::uint32&& key, common::proto1::EnumTest&& value);</br>
	void remove_m_uint32enum(const common::miniproto::uint32& key); </br>
	size_t m_uint32enum_size() const; </br>
	const common::proto1::EnumTest* find_m_uint32enum(const common::miniproto::uint32& key) const; </br>
	common::miniproto::MapConstIt<common::miniproto::uint32, common::proto1::EnumTest, A> m_uint32enum_begin() const; </br>
	common::miniproto::MapConstIt<common::miniproto::uint32, common::proto1::EnumTest, A> m_uint32enum_end() const;</br>
	void clear_m_uint32enum(); </br>
	void release_m_uint32enum(); </br>
	bool has_m_uint32enum() const; </br>
private:</br>
	void set_has_m_uint32enum();</br>
	void clear_has_m_uint32enum();</br>
private:</br>
	common::miniproto::Map<common::miniproto::uint32, common::proto1::EnumTest, A> m_uint32enum_; </br>
</br>
private:</br>
	common::miniproto::ProtoBitMap<5> m_bits; </br>
};</br>
</br>
it is used stl container for repeated/set/map, and you can set allocator by yourself. so you can control memory malloc and free enjoy yourself.</br>
it has Release interface for free and Clear interface for clear. so you can Clear repeated field but not free, and Release it for free memory.</br>
it is support right-value operator, and you need build it by c++11.</br>
you can traversed repeated field by index, and traversed set/map by begin/end interface.</br>
you can reserve repeated field by reserve interface for realloc memory.