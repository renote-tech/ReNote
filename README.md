<h1 align="center">
  <b>ReNote</b>
  <br>
  <sub><sup><b>Goes<i> Beyond Limitations</i></b></sup></sub>
</h1>

<p align="center">
  ReNote brings you an easy-to-use timetable software, including all the tools you need to empower your productivity as a teacher or student.
  <br><br>
  <img src="Documentation/show_new.png">
  <br><br>
</p>

# Build ReNote
Navigate to the Tools folder; you'll find 4 scripts, you can use either the version for Windows (Batch scripts) or the version for Linux/macOS (Bash scripts). Here are some examples for the `build_project` script:

### Usage
```
build_project.[sh/bat] [client|server] [Debug|Release] [win|linux|osx] [x86|x64] [netX.X]
```

### Windows
```
build_project.bat client Debug win x86
```

### Linux
```
./build_project.sh server Release linux x64
```

Notes:
  - You need the .NET SDK 7.0+ to run the commands above. Download <a href="https://aka.ms/netcore">here</a>.
  - It is not **yet** recommended to build for macOS as I will not provide any support in the current state. (Scripts may not work as expected as well).
  - If you changed the .NET Version in one of the projects, make sure to specify the new .NET Version as well as the fifth argument such as `net7.0`.

# Run ReNote
If there is a pre-built version of ReNote available in the Releases <a href="https://github.com/renote-tech/ReNote/releases">section</a>, you'll need .NET Runtime 7.0+ to run it.
Download <a href="https://dotnet.microsoft.com/en-us/download/dotnet/7.0">here</a>.

# Features
### → Server
 - Full support for static websites & Vue.js
 - Integrated API server
 - Integrated socket server
 - Dedicated database system
 
### → Client
 - Easy-to-use interface
 - Useful tools for productivity.

# Contribute
If you have suggestions, bugs or problems with ReNote, you can let us know via the <a href="https://discord.gg/Z2wh3CHusT">Discord server</a> or by creating an issue. You can also donate to our <a href="">patreon</a> (Not available yet).

# License
- ReNote is licensed under the <a href="LICENSE">MIT</a> License.
- Newtonsoft.Json is licensed under the <a href="https://github.com/JamesNK/Newtonsoft.Json/blob/master/LICENSE.md">MIT</a> License.
- ProtoBuf is licensed under the <a href="https://github.com/protocolbuffers/protobuf/blob/main/LICENSE">BSD</a> License.
- ProtoBuf.NET is licensed under the <a href="https://github.com/protobuf-net/protobuf-net/blob/main/Licence.txt">Apache</a> License.
- Avalonia is licensed under the <a href="https://github.com/AvaloniaUI/Avalonia/blob/master/licence.md">MIT</a> License.
