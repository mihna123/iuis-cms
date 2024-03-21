#! /bin/bash

csc -out:CMS.exe -r:System.Windows.Forms.dll -r:System.Drawing.dll *.cs
