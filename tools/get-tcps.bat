@echo off
setlocal enabledelayedexpansion

REM Limitar a execução a 12 vezes (1 hora com intervalos de 5 minutos)
set "counter=0"

:loop
REM Substituir as barras da data por hifens para criar um nome de arquivo válido
set "currentDate=%date:/=-%"
set "currentTime=%time::=-%"
set "fileName=netstat_!currentDate!_!currentTime!.txt"

REM Remover caracteres inválidos do nome do arquivo
set "fileName=!fileName: =!"

REM Executar o comando e salvar no arquivo
netstat -an >> "!fileName!"

REM Incrementar o contador
set /a counter+=1

REM Verificar se já executou 12 vezes
if !counter! geq 18 goto end

REM Aguardar 5 minutos (300 segundos)
timeout /t 300 /nobreak >nul

REM Voltar para o início do loop
goto loop

:end
echo Finalizado após 1 hora.