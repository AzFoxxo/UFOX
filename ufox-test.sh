#!/usr/bin/env sh

cd src
dotnet run -- build -s ../test/main.ufox -t visofox16 -o ../builds/rom.bin