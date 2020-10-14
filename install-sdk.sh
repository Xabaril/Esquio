#!/bin/bash

SdkVersion="5.0.100-rc.2.20479.15"
./dotnet-install.sh -Version $SdkVersion
export PATH="$PATH:$HOME/.dotnet"
