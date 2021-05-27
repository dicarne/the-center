from TheCenterAPI.APIServer import PModule, Run, reg_module
from TheCenterAPI.BaseUICom import Button, Text

if __name__ == "__main__":
    Run()
    

class HelloPythonModule(PModule):
    hello = Text("textUI", "Hello Python!")

    clickMe = Button("clickMe").text("点我Hello").onClick("OnClick")

    def buildInterface(self):
        self.hello.text(str(self.getValue("count")))
        return [self.hello.ui, self.clickMe.ui]

    def OnClick(self):
        print("Click!")
        self.ensureValue([["count", 0]])
        self.setValue("count", self.getValue("count") + 1)
        self.syncUI()

    def customEvent(self, control, eventname, args):
        print(control)
    
    def onLoad(self):
        self.ensureValue([["count", 0]])

reg_module("#", HelloPythonModule)