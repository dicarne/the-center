import { HubConnectionState } from "@microsoft/signalr";
import { onUnmounted } from "vue";
import { connection, call_after_connected } from "../connection/Server";


export function onConnected(action: () => void) {
  let complete = false;
  let act = () => {
    complete = true;
    action();
  };
  if (connection.state === HubConnectionState.Connected) {
    act();
  }
  call_after_connected.push(act);

  onUnmounted(() => {
    const ind = call_after_connected.findIndex(act);
    if (ind != -1) {
      call_after_connected.splice(ind, 1);
    }

  });
}

export async function GetWorkspaceList() {
  return (await connection.invoke("GetWorkspaces")) as WorkspaceDesc[];
}

export async function GetBoards(workspace: string) {
  return (await connection.invoke("GetBoards", workspace)) as BoardUI[];
}

export async function CreateWorkspace(name: string) {
  return (await connection.invoke("CreateWorkspace", name)) as boolean;
}

export async function CreateBoard(workspace: string, boardtype: string) {
  return (await connection.invoke(
    "CreateBoard",
    workspace,
    boardtype
  )) as boolean;
}
export async function DeleteBoard(workspace: string, boardid: string) {
  return (await connection.invoke(
    "DeleteBoard",
    workspace,
    boardid
  )) as boolean;
}

export async function RenameBoard(wk: string, board: string, newname: string) {

  await connection.invoke(
    "RenameBoard",
    wk,
    board,
    newname
  )
}

export async function HandleBoardUIEvent(
  workspace: string,
  boardid: string,
  ui: string,
  event: string,
  arg?: string[]
) {
  return await connection.invoke("HandleEvent", JSON.stringify({
    wk: workspace,
    bd: boardid,
    ui,
    e: event,
    arg: arg
  }));
}

export async function FocusWorkspace(workspace: string) {
  console.log("focus " + workspace)
  return (await connection.invoke("FocusWorkspace", workspace))
}

export interface ApiDoc {
  docs: ApiDocData[]
}
export interface ApiDocData {
  type: string;
  styles: any;
  events: any;
  param: any;
}
export async function GetAPIDoc(): Promise<ApiDoc> {
  return { docs: JSON.parse(await connection.invoke("GetAPIDoc")) }
}

export interface ModuleTypeNamePair {
  type: string;
  name: string;
}
export async function GetAllBoardTypes(): Promise<ModuleTypeNamePair[]> {
  return (await connection.invoke("GetAllBoardType"))
}

export interface Board {
  cardType: string;
  cName: string;
  prop: any;
  w: number;
  h: number;
  id: string;
}
export interface WorkspaceDesc {
  wName: string;
  id: string;
  boards: Board[];
}
export interface BoardUI extends Board {
  uIComs: UICom[];
  ver: number;
  hide: boolean;
}
export interface UICom {
  id: string;
  type: string;
  style: Object;
  prop: Object;
  event: string[];
}
