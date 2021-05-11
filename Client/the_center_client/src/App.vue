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
                <a-button @click="createWorkspace">+</a-button>
                <a-menu-item :key="item.id" v-for="item in workspaces">
                    <p>{{ item.id }}</p>
                </a-menu-item>
            </a-menu>
        </a-layout-sider>
    </a-layout>
    <a-layout :style="{ marginLeft: '50px', minHeight: '100vh' }">
        <a-layout-content :style="{ margin: '24px 16px 0', overflow: 'initial' }">
            <MainWorkspace v-if="currentWorkspace != null && currentWorkspace != 'home'" :workspace="currentWorkspace" :key="currentWorkspace"></MainWorkspace>
            
        </a-layout-content>
    </a-layout>
</template>

<script lang="ts">
import { defineComponent, reactive, ref } from "vue";
import MainWorkspace from "./pages/MainWorkspace.vue";
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
            selectedKeys: []
        })

        const createWorkspace = async () => {
            const ret = await CreateWorkspace("test");
            if (ret) {
                await getWorkspace();
            }
        }

        const changeWorkspace = (e: any) => {
            currentWorkspace.value = e.key;
        }

        return {
            currentWorkspace,
            control,
            workspaces,
            createWorkspace,
            changeWorkspace
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
