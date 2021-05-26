from typing import Dict, List
from fastapi import FastAPI
from fastapi.encoders import jsonable_encoder
from starlette.responses import JSONResponse
import uvicorn

app = FastAPI()


@app.get('/')
def index():
    return {'message': '你已经正确创建 FastApi 服务！'}


if __name__ == "__main__":
    uvicorn.run("index:app", host="0.0.0.0", port=8899, reload=True)


@app.get("/interface")
def buildInterface():
    hello = Text("textUI", "Hello Python!")
    print(jsonable_encoder(hello))
    return JSONResponse(content=jsonable_encoder([hello]))


class UICom:
    id: str
    type: str
    style: Dict[str, str]
    prop: Dict[str, str]
    event: List[str]
    def __init__(self) -> None:
        self.style = {}
        self.prop = {}
        self.event = {}


class Text(UICom):
    def __init__(self, id, text) -> None:
        super().__init__()
        self.type = "text"
        self.id = id
        self.prop['text'] = text
