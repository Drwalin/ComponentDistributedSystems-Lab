

all:
	client dll

client:
	"C:\Program Files (x86)\Microsoft Visual Studio 9.0\VC\vcvarsall.bat"
	(cd clientcs && dotnet run -c Release --arch x86)

dll:
	"C:\Program Files (x86)\Microsoft Visual Studio 9.0\VC\vcvarsall.bat"
	(cd dllcs && dotnet build -c Release --arch x86)


