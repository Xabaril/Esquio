#!/bin/bash

SdkVersion="3.0.100"
./dotnet-install.sh -Version $SdkVersion
export PATH="$PATH:$HOME/.dotnet"
