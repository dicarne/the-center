<template>
    <div class="board-element">
        <a-button
            v-if="ui.type === 'transfer'"
            @click="transfer.open"
            :style="uiStyle"
        >{{ textvalue }}</a-button>
        <a-button v-if="ui.type === 'button'" @click="click" :style="uiStyle">{{ textvalue }}</a-button>
        <p v-if="ui.type === 'text'" class="text" :style="uiStyle">{{ textvalue }}</p>
        <a-input
            v-if="ui.type === 'input'"
            v-model:value="textvalue"
            @change="onTextChange"
            :placeholder="uiProp['placeholder']"
            :style="uiStyle"
        ></a-input>
    </div>

    <a-modal title="选择卡片" v-model:visible="transfer.open_stat" @ok="transfer.comfirm">
        <a-transfer
            :data-source="transfer.list"
            :titles="['备选', '运行']"
            :target-keys="transfer.tar"
            :selected-keys="transfer.select"
            :render="transfer.render"
            @change="transfer.handleChange"
            @selectChange="transfer.handleSelectChange"
        />
    </a-modal>
</template>
<script lang="ts">
import { defineComponent, PropType, reactive, ref } from "vue";
import { BoardUI, HandleBoardUIEvent, UICom } from "../api/workspace"

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
        environment: {
            type: Object as PropType<BoardUI[]>,
            required: true
        }
    },
    setup: (prop) => {
        const ui = prop.ui as UICom
        const click = async () => {
            const ret = await HandleBoardUIEvent(prop.workspace, prop.board, ui.id, 'onClick')
        }

        const uiProp = (ui as any).prop as any
        const uiStyle = (ui as any).style as any

        //const sc = (propname: string, ...arg: string[][]) => {
        //    const v = uiStyle[propname]
        //
        //    for (let i = 0; i < arg.length; i++) {
        //        const [name, value] = arg[i];
        //        if (name == v) return value
        //    }
        //    return ""
        //}
        //
        //const css = (...names: string[]) => {
        //    let classname = ""
        //    for (let i = 0; i < names.length; i++) {
        //        const c = names[i];
        //        if (c && c != "") {
        //            classname += c + " "
        //        }
        //    }
        //    return classname
        //}

        const textvalue = ref(uiProp['text'])
        const onTextChange = async () => {
            await HandleBoardUIEvent(prop.workspace, prop.board, ui.id, 'onChange', [textvalue.value])
        }

        // Transfer
        const transfer = reactive({
            open_stat: false,
            open: async () => {
                transfer.open_stat = true
                var ret = await HandleBoardUIEvent(prop.workspace, prop.board, ui.id, "onShow")
                transfer.tar = ret.ava
            },
            comfirm: async () => {
                transfer.open_stat = false;
                await HandleBoardUIEvent(prop.workspace, prop.board, ui.id, "onChange", [JSON.stringify(transfer.tar), "[]"])
            },
            list: prop.environment.map(it => { return { ...it, key: it.id, title: it.cName, disabled: it.id === prop.board } }),
            tar: [] as string[],
            select: [] as string[],
            render: (k: BoardUI) => {
                switch (uiProp['type']) {
                    case 'local_boards':

                        break;

                    default:
                        break;
                }
                return k.cName
            },
            handleChange: (nextTargetKeys: string[], direction: string, moveKeys: string[]) => {
                transfer.tar = nextTargetKeys;
            },
            handleSelectChange: (sourceSelectedKeys: string[], targetSelectedKeys: string[]) => {
                transfer.select = [...sourceSelectedKeys, ...targetSelectedKeys];
            }
        })



        // --------
        return { ui, click, textvalue, onTextChange, uiProp, uiStyle, transfer }
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
.text {
    overflow: auto;
}
</style>