from TheCenterAPI.ServerData import BoardDesc, WorkspaceDesc
from typing import Any, Dict, List
from TheCenterAPI.BaseUICom import UIBase, UIEventReqBody
from fastapi import FastAPI
import uvicorn
import json
import requests

app = FastAPI()


def Run():
    config = json.load(open("pmodule.json"))
    uvicorn.run("TheCenterAPI.APIServer:app",
                host="127.0.0.1", port=config["port"], reload=True)


URL = "http://127.0.0.1:5000/PluginModule/"


class PModule:
    id: str
    workspaceID: str
    board: BoardDesc
    values: Dict[str, Any] = {}

    def HandleUIEvent(self, body: UIEventReqBody):
        if hasattr(self, body.control):
            ui = getattr(self, body.control)
            if body.args:
                getattr(self, ui.events[body.eventname])(*body.args)
            else:
                getattr(self, ui.events[body.eventname])()
        else:
            self.customEvent(body.control, body.eventname, body.args)

    def buildInterface():
        pass

    def customEvent(self, control, eventname, args):
        pass

    def getValue(self, key: str, force: bool = False):
        # if not force:
        #    if key in self.values:
        #        return self.values[key]
        req = requests.get(URL + "GetValue?work="+self.workspaceID +
                           "&board="+self.board.id+"&key="+key)
        v = req.json()["value"]
        self.values[key] = json.loads(v)
        return self.values[key]

    def setValue(self, key: str, value: Any):
        req = requests.post(URL+"SetValue?work="+self.workspaceID +
                            "&board="+self.board.id+"&key="+key, json={"value": json.dumps(value)})
        self.values[key] = value

    def ensureValue(self, keys: List[List[Any]]):
        for i in range(0, len(keys)):
            keys[i][1] = json.dumps(keys[i][1])
        req = requests.post(URL+"EnsureValue?work="+self.workspaceID +
                            "&board="+self.board.id, json={"value": keys})

    def syncUI(self):
        req = requests.post(URL+"SyncUI?work="+self.workspaceID +
                            "&board="+self.board.id)

    def onLoad(self):
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
    b.onLoad()
    return {
        "success": True
    }


@app.get("/interface")
def interface(work: str, board: str):
    if work not in _workspaces or board not in _workspaces[work].boards:
        req = requests.get(URL+"GetBoardDesc?work="+work +
                           "&board="+board)
        body = req.json()
        desc = BoardDesc()
        desc.cardType = body["cardType"]
        desc.cName = body["cName"]
        desc.group = body["group"]
        desc.h = body["h"]
        desc.hide = body["hide"]
        desc.id = body["id"]
        desc.prop = body["prop"]
        desc.w = body["w"]
        init(work, board, desc)

    return _workspaces[work].boards[board].buildInterface()


def reg_module(typeName: str, type):
    _reg_module[typeName] = type
