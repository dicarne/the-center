<template>
    <a-button v-if="ui.type === 'button'" @click="click">CLICK</a-button>
    <p v-if="ui.type === 'text'">{{ prop["text"] }}</p>
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
        }
        return { ui, click, prop: (ui as any).prop as any }
    },
})
</script>
<style>
</style>