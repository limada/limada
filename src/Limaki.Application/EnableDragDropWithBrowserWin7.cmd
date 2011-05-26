@echo run as administrator!
set /P binDir=Type the directory of limada-exe:
dir %binDir%
icacls %binDir%\*.* /setintegritylevel hiqh
pause