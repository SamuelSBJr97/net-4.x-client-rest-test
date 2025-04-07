# Configuração do executável e do intervalo de monitoramento
$executablePath = "C:\Users\samue\source\repos\SamuelSBJr97\net-4.x-client-rest-test\src\client\ApiClient46.Test\bin\Debug\net462\ApiClient46.Test.exe"
$logDirectory = "C:\Users\samue\source\repos\SamuelSBJr97\net-4.x-client-rest-test\tools\logs"
$stdoutLogFile = "C:\Users\samue\source\repos\SamuelSBJr97\net-4.x-client-rest-test\tools\logs\executable_stdout_log.txt"
$stderrLogFile = "C:\Users\samue\source\repos\SamuelSBJr97\net-4.x-client-rest-test\tools\logs\executable_stderr_log.txt"
$intervalSeconds = 60 # 1 minuto

# Garantir que o diretório de logs exista
if (!(Test-Path -Path $logDirectory)) {
    New-Item -ItemType Directory -Path $logDirectory | Out-Null
}

# Iniciar o executável e redirecionar saída padrão e de erro para arquivos separados
Start-Process -FilePath $executablePath -NoNewWindow -RedirectStandardOutput $stdoutLogFile -RedirectStandardError $stderrLogFile

# Função para obter informações de uso de CPU e memória
function Get-ProcessInfo {
    $process = Get-Process | Where-Object { $_.Path -eq $executablePath }
    if ($process) {
        return @{
            CPU = $process.CPU
            Memory = [math]::Round($process.WorkingSet / 1MB, 2)
        }
    } else {
        return $null
    }
}

# Loop de monitoramento
while ($true) {
    # Obter data e hora atual
    $timestamp = Get-Date -Format "yyyy-MM-dd_HH-mm-ss"
    $logFilePath = Join-Path -Path $logDirectory -ChildPath "monitor_log_$timestamp.txt"

    # Obter informações de portas abertas
    $netstat = netstat -an

    # Contar conexões estabelecidas (requests)
    $establishedConnections = ($netstat | Select-String "ESTABLISHED").Count

    # Filtrar apenas conexões em estado LISTENING
    $listeningPorts = $netstat

    # Obter informações de uso de CPU e memória
    $processInfo = Get-ProcessInfo

    # Escrever no log
    if ($processInfo) {
        $logEntry = @"
Timestamp: $timestamp
Open Ports:
$($listeningPorts -join "`n")
Established Connections (Requests): $establishedConnections
CPU Usage: $($processInfo.CPU) seconds
Memory Usage: $($processInfo.Memory) MB
"@
    } else {
        $logEntry = "[$timestamp] Process not found."
    }

    Add-Content -Path $logFilePath -Value $logEntry

    # Aguardar o intervalo definido
    Start-Sleep -Seconds $intervalSeconds
}