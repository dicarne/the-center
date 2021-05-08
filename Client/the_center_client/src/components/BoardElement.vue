<template>
    <a-button v-if="ui.type === 'button'" @click="click">CLICK</a-button>
</template>
<script lang="ts">
import { defineComponent } from "vue";
import { HandleBoardUIEvent, UICom } from "../api/workspace"

export default defineComponent({
    components: {

    },
    props: {
        ui: {
            type: Object,
            required: true
        },
        workspace: {
            type: String,
            required: true,
        },
        board: {
            type: String,
            required: true,
        },
    },
    setup: (prop) => {
        const ui = prop.ui as UICom
        const click = async () => {
            const ret = await HandleBoardUIEvent(prop.workspace, prop.board, ui.id, 'onclick', ['test'])
            console.log(ret)
        }
        return { ui, click }
    },
})
</script>
<style>
</style>