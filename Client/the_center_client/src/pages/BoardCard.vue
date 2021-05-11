<template>
    <div class="card-board card-body">
        <BoardElement
            v-for="ui in uIComs"
            :key="ui.id + ver"
            :ui="ui"
            :workspace="workspace"
            :board="boardid"
        />
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
            </a-menu>
        </template>
    </a-dropdown>
</template>
<script lang="ts">
import { createVNode, defineComponent, PropType } from "vue";
import { DeleteBoard, UICom } from "../api/workspace";
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
        }
    },
    setup: (prop) => {
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

                default:
                    break;
            }
        }
        return {
            onClick
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
</style>