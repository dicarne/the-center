## Python Plugin
### 创建一个Python插件
1. 新建工程目录，在其中添加TheCenterAPI（当前仅支持手动复制到工程目录）
2. 创建pmodule.json，内容如下示例
```json
{
    "main": "index.py", //开始文件
    "interpreter": "python3",  //解释器，需要用户安装并可调用
    "singleton": true,  //是否单例，即多个卡片使用同一个进程处理，需要在进程内部自行管理卡片与空间
    "port": 8899 //进程监听端口，通过此端口通信，不要和其他插件相同
}
```
3. 创建`index.py`，编写如下内容。
```python
#导入基础设施
from TheCenterAPI.APIServer import Module, PModule, Run, data, ui
#导入UI控件
from TheCenterAPI.BaseUICom import Button, Input, Text

#开始运行服务器
if __name__ == "__main__":
    Run()

#此装饰器注册模块，必须
@Module
class HelloPythonModule(PModule):
#模块必须继承自PModule

    #ui装饰器声明这是个UI控件，自动注册id，必须
    @ui
    def hello(self):
        return Text("Hello Python!")
    @ui
    def clickMe(self):
        return Button().text("点我Hello").onClick(self.OnClick)
    @ui
    def input1(self):
        return Input().placeholder("haha").onChange(self.OnInput1Change)

    #data装饰器声明这是个持久储存的数据项，自动绑定，必须
    @data
    def count():
        return int
    @data
    def inputtext():
        return str

    def __init__(self) -> None:
        super().__init__()

    #重写此函数，构建用户界面，需要返回一个列表
    def buildInterface(self):
        self.hello.text(str(self.count))
        self.input1.text(self.inputtext)
        return [self.hello.ui, self.clickMe.ui, self.input1.ui]

    #自定义的点击回调函数，在控件中绑定
    def OnClick(self):
        self.count = self.count + 1
        #调用syncUI，将后端的数据更改同步到用户界面上
        self.syncUI()

    def OnInput1Change(self, newtext):
        self.inputtext = newtext

    #重写此函数，可自行处理未被处理的事件
    #通常动态生成的UI控件产生的事件会出现在这里
    def customEvent(self, control, eventname, args):
        pass

    #重写此函数，在初始化时调用
    def onLoad(self):
        self.ensureValue([["count", 0], ["inputtext", ""]])


```