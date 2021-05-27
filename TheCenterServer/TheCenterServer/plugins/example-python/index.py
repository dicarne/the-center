from TheCenterAPI.APIServer import PModule, Run, reg_module
from TheCenterAPI.BaseUICom import Button, Input, Text

if __name__ == "__main__":
    Run()
    

class HelloPythonModule(PModule):
    hello = Text("textUI", "Hello Python!")
    clickMe = Button("clickMe").text("点我Hello").onClick("OnClick")
    input1 = Input("input1").placeholder("haha").onChange("OnInput1Change")

    def buildInterface(self):
        self.hello.text(str(self.getValue("count")))
        self.input1.text(self.getValue("inputtext"))
        return [self.hello.ui, self.clickMe.ui, self.input1.ui]

    def OnClick(self):
        print("Click!")
        self.ensureValue([["count", 0]])
        self.setValue("count", self.getValue("count") + 1)
        self.syncUI()

    def customEvent(self, control, eventname, args):
        print(control)

    def OnInput1Change(self, newtext):
        self.setValue("inputtext", newtext)
    
    def onLoad(self):
        self.ensureValue([["count", 0], ["inputtext", ""]])

reg_module("#", HelloPythonModule)