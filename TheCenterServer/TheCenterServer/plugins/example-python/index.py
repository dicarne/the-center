from TheCenterAPI.APIServer import PModule, Run, reg_module
from TheCenterAPI.BaseUICom import Button, Text

f = open("test.txt", "w")
f.writelines("run!")
f.flush()

if __name__ == "__main__":
    Run()

class HelloPythonModule(PModule):
    hello = Text("textUI", "Hello Python!")

    button = Button("clickMe").text("点我Hello").onClick("OnClick")

    def buildInterface(self):
        return [self.hello.ui, self.button.ui, Button("111").text("dynamic").onClick("c:111").ui]

    def OnClick(self):
        print("Click!")

    def customEvent(self, control, eventname, args):
        print(control)

reg_module("#", HelloPythonModule)