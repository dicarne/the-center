<template>
    <div class="board-element">
        <a-button v-if="ui.type === 'button'" @click="click">CLICK</a-button>
        <p v-if="ui.type === 'text'">{{ textvalue }}</p>
        <a-input v-if="ui.type === 'input'" v-model:value="textvalue" @change="onTextChange" :placeholder="uiProp['placeholder']"></a-input>
    </div>
</template>
<script lang="ts">
import { defineComponent, ref } from "vue";
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
            const ret = await HandleBoardUIEvent(prop.workspace, prop.board, ui.id, 'onClick', ['test'])
        }

        const uiProp = (ui as any).prop as any
        const textvalue = ref(uiProp['text'])
        const onTextChange = async () => {
            await HandleBoardUIEvent(prop.workspace, prop.board, ui.id, 'onChange', [textvalue.value])
        }
        return { ui, click, textvalue, onTextChange, uiProp }
    },
})
</script>
<style>
.board-element {
    margin: 10px;
}
</style>