import { HubConnectionBuilder } from "@microsoft/signalr";
export const dev = import.meta.env.MODE == "development";
console.log(dev)
export const connection = new HubConnectionBuilder()
  .withUrl(`http://localhost:${dev ? 5000 : 5800}/workspace`)
  .withAutomaticReconnect()
  .build();

export const call_after_connected: (() => void)[] = [];

export async function start() {
  try {
    await connection.start();
    console.log("SignalR Connected.");
    call_after_connected.forEach((item) => {
      try {
        item();
      } catch (error) {
        console.error(error);
      }
    });
  } catch (err) {
    console.log(err);
    setTimeout(start, 5000);
  }
}
connection.onreconnected((id) => {
  console.log("reconnect")
  call_after_connected.forEach((item) => {
    try {
      item();
    } catch (error) {
      console.error(error);
    }
  });
})

connection.on("send", (data) => {
  console.log(data);
});

connection.on(
  "HandleServer",
  (workspace: string, board: string, data: string) => {
    workspaces.get(workspace)?.(board, data);
  }
);
export const workspaces = new Map<
  string,
  (board: string, data: string) => void
>();
