<template>
    <a-col>
        <a-row>
            <a-button @click="stopServer">Stop</a-button>
        </a-row>
        <a-row>
            <p>{{ serverAlive ? "在线" : "离线" }}</p>
        </a-row>
        <a-row>
            <a-button @click="createWorkspace">+</a-button>
        </a-row>
    </a-col>
</template>
<script lang="ts">
import { defineComponent, onMounted, onUnmounted, PropType, ref } from "vue";
import { Ping, StopServer } from "../api/workspace";
export default defineComponent({
    components: {

    },
    props: {
        createWorkspace: {
            type: Function as PropType<(name: string) => void>,
            required: true
        }
    },
    setup: (prop) => {
        const stopServer = async () => {
            await StopServer();
        }

        const pingTimer = ref<null | NodeJS.Timeout>(null)
        const serverAlive = ref(false)
        onMounted(() => {
            pingTimer.value = setInterval(async () => {
                try {
                    const pr = await Ping();
                    if (pr) {
                        serverAlive.value = true
                    } else {
                        serverAlive.value = false
                    }
                } catch (error) {
                    serverAlive.value = false
                }

            }, 1000)
        })
        onUnmounted(() => {
            if (pingTimer.value != null) {
                clearInterval(pingTimer.value)
                pingTimer.value = null
            }
        })

        const createWorkspace = () => {
            prop.createWorkspace("test")
        }
        return {
            stopServer, serverAlive,
            createWorkspace
        }
    },
})
</script>
<style>
</style>