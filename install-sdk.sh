#!/bin/bash

SdkVersion="3.0.100-preview9-014004"
./dotnet-install.sh -Version $SdkVersion
export PATH="$PATH:$HOME/.dotnet"
