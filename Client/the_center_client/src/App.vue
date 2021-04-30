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
            >
                <a-menu-item key="1">
                    <user-outlined />
                </a-menu-item>
                <a-menu-item key="2">
                    <video-camera-outlined />
                </a-menu-item>
                <a-menu-item key="3">
                    <upload-outlined />
                </a-menu-item>
                <a-menu-item key="4">
                    <bar-chart-outlined />
                </a-menu-item>
                <a-menu-item key="5">
                    <cloud-outlined />
                </a-menu-item>
                <a-menu-item key="6">
                    <appstore-outlined />
                </a-menu-item>
                <a-menu-item key="7">
                    <team-outlined />
                </a-menu-item>
                <a-menu-item key="8">
                    <shop-outlined />
                </a-menu-item>
            </a-menu>
        </a-layout-sider>
    </a-layout>
    <a-layout :style="{ marginLeft: '50px' }">
        <a-layout-content
            :style="{ margin: '24px 16px 0', overflow: 'initial' }"
        >
            <MainWorkspace
                v-if="currentWorkspace != null"
                :workspace="currentWorkspace"
            />
        </a-layout-content>
    </a-layout>
</template>

<script lang="ts">
import { defineComponent, reactive, ref } from "vue";
import MainWorkspace from "./components/MainWorkspace.vue";
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
import { onConnected } from "./api/workspace";
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
        const currentWorkspace = ref(null as string | null);
        onConnected(() => {
            currentWorkspace.value = "TEST";
        });

        const control = reactive({
            selectedKeys: ["1"]
        })

        return {
            currentWorkspace,
            control
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
