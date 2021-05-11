<template>
    <a-layout>
        <a-layout-sider
            width="50"
            :style="{
                overflow: 'auto',
                height: '100vh',
                position: 'fixed',
                left: 0,
            }"
        >
            <a-menu
                theme="dark"
                mode="inline"
                v-model:selectedKeys="control.selectedKeys"
                @click="changeWorkspace"
            >
                <a-button @click="home">+</a-button>
                <a-menu-item :key="item.id" v-for="item in workspaces">
                    <p>{{ item.id }}</p>
                </a-menu-item>
            </a-menu>
        </a-layout-sider>
    </a-layout>
    <a-layout :style="{ marginLeft: '50px', minHeight: '100vh' }">
        <a-layout-content :style="{ margin: '24px 16px 0', overflow: 'initial' }">
            <MainWorkspace
                v-if="currentWorkspace != null && currentWorkspace != 'home'"
                :workspace="currentWorkspace"
                :key="currentWorkspace"
            ></MainWorkspace>
            <Home v-if="currentWorkspace == 'home'" :createWorkspace="createWorkspace" />
        </a-layout-content>
    </a-layout>
</template>

<script lang="ts">
import { defineComponent, reactive, ref } from "vue";
import MainWorkspace from "./pages/MainWorkspace.vue";
import Home from "./pages/Home.vue"
import {
    UserOutlined,
    VideoCameraOutlined,
    UploadOutlined,
    BarChartOutlined,
    CloudOutlined,
    AppstoreOutlined,
    TeamOutlined,
    ShopOutlined,
} from "@ant-design/icons-vue";
import { GetWorkspaceList, onConnected, WorkspaceDesc, CreateWorkspace } from "./api/workspace";
export default defineComponent({
    name: "App",
    components: {
        MainWorkspace,
        UserOutlined,
        VideoCameraOutlined,
        UploadOutlined,
        BarChartOutlined,
        CloudOutlined,
        AppstoreOutlined,
        TeamOutlined,
        ShopOutlined,
        Home
    },
    setup: () => {
        const currentWorkspace = ref("home");
        const workspaces = ref<null | WorkspaceDesc[]>(null);

        const getWorkspace = async () => {
            workspaces.value = await GetWorkspaceList();
        }

        onConnected(async () => {
            await getWorkspace();
        });

        const control = reactive({
            selectedKeys: [] as string[]
        })

        const home = async () => {
            currentWorkspace.value = "home"
        }
        const createWorkspace = async (name: string) => {
            const ret = await CreateWorkspace(name);
            await getWorkspace();
            currentWorkspace.value = ret;
            control.selectedKeys = [ret]
        }

        const changeWorkspace = (e: any) => {
            currentWorkspace.value = e.key;
        }

        return {
            currentWorkspace,
            control,
            workspaces,
            createWorkspace,
            changeWorkspace,
            home
        };
    },
});
</script>

<style>
#app {
    font-family: Avenir, Helvetica, Arial, sans-serif;
    -webkit-font-smoothing: antialiased;
    -moz-osx-font-smoothing: grayscale;
    text-align: center;
    color: #2c3e50;
}
</style>
