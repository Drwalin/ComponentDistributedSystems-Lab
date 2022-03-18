using System;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Collections.Generic;

[assembly:AssemblyKeyFileAttribute("keyfile.snk")]
namespace KSR {
	
	[Guid("9EE6ED09-D4F1-4EF4-8126-ACF25675B29E"), ComVisible(true),
		InterfaceType(ComInterfaceType.InterfaceIsDual)
// 	,assembly:AssemblyKeyFile("keyfile.snk")
	]
	public interface IStos {
		uint Push(int v);
	}

	
	[Guid("27F10FA1-8215-4D14-B5F5-D6B850F49E5E"), ComVisible(true),
		ClassInterface(ClassInterfaceType.None)//, ProgId("KSR.Stos.1")
// 	, assembly:AssemblyKeyFile("keyfile.snk")
	]
	public class Stos : IStos {
		public Stos() {
		}
		
		public uint Push(int v) {
			Console.WriteLine("Pushed: " + v);
			return 0;
		}
	}
}

/*
   
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
	<OutputType>Library</OutputType>
    <TargetFramework>net5.0</TargetFramework>
	<PlatformTargeet>x86</PlatformTargeet>
  </PropertyGroup>

</Project>
*/



/*
<?xml version="1.0" encoding="utf-8"?>
<Project ToolsVersion="15.0" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <Import Project="$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props" Condition="Exists('$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props')" />
  <PropertyGroup>
    <Configuration Condition=" '$(Configuration)' == '' ">Debug</Configuration>
    <Platform Condition=" '$(Platform)' == '' ">AnyCPU</Platform>
    <ProjectGuid>{9CA0A2DD-9EB8-4E50-ACF4-180355A55DDB}</ProjectGuid>
    <OutputType>Library</OutputType>
    <AppDesignerFolder>Properties</AppDesignerFolder>
    <RootNamespace>KSR</RootNamespace>
    <AssemblyName>dllcs</AssemblyName>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
    <FileAlignment>512</FileAlignment>
    <Deterministic>true</Deterministic>
  </PropertyGroup>
  <PropertyGroup Condition=" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' ">
    <DebugSymbols>true</DebugSymbols>
    <DebugType>full</DebugType>
    <Optimize>false</Optimize>
    <OutputPath>bin\Debug\</OutputPath>
    <DefineConstants>DEBUG;TRACE</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
    <PlatformTarget>x86</PlatformTarget>
  </PropertyGroup>
  <PropertyGroup Condition=" '$(Configuration)|$(Platform)' == 'Release|AnyCPU' ">
    <DebugType>pdbonly</DebugType>
    <Optimize>true</Optimize>
    <OutputPath>bin\Release\</OutputPath>
    <DefineConstants>TRACE</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
  </PropertyGroup>
  <ItemGroup>
    <Reference Include="System" />
    <Reference Include="System.Core" />
    <Reference Include="System.Xml.Linq" />
    <Reference Include="System.Data.DataSetExtensions" />
    <Reference Include="Microsoft.CSharp" />
    <Reference Include="System.Data" />
    <Reference Include="System.Net.Http" />
    <Reference Include="System.Xml" />
  </ItemGroup>
  <ItemGroup>
    <Compile Include="Class1.cs" />
  </ItemGroup>
  <Import Project="$(MSBuildToolsPath)\Microsoft.CSharp.targets" />
</Project>
*/
