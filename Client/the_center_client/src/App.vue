<template>
    <a-layout>
        <a-layout-sider
            width="100"
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
                <a-menu-item key="home">
                    <p>主页</p>
                </a-menu-item>
                <a-menu-divider />
                <a-menu-item :key="item.id" v-for="item in workspaces">
                    <p>{{ item.wName }}</p>
                </a-menu-item>
            </a-menu>
        </a-layout-sider>
    </a-layout>
    <a-layout :style="{ marginLeft: '100px', minHeight: '100vh' }">
        <a-layout-content :style="{ margin: '24px 16px 0', overflow: 'initial' }">
            <MainWorkspace
                v-if="currentWorkspace != null && currentWorkspace != 'home' && currentWorkspaceObj != null"
                :workspace="currentWorkspace"
                :key="currentWorkspace"
                :workspaceObj="currentWorkspaceObj"
                :reload="reload"
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
        const currentWorkspaceObj = ref<null | WorkspaceDesc>(null)
        const workspaces = ref<null | WorkspaceDesc[]>(null);

        const getWorkspace = async () => {
            workspaces.value = await GetWorkspaceList();
        }

        onConnected(async () => {
            await getWorkspace();
        });

        const control = reactive({
            selectedKeys: ["home"] as string[]
        })


        const createWorkspace = async (name: string) => {
            const ret = await CreateWorkspace(name);
            await getWorkspace();
            currentWorkspace.value = ret;
            currentWorkspaceObj.value = workspaces.value?.find(w => w.id === ret)!
            control.selectedKeys = [ret]
        }

        const changeWorkspace = (e: any) => {
            currentWorkspace.value = e.key;
            if (e.key != "home") {
                currentWorkspaceObj.value = workspaces.value?.find(w => w.id == e.key) || null

            } else {
                currentWorkspaceObj.value = null
            }
        }

        const reload = async (old: string) => {
            const oldindex = workspaces.value?.findIndex(w => w.id === old)!
            await getWorkspace();
            if (currentWorkspace.value != "home" && workspaces.value?.findIndex((w) => w.id === currentWorkspace.value) === -1) {
                if (workspaces.value.length === 0) {
                    currentWorkspace.value = "home"
                    control.selectedKeys = ["home"]
                } else {
                    if (workspaces.value.length <= oldindex) {
                        currentWorkspace.value = workspaces.value[workspaces.value.length - 1].id
                        currentWorkspaceObj.value = workspaces.value[workspaces.value.length - 1]
                        control.selectedKeys = [currentWorkspace.value]
                    } else {
                        currentWorkspace.value = workspaces.value[oldindex].id
                        currentWorkspaceObj.value = workspaces.value[oldindex]
                        control.selectedKeys = [currentWorkspace.value]
                    }
                }
            }
        }
        return {
            currentWorkspace,
            control,
            workspaces,
            createWorkspace,
            changeWorkspace,
            currentWorkspaceObj,
            reload
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
