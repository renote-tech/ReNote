<h1 align="center">
  <b>ReNote</b>
  <br>
  <sub><sup><b><i>Beyond limitations</i></b></sup></sub>
</h1>

<p align="center">
  ReNote offers a kit to interact with <a href="https://www.index-education.com/fr/logiciel-gestion-vie-scolaire.php">Pronote</a> APIs in the easiest way possible as well as a suit of tools to boost your productivity.
  <br><br>
  <img src="documentation/show.png">
  <br><br>
</p>

# Build ReNote
Use the `cd` command to navigate either to the `Client` directory or to the `Server` one, next run the command below for the corresponding platform.

### Windows
```bash
dotnet build --runtime win-x64
```

### Linux
```bash
dotnet build --runtime linux-x64
```

### macOS
```bash
dotnet build --runtime osx-x64
```

Notes:
   - You can add the `--self-contained` parameter to add necessary dependencies to the output folder.
   - You can also build for 32bit platforms by changing the suffix `-x64` to `-x86` (Windows only).
   - Same apply for arm64 platforms by changing the suffix to `-arm64`. 


# Features
### → Server
 - Full support for static websites & Vue.js
 - Integrated API server
 - Integrated Socket server
 
### → Client
 - Easy-to-use interface
 - Full Pronote Implementation
 - New tools

# Contribute
If you have suggestions, bugs or problems with ReNote, you can let us know via the <a href="https://discord.gg/Z2wh3CHusT">Discord server</a> or by creating an issue. You can also donate to our <a href="">patreon</a> (Not available yet).

# License
ReNote is licensed under the <a href="LICENSE">MIT</a> License.
