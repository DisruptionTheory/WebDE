﻿using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using SharpKit.JavaScript;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("WebDE")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("CodStore Israel Ltd.")]
[assembly: AssemblyProduct("WebDE")]
[assembly: AssemblyCopyright("Copyright © CodStore Israel Ltd. 2010")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

[assembly: JsMergedFile(Filename = "scripts/WebDE.js", Sources = new string[] { 
    "scripts/Main.js", "scripts/AI.js", "scripts/Rendering.js", "scripts/Animation.js", "scripts/Objects.js", "scripts/Misc.js",
    "scripts/GUI.js", "scripts/Networking.js", "scripts/Helpah.js",
    "scripts/WebDE.Input.js", "scripts/WebDE.Clock.js", "scripts/WebDE.Net.js", "scripts/WebDE.Net.GameClient.js", "scripts/WebDE.Audio.js" })]
//[assembly: JsMergedFile(Filename = "MySite.min.js", Sources = new string[] { "MySite.js" }, Minify = true)]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("f52d909d-b3c5-4899-94a5-1a2271a4aad5")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Revision and Build Numbers 
// by using the '*' as shown below:
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
