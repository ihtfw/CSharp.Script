# CSharp.Script
Simple library, that helps compile and execute C# code in runtime. For compile is used [Microsoft.CodeAnalysis.CSharp](https://www.nuget.org/packages/Microsoft.CodeAnalysis.CSharp) pacakge so we can use up to C# 9.

# Main idea
All you have to do is to write methods, properties and fields, everything else will be done by framework.
```csharp
public string Foo(){ return "HelloWorld; }
```
would be compiled to
```csharp
using System;
using System.Text;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace UNGb67bdaca8048407eb33a8ec35ededec0
{
    public class UNG9088b1f3fd194b02ac8f2f8db543eaae
    {
        public string Foo() { return "HelloWorld"; }
    }
}
```
for each compilation unique namespace and class name are generated.

# Basic usage
```csharp
//build caching compiler, that would return same assembly for 
var compilerBuilder = new CompilerBuilder();
var compiler = compilerBuilder.Build(); 
//compile
var assembly = compiler.Compile(@"public string Foo(){ return ""Hello World!""; }");
//script container provide methods to interact with methods, properties and fields
var scriptContainer = new ScriptContainer(assembly);
var returnValue = scriptContainer.Methods.Get("Foo").Invoke<string>();

Assert.AreEqual("Hello World!", returnValue);
```

# State
Each ScriptContatiner has instance of class that was compiled. It means that it has state!
```csharp
var compilerBuilder = new CompilerBuilder();
var compiler = compilerBuilder.Build(); 

var sourceCodeBuilder = new StringBuilder();
sourceCodeBuilder.AppendLine(@"public int StateProp { get; set; }");
sourceCodeBuilder.AppendLine(@"public void IncrementStateProp() { StateProp++; }");
var assembly = compiler.Compile(sourceCodeBuilder.ToString());
var scriptContainer = new ScriptContainer(assembly);

//default value StateProp is 0
var stateProp = scriptContainer.Properties.Get("StateProp");
Assert.AreEqual(0, stateProp.Get<int>());

//invrement StateProp by invoking method IncrementStateProp()
scriptContainer.Methods.Get("IncrementStateProp").Invoke();
Assert.AreEqual(1, stateProp.Get<int>());
```

# I need references to my own assemblies!
```csharp
var compilerBuilder = new CompilerBuilder();
//if you need extra using
compilerBuilder.Usings.Add("MySpecialUsing");
//if you need extra reference
compilerBuilder.References.Add(myAssembly);
//if you want to add usings and references by type
compilerBuilder.Types.Add(typeof(MySuperClass));
```

# Naming conventions
### SetDefaultValues method
There is special method SetDefaultValues()
```csharp
public void SetDefaultValues(){ PropertyThatShouldBeInitialized = 3; }
public int PropertyThatShouldBeInitialized { get; set; }
```
that would be invoked in ctor of ScriptContainer if exists
### Extend class
```csharp
extend MySuperClass;

public string Foo(){ return "HelloWorld; }
```
would be compiled to
```csharp
using System;
using System.Text;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using MyAssemblyNamespace;

namespace UNGb67bdaca8048407eb33a8ec35ededec0
{
    public class UNG9088b1f3fd194b02ac8f2f8db543eaae : MySuperClass 
    {
        public string Foo() { return "HelloWorld"; }
    }
}
```
don't forget to add using and refence to MySuperClass in such scenario.

### Build package
dotnet pack -c Release -p:PackageVersion=2021.3.17.1