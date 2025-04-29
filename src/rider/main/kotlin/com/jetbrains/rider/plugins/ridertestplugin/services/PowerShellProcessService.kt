package com.jetbrains.rider.plugins.ridertestplugin.services

import com.intellij.execution.process.ProcessHandler

interface PowerShellProcessService {
    val processHandler: ProcessHandler

    fun executeCommand(command: String);
}