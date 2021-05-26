<template>
    <div :class="prop.deep === 0 ? 'board-element' : ''">
        <a-button
            v-if="ui.type === 'transfer'"
            @click="transfer.open"
            :style="uiStyle"
        >{{ textvalue }}</a-button>
        <a-button v-if="ui.type === 'button'" @click="click" :style="uiStyle">{{ textvalue }}</a-button>
        <a-checkbox
            v-if="ui.type === 'checkBox'"
            v-model:checked="checkBoxValue"
            @change="checkBoxChange"
        ></a-checkbox>
        <a
            v-if="ui.type === 'more'"
            @click="click"
            class="ant-dropdown-link"
            href="javascript:;"
            :style="uiStyle"
        >
            <UpOutlined v-if="uiProp.isshow" />
            <DownOutlined v-if="!uiProp.isshow" />
            {{ " " + textvalue }}
        </a>
        <p v-if="ui.type === 'text'" class="text" :style="uiStyle">{{ textvalue }}</p>
        <a-input
            v-if="ui.type === 'input'"
            v-model:value="textvalue"
            @change="onTextChange"
            :placeholder="uiProp['placeholder']"
            :style="uiStyle"
        ></a-input>
        <div v-if="ui.type === 'group'">
            <a-row
                v-if="uiProp['hor'] == 'true'"
                :align="'middle'"
                :gutter="JSON.parse(uiProp['spacing'] || '0')"
            >
                <a-col
                    v-for="ui in groupComs"
                    :span="ui.prop['span'] ? JSON.parse(uiProp['span']) : undefined"
                    :flex="ui.prop['flex']"
                >
                    <BoardElement
                        :key="ui.id"
                        :ui="ui"
                        :workspace="workspace"
                        :board="prop.board"
                        :environment="prop.environment"
                        :deep="prop.deep + 1"
                    />
                </a-col>
            </a-row>
            <div v-else>
                <BoardElement
                    v-for="ui in groupComs"
                    :key="ui.id"
                    :ui="ui"
                    :workspace="workspace"
                    :board="prop.board"
                    :environment="prop.environment"
                    :deep="prop.deep + 1"
                />
            </div>
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
    </div>
</template>
<script lang="ts">
import { defineComponent, PropType, reactive, ref } from "vue";
import { BoardUI, HandleBoardUIEvent, UICom } from "../api/workspace"
import { DownOutlined, UpOutlined } from '@ant-design/icons-vue';

export default defineComponent({
    components: {
        DownOutlined,
        UpOutlined
    },
    name: "BoardElement",
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
            type: Object as PropType<{ boards: BoardUI[] }>,
            required: true
        },
        deep: {
            type: Number,
            required: true,
        },
    },
    setup: (prop) => {
        const ui = prop.ui as UICom
        const click = async () => {
            const ret = await HandleBoardUIEvent(prop.workspace, prop.board, ui.id, 'onClick')
        }

        const uiProp = (ui as any).prop as any
        const uiStyle = (ui as any).style as any

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
                transfer.list = prop.environment.boards.map(it => { return { ...it, key: it.id, title: it.cName, disabled: it.id === prop.board } })
            },
            comfirm: async () => {
                transfer.open_stat = false;
                await HandleBoardUIEvent(prop.workspace, prop.board, ui.id, "onChange", [JSON.stringify(transfer.tar), "[]"])
            },
            list: prop.environment.boards.map(it => { return { ...it, key: it.id, title: it.cName, disabled: it.id === prop.board } }),
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
        if (ui.type === "more") {
            uiProp.isshow = JSON.parse(uiProp.isshow)
        }
        const groupComs = ref<UICom[] | null>(null)
        if (ui.type === "group") {
            groupComs.value = JSON.parse(uiProp["children"])
        }
        // --------
        const checkBoxValue = ref(false)
        if (ui.type === "checkBox") {
            checkBoxValue.value = uiProp["value"] === "true" ? true : false
        }
        const checkBoxChange = async () => {
            await HandleBoardUIEvent(prop.workspace, prop.board, ui.id, "onChange", [JSON.stringify(checkBoxValue.value)])
        }
        //console.log(uiProp)
        // --------
        return { prop, ui, click, textvalue, onTextChange, uiProp, uiStyle, transfer, groupComs, checkBoxValue, checkBoxChange }
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