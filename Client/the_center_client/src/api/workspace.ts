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
    const ind = call_after_connected.findIndex((a) => act === a);
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
  return (await connection.invoke("CreateWorkspace", name)) as string;
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

export async function SortBoards(workspace: string, boards: string[]) {
  return await connection.invoke(
    "SortBoards",
    workspace,
    boards
  );
}

export async function RenameBoard(wk: string, board: string, newname: string) {

  await connection.invoke(
    "RenameBoard",
    wk,
    board,
    newname
  )
}

export async function SetBoardGroup(wk: string, board: string, newgroup: string) {

  await connection.invoke(
    "SetBoardGroup",
    wk,
    board,
    newgroup
  )
}

export async function StopServer() {

  await connection.invoke(
    "ExitServer"
  )
}
export async function Ping() {
  return await connection.invoke(
    "Alive"
  )
} RenameWorkspace
export async function RenameWorkspace(id: string, newname: string) {

  return await connection.invoke(
    "RenameWorkspace", id, newname
  )
}
export async function DeleteWorkspace(id: string) {

  return await connection.invoke(
    "DeleteWorkspace", id
  )
}
export async function HandleBoardUIEvent(
  workspace: string,
  boardid: string,
  ui: string,
  event: string,
  arg?: any[]
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
  group?: string
  breakpointH: Record<string, number>
}
export interface WorkspaceDesc {
  wName: string;
  id: string;
  boards: Board[];
  groups: GroupBoard[]
}
interface GroupBoard {
  name: string
  color: string
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
  prop: any;
  eventlist: string[];
}


export async function Config_SetDBPath(id: string) {

  return await connection.invoke(
    "Config_SetDBPath", id
  )
} export async function Config_GetDBPath(): Promise<string> {

  return (await connection.invoke(
    "Config_GetDBPath"
  )) as string
}

//

export async function GetWorkspaceGlobalVariables(workspace: string) {
  return (await connection.invoke("GetWorkspaceGlobalVariables", workspace))
}
export async function SetWorkspaceGlobalVariables(workspace: string, values: string[][]) {
  return (await connection.invoke("SetWorkspaceGlobalVariables", workspace, values))
}