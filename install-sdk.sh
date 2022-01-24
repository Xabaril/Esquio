#!/bin/bash

SdkVersion="6.0.101"
./dotnet-install.sh -Version $SdkVersion
export PATH="$PATH:$HOME/.dotnet"
