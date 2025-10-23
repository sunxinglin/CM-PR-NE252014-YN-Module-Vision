
## 工程项目介绍

- AsZero.WebApi: 服务端
- DummyApp: 客户端入口
- SmartPack.FunFlows.Plugins: 和装配流程本身相关的插件，主要定义了流程活动、及其所依赖的服务
- SmartPack.FunFlows.WPF: 主要定义了装配流程的界面视图、视图模型、流程触发逻辑等
- SmartPack.Shared: 一些基本的共享类型


## 编写Fun及其UI界面教程

Fun代表业务流程中的一个具体活动。编写Fun的代码示例，可以参考本代码仓库中HASH为[2277639e4c7b2d07c14f4c67a0edbc84ed26628a]的这次git提交。

### 编写Fun及其依赖的服务

- 编写Fun
- 编写Fun所依赖的服务
- 声明Fun所带来的变化消息
- 如果相关Fun会阻塞相关流程等待特定的信号输入，还需要建立相关的信号监听机制，来触发流程

至此，已经可以在无界面的情况下，运行流程了。

### 为Fun编写界面

下面，让我们为刚刚的Fun建立监控界面：

- 在FlowInspector中，为Fun引起的那些变化消息增加Subject，比如加载进度、用户输入等
- 为Fun编写ViewModel，ViewModel需注解其所关联的Fun类型；并根据上一步的Subject，映射事件带来的视图变化
- 为Fun编写View，并绑定其与ViewModel的关系