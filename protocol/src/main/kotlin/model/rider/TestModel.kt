package model.rider

import com.jetbrains.rd.generator.nova.Ext
import com.jetbrains.rd.generator.nova.PredefinedType
import com.jetbrains.rd.generator.nova.async
import com.jetbrains.rd.generator.nova.const
import com.jetbrains.rd.generator.nova.csharp.CSharp50Generator
import com.jetbrains.rd.generator.nova.field
import com.jetbrains.rd.generator.nova.kotlin.Kotlin11Generator
import com.jetbrains.rd.generator.nova.list
import com.jetbrains.rd.generator.nova.property
import com.jetbrains.rd.generator.nova.setting
import com.jetbrains.rider.model.nova.ide.SolutionModel

class TestModel : Ext(SolutionModel.Solution) {

    private val ProjectInfo = structdef {
        field("id", PredefinedType.guid)
        field("name", PredefinedType.string)
    }
    
    private val SourceInfo = structdef {
        field("id", PredefinedType.string)
        field("name", PredefinedType.string)
    }

    init {
        setting(Kotlin11Generator.Namespace, "com.jetbrains.rider.plugins.testplugin.rd")
        setting(CSharp50Generator.Namespace, "JetBrains.Rider.Plugins.TestPlugin.Rd")

        list("projects", ProjectInfo)
        list("sources", SourceInfo)
    }
}