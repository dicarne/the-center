const { app, BrowserWindow } = require('electron')
const path = require('path')

const isDebug = !app.isPackaged

function createWindow() {
  const win = new BrowserWindow({
    width: 1000,
    height: 800,
    webPreferences: {
      //preload: path.join(__dirname, 'preload.js')
    }
  })

  win.loadFile(`${isDebug ? __dirname : process.resourcesPath}/web/index.html`)
}

app.whenReady().then(() => {
  createWindow()

  app.on('activate', () => {
    if (BrowserWindow.getAllWindows().length === 0) {
      createWindow()
    }
  })
})


const exec = require('child_process').exec;

const serverpath = `${isDebug ? __dirname : process.resourcesPath}/server/TheCenterServer.exe --dbpath D:/centerLib.db`
const server = exec(serverpath, function (err, data) { if (err) { throw err; } console.log(data.toString()); });


app.on('window-all-closed', () => {
  if (process.platform !== 'darwin') {
    app.quit()
  }
})