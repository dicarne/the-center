<template>
    <div class="card-board card-body">
        <p class="group-color" :style="{ backgroundColor: boardColor }"></p>
        <p class="title">{{ title }}</p>
        <div style="margin-top: 5px;">
            <BoardElement
                v-for="ui in uIComs"
                :key="ui.id + ver"
                :ui="ui"
                :workspace="workspace"
                :board="boardid"
                :environment="env"
                :deep="0"
            />
        </div>
    </div>
    <a-dropdown :trigger="['click']" class="close-pos">
        <a class="ant-dropdown-link" @click.prevent>
            <DownOutlined />
        </a>
        <template #overlay>
            <a-menu @click="onClick">
                <a-menu-item key="delete">
                    <p>删除</p>
                </a-menu-item>
                <a-menu-item key="rename">
                    <p>重命名</p>
                </a-menu-item>
                <a-menu-item key="setGroup">
                    <p>编辑组</p>
                </a-menu-item>
            </a-menu>
        </template>
    </a-dropdown>
    <a-modal title="重命名" v-model:visible="menuVisiable.rename" @ok="rename">
        <a-input v-model:value="rename_value" placeholder="新名称" />
    </a-modal>
    <a-modal title="编辑组/颜色" v-model:visible="menuVisiable.setGroup" @ok="setGroupOk">
        <a-input v-model:value="new_group" placeholder="新组/颜色" />
    </a-modal>
</template>
<script lang="ts">
import { computed, createVNode, defineComponent, inject, PropType, reactive, Ref, ref, watch } from "vue";
import { BoardUI, DeleteBoard, RenameBoard, SetBoardGroup, UICom, WorkspaceDesc } from "../api/workspace";
import BoardElement from "../components/BoardElement.vue"
import { DownOutlined, ExclamationCircleOutlined } from '@ant-design/icons-vue';
import { Modal } from "ant-design-vue";

export default defineComponent({
    components: {
        BoardElement,
        DownOutlined
    },
    props: {
        workspace: {
            type: String,
            required: true,
        },
        boardid: {
            type: String,
            required: true,
        },
        ver: {
            type: Number,
            required: true
        },
        uIComs: {
            type: Object as PropType<UICom[]>,
            required: true
        },
        getboard: {
            type: Function,
            required: true
        },
        board: {
            type: Object as PropType<BoardUI>,
            required: true
        },
        environment: {
            type: Object as PropType<{ boards: BoardUI[] }>,
            required: true
        }
    },
    setup: (prop) => {
        const title = ref(prop.board.cName)
        const menuVisiable = reactive({
            rename: false,
            setGroup: false
        })
        const onClick = (e: any) => {
            switch (e.key) {
                case "delete":
                    Modal.confirm({
                        title: '删除',
                        icon: createVNode(ExclamationCircleOutlined),
                        content: '是否要删除此卡片？',
                        okText: '删除',
                        okType: 'danger',
                        cancelText: '取消',
                        async onOk() {
                            await DeleteBoard(prop.workspace, prop.boardid);
                            await prop.getboard();
                        },
                        onCancel() {

                        },
                    });
                    break;
                case "rename":
                    menuVisiable.rename = true;
                    break;
                case "setGroup":
                    menuVisiable.setGroup = true;
                    new_group.value = group.value
                    break;
                default:
                    break;
            }
        }

        const rename_value = ref("")
        const rename = async () => {
            await RenameBoard(prop.workspace, prop.boardid, rename_value.value)
            title.value = rename_value.value
            menuVisiable.rename = false
        }
        const group = ref<string>(prop.board.group || "")
        const new_group = ref("")
        const setGroupOk = async () => {
            await SetBoardGroup(prop.workspace, prop.boardid, new_group.value)
            group.value = new_group.value
            menuVisiable.setGroup = false
        }
        const workspaceObj = inject("workspace") as Ref<WorkspaceDesc>
        const boardColor = computed(() => {
            if (!!workspaceObj.value.groups) {
                const go = workspaceObj.value.groups.find(i => i.name === group.value)
                if (!!go) {
                    return go.color
                }
                return group.value
            }
            return ""

        })
        return {
            onClick, title, menuVisiable, rename, rename_value, setGroupOk, new_group, env: prop.environment, boardColor: boardColor
        }
    },
})
</script>
<style>
.close-pos {
    right: 20px;
    top: 15px;
    position: absolute;
}

.title {
    position: absolute;
    left: 12px;
    top: 8px;
    font-size: 12px;
    color: rgb(161, 161, 161);
}

.group-color {
    position: absolute;
    left: 0px;
    top: 0px;
    height: 6px;
    width: 100%;
    border-radius: 3px 3px 0px 0px;
}
</style>