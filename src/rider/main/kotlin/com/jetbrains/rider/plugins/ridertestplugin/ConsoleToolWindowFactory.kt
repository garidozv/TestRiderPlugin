package com.jetbrains.rider.plugins.ridertestplugin

import com.intellij.icons.AllIcons
import com.intellij.openapi.actionSystem.ActionManager
import com.intellij.openapi.project.Project
import com.intellij.openapi.rd.util.lifetime
import com.intellij.openapi.roots.ui.componentsList.components.ScrollablePanel
import com.intellij.openapi.ui.ComboBox
import com.intellij.openapi.ui.SimpleToolWindowPanel
import com.intellij.openapi.wm.ToolWindow
import com.intellij.openapi.wm.ToolWindowFactory
import com.intellij.terminal.TerminalExecutionConsole
import com.intellij.ui.components.JBPanel
import com.intellij.ui.content.ContentFactory
import com.intellij.ui.dsl.builder.actionButton
import com.intellij.ui.dsl.builder.panel
import com.jetbrains.rider.plugins.ridertestplugin.services.PowerShellProcessService
import com.jetbrains.rider.plugins.testplugin.rd.testModel
import com.jetbrains.rider.projectView.solution
import java.awt.BorderLayout
import javax.swing.DefaultComboBoxModel


class ConsoleToolWindowFactory : ToolWindowFactory {
    override fun createToolWindowContent(project: Project, toolWindow: ToolWindow) {
        val myToolWindow = ConsoleToolWindowContent(project,toolWindow)
        val contentFactory = ContentFactory.getInstance()
        val content = contentFactory.createContent(myToolWindow.getContent(), "", false)
        toolWindow.contentManager.addContent(content)
    }

    class ConsoleToolWindowContent(private val project: Project, toolWindow: ToolWindow) {
        private val contentPanel = SimpleToolWindowPanel(true)

        init {
            contentPanel.layout = BorderLayout()
            contentPanel.toolbar = createToolBar()

            contentPanel.add(createConsolePanel())
        }

        private fun createToolBar() : JBPanel<*> {
            return panel {
                row {
                    comboBox(DefaultComboBoxModel<String>().apply {
                        project.solution.testModel.sources.advise(project.lifetime) {
                            // TODO: Add 'All'
                            removeAllElements()
                            addAll(project.solution.testModel.sources.map { it.name })
                            selectedItem = project.solution.testModel.sources.map { it.name }.firstOrNull();
                        }
                    }).label("Package Sources: ")

                    actionButton(ActionManager.getInstance().getAction("RiderNuGetShowSourcesAction"))
                        .applyToComponent { presentation.icon = AllIcons.General.GearPlain }


                    comboBox(DefaultComboBoxModel<String>().apply {
                        project.solution.testModel.projects.advise(project.lifetime) {
                            removeAllElements()
                            addAll(project.solution.testModel.projects.map { it.name })
                            selectedItem = project.solution.testModel.projects.map { it.name }.firstOrNull();
                        }
                    }).label("Default Projects: ")

                    actionButton(ActionManager.getInstance().getAction("TestPlugin.ClearConsole"))
                    actionButton(ActionManager.getInstance().getAction("TestPlugin.StopConsoleExecution"))
                }
            }
        }

        private fun createConsolePanel() : ScrollablePanel {
            val processService = project.getService(PowerShellProcessService::class.java)
            val processHandler = processService.processHandler
            val terminal = TerminalExecutionConsole(project, processHandler)

            terminal.clear()

            val panel = ScrollablePanel()
            panel.layout = BorderLayout()
            panel.add(terminal.component, BorderLayout.CENTER)

            processHandler.startNotify()

            return panel
        }

        fun getContent() = contentPanel
    }
}
