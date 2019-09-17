#!/bin/bash

SdkVersion="3.0.100-rc1-014190"
./dotnet-install.sh -Version $SdkVersion
export PATH="$PATH:$HOME/.dotnet"
