from TheCenterAPI.APIServer import Module, PModule, Run, data, ui
from TheCenterAPI.BaseUICom import Button, Input, Text

if __name__ == "__main__":
    Run()


@Module
class HelloPythonModule(PModule):

    @ui
    def hello(self):
        return Text("Hello Python!")

    @ui
    def clickMe(self):
        return Button().text("点我Hello").onClick(self.OnClick)

    @ui
    def input1(self) -> Input:
        return Input().placeholder("输入环境变量名").onChange(self.OnInput1Change)

    @ui
    def showVariable(self):
        return Text("未知")

    @data
    def count():
        return int

    @data
    def inputtext():
        return str

    def __init__(self) -> None:
        super().__init__()

    def buildInterface(self):
        self.isKnown()
        self.hello.text(str(self.count))
        self.input1.text(self.inputtext)
        
        return [
            self.hello.ui,
            self.clickMe.ui,
            self.input1.ui,
            Text("环境变量查看").ui,
            self.showVariable.ui
        ]

    def OnClick(self):
        self.count = self.count + 1
        self.syncUI()

    def customEvent(self, control, eventname, args):
        pass

    def OnInput1Change(self, newtext: str):
        self.inputtext = newtext
        self.syncUI()
    
    def isKnown(self):
        glo = self.getGlobalVariable(self.inputtext)
        if glo is not None:
            self.showVariable.text(glo)
            return True
        else:
            self.showVariable.text("未知")
            return False

    def onLoad(self):
        self.ensureValue([["count", 0], ["inputtext", ""]])

    def onGlobalVariableChange(self):
        self.syncUI()
