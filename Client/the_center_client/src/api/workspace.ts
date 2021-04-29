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
        return;
    } else {
        call_after_connected.push(act);
    }
    onUnmounted(() => {
        if (!complete) {
            const ind = call_after_connected.findIndex(act);
            if (ind != -1) {
                call_after_connected.splice(ind, 1);
            }
        }
    });
}
/**
 * TODO
 */
export async function GetWorkspaceList() {
    return await connection.invoke("GetWorkspaceList");
}

export async function GetBoards(workspace: string) {
    return await connection.invoke("GetBoards", workspace);
}

export interface Board {
    cardType: string;
    cName: string;
    prop: any;
    w: number;
    h: number;
    id: string;
}
