docfx ./docfx_project/docfx.json

Copy-Item -Path ./docfx_project/_site/* -Destination ./docs -Recurse -Force
