<template>
    <div class="board-element">
        <a-button v-if="ui.type === 'button'" @click="click">CLICK</a-button>
        <p
            v-if="ui.type === 'text'"
            :class="css(sc('text-align', ['left', 'text-left'], ['right', 'text-right']))"
        >{{ textvalue }}</p>
        <a-input
            v-if="ui.type === 'input'"
            v-model:value="textvalue"
            @change="onTextChange"
            :placeholder="uiProp['placeholder']"
        ></a-input>
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
        const uiStyle = (ui as any).style as any

        const sc = (propname: string, ...arg: string[][]) => {
            const v = uiStyle[propname]
            
            for (let i = 0; i < arg.length; i++) {
                const [name, value] = arg[i];
                if (name == v) return value
            }
            return ""
        }

        const css = (...names: string[]) => {
            let classname = ""
            for (let i = 0; i < names.length; i++) {
                const c = names[i];
                if (c && c != "") {
                    classname += c + " "
                }
            }
            return classname
        }

        const textvalue = ref(uiProp['text'])
        const onTextChange = async () => {
            await HandleBoardUIEvent(prop.workspace, prop.board, ui.id, 'onChange', [textvalue.value])
        }
        return { ui, click, textvalue, onTextChange, uiProp, sc, css }
    },
})
</script>
<style>
.board-element {
    margin: 10px;
}
.text-left {
    text-align: left;
}
.text-right {
    text-align: right;
}
</style>