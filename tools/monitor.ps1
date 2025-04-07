# Configuração do executável e do intervalo de monitoramento
$executablePath = "C:\Users\samue\source\repos\SamuelSBJr97\net-4.x-client-rest-test\src\client\ApiClient46.Test\bin\Debug\net462\ApiClient46.Test.exe"
$logFilePath = "C:\Users\samue\source\repos\SamuelSBJr97\net-4.x-client-rest-test\tools\monitor_log.txt"
$executableLogFile = "C:\Users\samue\source\repos\SamuelSBJr97\net-4.x-client-rest-test\tools\executable_output_log.txt"
$intervalSeconds = 300 # 5 minutos

# Iniciar o executável e redirecionar saída para o arquivo de log
Start-Process -FilePath $executablePath -NoNewWindow -RedirectStandardOutput $executableLogFile -RedirectStandardError $executableLogFile

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
    $timestamp = Get-Date -Format "yyyy-MM-dd HH:mm:ss"

    # Obter informações de portas abertas
    $netstat = netstat -an | Select-String "LISTENING"

    # Obter informações de uso de CPU e memória
    $processInfo = Get-ProcessInfo

    # Escrever no log
    if ($processInfo) {
        $logEntry = @"
Timestamp: $timestamp
Open Ports:
$($netstat -join "`n")
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