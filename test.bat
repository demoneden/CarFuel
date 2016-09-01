@echo off
packages\xunit.runner.console.2.1.0\tools\xunit.console.exe ^
CarFuel.Tests\bin\Debug\CarFuel.Tests.dll ^
-parallel all ^
-html Result.html ^
-nologo
@echo on