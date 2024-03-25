#! /bin/bash

csc -out:CMS.exe -r:System.Xml.dll -r:System.Xml.Serialization.dll -r:chilkatMono.dll -r:System.Windows.Forms.dll -r:System.Drawing.dll *.cs
