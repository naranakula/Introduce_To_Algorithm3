notes for introduce to algorithm 3.P834


  如何添加管理员运行
        1、右键属性>安全性>启用ClickOnce安全设置,自动在Properties下生成app.manifest
        2、修改app.maifest为 requestedExecutionLevel level="requireAdministrator" uiAccess="false" 
        3、禁用 安全性 ClickOnce安全设置