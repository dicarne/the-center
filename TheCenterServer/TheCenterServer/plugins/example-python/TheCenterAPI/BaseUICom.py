from typing import Any, Dict, List, Optional
from pydantic import BaseModel
import json


class UIEventReqBody(BaseModel):
    control: str
    eventname: str
    args: Optional[List[str]]

class UICom:
    id: str
    type: str
    style: Dict[str, str]
    prop: Dict[str, str]
    event: List[str]
    def __init__(self, type) -> None:
        self.style = {}
        self.prop = {}
        self.event = []
        self.type = type
    

class UIBase:
    ui: UICom
    events: Dict[str, Any]
    def __init__(self, type) -> None:
        self.ui = UICom(type)
        self.events = {}
    
    def bindEvent(self, eventName, method):
        if eventName not in self.events:
            self.ui.event.append(eventName)
        self.events[eventName] = method

    def set_id(self, newid):
        self.ui.id = newid

class Text(UIBase):
    def __init__(self, text) -> None:
        super().__init__("text")
        self.ui.prop['text'] = text

    def text(self, content):
        self.ui.prop['text'] = content
        return self

class Button(UIBase):
    def __init__(self) -> None:
        super().__init__("button")
    
    def text(self, content):
        self.ui.prop['text'] = content
        return self
    
    def onClick(self, func):
        self.bindEvent("onClick", func)
        return self

class CheckBox(UIBase):
    def __init__(self) -> None:
        super().__init__("checkBox")

    def value(self, v: bool):
        self.ui.prop["value"] = json.dumps(v)
        return self

    def onChange(self, onchange: str):
        self.bindEvent("onChange", onchange)
        return self

class Group(UIBase):
    def __init__(self) -> None:
        super().__init__("group")
    
    def children(self, childs: List[UICom]):
        self.ui.prop["children"] = json.dumps(childs)
        return self
    
    def horizon(self, hor: bool):
        self.ui.prop["hor"] = json.dumps(hor)
        return self

    def spacing(self, hor_value: float, ver_value: Optional[float]):
        s = [hor_value]
        if(ver_value is not None):
            s.append(ver_value)
        self.ui.prop["spacing"] = json.dumps(s)

class Input(UIBase):
    def __init__(self) -> None:
        super().__init__("input")
    
    def text(self, content):
        self.ui.prop['text'] = content
        return self

    def onChange(self, onchange: str):
        self.bindEvent("onChange", onchange)
        return self

    def placeholder(self, content):
        self.ui.prop['placeholder'] = content
        return self

class More(UIBase):
    def __init__(self) -> None:
        super().__init__("more")

    def text(self, content):
        self.ui.prop['text'] = content
        return self

    def isshow(self, newtext: bool):
        self.ui.prop["isshow"] = json.dumps(newtext)
        return self
 
    def onClick(self, func):
        self.bindEvent("onClick", func)
        return self

class Transfer(UIBase):
    def __init__(self) -> None:
        super().__init__("transfer")

    def onChange(self, onchange: str):
        self.bindEvent("onChange", onchange)
        return self

    def text(self, content):
        self.ui.prop['text'] = content
        return self
 
    class ShowData:
        ava: List[str]
        all: List[str]

    def onShow(self, onShow: str):
        self.bindEvent("onShow", onShow)
        return self
        
    def avaliable(self, newt: List[str]):
        self.ui.prop["avaliable"] = json.dumps(newt)
        return self

    def all(self, newt: List[str]):
        self.ui.prop["all"] = json.dumps(newt)
        return self

    def type(self, newt: str):
        self.ui.prop["type"] = newt
        return self