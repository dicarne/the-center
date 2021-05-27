from TheCenterAPI.APIServer import PModule, Run, data, reg_module, ui
from TheCenterAPI.BaseUICom import Button, Input, Text

if __name__ == "__main__":
    Run()
    

class HelloPythonModule(PModule):

    @ui
    def hello():
        return Text("Hello Python!")
    @ui
    def clickMe():
        return Button().text("点我Hello").onClick("OnClick")
    @ui
    def input1():
        return Input().placeholder("haha").onChange("OnInput1Change")

    @data
    def count():
        return int
    @data
    def inputtext():
        return str

    def __init__(self) -> None:
        super().__init__()

    def buildInterface(self):
        self.hello.text(str(self.count))
        self.input1.text(self.inputtext)
        return [self.hello.ui, self.clickMe.ui, self.input1.ui]

    def OnClick(self):
        self.count = self.count + 1
        self.syncUI()

    def customEvent(self, control, eventname, args):
        pass

    def OnInput1Change(self, newtext):
        self.inputtext = newtext
    
    def onLoad(self):
        self.ensureValue([["count", 0], ["inputtext", ""]])

    


reg_module("#", HelloPythonModule)