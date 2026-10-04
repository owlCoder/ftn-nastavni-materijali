#!/usr/bin/env sh
set -eu

cd "$(dirname "$0")"

for solution in */*.sln; do
  dotnet test "$solution" --nologo
done
