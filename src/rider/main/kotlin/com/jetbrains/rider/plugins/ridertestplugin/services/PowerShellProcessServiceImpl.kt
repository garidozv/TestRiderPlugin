package com.jetbrains.rider.plugins.ridertestplugin.services

import com.intellij.execution.configurations.GeneralCommandLine
import com.intellij.execution.process.ProcessHandler
import com.intellij.openapi.project.Project
import com.jetbrains.rider.run.TerminalProcessHandler
import java.nio.charset.StandardCharsets

class PowerShellProcessServiceImpl(project: Project): PowerShellProcessService {
    override val processHandler: ProcessHandler

    init {
        val commandLine = GeneralCommandLine("powershell.exe")
        commandLine.charset = StandardCharsets.UTF_8
        commandLine.setWorkDirectory(project.basePath)

        processHandler = TerminalProcessHandler(project, commandLine)

        executeCommand("function prompt { \"PM> \" }\n")
    }

    override fun executeCommand(command: String) {
        processHandler.processInput?.write(command.toByteArray(StandardCharsets.UTF_8))
        processHandler.processInput?.flush()
    }

}