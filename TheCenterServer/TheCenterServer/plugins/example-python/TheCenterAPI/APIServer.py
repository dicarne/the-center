from TheCenterAPI.ServerData import BoardDesc, WorkspaceDesc
from typing import Any, Dict
from TheCenterAPI.BaseUICom import UIBase, UIEventReqBody
from fastapi import FastAPI
import uvicorn
import json

app = FastAPI()

def Run():
    config = json.load(open("pmodule.json"))
    uvicorn.run("TheCenterAPI.APIServer:app",
                host="0.0.0.0", port=config["port"], reload=True)
    


class PModule:
    id: str
    workspaceID: str
    board: BoardDesc
    def HandleUIEvent(self, body: UIEventReqBody):
        print(body.eventname)
        
        if hasattr(self, body.control):
            ui = self[body.control]
            if body.args:
                self[ui.events[body.eventname]](*body.args)
            else:
                self[ui.events[body.eventname]]()
        else:
            self.customEvent(body.control, body.eventname, body.args)

    def buildInterface():
        pass

    def customEvent(self, control, eventname, args):
        pass
    


class WorlspaceIns:
    workspace: WorkspaceDesc
    boards: Dict[str, PModule]

    def __init__(self) -> None:
        self.boards = {}


_workspaces: Dict[str, WorlspaceIns] = {}
_reg_module: Dict[str, Any] = {}


@app.post("/uievent")
def uievent(body: UIEventReqBody, work: str, board: str, ui: str):
    _workspaces[work].boards[board].HandleUIEvent(body)


@app.post("/init")
def init(work: str, board: str, boardBody: BoardDesc):
    if work not in _workspaces:
        _workspaces[work] = WorlspaceIns()
    wk = _workspaces[work]
    if board not in wk.boards:
        wk.boards[board] = _reg_module["#"]()
    else:
        return {
        "success": True
    }
    b = wk.boards[board]
    b.id = board
    b.workspaceID = work
    b.board = boardBody
    return {
        "success": True
    }


@app.get("/interface")
def interface(work: str, board: str):
    return _workspaces[work].boards[board].buildInterface()


def reg_module(typeName: str, type):
    _reg_module[typeName] = type
