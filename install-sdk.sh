#!/bin/bash

SdkVersion="3.1.401"
./dotnet-install.sh -Version $SdkVersion
export PATH="$PATH:$HOME/.dotnet"
