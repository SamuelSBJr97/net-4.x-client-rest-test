@echo off
setlocal enabledelayedexpansion

REM Substituir as barras da data por hifens para criar um nome de arquivo válido
set "currentDate=%date:/=-%"
netstat -an >> netstat_!currentDate!.txt