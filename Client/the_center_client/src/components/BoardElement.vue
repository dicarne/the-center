<template>
    <div :class="deep === 0 ? 'board-element' : ''">
        <a-button
            v-if="ui.type === 'transfer'"
            @click="transfer.open"
            :style="ui.style"
        >{{ ui.prop.text }}</a-button>
        <a-button v-if="ui.type === 'button'" @click="click" :style="ui.style">{{ ui.prop.text }}</a-button>
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
            :style="ui.style"
        >
            <UpOutlined v-if="ui.prop.isshow === 'true'" />
            <DownOutlined v-if="ui.prop.isshow === 'false'" />
            {{ " " + ui.prop.text }}
        </a>
        <p v-if="ui.type === 'text'" class="text" :style="ui.style">{{ ui.prop.text }}</p>
        <a-input
            v-if="ui.type === 'input'"
            v-model:value="textvalue"
            @change="onTextChange"
            :placeholder="ui.prop['placeholder']"
            :style="ui.style"
        ></a-input>
        <div v-if="ui.type === 'group'">
            <a-row
                v-if="ui.prop['hor'] == 'true'"
                :align="'middle'"
                :gutter="JSON.parse(ui.prop['spacing'] || '0')"
            >
                <a-col
                    v-for="cui in groupComs"
                    :span="cui.prop['span'] ? JSON.parse(ui.prop['span']) : undefined"
                    :flex="cui.prop['flex']"
                >
                    <BoardElement
                        :key="cui.id"
                        :ui="cui"
                        :workspace="workspace"
                        :board="board"
                        :environment="environment"
                        :deep="deep + 1"
                    />
                </a-col>
            </a-row>
            <div v-else>
                <BoardElement
                    v-for="ui in groupComs"
                    :key="ui.id"
                    :ui="ui"
                    :workspace="workspace"
                    :board="board"
                    :environment="environment"
                    :deep="deep + 1"
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
import { defineComponent, PropType, reactive, ref, toRaw, watchEffect } from "vue";
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
            type: Object as PropType<UICom>,
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
        const id = prop.ui.id
        const type = prop.ui.type

        const click = async () => {
            const ret = await HandleBoardUIEvent(prop.workspace, prop.board, id, 'onClick')
        }

        const textvalue = ref(prop.ui.prop['text'] as string)
        watchEffect(() => {
            if (textvalue.value != prop.ui.prop['text']) {
                textvalue.value = prop.ui.prop['text']
            }

        })
        const onTextChange = async () => {
            await HandleBoardUIEvent(prop.workspace, prop.board, id, 'onChange', [textvalue.value])
        }

        // Transfer
        const transfer = reactive({
            open_stat: false,
            open: async () => {
                transfer.open_stat = true
                var ret = await HandleBoardUIEvent(prop.workspace, prop.board, id, "onShow")
                transfer.tar = ret.ava
                transfer.list = prop.environment.boards.map(it => { return { ...it, key: it.id, title: it.cName, disabled: it.id === prop.board } })
            },
            comfirm: async () => {
                transfer.open_stat = false;
                await HandleBoardUIEvent(prop.workspace, prop.board, id, "onChange", [JSON.stringify(transfer.tar), "[]"])
            },
            list: prop.environment.boards.map(it => { return { ...it, key: it.id, title: it.cName, disabled: it.id === prop.board } }),
            tar: [] as string[],
            select: [] as string[],
            render: (k: BoardUI) => {
                switch (prop.ui.prop.type) {
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

        const groupComs = ref<UICom[] | null>(null)
        if (type === "group") {
            watchEffect(() => {
                groupComs.value = reactive(JSON.parse(prop.ui.prop["children"]))
            })

        }
        // --------
        const checkBoxValue = ref(false)
        if (type === "checkBox") {
            watchEffect(() => {
                checkBoxValue.value = prop.ui.prop["value"] === "true" ? true : false
            })

        }
        const checkBoxChange = async () => {
            await HandleBoardUIEvent(prop.workspace, prop.board, id, "onChange", [JSON.stringify(checkBoxValue.value)])
        }
        // --------
        return { click, textvalue, onTextChange, transfer, groupComs, checkBoxValue, checkBoxChange }
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