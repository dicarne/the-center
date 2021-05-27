from typing import Dict, List, Optional
from pydantic import BaseModel

class GroupBoardColor:
    name: str
    color: str

class BoardDesc(BaseModel):
    cardType: str = ""
    cName: str = ""
    group: Optional[str]
    id: str = ""
    prop: Dict[str, str] = {}
    w: int = 6
    h: int = 1
    hide: bool = False

class WorkspaceDesc:
    wName: str
    id: str
    boards: List[BoardDesc] = []
    groups: List[GroupBoardColor] = []
