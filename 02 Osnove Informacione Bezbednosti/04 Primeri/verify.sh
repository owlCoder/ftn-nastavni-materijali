#!/usr/bin/env sh
set -eu

for project in */*.csproj; do
  dotnet build "$project" --nologo
done
