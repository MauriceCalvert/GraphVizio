FOR /F %%G IN ('DIR bin* /B /A:D /S') DO RMDIR /S /Q "%%G"
FOR /F %%G IN ('DIR obj* /B /A:D /S') DO RMDIR /S /Q "%%G"
FOR /F %%G IN ('DIR testresults* /B /A:D /S') DO RMDIR /S /Q "%%G"
DEL *.log /Q /S
DEL *.tmp /Q /S
DEL *.pdb /Q /S
DEL *.cache /Q /S
