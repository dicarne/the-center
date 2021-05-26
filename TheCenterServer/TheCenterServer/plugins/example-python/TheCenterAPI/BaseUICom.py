from typing import Any, Dict, List, Optional
from pydantic import BaseModel

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
    def __init__(self, type, id) -> None:
        self.style = {}
        self.prop = {}
        self.event = []
        self.id = id
        self.type = type

class UIBase:
    ui: UICom
    events: Dict[str, str]
    def __init__(self, type, id) -> None:
        self.ui = UICom(type, id)
        self.events = {}
    
    def bindEvent(self, eventName, method):
        if eventName not in self.events:
            self.ui.event.append(eventName)
        self.events[eventName] = method

class Text(UIBase):
    def __init__(self, id, text) -> None:
        super().__init__("text", id)
        self.ui.prop['text'] = text

    def text(self, content):
        self.ui.prop['text'] = content
        return self

class Button(UIBase):
    def __init__(self, id) -> None:
        super().__init__("button", id)
    
    def text(self, content):
        self.ui.prop['text'] = content
        return self
    
    def onClick(self, func):
        self.bindEvent("onClick", func)
        return self

