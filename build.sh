
#! /bin/bash
# Linux build script

rm -R -f output
mkdir output

cp -R data output

# Dll
cp ManicDigger.Common/bin/Release/ManicDigger.Common.dll output

# Scripting API
cp ManicDigger.ScriptingApi/bin/Release/ManicDigger.ScriptingApi.dll output

# Game ManicDigger/bin/ManicDigger/bin/Release/

# Server
cp ManicDigger.Server/bin/Release/*.exe output

# Monster editor
cp ManicDigger.MonsterEditor/bin/Release/*.exe output

# Server Mods
cp -R ManicDigger.Common/Server/Mods output

# Third-party libraries
cp Lib/* output

# NuGet packages
cp packages/OpenTK.3.3.3/lib/net20/OpenTK.dll output
cp packages/OpenTK.3.3.3/content/OpenTK.dll.config output
cp packages/protobuf-net.2.4.0/lib/net40/protobuf-net.dll output
cp packages/Newtonsoft.Json.13.0.3/lib/net40/Newtonsoft.Json.dll output

cp COPYING.md output/credits.txt
# Nuget packages 2 time sometimes when developing dll's go missing? maybe its monodevelop thing
cp packages/OpenTK.3.3.3/lib/net20/OpenTK.dll ManicDigger/bin/Release/    
cp packages/OpenTK.3.3.3/content/OpenTK.dll.config ManicDigger/bin/Release/ 
cp packages/protobuf-net.2.4.0/lib/net40/protobuf-net.dll ManicDigger/bin/Release/
cp packages/Newtonsoft.Json.13.0.3/lib/net40/Newtonsoft.Json.dll ManicDigger/bin/Release/

rm -f output/*vshost.exe

# pause
