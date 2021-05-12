<template>
    <div class="card-board card-body">
        <p class="title">{{ title }}</p>
        <div style="margin-top: 5px;">
            <BoardElement
                v-for="ui in uIComs"
                :key="ui.id + ver"
                :ui="ui"
                :workspace="workspace"
                :board="boardid"
                :environment="env"
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
            </a-menu>
        </template>
    </a-dropdown>
    <a-modal title="重命名" v-model:visible="menuVisiable.rename" @ok="rename">
        <a-input v-model:value="rename_value" placeholder="新名称" />
    </a-modal>
</template>
<script lang="ts">
import { createVNode, defineComponent, PropType, reactive, ref, watch } from "vue";
import { BoardUI, DeleteBoard, RenameBoard, UICom } from "../api/workspace";
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
            rename: false
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

        return {
            onClick, title, menuVisiable, rename, rename_value, env: prop.environment
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
</style>