import { createApp } from 'vue'
import Antd from 'ant-design-vue';
import App from './App.vue'
import APIDoc from "./pages/APIDoc.vue"
import Route from "./Route.vue"
import 'ant-design-vue/dist/antd.css';
import { start } from "./connection/Server"
import { createRouter, createWebHashHistory } from "vue-router"

const routes = [
    { path: '/', component: App },
    { path: '/apidoc', component: APIDoc },
]
const router = createRouter({
    history: createWebHashHistory(),
    routes,
})

const app = createApp(Route)
app.use(Antd)
app.use(router)
app.mount('#app')
start()