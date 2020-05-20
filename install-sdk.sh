#!/bin/bash

SdkVersion="3.1.300"
./dotnet-install.sh -Version $SdkVersion
export PATH="$PATH:$HOME/.dotnet"
