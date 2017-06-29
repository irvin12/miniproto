本工程目录结构说明：
MiniProto ─┬─ MiniProto ───────────┬─ logic ─────┬─ ProtoParse.h/ProtoParse.cpp			// 用来解析proto文件
           │                       │             └─ ProtoCodeTool.h/ProtoCodeTool.cpp		// 用来生成代码
           │                       ├─ tool ──────┬─ StringTool.h/StringTool.cpp			// 工具类，用于字符串处理
           │                       │             └─ FileData.h/FileData.cpp			// 工具类，用于文本文件读写
           │                       ├─ MiniProto.cpp						// main函数，处理运行参数，调用对应功能
           │                       └─ MiniProto.vcxproj						// vs2013的c++工程文件，生成代码生成工具miniproto.exe，生成在Release目录
           │ 
           ├─ MiniProtoCppLib ─────┬─ miniproto ─┬─ ProtoDefine.h				// miniproto的c++库文件，一些类型/枚举/别名定义
           │                       │             ├─ ProtoBase.h/ProtoBase.cpp			// miniproto的c++库文件，所有message的基类
           │                       │             ├─ ProtoTool.h/ProtoTool.cpp			// miniproto的c++库文件，各种数据类型的编解码逻辑
           │                       │             └─ ProtoBitMap.h				// miniproto的c++库文件，用于标记message字段是否有值的位图
           │                       └─ MiniProtoCppLib.vcxproj					// vs2013的c++工程文件，生成libminiproto.lib，生成在Release目录
           │ 
           ├─ MiniProtoCppTest ────┬─ message ───┬─ Proto1.h					// Release文件夹中proto1.proto测试文件生成的对应c++代码文件
           │                       │             └─ Proto2.h					// Release文件夹中proto2.proto测试文件生成的对应c++代码文件
           │                       ├─ MiniProtoCppTest.cpp					// main函数，调用proto1/proto2中的message，进行序列化/反序列化测试
           │                       └─ MiniProtoCppTest.vcxproj					// vs2013的c++工程文件，用于示例演示
           │ 
           ├─ MiniProtoCSharpDll ──┬─ miniproto ─┬─ ProtoDefine.cs				// miniproto的c#库文件，一些类型/枚举定义
           │                       │             ├─ ProtoBase.cs				// miniproto的c#库文件，所有message的基类
           │                       │             ├─ ProtoTool.cs				// miniproto的c#库文件，各种数据类型的编解码逻辑
           │                       │             └─ ProtoBitMap.cs				// miniproto的c#库文件，用于标记message字段是否有值的位图
           │                       ├─ Properties ── AssemblyInfo.cs				// vs2013的c#程序集信息文件
           │                       └─ MiniProtoCSharpDll.csproj					// vs2013的c#工程文件，生成dllminiproto.dll，生成在Release目录
           │
           ├─ MiniProtoCSharpTest ─┬─ message ───┬─ Proto1.cs					// Release文件夹中proto1.proto测试文件生成的对应c#代码文件
           │                       │             └─ Proto2.cs					// Release文件夹中proto2.proto测试文件生成的对应c#代码文件
           │                       ├─ Program.cpp						// main函数，调用proto1/proto2中的message，进行序列化/反序列化测试
           │                       ├─ App.config						// c#程序配置文件
           │                       ├─ Properties ── AssemblyInfo.cs				// vs2013的c#程序集信息文件
           │                       └─ MiniProtoCSharpTest.csproj				// vs2013的c#工程文件，用于示例演示
           │
           ├─ MiniProtoJavaJar ────┬─ src ──────── common ─── miniproto ─┬─ ProtoDefine.java	// miniproto的java库文件，一些类型/枚举定义
           │                       │                                     ├─ ProtoBase.java	// miniproto的java库文件，所有message的基类
           │                       │                                     ├─ ProtoTool.java	// miniproto的java库文件，各种数据类型的编解码逻辑
           │                       │                                     └─ ProtoBitMap.java	// miniproto的java库文件，用于标记message字段是否有值的位图
           │                       ├─ .settings							// MyEclipse的java项目文件，生成miniproto.jar，导出jar时请设置为Release/miniproto.jar
           │                       ├─ .classpath						// MyEclipse的java项目文件
           │                       └─ .project							// MyEclipse的java项目文件
           │
           ├─ MiniProtoJavaTest ───┬─ src ──────┬─ common ─┬─ proto1 ────── Proto1.java		// Release文件夹中proto1.proto测试文件生成的对应java代码文件
           │                       │            │          └─ proto2 ────── Proto2.java		// Release文件夹中proto2.proto测试文件生成的对应java代码文件
           │                       │            └─ MiniProtoJavaTest.java			// main函数，调用proto1/proto2中的message，进行序列化/反序列化测试
           │                       ├─ .settings							// MyEclipse的java项目文件，referenced libraries请导入Release/miniproto.jar
           │                       ├─ .classpath						// MyEclipse的java项目文件
           │                       └─ .project							// MyEclipse的java项目文件
           │
           ├─ Release ─────────────┬─ Proto1.proto						// 用于测试用的proto文件
           │                       ├─ Proto2.proto						// 用于测试用的proto文件
           │                       ├─ libminiproto.lib						// 用于c++项目的lib静态库
           │                       ├─ dllminiproto.dll						// 用于c#项目的dll动态库
           │                       ├─ miniproto.jar						// 用于java项目的jar类库
           │                       ├─ MiniProto.exe						// miniproto的代码生成工具
           │                       └─ MiniProto.bat						// MiniProto.exe的批处理文件，exe的运行参数示例
           │
           ├─ Clean.bat										// 清理生成文件的批处理
	   ├─ readme.txt									// 本说明文件
	   └─ Tools.sln										// vs2013的工程文件


编译说明：
用vs2013或更高版本，打开Tools.sln，选择release，win32，MiniProto项目需要设置你自己的boost库路径。
编译MiniProto，在release目录下可以生成MiniProto.exe，即代码自动生成工具exe
编译MiniProtoCppLib，在release目录下可以生成libminiproto.lib，是miniproto的lib文件，用于c++项目
编译MiniProtoCSharpDll，在release目录下可以生成dllminiproto.dll，是miniproto的dll文件，用于c#项目
用myeclipse导入MiniProtoJavaJar，导出jar包到release目录下，得到java项目的类库